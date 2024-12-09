using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using System.Diagnostics.Eventing.Reader;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math; 

namespace USTeco
{
    public partial class AttendReport : Form
    {
        OleDbConnection accCon = new OleDbConnection();
        OracleConnection oraCon = new OracleConnection();
        SqlConnection sqlCon = new SqlConnection();
        OracleDataAdapter oraAdapter = new OracleDataAdapter();
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        OleDbDataAdapter accAdapter = new OleDbDataAdapter();
        int numberOfItemsPrintedSoFar = 0;
        int numberOfItemsPerPage = 0;
        int currentPageNo = 1;
        int currentPageCount = 0;
        string myDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UST Applications", "Handpunch");
        public AttendReport()
        {
            InitializeComponent();
            accCon.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + myDataPath + "\\handdb.mdb; Jet OLEDB:Database Password = Ajoset25";
            accCon.Open();
            BackColor = MyGlobal.color1;
            panel2.BackColor = MyGlobal.color2;
        }

        private void repButtShow_Click(object sender, EventArgs e)
        {
            string mySql;
            OleDbCommand cmd;
            OleDbDataReader reader;
            //List<string> startTime = new List<string>();
            //List<string> endTime = new List<string>();
            string[] startTime = new string[7];
            string[] endTime = new string[7];
            bool[] workDay = new bool[7];
            string[] arabicDay = new string[] { "الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت" };
            string myDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            if (!string.IsNullOrEmpty(repEmpNo.Text))
            {
                if (CheckDatabase())
                {
                    mySql = "Select Emp_Name From Employee Where Emp_id = '" + repEmpNo.Text + "'";
                    cmd = new OleDbCommand(mySql, accCon);
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        label33.Text = reader.GetString(0);
                    }
                    reader.Close();
                    mySql = "Select * From ShiftsTimes,Employee Where ShiftId=Emp_Shift1 " +
                        "And Emp_Id='" + repEmpNo.Text + "'";

                    cmd = new OleDbCommand(mySql, accCon);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        for (int i = 0; i < 7; i++)
                        {
                            int index = i * 3 + 2;
                            workDay[i] = reader.GetBoolean(index);
                            startTime[i] = reader.GetString(index + 1);
                            endTime[i] = reader.GetString(index + 2);

                        }
                    }
                    switch (MyGlobal.ServerType)
                    {
                        case 0:
                            mySql =
                                "WITH MAR1 AS " + "(SELECT To_Date('"+repDateFrom.Value.ToString("dd/MM/yyyy")+
                                "','dd/mm/yyyy')+ LEVEL - 1 AS SignDate " +
                                "FROM DUAL CONNECT BY LEVEL <= (To_Date('" + repDateTo.Value.ToString("dd/MM/yyyy") +
                                "','dd/mm/yyyy') - To_Date('" + repDateFrom.Value.ToString("dd/MM/yyyy") +
                                "','dd/mm/yyyy') + 1)) " +
                                "SELECT MAR1.SignDate, TO_CHAR(MAR1.SignDate, 'Day' ) as dayName," +
                                    "CASE When TO_NUMBER(To_Char(MAR1.SignDate, 'D' )) = 6 " +
                                        "THEN '#' " +
                                        "ELSE " +
                                            "CASE WHEN TO_NUMBER(TO_CHAR(MIN(SIGN_TIME), 'HH24')) < 12 " +
                                                 "THEN TO_CHAR(MIN(SIGN_TIME),'HH:MI AM') " +
                                                 "ELSE 'غـ' " +
                                            "END "+
                                    "END AS Check_In," +
                                    "CASE When TO_NUMBER(To_Char(MAR1.SignDate, 'D' )) = 6 " +
                                        "THEN '#' " +
                                        "ELSE " +
                                            "CASE WHEN TO_NUMBER(TO_CHAR(MAX(SIGN_TIME), 'HH24')) >= 12 " +
                                                 "THEN TO_CHAR(MAX(SIGN_TIME),'HH:MI AM') " +
                                                 "ELSE 'غـ' " +
                                            "END " +
                                    "END AS Check_out, " +

                                    //"CASE When TO_NUMBER(To_Char(MAR1.SignDate, 'D' )) = 6 " +
                                    //    "THEN 0 " +
                                    //    "ELSE " +
                                    //        "CASE WHEN TO_NUMBER(TO_CHAR(MIN(SIGN_TIME), 'HH24')) < 12 " +
                                    //             "THEN " +
                                    //                "Case When To_Date(TO_char(Min(SIGN_TIME),'hh24:mi'),'hh24:mi') <  To_Date('08:00','hh24:mi') " +
                                    //                    //"Case When To_Char(MAR1.SignDate, 'D' ) = 1 " +
                                    //                    //    "Then To_Date('" + startTime[2] + "','hh24:mi') " +
                                    //                    //    " Else Case When To_Char(MAR1.SignDate, 'D' ) = 2 " +
                                    //                    //            "Then To_Date('" + startTime[3] + "','hh24:mi') " +
                                    //                    //            " Else Case When To_Char(MAR1.SignDate, 'D' ) = 3 " +
                                    //                    //                    "Then To_Date('" + startTime[4] + "','hh24:mi') " +
                                    //                    //                    " Else Case When To_Char(MAR1.SignDate, 'D' ) = 4 " +
                                    //                    //                            "Then To_Date('" + startTime[5] + "','hh24:mi') " +
                                    //                    //                            " Else Case When To_Char(MAR1.SignDate, 'D' ) = 6 " +
                                    //                    //                                    "Then To_Date('" + startTime[0] + "','hh24:mi') " +
                                    //                    //                                    " Else To_Date('" + startTime[1] + "','hh24:mi') " +
                                    //                    //                                    " End " +
                                    //                    //                            "End " +
                                    //                    //                    "End " +
                                    //                    //            "End " +
                                    //                    //    "End " +

                                    //                    //-------------------------28800) - 28800)/60) "

                                    //                    "Then 0 " +
                                    //                    "Else TRUNC((nvl(to_number(TO_char(Min(SIGN_TIME),'SSSSS')),28800)-28800)/60) " +
                                    //                //"Case When To_Char(MAR1.SignDate, 'D' ) = 1 " +
                                    //                //    "Then EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[2] + "', 'HH24:MI'))*3600) - " + 
                                    //                //        "EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[2] + "', 'HH24:MI'))*3600)/60)" +
                                    //                //    //" Else Case When To_Char(MAR1.SignDate, 'D' ) = 2 " +
                                    //                //    //    "Then EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[3] + "', 'HH24:MI'))*60*60 - " +
                                    //                //    //        "EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[3] + "', 'HH24:MI'))*60*60)/60))" +
                                    //                //    //    " Else Case When To_Char(MAR1.SignDate, 'D' ) = 3 " +
                                    //                //    //        "Then EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[4] + "', 'HH24:MI'))*60*60 - " +
                                    //                //    //            "EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[4] + "', 'HH24:MI'))*60*60)/60))" +
                                    //                //    //        " Else Case When To_Char(MAR1.SignDate, 'D' ) = 4 " +
                                    //                //    //            "Then EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[5] + "', 'HH24:MI'))*60*60 - " +
                                    //                //    //                "EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[5] + "', 'HH24:MI'))*60*60)/60))" +
                                    //                //    //            " Else Case When To_Char(MAR1.SignDate, 'D' ) = 6 " +
                                    //                //    //                "Then EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[0] + "', 'HH24:MI'))*60*60 - " +
                                    //                //    //                    "EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[0] + "', 'HH24:MI'))*60*60)/60))" +
                                    //                //    //                " Else EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[1] + "', 'HH24:MI'))*60*60 - " +
                                    //                //    //                    "EXTRACT(HOUR FROM TO_TIMESTAMP('" + startTime[1] + "', 'HH24:MI'))*60*60)/60))" +
                                    //                //    //                "End " +
                                    //                //    //            "End " +
                                    //                //    //        "End " +
                                    //                //    //    "End " +
                                    //                //    "End " +
                                    //                //"End " +
                                    //                "End " +
                                    //             "ELSE 30 " + //210
                                    //        "END " +
                                    //"END + " +
                                    //"CASE When TO_NUMBER(To_Char(MAR1.SignDate, 'D' )) = 6 " +
                                    //    "THEN 0 " +
                                    //    "ELSE " +
                                    //        "CASE WHEN TO_NUMBER(TO_CHAR(MAX(SIGN_TIME), 'HH24')) >= 12" +
                                    //             "THEN " +
                                    //                "Case When To_Char(MAR1.SignDate, 'D' ) = 5 " +
                                    //                    "THEN " +
                                    //                        "Case When to_Date(TO_char(Max(SIGN_TIME),'hh24:mi'),'hh24:mi') >= to_Date('13:00','hh24:mi') " +
                                    //                            "Then 0 " +
                                    //                            "Else TRUNC(TO_NUMBER( 46800 - nvl(to_number(TO_char(Max(SIGN_TIME),'SSSSS')),46800))/60 ) " +
                                    //                        "End " +
                                    //                    "ELSE " +
                                    //                        "Case When to_Date(TO_char(Max(SIGN_TIME),'hh24:mi'),'hh24:mi') >= to_Date('15:00','hh24:mi') " +
                                    //                            "Then 0 " +
                                    //                            "Else TRUNC(TO_NUMBER( 56000 - nvl(to_number(TO_char(Max(SIGN_TIME),'SSSSS')),56000))/60 ) " +
                                    //                        "End " +
                                    //                "END " +
                                    //             "ELSE 30 " +  //210
                                    //        "END " +
                                    //"END AS dayNo " +
                                    "TO_NUMBER(To_Char(MAR1.SignDate, 'D')) AS dayNo "+
                                "FROM MAR1 LEFT JOIN EMS_EMP_FINGERS ON MAR1.SignDate = SIGN_DATE " +
                                    "AND EMP_ID = " + repEmpNo.Text +
                                " GROUP BY MAR1.SignDate" +
                                " ORDER BY MAR1.SignDate";

                            oraAdapter = new OracleDataAdapter(mySql, oraCon);
                            DataTable table = new DataTable();
                            oraAdapter.Fill(table);
                            gridView4.DataSource = table;
                            //label39.Text = Int32.Parse(table.Compute("SUM(dayNo)", string.Empty).ToString()).ToString("###,###");
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Enter Employee Number", "Alert",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                repEmpNo.Focus();
            }
        }

