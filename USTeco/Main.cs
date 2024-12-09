using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Oracle.ManagedDataAccess.Client;
using RecogSys.RdrAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Web.UI.WebControls;
using System.Windows.Forms;
using zkemkeeper;
using MySql.Data.MySqlClient;



namespace USTeco
{
    public partial class Main : Form
    {
        CRsiComWinsock myIP = new CRsiComWinsock();
        CZKEM zkRdr = new CZKEM();
        List<CZKEM> zkSRdr = new List<CZKEM> {};
        OleDbConnection accCon = new OleDbConnection();
        OracleConnection oraCon = new OracleConnection();
        SqlConnection sqlCon = new SqlConnection();
        OracleDataAdapter oraAdapter = new OracleDataAdapter();
        MySqlConnection connection = new MySqlConnection();
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        OleDbDataAdapter accAdapter = new OleDbDataAdapter();
        OleDbCommand gCmd;
        List<int> FailedDevice = new List<int>();
        CRsiHandReader myRdr;
        CRsiNetwork myNetwork;
        RSI_STATUS pStatus = new RSI_STATUS();
        RSI_READER_INFO pReaderInfo = new RSI_READER_INFO();
        RSI_SETUP_DATA pSetup = new RSI_SETUP_DATA();
        RSI_EXT_SETUP_DATA xSetup = new RSI_EXT_SETUP_DATA();
        RSI_TIME_DATE pTimeDate = new RSI_TIME_DATE();
        RSI_TEMPLATE template = new RSI_TEMPLATE();
        RSI_STATUS response = new RSI_STATUS();
        string currIpAddr, currDevType, currPass, PlaceName,
            mySql, my_querry, ConnPlace;
        int currDevId, placeId, currPort, MyPlace;
        List<int> placeStateId = new List<int> {};
        bool Auto_Clear = false;
        bool ReadingMode = false;
        const int TRUE = 1;
        const int FALSE = 0;
        string[,] ZkError = new string[11, 2];
        Thread myThread,svThread;
        bool allDeviceSelected = true;
        bool IsReadBtn, IsDataConnect = false;
        bool ThreadInProgress = false;
        int recordCount;
        ArrayList row = new ArrayList();
        int senderType;
        bool ReadyToRead = false;
        string myDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UST Applications", "Handpunch");

        static readonly HttpClient client = new HttpClient();

        public Main()
        {
            InitializeComponent();
            
            accCon.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + myDataPath + "\\handdb.mdb; Jet OLEDB:Database Password = Ajoset25";
            accCon.Open();
            CheckEveryThing();
            LoadZkErrorCode();
            this.Strip1.ImageScalingSize = new Size(35, 35);
            CustomToolStripRenderer r = new CustomToolStripRenderer();
            r.RoundedEdges = false;
            Strip1.Renderer = r;
            foreach (ToolStripItem item in Strip1.Items)
            {
                item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            }
            ComboBox1.SelectedIndex = 0;
            allDeviceSelected = true;
            //CreateEvent();
            OleDbCommand cmd;
            OleDbDataReader reader;
            mySql = "Select mysql_server,mysql_database,mysql_uid,mysql_password,apiLink " +
                    "from Settings";
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                try
                {
                    string server = reader.GetString(0);
                    string database = reader.GetString(1);
                    string uid = reader.GetString(2);
                    string password = reader.GetString(3);
                    string apiLink = reader.GetString(4);
                    string connectionString = "SERVER=" + server + ";  DATABASE=" +
                        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                }
                catch
                {
                    MessageBox.Show("Check MySql Server settings in settings form",
                        "MySql Server Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //UsingApiButt.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Check MySql Server settings in settings form",
                    "MySql Server Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //StartButt.Enabled = false;
            }
            reader.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            string my_querry = "Select color1,color2 From Settings";
            OleDbCommand cmd = new OleDbCommand(my_querry, accCon);
            OleDbDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string col1 = reader.GetString(0);
            string col2 = reader.GetString(1);
            if (col1 == "" || col2 == "")
            {
                MyGlobal.color1 = Color.LemonChiffon;
                MyGlobal.color2 = Color.Azure;
            }
            else
            {
                MyGlobal.color1 = Color.FromArgb(Convert.ToInt32(reader.GetString(0), 16));
                MyGlobal.color2 = Color.FromArgb(Convert.ToInt32(reader.GetString(1), 16));
            }
            Change_Color();
            mySql = "Select * From Settings";
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                MyGlobal.MyServer = reader.GetString(1);
                MyGlobal.MyPort = reader.GetInt32(2);
                MyGlobal.MySid = reader.GetString(3);
                MyGlobal.MyUser = reader.GetString(4);
                MyGlobal.MyPass = reader.GetString(5);
                MyGlobal.MyPeriod = reader.GetInt32(6);

                MyGlobal.showDetails = reader.GetBoolean(10);
                MyGlobal.ServerType = reader.GetInt32(12);
                MyGlobal.sqlServer = reader.GetString(13);
                MyGlobal.sqlUser = reader.GetString(14);
                MyGlobal.mysqlServer = reader.GetString(22);
                MyGlobal.mysqlDatabase = reader.GetString(23);
                MyGlobal.mysqlUid = reader.GetString(24);
                MyGlobal.mysqlPassword = reader.GetString(25);
                MyGlobal.apiLink = reader.GetString(26);
            }
        }

        private void Change_Color()
        {
            CustomToolStripRenderer.color1 = MyGlobal.color1;
            CustomToolStripRenderer.color2 = MyGlobal.color2;
            CustomToolStripRenderer r = new CustomToolStripRenderer();
            r.RoundedEdges = false;
            Strip1.Renderer = r;
            string my_querry = "Update Settings set color1='" +
                (MyGlobal.color1.ToArgb() & 0xFFFFFFFF).ToString("X8") + "',color2='" +
                (MyGlobal.color2.ToArgb() & 0xFFFFFFFF).ToString("X8") + "'";
            OleDbCommand cmd = new OleDbCommand(my_querry, accCon);
            cmd.ExecuteNonQuery();
            progressBar1.BarColor1 = MyGlobal.color1;
            progressBar1.BarColor2 = MyGlobal.color1;
            progressBar2.BarColor1 = MyGlobal.color1;
            progressBar2.BarColor2 = MyGlobal.color1;
        }

