using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;

namespace USTeco
{
    public partial class HistReport : Form
    {
        OleDbConnection accCon = new OleDbConnection();
        OracleConnection oraCon = new OracleConnection();
        SqlConnection sqlCon = new SqlConnection();
        OleDbCommand cmd;
        ArrayList row = new ArrayList();
        int numberOfItemsPrintedSoFar = 0;
        int numberOfItemsPerPage = 0;
        int currentPageNo = 1;
        int currentPageCount = 0;
        string mySql;
        string myDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UST Applications", "Handpunch");
        public HistReport()
        {
            InitializeComponent();
            accCon.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + myDataPath + "\\handdb.mdb; Jet OLEDB:Database Password = Ajoset25";
            accCon.Open();
            BackColor = MyGlobal.color1;
            panel2.BackColor = MyGlobal.color2;
            mySql = "Select Place_id,Place_Name From Devices";
            ComboBox1.Items.Add("All Devices");
            cmd = new OleDbCommand(mySql, accCon);
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            int MyPlace;
            string PlaceName;
            while (reader.Read())
            {
                MyPlace = reader.GetInt32(0);
                try
                {
                    PlaceName = reader.GetString(1);
                }
                catch
                {
                    PlaceName = "";
                }
                ComboBox1.Items.Add(MyPlace.ToString() + "-" + PlaceName);
            }
            ComboBox1.SelectedIndex = 0;
        }

        private bool CheckDatabase()
        {
            oraCon.Close();
            sqlCon.Close();
            bool success = false;
            switch (MyGlobal.ServerType)
            {
                case 0:
                    string MyData = "(DESCRIPTION = " +
                                    "(ADDRESS = " +
                                        "(PROTOCOL = TCP) " +
                                        "(HOST = " + MyGlobal.MyServer + ") " +
                                        "(PORT = " + MyGlobal.MyPort.ToString() + ") " +
                                    ") " +
                                    "(CONNECT_DATA = " +
                                        "(SERVER = dedicated) " +
                                        "(SERVICE_NAME = " + MyGlobal.MySid + ") " +
                                    ") " +
                                    ")";
                    oraCon.ConnectionString = @"DATA SOURCE=" + MyData + ";USER ID=" + MyGlobal.MyUser + ";PASSWORD=" + MyGlobal.MyPass;
                    try
                    {
                        oraCon.Open();
                        success = true;
                    }
                    catch
                    {
                        success = false;
                    }
                    break;
                case 1:
                    sqlCon.ConnectionString = @"Server = " + MyGlobal.sqlServer + "; Database = master; Trusted_Connection = True;";
                    try
                    {
                        sqlCon.Open();
                        try
                        {
                            mySql = "Use " + MyGlobal.sqlUser;
                            SqlCommand sqlcmd = new SqlCommand(mySql, sqlCon);
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            mySql = "Create Database " + MyGlobal.sqlUser;
                            SqlCommand sqlcmd = new SqlCommand(mySql, sqlCon);
                            sqlcmd.ExecuteNonQuery();
                            mySql = "Use " + MyGlobal.MyUser + " Create Table EMS_EMP_FINGERS (Sign_Seq numeric(18, 0) IDENTITY(1,1) PRIMARY KEY," +
                                "Emp_Id nvarchar(10),Sign_Date nvarchar(10),Sign_Time nvarchar(10)," +
                                "Sign_Plc_Id nvarchar(10))";
                            sqlcmd = new SqlCommand(mySql, sqlCon);
                            sqlcmd.ExecuteNonQuery();
                        }
                        success = true;
                    }
                    catch
                    {
                        success = false;
                    }
                    break;
                case 2:
                    //accCon.Open();
                    success = true;
                    break;
            }
            return success;
        }

