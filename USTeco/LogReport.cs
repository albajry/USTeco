using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Collections;
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace USTeco
{
    public partial class LogReport : Form
    {
        OleDbConnection accCon = new OleDbConnection();
        OracleConnection oraCon = new OracleConnection();
        SqlConnection sqlCon = new SqlConnection();
        OracleDataAdapter oraAdapter = new OracleDataAdapter();
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        OleDbDataAdapter accAdapter = new OleDbDataAdapter();
        
        string my_querry, mySql;
        int numberOfItemsPrintedSoFar = 0;
        int numberOfItemsPerPage = 0;
        int currentPageNo = 1;
        int currentPageCount = 0;
        string myDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UST Applications", "Handpunch");
        public LogReport()
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            InitializeComponent();
            accCon.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + myDataPath + "\\handdb.mdb; Jet OLEDB:Database Password = Ajoset25";
            accCon.Open();
            BackColor = MyGlobal.color1;
            panel2.BackColor = MyGlobal.color2;
            int MyPlace;
            string PlaceName;
            mySql = "Select Place_id,Place_Name From Devices";
            comboBox1.Items.Add("By Employee No");
            comboBox1.Items.Add("All Places");
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
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
                comboBox1.Items.Add(MyPlace.ToString() + ". " + PlaceName);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void logRepButt_Click(object sender, EventArgs e)
        {

            string cond = "1=1";
            OracleCommand oraCmd;
            OracleDataReader oraReader;
            OleDbCommand accCmd;
            OleDbDataReader accReader;
            ArrayList row = new ArrayList();
            gridView2.Rows.Clear();
            gridView2.Update();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    cond = "Emp_Id = " + textBox2.Text;
                    break;
                case 1:
                    cond = comboBox2.Text + " " + comboBox3.Text + " " + extCond.Text + " ";
                    label13.Text = "All Site ";
                    break;
                default:
                    cond = "Sign_Plc_Id = " + textBox2.Text;
                    label13.Text = comboBox1.Text.Split('.')[1];
                    break;
            }
            if (!(textBox2.Text.Equals("")))
            {
                if (CheckDatabase())
                {
                    switch (MyGlobal.ServerType)
                    {
                        case 0:
                            Cursor.Current = Cursors.WaitCursor;
                            string myDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                                my_querry = "Select Emp_Id,Sign_Date," +
                                "TO_CHAR(Sign_Time,'hh24:mi:ss') Sign_Time,Sign_Plc_Id,Time_Stamp " +
                                "From EMS_EMP_FINGERS Where " + cond + " And Sign_Date >= TO_DATE('" +
                                    attDateFrom.Value.ToString("dd/MM/yyyy") + "','dd/MM/yyyy') and Sign_Date <= TO_DATE('" +
                                    attDateTo.Value.ToString("dd/MM/yyyy") + "','dd/MM/yyyy') " +
                                    "Group by Sign_Date,Sign_Time,Sign_Plc_Id,Emp_Id,Time_Stamp " +
                                    "order by EMP_ID,Sign_Plc_Id,Sign_Date,Sign_Time";
                                oraCmd = new OracleCommand(my_querry, oraCon);
                                oraReader = oraCmd.ExecuteReader();
                            while (oraReader.Read())
                            {
                                if (comboBox1.SelectedIndex == 0)
                                {
                                    row.Clear();
                                    mySql = "Select Place_Name From Devices Where Place_Id =" + oraReader.GetInt32(3);
                                    accCmd = new OleDbCommand(mySql, accCon);
                                    accReader = accCmd.ExecuteReader();
                                    accReader.Read();
                                    row.Clear();
                                    row.Add(oraReader.GetInt32(3));
                                    try
                                    {
                                        row.Add(accReader.GetString(0));
                                    }
                                    catch
                                    {
                                        row.Add("غير معروف");
                                    }
                                    accReader.Close();
                                    mySql = "Select Emp_Name From Employee Where Emp_Id ='" + oraReader.GetInt32(0) + "'";
                                    accCmd = new OleDbCommand(mySql, accCon);
                                    accReader = accCmd.ExecuteReader();
                                    accReader.Read();
                                    try
                                    {
                                        row.Add(accReader.GetString(0));
                                    }
                                    catch
                                    {
                                        row.Add("غير معروف");
                                    }
                                    accReader.Close();
                                    row.Add(oraReader.GetDateTime(1).ToString("dd/MM/yyyy"));
                                    row.Add(oraReader.GetString(2));
                                    //row.Add(oraReader.GetInt32(3));
                                    row.Add(oraReader.GetDateTime(4).ToString("dd/MM/yyyy HH:mm:ss"));
                                    gridView2.Rows.Add(row.ToArray());
                                }
                                else
                                {
                                    mySql = "Select Emp_Name From Employee Where Emp_Id ='" + oraReader.GetInt32(0) + "'";
                                    accCmd = new OleDbCommand(mySql, accCon);
                                    accReader = accCmd.ExecuteReader();
                                    accReader.Read();
                                    row.Clear();
                                    row.Add(oraReader.GetInt32(0));
                                    try
                                    {
                                        row.Add(accReader.GetString(0));
                                    }
                                    catch
                                    {
                                        row.Add("غير معروف");
                                    }
                                    accReader.Close();
                                    mySql = "Select Place_Name From Devices Where Place_Id =" + oraReader.GetInt32(3);
                                    accCmd = new OleDbCommand(mySql, accCon);
                                    accReader = accCmd.ExecuteReader();
                                    accReader.Read();
                                    try
                                    {
                                        row.Add(accReader.GetString(0));
                                    }
                                    catch
                                    {
                                        row.Add("غير معروف");
                                    }
                                    accReader.Close();
                                    row.Add(oraReader.GetDateTime(1).ToString("dd/MM/yyyy"));
                                    row.Add(oraReader.GetString(2));
                                    row.Add(oraReader.GetDateTime(4).ToString("dd/MM/yyyy HH:mm:ss"));
                                    gridView2.Rows.Add(row.ToArray());
                                }
                            }
                            oraReader.Close();
                            Cursor.Current = Cursors.Default;
                            break;
                        case 1:
                            my_querry = "Select Sign_Seq,Emp_Id,Sign_Date,Sign_Time " +
                                        "From EMS_EMP_FINGERS Where Emp_Id ='" + textBox2.Text +
                                        "' and convert(date, Sign_Date, 103) >= CAST('" + attDateFrom.Text +
                                        "' as date) and convert(date, Sign_Date, 103) <= CAST('" + attDateTo.Text +
                                        "' as date) order by convert(date, Sign_Date, 103)";
                            //sqlAdapter = new SqlDataAdapter(my_querry, sqlCon);
                            //table = new DataTable();
                            //sqlAdapter.Fill(table);
                            //gridView2.DataSource = table;
                            break;
                        case 2:
                            my_querry = "Select Sign_Seq,Emp_Id,Sign_Date,Sign_Time " +
                                "From EMS_EMP_FINGERS Where Emp_Id='" + textBox2.Text + "'";
                            //"' AND DateValue(Sign_Date) >= DateValue('" + attDateFrom.Text + "')";
                            // "' and DateValue(Sign_Date) Between DateValue('" + attDateFrom.Text + 
                            // "') AND DateValue('"+addDateTo.Text+"')";
                            // "' and Sign_Date <='" + + "'";
                            //accAdapter = new OleDbDataAdapter(my_querry, accCon);
                            //table = new DataTable();
                            //accAdapter.Fill(table);
                            //gridView2.DataSource = table;
                            break;
                    }

                }
            }
            else
            {
                MessageBox.Show("Please enter " + comboBox1.Text + " number!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            gridView2.Focus();
            label1.Text = gridView2.RowCount.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Enabled = (comboBox1.SelectedIndex == 0);
            textBox2.Enabled = (comboBox1.SelectedIndex == 0);
            extCond.Enabled  = (comboBox1.SelectedIndex == 1);
            comboBox2.Enabled = (comboBox1.SelectedIndex == 1);
            comboBox3.Enabled = (comboBox1.SelectedIndex == 1);
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            extCond.Text = "";
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    gridView2.Rows.Clear();
                    textBox2.Text = "";
                    textBox2.Focus();
                    gridView2.Columns[0].HeaderText = "P_Id";
                    gridView2.Columns[1].HeaderText = "Place Name";
                    gridView2.Columns[2].HeaderText = "Employee Name";
                    label13.Text = "";
                    break;
                case 1:
                    textBox2.Text = "*";
                    label13.Text = "All Places";
                    gridView2.Columns[0].HeaderText = "E_ID";
                    gridView2.Columns[1].HeaderText = "Employee Name";
                    gridView2.Columns[2].HeaderText = "Place Name";
                    break;
                default:
                    gridView2.Rows.Clear();
                    textBox2.Text = comboBox1.Text.Split('.')[0];
                    label13.Text = comboBox1.Text.Split('.')[1];
                    gridView2.Columns[0].HeaderText = "E_ID";
                    gridView2.Columns[1].HeaderText = "Employee Name";
                    gridView2.Columns[2].HeaderText = "Place Name";
                    break;
            }
        }

        private void printList_Click(object sender, EventArgs e)
        {
            //printPreviewDialog1.ShowDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileName = new FileInfo(saveFileDialog1.FileName);
                using (var package = new ExcelPackage())
                {
                    using (var worksheet = package.Workbook.Worksheets.Add("Attendance"))
                    {
                        worksheet.Cells[1, 1].Value = label13.Text +
                                " Employee Logs";
                        worksheet.Cells[2, 1].Value = "From " + attDateFrom.Text + " To " + attDateTo.Text;
                        worksheet.Cells["A1:E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:E1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells["A1:E1"].Merge = true;
                        worksheet.Cells["A2:E2"].Merge = true;
                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.Font.Size = 16;
                        worksheet.Cells[2, 1].Style.WrapText = true;
                        worksheet.Cells[2, 1].Style.Font.Size = 16;
                        worksheet.Cells[1, 1, 2, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1, 2, 5].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        worksheet.Cells[4,1,4,5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[4, 1, 4, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Column(2).Width = 22;
                        worksheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; 
                        worksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; 
                        worksheet.Column(3).Width = 22;
                        worksheet.Column(4).Width = 12;
                        for (int j = 0; j < gridView2.Columns.Count; j++)
                        {
                            worksheet.Cells[4, j + 1].Value = gridView2.Columns[j].HeaderText;
                        }
                        for (int i = 3; i < gridView2.Rows.Count + 3; i++)
                        {
                            for (int j = 0; j < gridView2.Columns.Count; j++)
                            {
                                worksheet.Cells[i + 2, j + 1].Value = gridView2.Rows[i-3].Cells[j].Value?.ToString() ?? "";
                            }
                        }
                        try
                        {
                            package.SaveAs(fileName);
                        }
                        catch
                        {
                            MessageBox.Show("Couldn't save file. May be it is already open", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            string MyName = "";
            if (gridView2.RowCount > 0)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        mySql = "Select Emp_Name From Employee Where Emp_Id='" + textBox2.Text + "'";
                        cmd = new OleDbCommand(mySql, accCon);
                        reader = cmd.ExecuteReader();
                        reader.Read();
                        MyName = reader.GetString(0);
                        reader.Close();
                        break;
                    case 1:
                        mySql = "Select Place_Name From Devices Where Place_Id=" + textBox2.Text;
                        cmd = new OleDbCommand(mySql, accCon);
                        reader = cmd.ExecuteReader();
                        reader.Read();
                        MyName = reader.GetString(0);
                        reader.Close();
                        break;
                    case 2:
                        MyName = "All Position";
                        break;
                }
                string g;
                string curdhead = "Attendance Finger Print for: " + MyName;
                e.Graphics.DrawString(curdhead, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Blue, 300, 50);
                string l1 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
                e.Graphics.DrawString(l1, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Red, 0, 100);
                g = "Place Name";
                e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 80, 130);
                g = "Emp No.";
                e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 280, 130);
                g = "Date";
                e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 350, 130);
                g = "Time";
                e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 450, 130);
                string l2 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
                e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Red, 0, 150);
                int height = 165;
                double num = gridView2.Rows.Count / 39.0;
                currentPageCount = Convert.ToInt32(Math.Ceiling(num));
                for (int l = numberOfItemsPrintedSoFar; l < gridView2.Rows.Count; l++)
                {
                    numberOfItemsPerPage++;
                    if (numberOfItemsPerPage <= 39)
                    {
                        numberOfItemsPrintedSoFar++;
                        if (numberOfItemsPrintedSoFar <= gridView2.Rows.Count)
                        {
                            height += gridView2.Rows[0].Height;
                            e.Graphics.DrawString(gridView2.Rows[l].Cells[0].FormattedValue.ToString(),
                                gridView2.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(80, height,
                                gridView2.Columns[0].Width, gridView2.Rows[0].Height));
                            e.Graphics.DrawString(gridView2.Rows[l].Cells[1].FormattedValue.ToString(),
                                gridView2.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(280, height,
                                gridView2.Columns[0].Width, gridView2.Rows[0].Height));
                            e.Graphics.DrawString(gridView2.Rows[l].Cells[2].FormattedValue.ToString(),
                                gridView2.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(350, height,
                                500, gridView2.Rows[0].Height));
                            e.Graphics.DrawString(gridView2.Rows[l].Cells[3].FormattedValue.ToString(),
                                gridView2.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(450, height,
                                500, gridView2.Rows[0].Height));
                        }
                        else
                        {
                            e.HasMorePages = false;
                        }
                    }
                    else
                    {
                        e.Graphics.DrawString(currentPageNo.ToString() + " of " + currentPageCount.ToString(),
                        gridView2.Font = new Font("Book Antiqua", 8),
                        Brushes.Black, new RectangleF(430, 1050,
                        500, gridView2.Rows[0].Height));
                        currentPageNo++;
                        numberOfItemsPerPage = 0;
                        e.HasMorePages = true;
                        return;
                    }
                }
                e.Graphics.DrawString(currentPageNo.ToString() + " of " + currentPageCount.ToString(),
                gridView2.Font = new Font("Book Antiqua", 8),
                Brushes.Black, new RectangleF(430, 1050,
                500, gridView2.Rows[0].Height));
                numberOfItemsPerPage = 0;
                numberOfItemsPrintedSoFar = 0;
                currentPageNo = 0;
            }
        }

        private void gridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            if (!string.IsNullOrEmpty(gridView2.Rows[e.RowIndex].Cells[1].Value.ToString()))
            {
                mySql = "Select Emp_Name From Employee Where Emp_id='" +
                  gridView2.Rows[e.RowIndex].Cells[1].Value.ToString() + "'";
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Read();
                try
                {
                    label13.Text = reader.GetString(0);
                }
                catch {
                    //label13.Text = "";
                }
                reader.Close();
            }
            else
            {
                label13.Text = "";
            }
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            label1.Text = "";
            label13.Text = "";
            OleDbCommand accCmd;
            OleDbDataReader accReader;
            mySql = "Select Emp_Name From Employee Where Emp_id = '" + textBox2.Text + "'";
            accCmd = new OleDbCommand(mySql, accCon);
            accReader = accCmd.ExecuteReader();
            if (accReader.Read())
            {
                label13.Text = accReader.GetString(0);
            }
            accReader.Close();
        }

        private void gridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = gridView1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            gridView1.Visible = false;
        }

        private void gridView1_Leave(object sender, EventArgs e)
        {
            gridView1.Visible = false;
        }

        private void gridView1_Enter(object sender, EventArgs e)
        {
            searchBtn_Click(sender, e);
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            OracleCommand oraCmd;
            OracleDataReader oraReader;
            string[] row = new string[2];
            if (CheckDatabase())
            {
                if (!String.IsNullOrEmpty(textBox1.Text))
                {
                    mySql = "Select * From USTHR.EMS_EMP_TEMP Where EMP_Name like '%" + textBox1.Text + "%'";
                    oraCmd = new OracleCommand(mySql, oraCon);
                    oraReader = oraCmd.ExecuteReader();
                    gridView1.Rows.Clear();
                    while (oraReader.Read())
                    {
                        row[0] = oraReader.GetInt32(0).ToString();
                        row[1] = oraReader.GetString(1);
                        gridView1.Rows.Add(row);
                    }
                    if (gridView1.RowCount > 0)
                    {
                        gridView1.Visible = true;
                        gridView1.Focus();
                    }
                    oraReader.Close();
                }
                else
                {
                    MessageBox.Show("Database not connected", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Enter Employee Name", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
            }
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
    }
}