        private void ColorButt1_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.SolidColorOnly = false;
            colorDialog1.Color = MyGlobal.color1;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                MyGlobal.color1 = colorDialog1.Color;
                Change_Color();
            }
        }

        private void ColorButt2_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = false;
            colorDialog1.SolidColorOnly = true;
            colorDialog1.Color = MyGlobal.color2;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                MyGlobal.color2 = colorDialog1.Color;
                Change_Color();
            }
        }

        private void ColorButt3_Click(object sender, EventArgs e)
        {
            MyGlobal.color1 = Color.LemonChiffon;
            MyGlobal.color2 = Color.Azure;
            Change_Color();
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            Label1.Text = "";
            Label2.Text = "";
            Label1.Width = 130;
            Label2.Width = 80;
            progressBar1.Top = this.Top + this.Height / 3 - progressBar1.Height / 2;
            progressBar2.Top = progressBar1.Top;
            progressBar1.Left = this.Left + this.Width / 3 - progressBar1.Width;
            progressBar2.Left = this.Left + this.Width / 3;

        }

        private void clearByDate_Click(object sender, EventArgs e)
        {
            if (MyGlobal.IsConnected)
            {
                string stVersion = "";
                if (zkRdr.GetFirmwareVersion(currDevId, ref stVersion))
                {
                    if (Int32.Parse(stVersion.Split(' ')[4]) >= 2019)
                    {
                        string[] myPeriod = ShowDialogDouble();
                        if (myPeriod.Length > 0)
                        {
                            DialogResult result = MessageBox.Show("This process will delete attendance log\n\n" +
                                "from " + myPeriod[0] + " to " + myPeriod[1] + ".\n\nWould you like to Continue?",
                                "Warning Message",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.Yes)
                            {
                                ArrayList row = new ArrayList();
                                if (zkRdr.DeleteAttlogBetweenTheDate(currDevId, myPeriod[0], myPeriod[1]))
                                {
                                    row.Clear();
                                    row.Add(placeId);
                                    row.Add(DateTime.Now);
                                    row.Add("Delete this period done successfully");
                                    gridView3.Rows.Add(row.ToArray());

                                }
                                else
                                {
                                    row.Clear();
                                    row.Add(placeId);
                                    row.Add(DateTime.Now);
                                    row.Add("Delete this period failed");
                                    gridView4.Rows.Insert(0, row.ToArray());
                                    gridView4.Rows[0].Selected = true;
                                    gridView4.Rows[0].Selected = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("This device not support to delete period of time",
                            "Alert Message",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Couldn't get firmware date. Try again",
                            "Alert Message",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                }
            }
            else
            {

                MessageBox.Show("No Device Selected. Place Select Device From List",
                    "Alert Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void clearAllAttLog_Click(object sender, EventArgs e)
        {
            if (!allDeviceSelected)
            {
                if (MyGlobal.IsConnected)
                {
                    DialogResult result = MessageBox.Show("This process will delete all attendance log.\n\n" +
                                    "The data cannot be retrieved after process.\n\nWould you like to Continue?",
                                    "Warning Message",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        ArrayList row = new ArrayList();
                        if (zkRdr.ClearGLog(currDevId))
                        {
                            row.Clear();
                            row.Add(placeId);
                            row.Add(DateTime.Now);
                            row.Add("Delete all attendance log done successfully");
                            gridView3.Rows.Add(row.ToArray());

                        }
                        else
                        {
                            row.Clear();
                            row.Add(placeId);
                            row.Add(DateTime.Now);
                            row.Add("Delete all attendanec log failed");
                            gridView4.Rows.Insert(0, row.ToArray());
                            gridView4.Rows[0].Selected = true;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The device you selected not connected",
                        "Alert Message",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No Device Selected. Place Select Device From List",
                        "Alert Message",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
            }
        }

        private void clearUntilDate_Click(object sender, EventArgs e)
        {
            if (MyGlobal.IsConnected)
            {
                string stVersion = "";
                if (zkRdr.GetFirmwareVersion(currDevId, ref stVersion))
                {
                    if (Int32.Parse(stVersion.Split(' ')[4]) >= 2019)
                    {
                        string[] myPeriod = ShowDialogSingle();
                        if (myPeriod.Length > 0)
                        {
                            DialogResult result = MessageBox.Show("This process will delete attendance log from beginning\n\n" +
                                "until " + myPeriod[0] + ". Would you like to Continue?",
                                "Confirm Message",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.Yes)
                            {
                                ArrayList row = new ArrayList();
                                if (zkRdr.DeleteAttlogByTime(currDevId, myPeriod[0]))
                                {
                                    row.Clear();
                                    row.Add(placeId);
                                    row.Add(DateTime.Now);
                                    row.Add("Delete this period done successfully");
                                    gridView3.Rows.Add(row.ToArray());

                                }
                                else
                                {
                                    row.Clear();
                                    row.Add(placeId);
                                    row.Add(DateTime.Now);
                                    row.Add("Delete this period failed");
                                    gridView4.Rows.Insert(0, row.ToArray());
                                    gridView4.Rows[0].Selected = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("This device not support to delete period of time",
                            "Warning Message",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Couldn't get firmware date. Try again",
                            "Warning Message",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No Device Selected. Place Select Device From List",
                    "Warning Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void CheckEveryThing()
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            try
            {
                mySql = "Select server_type from Settings";
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "ALTER TABLE Settings ADD COLUMN server_type INTEGER";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
                mySql = "ALTER TABLE Settings ADD COLUMN sql_server Text(255)";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
                mySql = "ALTER TABLE Settings ADD COLUMN sql_user Text(255)";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
                mySql = "Update Settings set server_type = 0,sql_server='',sql_user=''";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            try
            {
                mySql = "Select color1 from Settings";
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "ALTER TABLE Settings ADD COLUMN color1 Text(8),color2 Text(8)";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            try
            {
                mySql = "Select showDevice from Settings";
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "ALTER TABLE Settings ADD COLUMN showDevice YesNo DEFAULT 0, " +
                    "showRepA YesNo DEFAULT 0, showRepB YesNo DEFAULT 0, showHist YesNo DEFAULT 0";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            try
            {
                mySql = "Select RepFromLog from Settings";
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "ALTER TABLE Settings ADD COLUMN RepFromLog YesNo DEFAULT 0";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select * From LogFile";
            try
            {
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Close();
                try
                {
                    mySql = "Select devname from LogFile";
                    cmd = new OleDbCommand(mySql, accCon);
                    reader = cmd.ExecuteReader();
                    reader.Close();
                }
                catch
                {
                    mySql = "ALTER TABLE LogFile ADD COLUMN devname TEXT";
                    cmd = new OleDbCommand(mySql, accCon);
                    cmd.ExecuteNonQuery();
                }

            }
            catch
            {
                mySql = "CREATE TABLE LogFile([id] AUTOINCREMENT PRIMARY KEY, [devid] text,[devname] text, [rtime] text, [rDate] text, [rCount] text, [detail] text)";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select * From EMS_EMP_FINGERS";
            try
            {
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "CREATE TABLE EMS_EMP_FINGERS " +
                                "(EMP_ID NUMBER,SIGN_DATE DATE,SIGN_TIME DATE," +
                                "SIGN_PLC_ID NUMBER,STATUS NUMBER,DEV_LOC NUMBER," +
                                "SIGN_SEQ NUMBER,TIME_STAMP DATE)";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }

            mySql = "Select Place_Name From Devices";
            cmd = new OleDbCommand(mySql, accCon);
            try
            {
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "Alter Table Devices Add Column Place_Name Text DEFAULT ''";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select Dev_Status From Devices";
            cmd = new OleDbCommand(mySql, accCon);
            try
            {
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "Alter Table Devices Add Column Dev_Status YesNo DEFAULT 1";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select Auto_Clear From Devices";
            cmd = new OleDbCommand(mySql, accCon);
            try
            {
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "Alter Table Devices Add Column Auto_Clear YesNo DEFAULT 1";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
                mySql = "Update Devices Set Auto_Clear=1";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select * From Users";
            try
            {
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "CREATE TABLE Users([ID_KEY] AUTOINCREMENT PRIMARY KEY," +
                    "[PLACE_ID] text(5),[EMP_ID] text(10), [EMP_NAME] text(50), [EMP_AUTH] text(20)," +
                    "[EMP_STATUS] text(10))";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select * From Employee";
            try
            {
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "CREATE TABLE Employee([ID] AUTOINCREMENT PRIMARY KEY," +
                    "[EMP_ID] text(5), [EMP_NAME] text(100),[Emp_Shift1] text(100)," +
                    "[Emp_Shift2] text(100)";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select * From ShiftsTimes";
            try
            {
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteReader();
            }
            catch
            {
                mySql = "Create Table ShiftsTimes (ssn AutoIncrement CONSTRAINT PrimaryKey PRIMARY KEY," +
                    "Shiftid Integer, Day1 bit," +
                    "start1 Text(12),end1 Text(12),Day2 bit,start2 Text(12),end2 Text(12)," +
                    "Day3 bit,start3 Text(12),end3 Text(12),Day4 bit,start4 Text(12)," +
                    "end4 Text(12),Day5 bit,start5 Text(12),end5 Text(12),Day6 bit," +
                    "start6 Text(12),end6 Text(12),Day7 bit,start7 Text(12),end7 Text(12)," +
                    "only_Checkin integer,only_Checkout integer,Allow_Minutes Integer)";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select * From ShiftsNames";
            try
            {
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteReader();
            }
            catch
            {
                mySql = "Create Table ShiftsNames (ssn AutoIncrement CONSTRAINT PrimaryKey PRIMARY KEY, shiftid integer," +
                    "ShiftName Text(50))";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select readBy From LogFile";
            try
            {
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteReader();
            }
            catch
            {
                mySql = "Alter Table LogFile Add Column readBy Text(100) DEFAULT 'Unknown'";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select Place_id,Place_Name From Devices";
            ComboBox1.Items.Add("All Devices");
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
                ComboBox1.Items.Add(MyPlace.ToString() + " " + PlaceName);
            }
            ComboBox1.SelectedIndex = 0;
            allDeviceSelected = true;
            reader.Close();
            mySql = "Select * From Settings";
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                MyGlobal.MyServer = reader.GetString(1);
                MyGlobal.MyPort = reader.GetInt32(2);
                MyGlobal.MySid = reader.GetString(3);
                MyGlobal.MyUser = reader.GetString(4);
                MyGlobal.MyPass = reader.GetString(5);
                MyGlobal.MyPeriod = reader.GetInt32(6);
                MyGlobal.showLogs = reader.GetBoolean(7);
                MyGlobal.autoStart = reader.GetBoolean(8);
                MyGlobal.stopFailed = reader.GetBoolean(9);
                MyGlobal.showDetails = reader.GetBoolean(10);
                MyGlobal.ServerType = reader.GetInt32(12);
                MyGlobal.sqlServer = reader.GetString(13);
                MyGlobal.sqlUser = reader.GetString(14);
                deviceToolStrip.Enabled = reader.GetBoolean(17);
                fingerToolStrip.Enabled = reader.GetBoolean(18);
                FReportButt.Enabled = reader.GetBoolean(18);
                attendToolStrip.Enabled = reader.GetBoolean(19);
                historyTool.Enabled = reader.GetBoolean(20);
                MyGlobal.RepFromLog = reader.GetBoolean(21);
                ReadButt.Enabled = true;
                if (MyGlobal.autoStart)
                    ReadButt_Click(this, null);
            }
            else
            {
                ReadButt.Enabled = false;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            //clearText();
            Cursor.Current = Cursors.WaitCursor;
            allDeviceSelected = (ComboBox1.SelectedIndex == 0);
            ReadButt.Enabled = allDeviceSelected;
            ShowButt.Enabled = !allDeviceSelected;
            Strip1.Update();
            if (!allDeviceSelected)
            {
                progressBar2.Left = this.Left + this.Width / 3 - progressBar2.Width / 2;
                mySql = "Select * From Devices Where Place_id =" +
                    ComboBox1.Text.ToString().Split(' ')[0];
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Read();
                PlaceName = reader.GetString(7);
                currIpAddr = reader.GetString(1);
                currPort = reader.GetInt32(2);
                currDevId = reader.GetInt32(3);
                currDevType = reader.GetString(4).ToUpper();
                placeId = reader.GetInt32(6);
                currPass = reader.GetString(8).ToUpper();
                reader.Close();
                switch (currDevType.ToUpper())
                {
                    case "HANDPUNCH":
                        ConnectPunch(currIpAddr, currPort, currDevId, PlaceName);
                        if (MyGlobal.IsConnected)
                        {
                            DevButt.Image = Properties.Resources.fingerprint_on;
                            ShowButt.Enabled = false;
                            if (CheckDatabase())
                            {
                                ReadButt.Enabled = true;
                                DataButt.Image = Properties.Resources.database_on;
                                IsDataConnect = true;
                            }
                            else
                            {
                                ReadButt.Enabled = false;
                                DataButt.Image = Properties.Resources.database_off;
                                IsDataConnect = false;
                            }
                        }
                        else
                        {
                            ReadButt.Enabled = false;
                            ShowButt.Enabled = false;
                            DevButt.Image = Properties.Resources.fingerprint_off;
                        }
                        break;
                    case "ZKTECO":
                        ConnPlace = PlaceName;
                        MyGlobal.IsConnected = ZktConnect(currIpAddr, currPort, PlaceName);
                        if (MyGlobal.IsConnected)
                        {
                            ShowButt.Enabled = true;
                            DevButt.Image = Properties.Resources.fingerprint_on;
                            Strip1.Update();
                            if (CheckDatabase())
                            {
                                ReadButt.Enabled = true;
                                DataButt.Image = Properties.Resources.database_on;
                                IsDataConnect = true;
                            }
                            else
                            {
                                ReadButt.Enabled = false;
                                DataButt.Image = Properties.Resources.database_off;
                                IsDataConnect = false;
                            }
                        }
                        else
                        {
                            ReadButt.Enabled = false;
                            ShowButt.Enabled = false;
                            DevButt.Image = Properties.Resources.fingerprint_off;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                progressBar1.Left = this.Left + this.Width / 3 - progressBar1.Width;
                progressBar2.Left = this.Left + this.Width / 3;
                MyGlobal.IsConnected = false;
                DevButt.Image = Properties.Resources.fingerprint_all;
                DataButt.Image = Properties.Resources.database_all;
                Label1.Text = "";
                Label2.Text = "";
                //ReadButt.Enabled = true;
                //ShowButt.Enabled = true;
            }
            Cursor.Current = Cursors.Default;
            previewToolStrip.Enabled = ShowButt.Enabled;
            deleteToolStrip.Enabled = ShowButt.Enabled;
        }

        private bool ConnectPunch(string ipAdd, int port, int devId, string placeId)
        {
            Cursor.Current = Cursors.WaitCursor;
            myIP.SetHost(ipAdd);
            myIP.SetPortA(ushort.Parse(port.ToString()));
            myRdr = new CRsiHandReader(myIP, ((byte)devId));
            MyGlobal.IsConnected = false;
            if (myIP.Ping() == FALSE)
            {
                row.Clear();
                row.Add(placeId);
                row.Add(DateTime.Now);
                row.Add("Unable to ping device");
                gridView4.Rows.Insert(0, row.ToArray());
                gridView4.Rows[0].Selected = true;
                Cursor.Current = Cursors.Default;
                Label1.Text = "";
                Label2.Text = "";
                return false;
            }
            if (MyGlobal.showLogs)
            {
                row.Clear();
                row.Add(placeId);
                row.Add(DateTime.Now);
                row.Add("Success ping");
                gridView3.Rows.Add(row.ToArray());
                gridView3.Rows[gridView3.RowCount - 1].Selected = true;
                gridView3.Update();
            }
            myIP.Disconnect();
            myIP.ResetSocket();
            myIP.Connect();
            if (myIP.IsConnected() == FALSE)
            {
                row.Clear();
                row.Add(placeId);
                row.Add(DateTime.Now);
                row.Add("Unable to connect (" + placeId + ") " + DateTime.Now);
                gridView4.Rows.Insert(0, row.ToArray());
                gridView4.Rows[0].Selected = true;
                myIP.Disconnect();
                myIP.ResetSocket();
                Label1.Text = "";
                Label2.Text = "";
                MyGlobal.IsConnected = false;
                return false;
            }
            if (MyGlobal.showLogs)
            {
                row.Clear();
                row.Add(placeId);
                row.Add(DateTime.Now);
                row.Add("Connected successfully");
                gridView3.Rows.Add(row.ToArray());
                gridView3.FirstDisplayedScrollingRowIndex = gridView3.RowCount - 1;
                gridView3.Rows[gridView3.RowCount - 1].Selected = true;
                gridView3.Update();
            }
            myNetwork = new CRsiNetwork(myIP);
            myNetwork.Attach(myRdr);
            RSI_TIME_DATE currentDateTime = new RSI_TIME_DATE();
            if (myRdr.CmdGetTime(currentDateTime) != 0)
            {  // Gets current time from reader.      
                myRdr.CmdGetReaderInfo(pReaderInfo);
                progressBar2.Maximum = pReaderInfo.dlogsPresent;
                Label2.Text = "0/" + pReaderInfo.dlogsPresent;
                row.Clear();
                row.Add(placeId);
                row.Add(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                row.Add(myRdr.GetFamily().ToString().Split('_')[2]);
                row.Add(pReaderInfo.model.ToString().Split('_')[2]);
                Label1.Text = currentDateTime.day.ToString("00") + "/" +
                    currentDateTime.month.ToString("00") + "/" +
                    currentDateTime.year.ToString("2000") + " " +
                    currentDateTime.hour.ToString("00") + ":" +
                    currentDateTime.minute.ToString("00");
                row.Add(Label1.Text);
                row.Add("");
                row.Add(pReaderInfo.usersEnrolled.ToString());
                row.Add(pReaderInfo.dlogsPresent.ToString());
                gridView2.Rows.Add(row.ToArray());
                gridView2.FirstDisplayedScrollingRowIndex = gridView2.RowCount - 1;
                gridView2.Rows[gridView2.RowCount - 1].Selected = true;
                gridView2.Update();
                MyGlobal.IsConnected = true;
                if (MyGlobal.showLogs)
                {
                    row.Clear();
                    row.Add(placeId);
                    row.Add(DateTime.Now);
                    row.Add("Connected");
                    gridView3.Rows.Add(row.ToArray());
                    gridView3.FirstDisplayedScrollingRowIndex = gridView3.RowCount - 1;
                    gridView3.Rows[gridView3.RowCount - 1].Selected = true;
                    gridView3.Update();
                }
                return true;
            }
            else
            {
                RSI_ERROR error = new RSI_ERROR();
                error = myRdr.GetError();
                row.Clear();
                row.Add(placeId);
                row.Add(DateTime.Now);
                row.Add("Couldn't get (" + placeId + ") time. " + error.ToString());
                gridView4.Rows.Insert(0, row.ToArray());
                gridView4.Rows[0].Selected = true;
                MyGlobal.IsConnected = false;
                Label2.Text = "";
                return false;
            }
        }

        private bool ZktConnect(String ipAddress, int port, String placeId)
        {
            int dwYear = 0; int dwMonth = 0; int dwDay = 0;
            int dwHour = 0; int dwMinute = 0; int dwSecond = 0;
            string fmt = "00";
            string sProductCode = "";
            string stVersion = "";
            int statusValue = 0;
            int adminCount = 0;
            int userCount = 0;
            ArrayList row = new ArrayList();
            if (PingTheDevice(ipAddress))
            {
                if (MyGlobal.showLogs)
                {
                    row.Clear();
                    row.Add(placeId);
                    row.Add(DateTime.Now);
                    row.Add("Success ping");
                    gridView3.Rows.Add(row.ToArray());
                    gridView3.FirstDisplayedScrollingRowIndex = gridView3.RowCount - 1;
                    gridView3.Rows[gridView3.RowCount - 1].Selected = true;
                    gridView3.Update();
                }
                IPAddress ipAdd = IPAddress.Parse(ipAddress);
                DeleteEvent();
                if (string.IsNullOrEmpty(currPass.Trim()))
                {
                    currPass = "0";
                }
                zkRdr.SetCommPassword(int.Parse(currPass));
                if (zkRdr.Connect_Net(ipAdd.ToString(), port))
                {
                    MyGlobal.TryNo = 0;
                    if (zkRdr.GetDeviceTime(currDevId, ref dwYear, ref dwMonth, ref dwDay,
                            ref dwHour, ref dwMinute, ref dwSecond))
                    {
                        Label1.Text = dwYear.ToString("0000") + "/" + dwMonth.ToString(fmt) + "/" +
                            dwDay.ToString(fmt) + " " + dwHour.ToString(fmt) + ":" + dwMinute.ToString(fmt);
                    }
                    if (MyGlobal.showLogs)
                    {
                        row.Clear();
                        row.Add(placeId);
                        row.Add(DateTime.Now);
                        row.Add("Connected");
                        gridView3.Rows.Add(row.ToArray());
                        gridView3.FirstDisplayedScrollingRowIndex = gridView3.RowCount - 1;
                        gridView3.Rows[gridView3.RowCount - 1].Selected = true;
                        gridView3.Update();
                    }
                    row.Clear();
                    row.Add(placeId);
                    row.Add(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    if (zkRdr.GetPlatform(currDevId, ref stVersion))
                    {
                        row.Add(stVersion);
                    }
                    else
                    {
                        row.Add("");
                    }
                    if (zkRdr.GetProductCode(currDevId, out sProductCode))
                    {
                        row.Add(sProductCode);
                    }
                    else
                    {
                        row.Add("");
                    }
                    if (zkRdr.GetDeviceStatus(currDevId, 6, ref statusValue))
                    {
                        progressBar2.Maximum = statusValue;
                        Label2.Text = "0/" + statusValue;
                    }
                    else
                    {
                        progressBar1.Maximum = 90000;
                        Label2.Text = "0/???";
                    }
                    row.Add(Label1.Text);

                    if (zkRdr.GetDeviceStatus(currDevId, 1, ref adminCount))
                    {
                        row.Add(adminCount);
                    }
                    else
                    {
                        row.Add("");
                    }
                    if (zkRdr.GetDeviceStatus(currDevId, 2, ref userCount))
                    {
                        row.Add(userCount);
                    }
                    else
                    {
                        row.Add("");
                    }
                    if (zkRdr.GetFirmwareVersion(currDevId, ref stVersion))
                    {
                        row.Add(statusValue);
                    }
                    else
                    {
                        row.Add("");
                    }
                    gridView2.Rows.Add(row.ToArray());
                    gridView2.FirstDisplayedScrollingRowIndex = gridView2.RowCount - 1;
                    gridView2.Rows[gridView2.RowCount - 1].Selected = true;
                    gridView2.Update();
                    CreateEvent();
                    return true;
                }
                else
                {
                    int errRef = 0;
                    zkRdr.GetLastError(errRef);
                    row.Clear();
                    row.Add(placeId);
                    row.Add(DateTime.Now);
                    row.Add("Unable to connect device: " + errRef);
                    gridView4.Rows.Insert(0, row.ToArray());
                    gridView4.Rows[0].Selected = true;
                    Label1.Text = "";
                    Label2.Text = "";
                    return false;
                }
            }
            else
            {
                row.Clear();
                row.Add(placeId);
                row.Add(DateTime.Now);
                row.Add("Unable to ping device");
                gridView4.Rows.Insert(0, row.ToArray());
                gridView4.Rows[0].Selected = true;
                Cursor.Current = Cursors.Default;
                Label1.Text = "";
                Label2.Text = "";
                return false;
            }
        }

        private bool ZkStateConnect(CZKEM zkDevice, String ipAddress, int port, String placeId)
        {
            int dwYear = 0; int dwMonth = 0; int dwDay = 0;
            int dwHour = 0; int dwMinute = 0; int dwSecond = 0;
            string fmt = "00";
            string sProductCode = "";
            string stVersion = "";
            int statusValue = 0;
            int adminCount = 0;
            int userCount = 0;
            ArrayList row = new ArrayList();
            if (PingTheDevice(ipAddress))
            {
                IPAddress ipAdd = IPAddress.Parse(ipAddress);
                if (string.IsNullOrEmpty(currPass.Trim()))
                {
                    currPass = "0";
                }
                zkDevice.SetCommPassword(int.Parse(currPass));
                if (zkDevice.Connect_Net(ipAdd.ToString(), port))
                {
                    MyGlobal.TryNo = 0;
                    if (zkDevice.GetDeviceTime(currDevId, ref dwYear, ref dwMonth, ref dwDay,
                            ref dwHour, ref dwMinute, ref dwSecond))
                    {
                        Label1.Text = dwYear.ToString("0000") + "/" + dwMonth.ToString(fmt) + "/" +
                            dwDay.ToString(fmt) + " " + dwHour.ToString(fmt) + ":" + dwMinute.ToString(fmt);
                    }
                    row.Clear();
                    row.Add(placeId);
                    row.Add(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    if (zkDevice.GetPlatform(currDevId, ref stVersion))
                    {
                        row.Add(stVersion);
                    }
                    else
                    {
                        row.Add("");
                    }
                    if (zkDevice.GetProductCode(currDevId, out sProductCode))
                    {
                        row.Add(sProductCode);
                    }
                    else
                    {
                        row.Add("");
                    }
                    if (zkDevice.GetDeviceStatus(currDevId, 6, ref statusValue))
                    {
                        progressBar2.Maximum = statusValue;
                        Label2.Text = "0/" + statusValue;
                    }
                    else
                    {
                        progressBar1.Maximum = 90000;
                        Label2.Text = "0/???";
                    }
                    row.Add(Label1.Text);

                    if (zkDevice.GetDeviceStatus(currDevId, 1, ref adminCount))
                    {
                        row.Add(adminCount);
                    }
                    else
                    {
                        row.Add("");
                    }
                    if (zkDevice.GetDeviceStatus(currDevId, 2, ref userCount))
                    {
                        row.Add(userCount);
                    }
                    else
                    {
                        row.Add("");
                    }
                    if (zkDevice.GetFirmwareVersion(currDevId, ref stVersion))
                    {
                        row.Add(statusValue);
                    }
                    else
                    {
                        row.Add("");
                    }
                    gridView2.Rows.Add(row.ToArray());
                    gridView2.FirstDisplayedScrollingRowIndex = gridView2.RowCount - 1;
                    gridView2.Rows[gridView2.RowCount - 1].Selected = true;
                    gridView2.Update();
                    CreateZkEvent(zkDevice);
                    return true;
                }
                else
                {
                    int errRef = 0;
                    zkDevice.GetLastError(errRef);
                    row.Clear();
                    row.Add(placeId);
                    row.Add(DateTime.Now);
                    row.Add("Unable to connect device: " + errRef);
                    gridView4.Rows.Insert(0, row.ToArray());
                    gridView4.Rows[0].Selected = true;
                    Label1.Text = "";
                    Label2.Text = "";
                    return false;
                }
            }
            else
            {
                row.Clear();
                row.Add(placeId);
                row.Add(DateTime.Now);
                row.Add("Unable to ping device");
                gridView4.Rows.Insert(0, row.ToArray());
                gridView4.Rows[0].Selected = true;
                Cursor.Current = Cursors.Default;
                Label1.Text = "";
                Label2.Text = "";
                return false;
            }
        }

        private void StateConnect_Click(object sender, EventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            //OleDbCommand accCmd;
            MySqlCommand mySqlCmd;
            
            Cursor.Current = Cursors.WaitCursor;
            mySql = "Select * From Devices Where Dev_Type ='ZKTECO' And Dev_Status=True";
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            
            //mySql = "Select mysql_server,mysql_database,mysql_uid,mysql_password,apiLink " +
            //        "from Settings";
            //accCmd = new OleDbCommand(mySql, accCon);
            //reader = accCmd.ExecuteReader();
            //reader.Read();
            //StateConnButt.Enabled = false;
            //StateDisconButt.Enabled = false;
            //reader.Close();
            string query;
            int i = 0;
            while (reader.Read())
            {
                PlaceName = reader.GetString(7);
                currIpAddr = reader.GetString(1);
                currPort = reader.GetInt32(2);
                currDevId = reader.GetInt32(3);
                currDevType = reader.GetString(4).ToUpper();
                
                currPass = reader.GetString(8).ToUpper();
                if (PingTheDevice(currIpAddr))
                {
                    placeStateId.Add(reader.GetInt32(6));
                    zkSRdr.Add(new CZKEM());
                    query = "Update statetable set state=5 Where Device_num=" + placeStateId[i]; ;
                    mySqlCmd = new MySqlCommand(query, connection);
                    int affectedRows = mySqlCmd.ExecuteNonQuery();
                    ZkStateConnect(zkSRdr[i], currIpAddr, currPort, PlaceName);
                    i++;
                }
            }
            reader.Close();
            StateConnButt.Enabled = false;
            StateDisconButt.Enabled = true;
            ComboBox1.Enabled = false;
            ReadButt.Enabled = false;
            SettingsButt.Enabled = false;
            userInfoButt.Enabled = false;
            Cursor.Current = Cursors.Default;
        }
        
        private void StateDisconButt_Click(object sender, EventArgs e)
        {
            MySqlCommand mySqlCmd;
            string query;
            for (int i=0; i< zkSRdr.Count; i++)
            {
                query = "Update statetable set state=-1 Where Device_num=" + placeStateId[i]; ;
                mySqlCmd = new MySqlCommand(query, connection);
                int affectedRows = mySqlCmd.ExecuteNonQuery();
                try
                {
                    DeleteZkEvent(zkSRdr[i]);
                    zkSRdr[i].Disconnect();
                }catch { }
            }
            zkSRdr.Clear();
            placeStateId.Clear();
            gridView2.Rows.Clear();
            StateConnButt.Enabled = true;
            StateDisconButt.Enabled = false;
            ComboBox1.Enabled = true;
            ReadButt.Enabled = true;
            SettingsButt.Enabled = true;
            userInfoButt.Enabled=true;
        }

        private void ReadButt_Click(object sender, EventArgs e)
        {
            if (MyGlobal.showDetails)
            {
                gridView1.Columns[1].HeaderText = "Emp";
            }
            else
            {
                gridView1.Columns[1].HeaderText = "Cnt";
            }
            try
            {
                ToolStripMenuItem btn = (ToolStripMenuItem)sender;
                IsReadBtn = btn.Name.Contains("Read");
                if (btn.Name.Contains("PeriodAttendance"))
                {
                    senderType = 1;
                }
                else if (btn.Name.Contains("NewAttendance"))
                {
                    senderType = 2;
                }
                else
                {
                    senderType = 3;
                }
            }
            catch
            {
                ToolStripButton btn = (ToolStripButton)sender;
                IsReadBtn = btn.Name.Contains("Read");
                senderType = 3;
            }
            if (!ReadingMode)
            {
                if (!allDeviceSelected && !MyGlobal.IsConnected)
                {
                    MessageBox.Show("Selected Device Not Connected", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ReadingMode = true;
                    FailedDevice.Clear();
                    //myCount = 0;
                    if (IsReadBtn)
                    {
                        ReadButt.Image = Properties.Resources.cancel;
                        ReadButt.Text = "Stop";
                        ShowButt.Enabled = false;
                    }
                    else
                    {
                        ShowButt.Image = Properties.Resources.cancel;
                        ShowButt.Text = "Stop Show";
                        ReadButt.Enabled = false;
                    }
                    SettingsButt.Enabled = false;
                    userInfoButt.Enabled = false;
                    ComboBox1.Enabled = false;
                    menuStrip1.Enabled = false;
                    FReportButt.Enabled = false;
                    historyButt.Enabled = false;
                    Strip1.Update();
                    Timer1_Tick(sender, null);
                    timer1.Interval = MyGlobal.MyPeriod * 60000;
                    timer1.Enabled = IsReadBtn;
                }
            }
            else
            {
                ThreadInProgress = false;
                ReadButt.Image = Properties.Resources.download;
                ReadButt.Text = "Save";
                ShowButt.Image = Properties.Resources.preview;
                ShowButt.Text = "   Preview  ";
                ReadingMode = false;
                ReadButt.Enabled = (IsDataConnect || allDeviceSelected);
                ShowButt.Enabled = (currDevType == "ZKTECO");
                SettingsButt.Enabled = true;
                userInfoButt.Enabled = true;
                ComboBox1.Enabled = true;
                menuStrip1.Enabled = true;
                FReportButt.Enabled = true;
                historyButt.Enabled = true;
                Strip1.Update();
                timer1.Enabled = false;
            }
        }

        private void SettingsButt_Click(object sender, EventArgs e)
        {
            LoginForm myForm = new LoginForm();
            myForm.ShowDialog();
        }

        private void userInfoButt_Click(object sender, EventArgs e)
        {
            Authorized frm = new Authorized();
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                SavedUserList f2 = new SavedUserList();
                f2.ShowDialog();
            }
        }

        private void userListButt_Click(object sender, EventArgs e)
        {
            Authorized frm = new Authorized();
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                UsersList f2 = new UsersList();
                f2.ShowDialog();
            }
        }

        private void attendToolStrip_Click(object sender, EventArgs e)
        {
            AttendReport f2 = new AttendReport();
            f2.ShowDialog();
        }

        private void historyTool_Click(object sender, EventArgs e)
        {
            HistReport f2 = new HistReport();
            f2.ShowDialog();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //DateTime d1 = new DateTime(2020, 12, 24, 0, 0, 0);
                //if ((DateTime.Compare(d1, DateTime.Now) < 0))
                //{
                //    FMain_FormClosing(null, null);
                //}
                Cursor.Current = Cursors.WaitCursor;
                if (!IsReadBtn || (IsReadBtn && CheckDatabase()))
                {
                    myThread = new Thread(StartReadAttLog);
                    myThread.Start();
                }
                else
                {
                    row.Clear();
                    row.Add(PlaceName);
                    row.Add(DateTime.Now);
                    row.Add("Database not connected.");
                    gridView4.Rows.Insert(0, row.ToArray());
                    gridView4.Rows[0].Selected = true;
                    DevButt.Image = Properties.Resources.discconect;
                    ReadButt_Click(sender, null);
                    return;
                }

            }
            catch (Exception myExp)
            {
                row.Clear();
                row.Add(PlaceName);
                row.Add(DateTime.Now);
                row.Add("Error :" + myExp.Message);
                gridView4.Rows.Insert(0, row.ToArray());
                gridView4.Rows[0].Selected = true;
                MyGlobal.IsConnected = false;
            }
        }

        private void FReportButt_ClickAsync(object sender, EventArgs e)
        {

            LogReport f2 = new LogReport();
            f2.ShowDialog();
        }

        public static bool PingTheDevice(string ipAdd)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ipAdd);
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                // Create a buffer of 32 bytes of data to be transmitted. 
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 1000;

                PingReply reply = pingSender.Send(ipAddress, timeout);//, buffer, options);
                if (reply.Status == IPStatus.Success)
                    return true;
                else
                {

                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void StartReadAttLog()
        {
            int dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, ioMode, verMode;
            int workCode = 0, myRecCount = 0;
            string empId;
            bool okToRead, isEnable;
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            OleDbDataReader gReader;
            ThreadInProgress = true;
            if (allDeviceSelected)
            {
                mySql = "Select Count(*) From Devices Where Dev_Status=True";
                gCmd = new OleDbCommand(mySql, accCon);
                gReader = gCmd.ExecuteReader();
                gReader.Read();
                recordCount = gReader.GetInt32(0);
                gReader.Close();
                Invoke((MethodInvoker)(() =>
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = recordCount;
                    progressBar2.Value = 0;
                }));
                mySql = "Select * From Devices Where Dev_Status=True Order By ID";
            }
            else
            {
                mySql = "Select * From Devices Where Place_id =" + placeId;
                progressBar2.Value = 0;
            }
            gCmd = new OleDbCommand(mySql, accCon);
            gReader = gCmd.ExecuteReader();
            Invoke((MethodInvoker)(() =>
            {
                progressBar1.Visible = allDeviceSelected;
                progressBar2.Visible = true;
            }));
            while (ThreadInProgress && gReader.Read())
            {
                myRecCount = 0;
                isEnable = gReader.GetBoolean(9);
                Invoke((MethodInvoker)(() => progressBar1.Value += 1));
                if (isEnable)
                {
                    okToRead = true;
                    currIpAddr = gReader.GetString(1);
                    currPort = gReader.GetInt32(2);
                    currDevId = gReader.GetInt32(3);
                    currDevType = gReader.GetString(4).ToUpper();
                    MyPlace = gReader.GetInt32(6);
                    currPass = gReader.GetString(8).ToUpper();
                    Auto_Clear = gReader.GetBoolean(10);
                    try
                    {
                        PlaceName = gReader.GetString(7);
                    }
                    catch
                    {
                        PlaceName = "";
                    }
                    if (MyGlobal.stopFailed)
                        if (!IsDeviceOk(gReader.GetInt32(0)))
                            okToRead = false;
                    if (okToRead)
                    {
                        switch (currDevType)
                        {
                            case "HANDPUNCH":
                                int rtn;              // below, rtn will be 1 if reader responds and 0 (zero) if error (no response)
                                bool hasLog = false;                        // variable to track whether there's a log to get.
                                RSI_STATUS myRdrStatus = new RSI_STATUS();  // variable to receive reader’s status.
                                RSI_DATALOG myDataLog = new RSI_DATALOG();
                                if (allDeviceSelected)
                                {
                                    Invoke((MethodInvoker)(() =>
                                    {
                                        MyGlobal.IsConnected = ConnectPunch(currIpAddr, currPort, currDevId, PlaceName);
                                    }));
                                }
                                if (MyGlobal.IsConnected)
                                {
                                    //Invoke((MethodInvoker)(() =>
                                    //{
                                    //    progressBar1.Visible = allDeviceSelected;
                                    //    progressBar2.Visible = true;
                                    //}));
                                    rtn = myRdr.CmdGetStatus(myRdrStatus);      // Gets reader’s status and checks whether there are
                                    hasLog = (myRdrStatus.dlog_rdy > 0);        //    any data logs.
                                    if (hasLog && TRUE == rtn)
                                    {
                                        try
                                        {
                                            progressBar2.Value = 0;
                                            while (ThreadInProgress && hasLog && TRUE == rtn)
                                            {           // Keep repeating until no more logs or an error.
                                                rtn = myRdr.CmdGetDatalog(myDataLog);   // Get the log.
                                                if (FALSE == rtn)
                                                {                           // If attempt to get log fails (it shouldn’t), try
                                                    int counter = 0;        // up to three times to get it again.
                                                    while (counter < 3 && FALSE == rtn)
                                                    {
                                                        rtn = myRdr.CmdGetPreviousDatalog(myDataLog);  // (Attempt to get the same data log again.)
                                                        counter++;
                                                    }
                                                }
                                                int myId = Int32.Parse(myDataLog.pOperand.GetID());
                                                string pDate = myDataLog.pTimestamp.day.ToString("00/") +
                                                    myDataLog.pTimestamp.month.ToString("00/") +
                                                    myDataLog.pTimestamp.year.ToString("2000");
                                                string pTime = myDataLog.pTimestamp.hour.ToString("00:") +
                                                    myDataLog.pTimestamp.minute.ToString("00:") +
                                                    myDataLog.pTimestamp.second.ToString("00");
                                                int ioInt = 0;
                                                myRecCount++;
                                                DateTime myDate, myTime;
                                                myDate = DateTime.ParseExact(pDate, "dd/MM/yyyy", null);
                                                myTime = DateTime.ParseExact(pTime, "HH:mm:ss", null);
                                                SendToDatabase(myId, myDate, myTime, 4, ioInt);
                                                //SaveToLogTable(myId, pDate, pTime);
                                                if (MyGlobal.showDetails)
                                                {
                                                    row.Clear();
                                                    row.Add(MyPlace);
                                                    row.Add(myId);
                                                    row.Add(pDate);
                                                    row.Add(pTime);
                                                    row.Add("Palm");
                                                    //row.Add(ioStr);
                                                    Invoke((MethodInvoker)(() =>
                                                    {
                                                        gridView1.Rows.Add(row.ToArray());
                                                        gridView1.FirstDisplayedScrollingRowIndex = gridView1.RowCount - 1;
                                                    }));
                                                }
                                                rtn = myRdr.CmdGetStatus(myRdrStatus);       // Get reader’s status again to see if there are any more
                                                hasLog = (myRdrStatus.dlog_rdy > 0);         // logs. (If no more logs or if error, end while loop.)
                                                try
                                                {
                                                    progressBar2.Value++;
                                                }
                                                catch
                                                {
                                                    progressBar2.Value = 0;
                                                }
                                                try
                                                {
                                                    Label2.Text = myRecCount + "/" + Label2.Text.Split('/')[1];
                                                }
                                                catch
                                                {

                                                }
                                            }
                                        }
                                        catch
                                        {
                                            row.Clear();
                                            row.Add(PlaceName);
                                            row.Add(DateTime.Now);
                                            row.Add("Couldn't complete read this device " + myRecCount + " logs");
                                            Invoke((MethodInvoker)(() =>
                                            {
                                                gridView4.Rows.Add(row.ToArray());
                                            }));
                                        }
                                        //SaveToLogTable(myId, pDate, pTime);
                                        //saveLog(MyPlace, myRecCount, "finger print reading");
                                    }
                                }
                                if (MyGlobal.showDetails)
                                {
                                    row.Clear();
                                    row.Add(MyPlace);
                                    row.Add("");
                                    row.Add("Total Log");
                                    row.Add(myRecCount);
                                    //row.Add("------------");
                                    Invoke((MethodInvoker)(() =>
                                    {
                                        gridView1.Rows.Add(row.ToArray());
                                        gridView1.FirstDisplayedScrollingRowIndex = gridView1.RowCount - 1;
                                    }));
                                }
                                else
                                {
                                    row.Clear();
                                    row.Add(MyPlace);
                                    row.Add(myRecCount);
                                    row.Add(DateTime.Now.ToString("yyyy/MM/dd"));
                                    row.Add(DateTime.Now.ToString("hh:mm:ss"));
                                    Invoke((MethodInvoker)(() =>
                                    {
                                        gridView1.Rows.Add(row.ToArray());
                                        gridView1.FirstDisplayedScrollingRowIndex = gridView1.RowCount - 1;
                                    }));
                                }
                                break;
                            case "ZKTECO":
                                ReadyToRead = false;
                                if (allDeviceSelected)
                                {
                                    Invoke((MethodInvoker)(() =>
                                    {
                                        MyGlobal.IsConnected = ZktConnect(currIpAddr, currPort, PlaceName);
                                    }));
                                }
                                if (MyGlobal.IsConnected)
                                {
                                    progressBar2.Value = 0;
                                    if (senderType == 1)
                                    {
                                        string[] myPeriod = ShowDialogDouble();
                                        if (myPeriod.Length > 0)
                                        {
                                            if (zkRdr.ReadTimeGLogData(currDevId, myPeriod[0], myPeriod[1]))
                                            {
                                                ReadyToRead = true;
                                            }
                                        }
                                    }
                                    else if (senderType == 2)
                                    {
                                        if (zkRdr.ReadNewGLogData(currDevId))
                                        {
                                            ReadyToRead = true;
                                        }
                                    }
                                    else
                                    {
                                        if (allDeviceSelected)
                                        {
                                            ReadyToRead = true;
                                        }
                                        else
                                        {
                                            if (zkRdr.ReadAllGLogData(currDevId))
                                            {
                                                ReadyToRead = true;
                                            }
                                        }
                                    }
                                }
                                if (ReadyToRead)
                                {
                                    bool ReadingComplete = true;
                                    try
                                    {
                                        while (ThreadInProgress && zkRdr.SSR_GetGeneralLogData(currDevId, out empId, out verMode,
                                        out ioMode, out dwYear, out dwMonth, out dwDay, out dwHour,
                                        out dwMinute, out dwSecond, ref workCode))
                                        {
                                            int myId = Int32.Parse(empId);
                                            string pDate = dwDay.ToString("00/") +
                                                dwMonth.ToString("00/") + dwYear.ToString("0000");
                                            string pTime = dwHour.ToString("00:") +
                                                dwMinute.ToString("00:") + dwSecond.ToString("00");
                                            string verStr = "Passwoed";
                                            switch (verMode)
                                            {
                                                case 1:
                                                    verStr = "Finger";
                                                    break;
                                                case 3:
                                                    verStr = "Password";
                                                    break;
                                                case 4:
                                                    verStr = "Card";
                                                    break;
                                                case 15:
                                                    verStr = "Face";
                                                    break;
                                                case 25:
                                                    verStr = "Palm";
                                                    break;
                                                default:
                                                    verStr = verMode.ToString();
                                                    break;
                                            }
                                            myRecCount++;
                                            try
                                            {
                                                Label2.Text = myRecCount + "/" + Label2.Text.Split('/')[1];
                                            }
                                            catch { }
                                            progressBar2.Value++;
                                            if (MyGlobal.showDetails)
                                            {
                                                row.Clear();
                                                row.Add(MyPlace);
                                                row.Add(empId);
                                                row.Add(pDate);
                                                row.Add(pTime);
                                                row.Add(verStr);
                                                //row.Add(ioStr);
                                                Invoke((MethodInvoker)(() =>
                                                {
                                                    gridView1.Rows.Add(row.ToArray());
                                                    gridView1.FirstDisplayedScrollingRowIndex = gridView1.RowCount - 1;
                                                }));


                                            }
                                            if (IsReadBtn)
                                            {
                                                DateTime myDate, myTime;
                                                myDate = DateTime.ParseExact(pDate, "dd/MM/yyyy", null);
                                                myTime = DateTime.ParseExact(pTime, "HH:mm:ss", null);
                                                SendToDatabase(myId, myDate, myTime, verMode, ioMode);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        ReadingComplete = false;
                                        row.Clear();
                                        row.Add(PlaceName);
                                        row.Add(DateTime.Now);
                                        row.Add("Couldn't complete read this device " + myRecCount + " logs");
                                        Invoke((MethodInvoker)(() =>
                                        {
                                            gridView4.Rows.Add(row.ToArray());
                                        }));
                                    }
                                    //saveLog(MyPlace, myRecCount, "finger print reading");
                                    if (ThreadInProgress && IsReadBtn && ReadingComplete && Auto_Clear)
                                    {
                                        zkRdr.ClearGLog(currDevId);
                                    }
                                }
                                else
                                {
                                    row.Clear();
                                    row.Add(PlaceName);
                                    row.Add(DateTime.Now);
                                    row.Add("Couldn't read or feature not support");
                                    Invoke((MethodInvoker)(() =>
                                    {
                                        gridView4.Rows.Add(row.ToArray());
                                    }));
                                }
                                if (MyGlobal.showDetails)
                                {
                                    row.Clear();
                                    row.Add(MyPlace);
                                    row.Add("");
                                    row.Add("Total Log");
                                    row.Add(myRecCount);
                                    Invoke((MethodInvoker)(() =>
                                    {
                                        gridView1.Rows.Add(row.ToArray());
                                        gridView1.FirstDisplayedScrollingRowIndex = gridView1.RowCount - 1;
                                    }));
                                }
                                else
                                {
                                    row.Clear();
                                    row.Add(MyPlace);
                                    row.Add(myRecCount);
                                    row.Add(DateTime.Now.ToString("yyyy/MM/dd"));
                                    row.Add(DateTime.Now.ToString("hh:mm:ss"));
                                    Invoke((MethodInvoker)(() =>
                                    {
                                        gridView1.Rows.Add(row.ToArray());
                                        gridView1.FirstDisplayedScrollingRowIndex = gridView1.RowCount - 1;
                                    }));
                                }
                                break;
                        }
                    }
                }
                else
                {
                    row.Clear();
                    row.Add(PlaceName);
                    row.Add(DateTime.Now);
                    row.Add("Read from device not enable");
                    Invoke((MethodInvoker)(() =>
                    {
                        gridView4.Rows.Add(row.ToArray());
                        gridView4.FirstDisplayedScrollingRowIndex = gridView4.RowCount - 1;
                    }));
                }

                if (allDeviceSelected)
                {
                    zkRdr.Disconnect();
                    MyGlobal.IsConnected = false;
                }
            }
            gReader.Close();
            Invoke((MethodInvoker)(() =>
            {
                progressBar1.Visible = false;
                progressBar2.Visible = false;
            }));
            if (ThreadInProgress && !IsReadBtn)
            {
                Invoke((MethodInvoker)(() =>
                {
                    ReadButt_Click(ShowButt, null);
                }));
            }
            else
            {
                ThreadInProgress = false;
            }
        }

        public async void GetStudentStatus(int TerminalId, string FingerId)
        {
            string baseUrl = MyGlobal.apiLink;
            //string baseUrl = "https://portal.ust.edu.ye/ords/optimalstudportal/stud/finger/";
            try
            {
                if (FingerId.Trim() != "")
                {
                    string myUrl = $"{baseUrl}{FingerId}";
                    var response = await client.GetAsync(myUrl);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject responseObject = JObject.Parse(responseBody);
                    if (response.IsSuccessStatusCode)
                    {
                        if (responseObject["finger_id"].ToString() == FingerId)
                        {
                            string Stud_Name = responseObject["stud_a_name"].ToString();
                            string Stud_Finger = responseObject["finger_id"].ToString();
                            string Allow_Status = responseObject["allow_status"].ToString();
                            string Status_Text = responseObject["status_text"].ToString();
                            string query = "Update statetable set stud_name='" + Stud_Name +
                            "', stud_num='" + Stud_Finger + "', message='" + Status_Text +
                            "', state=" + Allow_Status + " Where Device_num=" + TerminalId.ToString();
                            try
                            {
                                connection.Open();
                            }
                            catch { }
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            cmd.ExecuteNonQuery();
                            //DisplayVendorInfo(TerminalId, FingerId, responseObject);
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            string query = "Update statetable set stud_num='" + FingerId +
                                "', stud_name ='هذا الرقم غير موجود' " +
                                "Where Device_num=" + TerminalId.ToString();
                            try
                            {
                                connection.Open();
                            }
                            catch { }
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            cmd.ExecuteNonQuery();
                            Thread.Sleep(3000);
                            row.Clear();
                            row.Add(ConnPlace);
                            row.Add(DateTime.Now);
                            row.Add($"This Number not exist. {FingerId}");
                            Invoke((MethodInvoker)(() =>
                            {
                                gridView4.Rows.Add(row.ToArray());
                            }));
                        }
                    }
                    else
                    {
                        string query = "Update statetable set stud_name='الرقم غير موجود أو النظام مشغول' " +
                                "Where Device_num=" + TerminalId.ToString();
                        try
                        {
                            connection.Open();
                        }
                        catch { }
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.ExecuteNonQuery();
                        Thread.Sleep(3000);
                        row.Clear();
                        row.Add(ConnPlace);
                        row.Add(DateTime.Now);
                        row.Add("Number not found or ERP not response.");
                        Invoke((MethodInvoker)(() =>
                        {
                            gridView4.Rows.Add(row.ToArray());
                        }));
                    }
                }
            }
            catch
            {
                row.Clear();
                row.Add(ConnPlace);
                row.Add(DateTime.Now);
                row.Add("Error. Thread " + TerminalId + " could not start");
                Invoke((MethodInvoker)(() =>
                {
                    gridView4.Rows.Add(row.ToArray());
                }));
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

        private void SendToDatabase(int myId, DateTime pDate, DateTime pTime, int pVerifyMode, int pIOMode)
        {
            switch (MyGlobal.ServerType)
            {
                case 0:
                    string machine = Environment.MachineName;
                    OracleCommand cmd;
                    my_querry = "INSERT INTO EMS_EMP_FINGERS (Emp_Id,Sign_Date,Sign_Time,Sign_Plc_Id," +
                        "Status,Mach_Name) " +
                        "VALUES(" + myId.ToString() + ", TO_DATE('" + pDate.ToString("dd/MM/yyyy") + "','DD/MM/YYYY')," +
                        "TO_DATE('" + pDate.ToString("dd/MM/yyyy") + " " + pTime.ToString("HH:mm:ss") + "','DD/MM/YYYY HH24:MI:SS'), " + MyPlace.ToString() +
                        "," + pVerifyMode + ",'" + machine + "')"; //NextValue.ToString() + ")";
                    cmd = new OracleCommand(my_querry, oraCon);
                    cmd.ExecuteNonQuery();
                    break;
                case 1:
                    SqlCommand sqlcmd;
                    my_querry = "Select nvl(Max(Sign_Seq),0) From EMS_EMP_FINGERS";
                    sqlcmd = new SqlCommand(my_querry, sqlCon);
                    my_querry = "INSERT INTO EMS_EMP_FINGERS (Emp_Id,Sign_Date,Sign_Time,Sign_Plc_Id) " +
                        "VALUES(" + myId.ToString() + ",'" + pDate.ToString("dd/MM/yyyy") + "','" + pTime.ToString("HH:mm:ss") + "','" +
                        MyPlace.ToString() + "')";
                    sqlcmd = new SqlCommand(my_querry, sqlCon);
                    sqlcmd.ExecuteNonQuery();
                    break;
                case 2:
                    OleDbCommand acscmd;
                    my_querry = "Select nvl(Max(Sign_Seq),0) From EMS_EMP_FINGERS";
                    sqlcmd = new SqlCommand(my_querry, sqlCon);
                    my_querry = "INSERT INTO EMS_EMP_FINGERS (Emp_Id,Sign_Date,Sign_Time,Sign_Plc_Id) " +
                        "VALUES(" + myId.ToString() + ",'" + pDate.ToString("dd/MM/yyyy") + "','" + pTime.ToString("HH:mm:ss") + "','" +
                        MyPlace.ToString() + "')";
                    acscmd = new OleDbCommand(mySql, accCon);
                    acscmd.ExecuteNonQuery();
                    break;
            }
            OleDbCommand accmd;
            OleDbDataReader accReader;
            double lastValue;
            mySql = "Select Max(Sign_Seq) From EMS_EMP_FINGERS";
            accmd = new OleDbCommand(mySql, accCon);
            accReader = accmd.ExecuteReader();
            accReader.Read();
            try
            {
                lastValue = accReader.GetDouble(0) + 1;
            }
            catch
            {
                lastValue = 1;
            }
            accReader.Close();
            mySql = "INSERT INTO EMS_EMP_FINGERS (Emp_Id,Sign_Date,Sign_Time,Sign_Plc_Id," +
                    "Status,Dev_Loc, Sign_Seq) " +
                    "VALUES(" + myId.ToString() + ", DateValue('" + pDate.ToString("dd/MM/yyyy") + "')," +
                    "Format('" + pDate.ToString("dd/MM/yyyy") + " " + pTime.ToString("HH:mm:ss") + "','dd/mm/yyyy Hh:mm:ss')," +    //, 'h: m: s'
                    MyPlace.ToString() +
                    ", 0, 0," + lastValue.ToString() + ")";
            accmd = new OleDbCommand(mySql, accCon);
            accmd.ExecuteNonQuery();
        }

        void saveLog(int placeNo, int count, string detail)
        {
            OleDbCommand cmd;
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            mySql = "Insert into LogFile (devid,devname,rtime,rdate,rcount,detail,readBy) values('" +
                placeNo + "','" + PlaceName + "','" + DateTime.Now.ToString("hh:mm:ss tt") + "','" +
                DateTime.Now + "','" + count + "','" + detail + "','" + userName + "')";
            cmd = new OleDbCommand(mySql, accCon);
            cmd.ExecuteNonQuery();
        }

        private bool IsDeviceOk(int ID)
        {
            for (int i = 0; i < FailedDevice.Count; i++)
            {
                if (FailedDevice[i] == ID)
                {
                    return false;
                }
            }
            return true;
        }

        private void DataButt_Click(object sender, EventArgs e)
        {

        }

        private void StartServer() 
        { 

            int port = 8077; // Change this to your desired port number
            TcpListener listener = null;

            try
            {
                // Create listener on all available interfaces
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
               // Task<> result  = listener.AcceptSocketAsync();
                row.Clear();
                row.Add(ConnPlace);
                row.Add(DateTime.Now);
                row.Add($"Listening on port {port}...");
                Invoke((MethodInvoker)(() =>
                {
                    gridView3.Rows.Add(row.ToArray());
                }));
                bool isClientConnected = false;
                while (!isClientConnected)
                {
                    // Wait for client connection
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        
                        isClientConnected = true;
                        row.Clear();
                        row.Add(ConnPlace);
                        row.Add(DateTime.Now);
                        row.Add("Client Connected "+ client.Client);
                        Invoke((MethodInvoker)(() =>
                        {
                            gridView3.Rows.Add(row.ToArray());
                        }));
                    }
                    catch (Exception m)
                    {
                        row.Clear();
                        row.Add(ConnPlace);
                        row.Add(DateTime.Now);
                        row.Add("Client Connecting failed!");
                        Invoke((MethodInvoker)(() =>
                        {
                            gridView3.Rows.Add(row.ToArray());
                        }));
                    }


                    // Handle client connection here

                }
            }
            catch (Exception m)
            {
                row.Clear();
                row.Add(ConnPlace);
                row.Add(DateTime.Now);
                row.Add($"Error: {m.Message}");
                Invoke((MethodInvoker)(() =>
                {
                    gridView3.Rows.Add(row.ToArray());
                }));
            }
            //finally
            //{
            //    listener?.Stop();
            //}
            //Task<JObject> task =
            //    RunApiGet(16117500);
            //RunApiUpdate(54);
            //RunApiInsert();
            //RunApiDelete(57);
        }

        public static async Task<JObject> RunApiInsert()
        {
            string apiUrl = "http://localhost:8888/ords/account/vendor/manage/";
            var vendor = new
            {
                vendor_name = "Kia Company",
                vendor_account = "2020010004",
                vendor_address = "Korea, Seoul",
                vendor_phone = "332332323",
                vendor_status = 1
            };

            using (var httpClient = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(vendor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject responseObject = JObject.Parse(responseBody);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(responseObject["message"].ToString());
                }
                else
                {
                    MessageBox.Show($"Error: {responseObject["message"].ToString()}");
                }
            }
            return null;
        }

        public static async Task<JObject> RunApiGet(int studId)
        {
            //string baseUrl = "http://localhost:8888/ords/account/vendor/manage/";
            string baseUrl = "https://portal.ust.edu.ye/ords/optimalstudportal/stud/finger/";
            try
            {
                if (studId != 0)
                {
                    string myUrl = $"{baseUrl}{studId}";
                    var response = await client.GetAsync(myUrl);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject responseObject = JObject.Parse(responseBody);
                    if (response.IsSuccessStatusCode)
                    {
                        DisplayVendorInfo(studId, responseObject);
                    }
                    else
                    {
                        MessageBox.Show($"Error: {responseObject["message"].ToString()}");
                    }
                }
                else
                {
                    MessageBox.Show("Vendor id must be greater than 0.", "Error Message:");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " Check the API link");
            }
            return null;
        }

        static void DisplayVendorInfo(int studId, JObject dataObject)
        {
            if (dataObject["finger_id"].ToString() == studId.ToString())
            {
                //string dataString = info["data"].ToString();
                //JObject dataObject = JObject.Parse(dataString);
                string Stud_Name = dataObject["stud_a_name"].ToString();
                string Stud_Finger = dataObject["finger_id"].ToString();
                string Allow_Status = dataObject["allow_status"].ToString();
                string Status_Text = dataObject["status_text"].ToString();
                MessageBox.Show($"Student Name : {Stud_Name}\n" +
                                $"Finger Number: {Stud_Finger}\n" +
                                $"Allow Status : {Allow_Status}\n" +
                                $"Status Text  : {Status_Text}", "Vendor Information:");
            }
            else
            {
                MessageBox.Show("Error: This Student not found", "Error Message:");
            }
        }

        public static async Task<JObject> RunApiUpdate(int vendorId)
        {
            string apiUrl = "http://localhost:8888/ords/account/vendor/manage/";
            var vendorInfo = new
            {
                vendor_id = vendorId,
                vendor_name = "LG Company",
                vendor_account = "2020010002",
                vendor_address = "Korea, Seoul",
                vendor_phone = "555423232",
                vendor_status = 1
            };
            try
            {
                if (vendorId != 0)
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var json = JsonConvert.SerializeObject(vendorInfo);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(apiUrl, content);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject responseObject = JObject.Parse(responseBody);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(responseObject["message"].ToString());
                    }
                    else
                    {
                        MessageBox.Show($"Error: {responseObject["message"].ToString()}");
                    }
                }
                else
                {
                    MessageBox.Show("Vendor Id Must be greater than 0.");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " Check the API link");
            }
            return null;
        }

        public static async Task<JObject> RunApiDelete(int vendorId)
        {
            string apiUrl = "http://localhost:8888/ords/account/vendor/manage/";
            try
            {
                if (vendorId != 0)
                {
                    if (MessageBox.Show("Are you sure to delete this record?",
                        "Confirm Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string myUrl = $"{apiUrl}?vendor_id={vendorId}";
                        var response = await client.DeleteAsync(myUrl);

                        string responseBody = await response.Content.ReadAsStringAsync();
                        JObject responseObject = JObject.Parse(responseBody);
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(responseObject["message"].ToString());
                        }
                        else
                        {
                            MessageBox.Show($"Error: {responseObject["message"].ToString()}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vendor Id Must be greater than 0.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " Check the API link");
            }
            return null;
        }

        private void DevButt_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (currDevType.ToUpper() == "ZKTECO")
            //    {
            //        zkRdr.EnableDevice(currDevId, true);
            //    }
            //    else
            //    {
            //        myRdr.SetEnabled(true);
            //    }
            //    MessageBox.Show("Selected Device Enabled", "Alert",
            //                MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch
            //{
            //    MessageBox.Show("There is no device selected", "Warning",
            //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void menu2_excel_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileName = new FileInfo(saveFileDialog1.FileName);
                using (var package = new ExcelPackage())
                {
                    using (var worksheet = package.Workbook.Worksheets.Add("Attendance"))
                    {
                        //worksheet.Cells[1, 1].Value = label13.Text +
                        //        " Employee Logs";
                        //worksheet.Cells[2, 1].Value = "From " + attDateFrom.Text + " To " + attDateTo.Text;
                        //worksheet.Cells["A1:E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //worksheet.Cells["A1:E1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        //worksheet.Cells["A1:E1"].Merge = true;
                        //worksheet.Cells["A2:E2"].Merge = true;
                        //worksheet.Cells[1, 1].Style.Font.Bold = true;
                        //worksheet.Cells[1, 1].Style.Font.Size = 16;
                        //worksheet.Cells[2, 1].Style.WrapText = true;
                        //worksheet.Cells[2, 1].Style.Font.Size = 16;
                        //worksheet.Cells[1, 1, 2, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[1, 1, 2, 6].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        worksheet.Cells[1, 1, 1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1, 1, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(3).Width = 12;
                        for (int j = 0; j < gridView1.Columns.Count; j++)
                        {
                            worksheet.Cells[1, j + 1].Value = gridView1.Columns[j].HeaderText;
                        }
                        for (int i = 0; i < gridView1.Rows.Count; i++)
                        {
                            for (int j = 0; j < gridView1.Columns.Count; j++)
                            {
                                worksheet.Cells[i + 2, j + 1].Value = gridView1.Rows[i].Cells[j].Value ?? "";//.ToString() ?? "";
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

        public static string[] ShowDialogDouble()
        {
            var myTime = new List<string>();
            Form prompt = new Form()
            {
                Width = 565,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Date & Time",
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true,
                MaximizeBox = false,
                MinimizeBox = false,

            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Start Date & Time" };
            Label textLabe2 = new Label() { Left = 300, Top = 20, Text = "End Date & Time" };
            DateTimePicker startDate = new DateTimePicker() { Left = 50, Top = 40, Width = 120 };
            DateTimePicker startTime = new DateTimePicker() { Left = 175, Top = 40, Width = 90 };
            startTime.Format = DateTimePickerFormat.Time;
            startTime.ShowUpDown = true;

            DateTimePicker endDate = new DateTimePicker() { Left = 300, Top = 40, Width = 120 };
            DateTimePicker endTime = new DateTimePicker() { Left = 425, Top = 40, Width = 90 };
            endTime.Format = DateTimePickerFormat.Time;
            endTime.ShowUpDown = true;

            Button confirmation = new Button()
            {
                Text = "Ok",
                Left = 415,
                Top = 90,
                Width = 100,
                DialogResult = DialogResult.OK
            };
            Button cancelation = new Button()
            {
                Text = "Cancel",
                Left = 300,
                Top = 90,
                Width = 100,
                DialogResult = DialogResult.Cancel
            };
            //confirmation.Click += (sender, e) => { prompt.Close(); };
            //cancelation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(startDate);
            prompt.Controls.Add(startTime);
            prompt.Controls.Add(endDate);
            prompt.Controls.Add(endTime);
            prompt.Controls.Add(cancelation);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textLabe2);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancelation;
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                myTime.Add(startDate.Value.ToString("yyyy-MM-dd " + startTime.Value.ToString("HH:mm:ss")));
                myTime.Add(endDate.Value.ToString("yyyy-MM-dd " + endTime.Value.ToString("HH:mm:ss")));
            }
            else
            {
                myTime.Clear();
            }
            return myTime.ToArray();
        }

        public static string[] ShowDialogSingle()
        {
            var myTime = new List<string>();
            Form prompt = new Form()
            {
                Width = 320,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Date & Time",
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true,
                MaximizeBox = false,
                MinimizeBox = false,

            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Until Date & Time" };
            DateTimePicker untilDate = new DateTimePicker() { Left = 50, Top = 40, Width = 120 };
            DateTimePicker untilTime = new DateTimePicker() { Left = 175, Top = 40, Width = 90 };
            untilTime.Format = DateTimePickerFormat.Time;
            untilTime.ShowUpDown = true;
            Button confirmation = new Button()
            {
                Text = "Ok",
                Left = 165,
                Top = 90,
                Width = 100,
                DialogResult = DialogResult.OK
            };
            Button cancelation = new Button()
            {
                Text = "Cancel",
                Left = 50,
                Top = 90,
                Width = 100,
                DialogResult = DialogResult.Cancel
            };
            //confirmation.Click += (sender, e) => { prompt.Close(); };
            //cancelation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(untilDate);
            prompt.Controls.Add(untilTime);
            prompt.Controls.Add(cancelation);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancelation;
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                myTime.Add(untilDate.Value.ToString("yyyy-MM-dd " + untilTime.Value.ToString("HH:mm:ss")));
            }
            else
            {
                myTime.Clear();
            }
            return myTime.ToArray();
        }

        private void LoadZkErrorCode()
        {
            ZkError[0, 0] = "-100"; ZkError[0, 1] = "Operation failed or data not exist";
            ZkError[1, 0] = "-10"; ZkError[1, 1] = "Transmitted data length is incorrect";
            ZkError[2, 0] = "-5"; ZkError[2, 1] = "Data already exists";
            ZkError[3, 0] = "-4"; ZkError[3, 1] = "Space is not enough";
            ZkError[4, 0] = "-3"; ZkError[4, 1] = "Error size";
            ZkError[5, 0] = "-2"; ZkError[5, 1] = "Error in file read/ write";
            ZkError[6, 0] = "-1"; ZkError[6, 1] = "SDK is not initialized and needs to be reconnected";
            ZkError[7, 0] = "0"; ZkError[7, 1] = "Data not found or data repeated";
            ZkError[8, 0] = "1"; ZkError[8, 1] = "Operation is correct";
            ZkError[9, 0] = "4"; ZkError[9, 1] = "Parameter is incorrect";
            ZkError[10, 0] = "101"; ZkError[10, 1] = "Error in allocating buffer";
        }

        private string GetErrorMessage(int errorCode)
        {
            for (int i = 0; i <= 10; i++)
                if (ZkError[i, 0] == errorCode.ToString())
                    return ZkError[i, 1];
            return "";
        }

        private void menu2_copy_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                string myString = "";
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        myString += gridView1.Rows[i].Cells[j].Value.ToString();
                        if (j < 3) myString += " , ";
                    }
                    myString += "\r\n";
                }
                Clipboard.SetText(myString);
                MessageBox.Show("All data copy to clipboard");

            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ThreadInProgress)
            {
                e.Cancel = true;
                MessageBox.Show("Log reading in progress. Stop reading first", "Stop",
                                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                accCon.Close();
                DeleteEvent();
                if (myNetwork != null)
                {
                    myNetwork.Detach(myRdr);
                    myNetwork.SetCom(null);  // Disassociates the network from the ComHandle.
                    myNetwork.Dispose();     // Critical! Without disposing of the network, you'd have a memory leak.
                    myNetwork = null;        // Optional. Sets network to null so that if you wereto accidentally
                                             // try to use this instance again, you wouldn't havewhat looked like
                                             // a network that didn't point to anything anymore; this isn't needed
                                             // as long as you don't try to use this instance again.
                }
                if (myIP != null)
                {
                    myIP.Dispose();      // Critical! Without disposing of the ComHandle, you'd have a memory leak.
                    myIP = null;         // Optional. (See note above.)
                }
                Application.Exit();
                Process.GetCurrentProcess().Kill();
            }
        }

        private void deviceToolStrip_Click(object sender, EventArgs e)
        {
            DeviceInfo f2 = new DeviceInfo();
            f2.ShowDialog();
        }

        private void menu2_clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to clear list?", "Warninig",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                gridView1.Rows.Clear();
            }
        }

        private void CreateEvent()
        {
            if (zkRdr.RegEvent(currDevId, 65535))//65535 Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            {
                zkRdr.OnConnected +=    new zkemkeeper._IZKEMEvents_OnConnectedEventHandler(zkRdr_OnConnected);
                zkRdr.OnDisConnected += new zkemkeeper._IZKEMEvents_OnDisConnectedEventHandler(zkRdr_OnDisConnected);
                zkRdr.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(zkRdr_OnFinger);
                zkRdr.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(zkRdr_OnVerify);
                zkRdr.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(zkRdr_OnAttTransactionEx);
                zkRdr.OnFingerFeature += new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(zkRdr_OnFingerFeature);
                zkRdr.OnEnrollFingerEx += new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(zkRdr_OnEnrollFingerEx);
                zkRdr.OnDeleteTemplate += new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(zkRdr_OnDeleteTemplate);
                zkRdr.OnNewUser += new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(zkRdr_OnNewUser);
                zkRdr.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(zkRdr_OnHIDNum);
                ////zkRdr.OnAlarm += new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(zkRdr_OnAlarm);
                //zkRdr.OnDoor += new zkemkeeper._IZKEMEvents_OnDoorEventHandler(zkRdr_OnDoor);
                //zkRdr.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(zkRdr_OnWriteCard);
                //zkRdr.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(zkRdr_OnEmptyCard);
            }
        }

        private void DeleteEvent()
        {
            if (zkRdr.RegEvent(currDevId, 65535))
            //65535 Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            {
                zkRdr.OnConnected -= new zkemkeeper._IZKEMEvents_OnConnectedEventHandler(zkRdr_OnConnected);
                zkRdr.OnDisConnected -= new zkemkeeper._IZKEMEvents_OnDisConnectedEventHandler(zkRdr_OnDisConnected);
                zkRdr.OnFinger -= new zkemkeeper._IZKEMEvents_OnFingerEventHandler(zkRdr_OnFinger);
                zkRdr.OnVerify -= new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(zkRdr_OnVerify);
                zkRdr.OnAttTransactionEx -= new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(zkRdr_OnAttTransactionEx);
                zkRdr.OnFingerFeature -= new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(zkRdr_OnFingerFeature);
                zkRdr.OnEnrollFingerEx -= new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(zkRdr_OnEnrollFingerEx);
                zkRdr.OnDeleteTemplate -= new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(zkRdr_OnDeleteTemplate);
                zkRdr.OnNewUser -= new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(zkRdr_OnNewUser);
                zkRdr.OnHIDNum -= new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(zkRdr_OnHIDNum);
                ////zkRdr.OnAlarm -= new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(zkRdr_OnAlarm);
                //zkRdr.OnDoor -= new zkemkeeper._IZKEMEvents_OnDoorEventHandler(zkRdr_OnDoor);
                //zkRdr.OnWriteCard -= new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(zkRdr_OnWriteCard);
                //zkRdr.OnEmptyCard -= new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(zkRdr_OnEmptyCard);
            }
        }

        private void CreateZkEvent(CZKEM zkDevice)
        {
            if (zkDevice.RegEvent(currDevId, 65535))//65535 Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            {
                //zkDevice.OnConnected += new zkemkeeper._IZKEMEvents_OnConnectedEventHandler(zkRdr_OnConnected);
                //zkDevice.OnDisConnected += new zkemkeeper._IZKEMEvents_OnDisConnectedEventHandler(zkRdr_OnDisConnected);
                //zkDevice.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(zkRdr_OnFinger);
                //zkDevice.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(zkRdr_OnVerify);
                zkDevice.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(zkRdr_OnAttTransactionEx);
                //zkRdr.OnFingerFeature += new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(zkRdr_OnFingerFeature);
                //zkRdr.OnEnrollFingerEx += new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(zkRdr_OnEnrollFingerEx);
                //zkRdr.OnDeleteTemplate += new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(zkRdr_OnDeleteTemplate);
                //zkRdr.OnNewUser += new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(zkRdr_OnNewUser);
                //zkRdr.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(zkRdr_OnHIDNum);
                ////zkRdr.OnAlarm += new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(zkRdr_OnAlarm);
                //zkRdr.OnDoor += new zkemkeeper._IZKEMEvents_OnDoorEventHandler(zkRdr_OnDoor);
                //zkRdr.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(zkRdr_OnWriteCard);
                //zkRdr.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(zkRdr_OnEmptyCard);
            }
        }

        private void DeleteZkEvent(CZKEM zkDevice)
        {
            if (zkDevice.RegEvent(currDevId, 65535))
            //65535 Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            {
                //zkDevice.OnConnected -= new zkemkeeper._IZKEMEvents_OnConnectedEventHandler(zkRdr_OnConnected);
                //zkDevice.OnDisConnected -= new zkemkeeper._IZKEMEvents_OnDisConnectedEventHandler(zkRdr_OnDisConnected);
                //zkDevice.OnFinger -= new zkemkeeper._IZKEMEvents_OnFingerEventHandler(zkRdr_OnFinger);
                //zkDevice.OnVerify -= new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(zkRdr_OnVerify);
                zkDevice.OnAttTransactionEx -= new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(zkRdr_OnAttTransactionEx);
                //zkDevice.OnFingerFeature -= new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(zkRdr_OnFingerFeature);
                //zkDevice.OnEnrollFingerEx -= new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(zkRdr_OnEnrollFingerEx);
                //zkDevice.OnDeleteTemplate -= new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(zkRdr_OnDeleteTemplate);
                //zkDevice.OnNewUser -= new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(zkRdr_OnNewUser);
                //zkDevice.OnHIDNum -= new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(zkRdr_OnHIDNum);
                ////zkDevice.OnAlarm -= new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(zkRdr_OnAlarm);
                //zkDevice.OnDoor -= new zkemkeeper._IZKEMEvents_OnDoorEventHandler(zkRdr_OnDoor);
                //zkDevice.OnWriteCard -= new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(zkRdr_OnWriteCard);
                //zkDevice.OnEmptyCard -= new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(zkRdr_OnEmptyCard);
            }
        }

        void zkRdr_OnConnected()
        {
            row.Clear();
            row.Add(ConnPlace);
            row.Add(DateTime.Now);
            row.Add("Machine Connected");
            Invoke((MethodInvoker)(() =>
            {
                gridView3.Rows.Add(row.ToArray());
            }));
        }

        void zkRdr_OnDisConnected()
        {
            row.Clear();
            row.Add(ConnPlace);
            row.Add(DateTime.Now);
            row.Add("Machine DisConnected");
            Invoke((MethodInvoker)(() =>
            {
                gridView3.Rows.Add(row.ToArray());
            }));
        }

        void zkRdr_OnFinger()
        {
            row.Clear();
            row.Add(ConnPlace);
            row.Add(DateTime.Now);
            row.Add("On Finger ...");
            Invoke((MethodInvoker)(() =>
            {
                gridView3.Rows.Add(row.ToArray());
            }));
        }

        void zkRdr_OnVerify(int UserID)
        {
            row.Clear();
            row.Add(ConnPlace);
            row.Add(DateTime.Now);
            row.Add("User: " + UserID + " finger print verified");
            Invoke((MethodInvoker)delegate
            {
                gridView3.Rows.Add(row.ToArray());
            });

        }

        void zkRdr_OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
        {
            string method = "", state = "";

            if (IsInValid == 0)
            {
                switch (VerifyMethod)
                {
                    case 0: method = "Password"; break;
                    case 1: method = "Finger "+WorkCode; break;
                    case 2: method = "Card"; break;
                    case 3: method = "Password"; break;
                    case 4: method = "Card "; break;
                    case 15: method = "Face "; break;
                    case 25: method = "Palm "; break;
                    default: method = VerifyMethod.ToString() + " Unknown"; break;
                }
                switch (AttState)
                {
                    case 0: state = " Check in "; break;
                    case 1: state = " Check out "; break;
                    case 2: state = " Break out "; break;
                    case 3: state = " Break in "; break;
                    case 4: state = " OvTm In "; break;
                    case 5: state = " OvTm Out "; break;
                }

                row.Clear();
                row.Add(ConnPlace);
                row.Add(Year + "/" + Month + "/" + Day + " " + Hour + ":" + Minute + ":" + Second);
                row.Add(EnrollNumber + state + "using " + method);
                Invoke((MethodInvoker)delegate
                {
                    gridView3.Rows.Add(row.ToArray());
                });

                myThread = new Thread(() => GetStudentStatus(161, EnrollNumber));
                myThread.Start();
            }
        }

        void zkRdr_OnFingerFeature(int Score)
        {
            row.Clear();
            row.Add(ConnPlace);
            row.Add(DateTime.Now);
            row.Add("Enroll fingerprint success. Score: " + Score);
            Invoke((MethodInvoker)delegate
            {
                gridView3.Rows.Add(row.ToArray());
            });
        }

        void zkRdr_OnEnrollFingerEx(string EnrollNumber, int FingerIndex, int ActionResult, int TemplateLength)
        {
            row.Clear();
            row.Add(PlaceName);
            row.Add(DateTime.Now);
            row.Add(EnrollNumber + " Finger: " + FingerIndex + " Enroll");
            Invoke((MethodInvoker)delegate
            {
                gridView3.Rows.Add(row.ToArray());
            });
        }

        void zkRdr_OnDeleteTemplate(int EnrollNumber, int FingerIndex)
        {
            row.Clear();
            row.Add(ConnPlace);
            row.Add(DateTime.Now);
            row.Add("User: " + EnrollNumber + " Finger: " + FingerIndex + " Delete");
            Invoke((MethodInvoker)(() =>
            {
                gridView3.Rows.Add(row.ToArray());
            }));
        }

        void zkRdr_OnNewUser(int EnrollNumber)
        {
            row.Clear();
            row.Add(ConnPlace);
            row.Add(DateTime.Now);
            row.Add("New User: " + EnrollNumber + " Added");
            Invoke((MethodInvoker)(() =>
            {
                gridView3.Rows.Add(row.ToArray());
            }));
        }

        void zkRdr_OnHIDNum(int CardNumber)
        {
            row.Clear();
            row.Add(ConnPlace);
            row.Add(DateTime.Now);
            row.Add("Card No " + CardNumber + " punched");
            Invoke((MethodInvoker)(() =>
            {
                gridView3.Rows.Add(row.ToArray());
            }));
        }

        //void zkRdr_OnAlarm(int AlarmType, int EnrollNumber, int Verified)
        //{
        //    Invoke((MethodInvoker)(() => listBox1.Items.Add("Alarm... ")));
        //}

        //void zkRdr_OnDoor(int DoorNo)
        //{
        //    row.Clear();
        //    row.Add(ConnPlace);
        //    row.Add(DateTime.Now);
        //    row.Add("Now, door no: " + DoorNo + " is opend");
        //    Invoke((MethodInvoker)(() =>
        //    {
        //        gridView3.Rows.Add(row.ToArray());
        //    }));
        //}

        //void zkRdr_OnWriteCard(int EnrollNumber, int ActionResult, int Length)
        //{
        //    row.Clear();
        //    row.Add(ConnPlace);
        //    row.Add(DateTime.Now);
        //    row.Add("Write Card... EnrollNum: "+EnrollNumber+" Action: "+ActionResult);
        //    Invoke((MethodInvoker)delegate
        //    {
        //        gridView3.Rows.Add(row.ToArray());
        //    });
        //}

        //void zkRdr_OnEmptyCard(int ActionResult)
        //{
        //    row.Clear();
        //    row.Add(ConnPlace);
        //    row.Add(DateTime.Now);
        //    row.Add("Empty Card... "+ ActionResult);
        //    Invoke((MethodInvoker)delegate
        //    {
        //        gridView3.Rows.Add(row.ToArray());
        //    });
        //}
    }

}

public class CustomToolStripRenderer : ToolStripProfessionalRenderer
{
    public static Color color1 { get; set; }
    public static Color color2 { get; set; }
    public CustomToolStripRenderer() { }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        //you may want to change this based on the toolstrip's dock or layout style
        LinearGradientMode mode = LinearGradientMode.Vertical;

        using (LinearGradientBrush b = new LinearGradientBrush(e.AffectedBounds, color1, color2, mode))
        {
            e.Graphics.FillRectangle(b, e.AffectedBounds);
        }
    }
}