        private bool CheckDatabase()
        {
            oraCon.Close();
            sqlCon.Close();
            bool success = false;
            string mySql;
            switch (MyGlobal.ServerType)
            {
                case 0:
                    //string MyData = "(DESCRIPTION = " +
                    //                "(ADDRESS = " +
                    //                    "(PROTOCOL = TCP) " +
                    //                    "(HOST = " + MyGlobal.MyServer + ") " +
                    //                    "(PORT = " + MyGlobal.MyPort.ToString() + ") " +
                    //                ") " +
                    //                "(CONNECT_DATA = " +
                    //                    "(SERVER = dedicated) " +
                    //                    "(SERVICE_NAME = " + MyGlobal.MySid + ") " +
                    //                ") " +
                    //                ")";
                    string Source = MyGlobal.MyServer+":"+ MyGlobal.MyPort.ToString()+"/"+MyGlobal.MySid;
                    oraCon.ConnectionString = @"DATA SOURCE=" + Source + ";USER ID=" + MyGlobal.MyUser + ";PASSWORD=" + MyGlobal.MyPass;
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

        private void repButtPrn_Click(object sender, EventArgs e)
        {
            if (gridView4.RowCount > 0)
            {
                printPreviewDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Enter Employee Number", "Alert",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                repEmpNo.Focus();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            string MyName = "";
            string mySql = "Select Emp_Name From Employee Where Emp_Id='" + repEmpNo.Text + "'";
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            MyName = reader.GetString(0);
            reader.Close();
            string g;
            string curdhead = "Attendance Finger Print for: " + MyName;
            e.Graphics.DrawString(curdhead, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Blue, 300, 50);
            string l1 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l1, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Red, 0, 100);
            g = "Day Name";
            e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 80, 130);
            g = "Date";
            e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 200, 130);
            g = "Check In";
            e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 300, 130);
            g = "Check Out";
            e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 400, 130);
            string l2 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Red, 0, 150);
            int height = 165;
            double num = gridView4.Rows.Count / 39.0;
            currentPageCount = Convert.ToInt32(Math.Ceiling(num));
            for (int l = numberOfItemsPrintedSoFar; l < gridView4.Rows.Count; l++)
            {
                numberOfItemsPerPage++;
                if (numberOfItemsPerPage <= 39)
                {
                    numberOfItemsPrintedSoFar++;
                    if (numberOfItemsPrintedSoFar <= gridView4.Rows.Count)
                    {
                        height += gridView4.Rows[0].Height;
                        e.Graphics.DrawString(gridView4.Rows[l].Cells[1].FormattedValue.ToString(),
                            gridView4.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(80, height,
                            gridView4.Columns[0].Width, gridView4.Rows[0].Height));
                        e.Graphics.DrawString(gridView4.Rows[l].Cells[0].FormattedValue.ToString(),
                            gridView4.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(200, height,
                            500, gridView4.Rows[0].Height));
                        e.Graphics.DrawString(gridView4.Rows[l].Cells[2].FormattedValue.ToString(),
                            gridView4.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(300, height,
                            500, gridView4.Rows[0].Height));
                        e.Graphics.DrawString(gridView4.Rows[l].Cells[3].FormattedValue.ToString(),
                            gridView4.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(400, height,
                            500, gridView4.Rows[0].Height));
                    }
                    else
                    {
                        e.HasMorePages = false;
                    }
                }
                else
                {
                    e.Graphics.DrawString(currentPageNo.ToString() + " of " + currentPageCount.ToString(),
                    gridView4.Font = new Font("Book Antiqua", 8),
                    Brushes.Black, new RectangleF(430, 1050,
                    500, gridView4.Rows[0].Height));
                    currentPageNo++;
                    numberOfItemsPerPage = 0;
                    e.HasMorePages = true;
                    return;
                }
            }
            e.Graphics.DrawString(currentPageNo.ToString() + " of " + currentPageCount.ToString(),
            gridView4.Font = new Font("Book Antiqua", 8),
            Brushes.Black, new RectangleF(430, 1050,
            500, gridView4.Rows[0].Height));
            numberOfItemsPerPage = 0;
            numberOfItemsPrintedSoFar = 0;
            currentPageNo = 0;
        }

        private void repButtShow_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(repButtShow, "Show Result");
        }

        private void addExitForm_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(repButtLeave, "Leave Form");
        }

        private void repButtPrn_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(repButtPrn, "Print Result");
        }

        private void repButtLeave_Click(object sender, EventArgs e)
        {
            bool okToPrint = false;
            string[] arabicDay = new string[] { "الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت" };
            if (gridView4.RowCount > 0){
                using (FileStream fs = new FileStream(@"D:\Checkout\Leaving_"+repEmpNo.Text+".html", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        string myText =
                            "<HTML>" +
                                "<head>" +
                                    "<title > نموذج مغادرة </title>" +
                                    "<meta charset = 'utf-8' >" +
                                    "<meta name = 'viewport' content = 'width=device-width, initial-scale=1.0'>" +
                                    "<link rel = 'stylesheet' href = 'D:\\Checkout\\src\\css\\bootstrap.min.css' >" +
                                    "<script src = 'D:\\Checkout\\src\\js\\jquery.min.js' ></script>" +
                                    "<script src = 'D:\\Checkout\\src\\js\\bootstrap.min.js' ></script>" +
                                    "<link rel = 'stylesheet' href = 'D:\\Checkout\\src\\css\\MyStyle.css?nocache=<?php echo time(); ?>'>" +
                                    "<link rel = 'stylesheet' href = 'https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css' >" +
                                    "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.6.4/jquery.min.js' ></script>" +
                                    "<script src = 'https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js' ></script>" +
                                    "<script src = 'https://code.jquery.com/jquery-3.6.0.min.js' ></script>" +
                                "</head>" +
                                "<body>" +
                                    "<div class='mainpage' dir='RTL'>";

                        for (int i = 0; i < gridView4.RowCount; i++)
                        {
                            okToPrint = false;
                            //MessageBox.Show(gridView4.Rows[i].Cells[4].Value.ToString());
                            if (gridView4.Rows[i].Cells[1].Value.ToString() != "Friday")
                            {
                                if (gridView4.Rows[i].Cells[2].FormattedValue.ToString() == "غـ" &&
                                    gridView4.Rows[i].Cells[3].FormattedValue.ToString() == "غـ")
                                {
                                    okToPrint = true;
                                    myText +=
                                        "<div class='maindiv'>" +
                                            "<img class='header-image' src = 'D:\\Checkout\\src\\header.png' >" +
                                            "<div class='row topdiv'>" +
                                                "<div class='col-sm-4'>" +
                                                    "الاسم : " + label33.Text +
                                                "</div>" +
                                                "<div class='col-sm-2'>" +
                                                    "الرقم : (" + repEmpNo.Text + ") " +
                                                "</div>" +
                                                "<div class='col-sm-3'>" +
                                                    "الوظيفة : ................... " +
                                                "</div>" +
                                                "<div class='col-sm-3'>" +
                                                    "الوحدة الإدارية : ITM" +
                                                "</div>" +
                                            "</div>" +
                                            "<div class='row' style='display:flex'>" +
                                                "<div class='col-sm-6 mytype-right'>" +
                                                    "<div class='row subhead'>" +
                                                        "<div class='col-sm-7'>" +
                                                            "<p style='text-align:left'>استئذان (من الرصيد السنوي)</p>" +
                                                        "</div>" +
                                                        "<div class='col-sm-5'>" +
                                                            "<img class='okcheck' src='D:\\Checkout\\src\\select.png'>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<p>أرجو السماح لي بالخروج من موقع العمل لمدة إجــــــازة</p>" +
                                                    "<div class='row'>" +
                                                        "<div class='col-sm-6'>" +
                                                            "<p>من الساعة :..............</p>" +
                                                        "</div>" +
                                                        "<div class='col-sm-6'>" +
                                                            "<p>حتى الساعة : ..............</p>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class='row'>" +
                                                        "<div class='col-sm-6'>" +
                                                            "ليوم :" + arabicDay[Int32.Parse(gridView4.Rows[i].Cells[4].Value.ToString()) - 1] +
                                                        "</div>" +
                                                        "<div class='col-sm-6'>" +
                                                            "الموافق : " + gridView4.Rows[i].Cells[0].FormattedValue.ToString() + "م" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class='col-sm-6 mytype-left'>" +
                                                    "<div class='row subhead'>" +
                                                        "<div class='col-sm-7'>" +
                                                            "<p style='text-align:left'>مهمة عمل</p>" +
                                                        "</div>" +
                                                        "<div class='col-sm-5'>" +
                                                             "<span class='nocheck'></span>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<p>تم تكليف المذكور بالذهاب إلى ...............................</p>" +
                                                    "<p>لإنجاز عمل ..................................................</p>" +
                                                    "<p>من الساعة .................. حتى الساعة ....................</p>" +
                                                    "<p>ليوم.........................الموافق ...../...../..............م</p>" +
                                                "</div>" +
                                            "</div>";
                                }
                                else
                                {
                                    if (gridView4.Rows[i].Cells[2].FormattedValue.ToString() == "غـ")
                                    {
                                        okToPrint = true;
                                        myText +=
                                            "<div class='maindiv'>" +
                                                "<img class='header-image' src = 'D:\\Checkout\\src\\header.png' >" +
                                                "<div class='row topdiv'>" +
                                                    "<div class='col-sm-4'>" +
                                                        "  الاسم : " + label33.Text +
                                                    "</div>" +
                                                    "<div class='col-sm-2'>" +
                                                        "الرقم : (" + repEmpNo.Text + ") " +
                                                    "</div>" +
                                                    "<div class='col-sm-3'>" +
                                                        "الوظيفة : ................... " +
                                                    "</div>" +
                                                    "<div class='col-sm-3'>" +
                                                        "الوحدة الإدارية : ITM" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class='row'>" +
                                                    "<div class='col-sm-6 mytype-right'>" +
                                                        "<div class='row subhead'>" +
                                                            "<div class='col-sm-7'>" +
                                                                "<p style='text-align:left'>استئذان (من الرصيد السنوي)</p>" +
                                                            "</div>" +
                                                            "<div class='col-sm-5'>" +
                                                                "<img class='okcheck' src='D:\\Checkout\\src\\select.png'>" +
                                                            "</div>" +
                                                        "</div>" +
                                                        "<p>أرجو السماح لي بالخروج من موقع العمل لمدة ..................</p>" +
                                                        "<div class='row'>" +
                                                            "<div class='col-sm-6'>" +
                                                                "<p>من الساعة : 8:00ص</p>" +
                                                            "</div>" +
                                                            "<div class='col-sm-6'>" +
                                                                "<p>حتى الساعة : ..............</p>" +
                                                            "</div>" +
                                                        "</div>" +
                                                        "<div class='row'>" +
                                                            "<div class='col-sm-4'>" +
                                                                "ليوم :" + arabicDay[Int32.Parse(gridView4.Rows[i].Cells[4].Value.ToString())-1] +
                                                            "</div>" +
                                                            "<div class='col-sm-8'>" +
                                                                "الموافق : " + gridView4.Rows[i].Cells[0].FormattedValue.ToString() + "م" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class='col-sm-6 mytype-left'>" +
                                                        "<div class='row subhead'>" +
                                                            "<div class='col-sm-7'>" +
                                                                "<p style='text-align:left'>مهمة عمل</p>" +
                                                            "</div>" +
                                                            "<div class='col-sm-5'>" +
                                                                 "<span class='nocheck'></span>" +
                                                            "</div>" +
                                                        "</div>" +
                                                        "<p>تم تكليف المذكور بالذهاب إلى ...............................</p>" +
                                                        "<p>لإنجاز عمل ..................................................</p>" +
                                                        "<p>من الساعة .................. حتى الساعة ....................</p>" +
                                                        "<p>ليوم.........................الموافق ...../...../..............م</p>" +
                                                    "</div>" +
                                                "</div>";
                                    }

                                    else if (gridView4.Rows[i].Cells[3].Value.ToString() == "غـ")
                                    {
                                        okToPrint = true;
                                        myText +=
                                            "<div class='maindiv'>" +
                                                "<img class='header-image' src = 'D:\\Checkout\\src\\header.png'>" +
                                                "<div class='row topdiv'>" +
                                                    "<div class='col-sm-4'>" +
                                                        "الاسم : " + label33.Text +
                                                    "</div>" +
                                                    "<div class='col-sm-2'>" +
                                                        "الرقم : (" + repEmpNo.Text + ") " +
                                                    "</div>" +
                                                    "<div class='col-sm-3'>" +
                                                        "الوظيفة : .................. " +
                                                    "</div>" +
                                                    "<div class='col-sm-3'>" +
                                                        "الوحدة الإدارية : ITM" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class='row' style='display:flex'>" +
                                                    "<div class='col-sm-6 mytype-right'>" +
                                                        "<div class='row subhead'>" +
                                                            "<div class='col-sm-7'>" +
                                                                "<p style='text-align:left'>استئذان (من الرضيد السنوي)</p>" +
                                                            "</div>" +
                                                            "<div class='col-sm-5'>" +
                                                                "<img class='okcheck' src='D:\\Checkout\\src\\select.png'>" +
                                                            "</div>" +
                                                        "</div>"+
                                                        "<p>أرجو السماح لي بالخروج من موقع العمل لمدة ..................</p>" +
                                                        "<div class='row'>" +
                                                            "<div class='col-sm-6'>" +
                                                                "<p>من الساعة :..............</p>" +
                                                            "</div>" +
                                                            "<div class='col-sm-6'>" +
                                                                "<p>حتى الساعة : 3:00م</p>" +
                                                            "</div>" +
                                                        "</div>" +
                                                        "<div class='row'>" +
                                                            "<div class='col-sm-6'>" +
                                                                "ليوم :" + arabicDay[Int32.Parse(gridView4.Rows[i].Cells[4].Value.ToString())-1] +
                                                            "</div>" +
                                                            "<div class='col-sm-6'>" +
                                                                "الموافق : " + gridView4.Rows[i].Cells[0].FormattedValue.ToString() + "م" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class='col-sm-6 mytype-left'>" +
                                                        "<div class='row subhead'>" +
                                                            "<div class='col-sm-7'>" +
                                                                "<p style='text-align:left'>مهمة عمل</p>" +
                                                            "</div>" +
                                                            "<div class='col-sm-5'>" +
                                                                 "<span class='nocheck'></span>" +
                                                            "</div>" +
                                                        "</div>" +
                                                        "<p>تم تكليف المذكور بالذهاب إلى ...............................</p>" +
                                                        "<p>لإنجاز عمل ..................................................</p>" +
                                                        "<p>من الساعة .................. حتى الساعة ....................</p>" +
                                                        "<p>ليوم.........................الموافق ...../...../..............م</p>" +
                                                    "</div>" +
                                                "</div>";
                                    }
                                    else
                                    {

                                    }
                                }
                                if (okToPrint)
                                {
                                    myText +=
                                                "<img class='header-image' src = 'D:\\Checkout\\src\\footer.png'>" +
                                            "</div><br>";
                                }
                            }
                        }
                        myText +=
                                    "</div>" +
                                "</body>" +
                            "</HTML>";
                        if (okToPrint)
                        {
                            w.WriteLine(myText);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Enter Employee Number", "Alert",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                repEmpNo.Focus();
            }
            
        }
    }
}