        private void logRepButt_Click(object sender, EventArgs e)
        {
            string mySql = "Select id,devId,devName,Format(rDate,'dd/MM/yyyy') as rDate,rTime,rCount " +
                "From LogFile " +
                "Where(((DateValue([rDate])) >= DateValue('" +
                logDateFrom.Text + "') And DateValue([rDate]) <= DateValue('" +
                logDateTo.Text + "'))) order by DateValue([rDate]) ";
            //using (OleDbDataAdapter dadapter = new OleDbDataAdapter(mySql, accCon))
            //{
            //    DataTable table = new DataTable();
            //    dadapter.Fill(table);
            //    gridView3.DataSource = table;
            //}
            if (CheckDatabase())
            {
                OracleCommand cmd;
                OracleDataReader reader;
                string MyCond;
                if (ComboBox1.SelectedIndex > 0)
                {
                    MyCond = "And Sign_Plc_Id=" + ComboBox1.Text.ToString().Split('-')[0];
                }
                else
                {
                    MyCond = "";
                }
                if (radioButt1.Checked)
                {
                    mySql = "Select '0000' As Id, Sign_Plc_Id As devId, ' ' As devName, Sign_Date As rDate, " +
                    "Count(Sign_Plc_id) As rCount, Max(Time_Stamp) As rTime " +
                    "From Ems_Emp_Fingers " +
                    "Where Sign_Date >= To_Date('" + logDateFrom.Value.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy') " +
                        "And Sign_Date <= To_Date('" + logDateTo.Value.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy') " +
                        MyCond +
                    " Group By Sign_Plc_Id,Sign_Date " +
                    "Order By Sign_Date, Sign_Plc_Id ";
                }else{
                    mySql = "Select '0000' As Id, Sign_Plc_Id As devId, ' ' As devName, To_Date(To_Char(Time_Stamp,'dd/mm/yyyy'),'dd/mm/yyyy') As rDate, " +
                        "Count(Sign_Plc_id) As rCount ,  Max(Time_Stamp) As rTime " +
                        "From Ems_Emp_Fingers " +
                        "Where To_Date(To_Char(Time_Stamp,'dd/mm/yyyy'),'dd/mm/yyyy') >= To_Date('" + logDateFrom.Value.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy') " +
                            "And To_Date(To_Char(Time_Stamp,'dd/mm/yyyy'),'dd/mm/yyyy') <= To_Date('" + logDateTo.Value.ToString("dd/MM/yyyy") + "', 'dd/mm/yyyy') " +
                            MyCond +
                        " Group By Sign_Plc_Id ,To_Date(To_Char(Time_Stamp,'dd/mm/yyyy'),'dd/mm/yyyy') " +
                        "Order By Sign_Plc_Id,To_Date(To_Char(Time_Stamp,'dd/mm/yyyy'),'dd/mm/yyyy') ";
                }
                cmd = new OracleCommand(mySql,oraCon);
                reader = cmd.ExecuteReader();
                gridView3.Rows.Clear();
                int j, k = 0;
                while (reader.Read())
                {
                    k++;  
                    j = reader.GetInt32(1);
                    row.Clear();
                    row.Add(k);
                    row.Add(reader.GetInt32(1));
                    for(int i = 1; i < ComboBox1.Items.Count; i++)
                    {
                        if (j == Int32.Parse(ComboBox1.Items[i].ToString().Split('-')[0])){
                            row.Add(ComboBox1.Items[i].ToString().Split('-')[1]);
                            break;
                        }
                    }
                    row.Add(reader.GetDateTime(3).ToString("dd/MM/yyyy"));
                    row.Add(reader.GetDateTime(5).ToString("HH:mm:ss"));
                    row.Add(reader.GetValue(4));
                    row.Add(reader.GetDateTime(5).ToString("dd/MM/yyyy HH:mm:ss"));
                    gridView3.Rows.Add(row.ToArray());
                }
                reader.Close();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (gridView3.RowCount > 0)
            {
                string g;
                string curdhead = "Attendance Finger Print Count";
                e.Graphics.DrawString(curdhead, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Blue, 300, 50);
                string l1 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
                e.Graphics.DrawString(l1, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Red, 0, 100);
                g = "Device No.";
                e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 80, 130);
                g = "Device Name";
                e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 200, 130);
                g = "Date";
                e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 350, 130);
                g = "Time";
                e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 450, 130);
                g = "Count";
                e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 550, 130);
                string l2 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
                e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Red, 0, 150);
                int height = 165;
                double num = gridView3.Rows.Count / 39.0;
                currentPageCount = Convert.ToInt32(Math.Ceiling(num));
                for (int l = numberOfItemsPrintedSoFar; l < gridView3.Rows.Count; l++)
                {
                    numberOfItemsPerPage++;
                    if (numberOfItemsPerPage <= 39)
                    {
                        numberOfItemsPrintedSoFar++;
                        if (numberOfItemsPrintedSoFar <= gridView3.Rows.Count)
                        {
                            height += gridView3.Rows[0].Height;
                            e.Graphics.DrawString(gridView3.Rows[l].Cells[1].FormattedValue.ToString(),
                                gridView3.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(80, height,
                                gridView3.Columns[0].Width, gridView3.Rows[0].Height));
                            e.Graphics.DrawString(gridView3.Rows[l].Cells[2].FormattedValue.ToString(),
                                gridView3.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(200, height,
                                500, gridView3.Rows[0].Height));
                            e.Graphics.DrawString(gridView3.Rows[l].Cells[3].FormattedValue.ToString(),
                                gridView3.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(350, height,
                                500, gridView3.Rows[0].Height));
                            e.Graphics.DrawString(gridView3.Rows[l].Cells[4].FormattedValue.ToString(),
                                gridView3.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(450, height,
                                gridView3.Columns[0].Width, gridView3.Rows[0].Height));
                            e.Graphics.DrawString(gridView3.Rows[l].Cells[5].FormattedValue.ToString(),
                                gridView3.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(550, height,
                                500, gridView3.Rows[0].Height));
                        }
                        else
                        {
                            e.HasMorePages = false;
                        }
                    }
                    else
                    {
                        e.Graphics.DrawString(currentPageNo.ToString() + " of " + currentPageCount.ToString(),
                        gridView3.Font = new Font("Book Antiqua", 8),
                        Brushes.Black, new RectangleF(430, 1050,
                        500, gridView3.Rows[0].Height));
                        currentPageNo++;
                        numberOfItemsPerPage = 0;
                        e.HasMorePages = true;
                        return;
                    }
                }
                e.Graphics.DrawString(currentPageNo.ToString() + " of " + currentPageCount.ToString(),
                gridView3.Font = new Font("Book Antiqua", 8),
                Brushes.Black, new RectangleF(430, 1050,
                500, gridView3.Rows[0].Height));
                numberOfItemsPerPage = 0;
                numberOfItemsPrintedSoFar = 0;
                currentPageNo = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }
    }
}
