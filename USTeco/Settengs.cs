using System;

using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.OleDb;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using zkemkeeper;
using System.Net.NetworkInformation;
using System.Net;
using RecogSys.RdrAccess;
using System.Windows.Media;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Net.Http;


namespace USTeco
{
    public partial class Setup : Form
    {
        const int TRUE = 1;
        const int FALSE = 0;
        OleDbCommand cmd;
        OleDbDataReader reader;
        CZKEM zkRdr = new CZKEM();
        CRsiComWinsock myIP = new CRsiComWinsock();
        CRsiHandReader myRdr = new CRsiHandReader();
        OracleConnection oraCon = new OracleConnection();
        SqlConnection sqlCon = new SqlConnection();
        string mySql, PlaceName;
        string currIpAddr, currDevType, currPass;
        int currPort, currDevId, currDevId2;
        int MyPlace;
        OleDbConnection accCon = new OleDbConnection();
        string myDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UST Applications", "Handpunch");

        public Setup()
        {
            InitializeComponent();
            tabs.TabPages.Remove(tabPage5);
            tabs.TabPages.Remove(tabPage6);
            BackColor = MyGlobal.color2;
            //panel1.BackColor = MyGlobal.color1;
            panel2.BackColor = MyGlobal.color1;
            panel3.BackColor = MyGlobal.color1;
            panel4.BackColor = MyGlobal.color1;
            panel5.BackColor = MyGlobal.color1;
            //panel6.BackColor = MyGlobal.color1;
            panel7.BackColor = MyGlobal.color1;
            panel14.BackColor = MyGlobal.color1;
            panel15.BackColor = MyGlobal.color1;
            panel16.BackColor = MyGlobal.color2;
            panel17.BackColor = MyGlobal.color2;    
            groupBox1.BackColor = MyGlobal.color1;
            listBox1.BackColor = MyGlobal.color1;
            tabPage4.BackColor = MyGlobal.color1;
            dataGridView1.BackgroundColor = MyGlobal.color1;
            secPanel.BackColor = MyGlobal.color1;
            accCon.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + myDataPath + "\\handdb.mdb; Jet OLEDB:Database Password = Ajoset25";
            accCon.Open();
            switch (MyGlobal.IsGuest)
            {
                case 0:
                    {
                        tabPage1.Dispose();
                        tabPage2.Dispose();
                        tabPage4.Dispose();
                        //tabPage7.Dispose();
                        groupBox3.Visible = false;
                        break;
                    }
                case 1:
                    {
                        tabPage4.Dispose();
                        //tabPage7.Dispose();
                        break;
                    }
            }
            secPanel.Visible = (MyGlobal.IsGuest == 3);
            if (MyGlobal.IsGuest == 3)
                panel7.Width = 750;
            else
                panel7.Width = 615;
            mySql = "Select Place_id,Place_Name,Dev_Type From Devices";
            comboBox1.Items.Add("All Devices");
            comboBox2.Items.Add("All Devices");
            comboBox4.Items.Add("Select Device");
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string MyName;
                MyPlace = reader.GetInt32(0);
                try
                {
                    MyName = reader.GetString(1);
                }
                catch
                {
                    MyName = "";
                }
                comboBox1.Items.Add(MyPlace.ToString() + " " + MyName);
                comboBox2.Items.Add(MyPlace.ToString() + " " + MyName);
                if (reader.GetString(2) == "Zkteco")
                {
                    comboBox4.Items.Add(MyPlace.ToString() + " " + MyName);
                }
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            reader.Close();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            string mySql;
            string strSql = "Select ID,IP_Addr,Port_No,Dev_Id,Dev_Type,Place_Id," +
                "Place_Name,Pass_Word,Dev_Status,Auto_Clear From Devices Order By ID";
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(strSql, accCon))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            };
            //strSql = "Update Employee set Emp_Shift=0";
            //cmd = new OleDbCommand(strSql, accCon);
            //cmd.ExecuteNonQuery();
            strSql = "Select * from employee Order By Emp_id";
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(strSql, accCon))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView2.DataSource = table;
            };
            mySql = "Select * From Settings";
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                
                serverTxt.Text = reader.GetString(1);
                portNo.Text = reader.GetInt32(2).ToString();
                sidTxt.Text = reader.GetString(3);
                userTxt.Text = reader.GetString(4);
                passTxt.Text = reader.GetString(5);
                periodTxt.Text = reader.GetInt32(6).ToString();
                showLogCheck.Checked = reader.GetBoolean(7);
                autoStartCheck.Checked = reader.GetBoolean(8);
                stopFailedCheck.Checked = reader.GetBoolean(9);
                showDetails.Checked = reader.GetBoolean(10);
                dataCheck.Checked = reader.GetBoolean(11);
                rOracle.Checked = (reader.GetInt32(12) == 0);
                rSqlServer.Checked = (reader.GetInt32(12) == 1);
                //rAccess.Checked = (reader.GetInt32(12) == 2);
                sqlServerTxt.Text = reader.GetString(13);
                dbaseTxt.Text = reader.GetString(14);
                checkBox4.Checked = reader.GetBoolean(17);
                checkBox5.Checked = reader.GetBoolean(18);
                checkBox6.Checked = reader.GetBoolean(19);
                checkBox7.Checked = reader.GetBoolean(20);
                checkBox8.Checked = reader.GetBoolean(21);

                sqlServerTxt.Text = MyGlobal.sqlServer;
                dbaseTxt.Text = MyGlobal.sqlUser;
                mysqlServer.Text = MyGlobal.mysqlServer;
                mysqlDatabase.Text = MyGlobal.mysqlDatabase;
                mysqlUid.Text = MyGlobal.mysqlUid;
                mysqlPasswd.Text = MyGlobal.mysqlPassword;
                apiLink.Text = MyGlobal.apiLink;
            }
            reader.Close();
            mySql = "Select * From ShiftsNames";
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            int myId = 0;
            while (reader.Read())
            {
                myId = reader.GetInt32(1);
                if (myId > 0)
                {
                    comboBox3.Items.Add(myId + "-" + reader.GetString(2));
                }
            }
            reader.Close();
            comboBox3.SelectedIndex = 0;
            comboBox3_SelectedIndexChanged(null, null);
            shiftApplyButt.Enabled = false;
            settSave.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (idTxt.Text != "")
            {
                if (addrTxt.Text != "" && portTxt.Text != "" && placeName.Text != "" &&
                devId.Text != "" && devType.Text != "" && placeTxt.Text != "")
                {
                    if (password.Text.Trim() == "")
                    {
                        password.Text = "0";
                    }
                    string my_querry = "Update Devices set IP_Addr='" + addrTxt.Text +
                        "',Port_No=" + Int32.Parse(portTxt.Text) + ",Dev_Id=" + Int32.Parse(devId.Text) +
                        ",Dev_Type='" + devType.Text + "',Place_Id=" + placeTxt.Text +
                        ",Place_Name='" + placeName.Text + "',Pass_Word='" + password.Text +
                        "',Dev_Status=" + checkBox1.Checked + ",Auto_Clear=" + checkBox3.Checked +
                        " Where ID=" + Int32.Parse(idTxt.Text);
                    cmd = new OleDbCommand(my_querry, accCon);
                    cmd.ExecuteNonQuery();
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show("Please fill all required fields!", "Setup");
                }
            }
            else
            {
                MessageBox.Show("Please select thr record you want to modify.", "Setup");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (idTxt.Text == "")
            {
                if (addrTxt.Text != "" && portTxt.Text != "" &&
                devId.Text != "" && devType.Text != "" && placeTxt.Text != "")
                {
                    string my_querry = "Insert Into Devices (IP_Addr,Port_No,Dev_Id," +
                        "Dev_Type,Place_id,Place_Name,Pass_Word,Dev_status,Auto_Clear) values('" +
                        addrTxt.Text + "'," + Int32.Parse(portTxt.Text) + "," +
                        Int32.Parse(devId.Text) + ",'" + devType.Text + "'," +
                        placeTxt.Text + ",'" + placeName.Text + "','" + password.Text + "'," +
                        checkBox1.Checked + "," + checkBox3.Checked + ")";
                    cmd = new OleDbCommand(my_querry, accCon);
                    cmd.ExecuteNonQuery();
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show("Please fill all required fields!", "Setup");
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (idTxt.Text != "")
            {
                DialogResult result = MessageBox.Show("Are you sure to delete this device?",
                    "Confirm Message",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    String my_querry = "Delete From Devices Where ID=" + Int32.Parse(idTxt.Text);
                    cmd = new OleDbCommand(my_querry, accCon);
                    cmd.ExecuteNonQuery();
                    RefreshGrid();
                }
            }
            else
            {
                MessageBox.Show("Please select the record you want to delete.", "Setup");
            }
        }

        private void RefreshGrid()
        {
            idTxt.Text = "";
            addrTxt.Text = "";
            //portTxt.Text = "";
            //devId.Text = "";
            devType.Text = "";
            placeTxt.Text = "";
            placeName.Text = "";
            password.Text = "";
            checkBox1.Checked = false;
            checkBox3.Checked = false;
            string query1 = "SELECT ID,IP_Addr,Port_No,Dev_Id,Dev_Type,Place_Id," +
                "Place_Name,Pass_Word,Dev_Status,Auto_Clear FROM Devices order by ID";
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(query1, accCon))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                this.dataGridView1.DataSource = table;
            }
        }

        private void RefreshGrid2()
        {
            indexText.Text = "";
            idText.Text = "";
            nameText.Text = "";
            shiftText.Text = "";
            string query1 = "SELECT * FROM Employee order by emp_id";
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(query1, accCon))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView2.DataSource = table;
            }
        }

        private void settSave_Click(object sender, EventArgs e)
        {
            if (rOracle.Checked)
            {
                if (serverTxt.Text.Trim() == "" || portNo.Text == "" || sidTxt.Text.Trim() == "" ||
                    userTxt.Text.Trim() == "" || periodTxt.Text == "")
                {
                    MessageBox.Show("Please fill all required fields!", "Setup",
                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            else if (rSqlServer.Checked)
            {
                if (sqlServerTxt.Text.Trim() == "" || dbaseTxt.Text.Trim() == "")
                {
                    MessageBox.Show("Please fill all required fields!", "Setup",
                            MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            if (mysqlServer.Text.Trim() == "" || mysqlDatabase.Text == "" || mysqlUid.Text.Trim() == "")
            {
                MessageBox.Show("Please fill all required mysql server fields!", "Setup",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (apiLink.Text.Trim() == "")
            {
                MessageBox.Show("Please enter API Link field", "Setup",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            String my_querry = "Select * from Settings";
            int serverType;
            if (rOracle.Checked)
            {
                serverType = 0;
            }
            else
            {
                serverType = 1;
            }
            
            cmd = new OleDbCommand(my_querry, accCon);
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            reader.Read();

            if (reader.HasRows)
            {
                my_querry = "Update Settings set Server='" + serverTxt.Text +
                    "',Port=" + portNo.Text + ",Service_name='" +
                    sidTxt.Text + "',User_Name='" + userTxt.Text +
                    "',Pass_Word='" + passTxt.Text + "',server_type=" + serverType +
                    ",sql_server='" + sqlServerTxt.Text + "',sql_user='" + dbaseTxt.Text +
                    "',mysql_server='" + mysqlServer.Text + "',mysql_database='" + mysqlDatabase.Text +
                    "',mysql_uid='" + mysqlUid.Text + "',mysql_password='" + mysqlPasswd.Text +
                    "',apiLink='" + apiLink.Text + "'";

                //my_querry = "Update Settings set Server='" + serverTxt.Text +
                //    "',Port=" + portNo.Text + ",Service_name='" +
                //    sidTxt.Text + "',User_Name='" + userTxt.Text +
                //    "',Pass_Word='" + passTxt.Text + "',Reading_Period = " +
                //    Int32.Parse(periodTxt.Text) + ",server_type=" + serverType +
                //    ",sql_server='" + sqlServerTxt.Text + "',sql_user='" + dbaseTxt.Text + "'";

            }
            else
            {
                my_querry = "Insert into Settings (Server,Port,Service_name,User_Name," +
                    "Pass_Word,server_type,sql_server,sql_user,mysql_server," +
                    "mysql_database,mysql_uid,mysql_password,apiLink) " +
                    "Values ('" + serverTxt.Text + "'," + portNo.Text + ",'" + sidTxt.Text +
                    "','" + userTxt.Text + "','" + passTxt.Text + "'," + mysqlServer.Text +
                    "," + serverType + ",'" + sqlServerTxt.Text + "','" + dbaseTxt.Text +
                    "','" + mysqlServer.Text + "','" + mysqlDatabase.Text + "','" + mysqlUid.Text +
                    "','" + mysqlPasswd + "','" + apiLink.Text + "')";

                //my_querry = "Insert into Settings (Server,Port,Service_name,User_Name," +
                //    "Pass_Word,Reading_period,server_type,sql_server,sql_user) " +
                //    "Values ('" + serverTxt.Text + "'," + portNo.Text + ",'" + sidTxt.Text +
                //    "','" + userTxt.Text + "','" + passTxt.Text + "'," + periodTxt.Text +
                //    "," + serverType + "," + sqlServerTxt.Text + "," + dbaseTxt.Text + ")";
            }
            cmd = new OleDbCommand(my_querry, accCon);
            cmd.ExecuteNonQuery();
            reader.Close();
            settSave.Enabled = false;
            MyGlobal.MyServer = serverTxt.Text;
            MyGlobal.MyPort = Int32.Parse(portNo.Text);
            MyGlobal.MySid = sidTxt.Text;
            MyGlobal.MyUser = userTxt.Text;
            MyGlobal.MyPass = passTxt.Text;
            MyGlobal.ServerType = serverType;
            MyGlobal.sqlServer = sqlServerTxt.Text;
            MyGlobal.sqlUser = dbaseTxt.Text;
            MyGlobal.MyPeriod = Int32.Parse(periodTxt.Text);
            if (!CheckDatabase())
            {
                MessageBox.Show("Create EMS_EMP_FINGERS table failed!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private bool CheckDatabase()
        {
            OracleConnection oraCon = new OracleConnection();
            SqlConnection sqlCon = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();
            OracleCommand OrCmd = new OracleCommand();
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
                        MessageBox.Show("Listener does not exist", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        success = false;
                    }
                    if (success)
                    {
                        success = false;
                        string my_querry = "Select * From EMS_EMP_FINGERS16";
                        try
                        {
                            OrCmd = new OracleCommand(my_querry, oraCon);
                            OrCmd.ExecuteReader();
                            success = true;
                        }
                        catch
                        {
                            my_querry = "CREATE TABLE EMS_EMP_FINGERS16 " +
                                "(EMP_ID NUMBER,SIGN_DATE DATE,SIGN_TIME DATE," +
                                "SIGN_PLC_ID NUMBER,STATUS NUMBER,DEV_LOC NUMBER," +
                                "SIGN_SEQ NUMBER,TIME_STAMP DATE  DEFAULT sysdate)";
                            OrCmd = new OracleCommand(my_querry, oraCon);
                            OrCmd.ExecuteNonQuery();
                            my_querry = "CREATE OR REPLACE TRIGGER EMS_EMP_FINGERS_SEQ16 " +
                                "BEFORE INSERT ON EMS_EMP_FINGERS16 REFERENCING NEW AS NEW OLD AS OLD " +
                                "FOR EACH ROW " +
                                    "DECLARE SEQ NUMBER:= 0; " +
                                    "BEGIN " +
                                        "IF INSERTING THEN " +
                                            "SELECT NVL(MAX(SIGN_SEQ),0)+1 INTO SEQ FROM EMS_EMP_FINGERS16; " +
                                                "IF :NEW.SIGN_SEQ IS NULL THEN " +
                                                    ":NEW.SIGN_SEQ:= SEQ; " +
                                                "END IF; " +
                                        "END IF; " +
                                    "END;";
                            OrCmd = new OracleCommand(my_querry, oraCon);
                            OrCmd.ExecuteNonQuery();
                            success = true;
                        }
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
                            sqlcmd = new SqlCommand(mySql, sqlCon);
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            mySql = "Create Database " + MyGlobal.sqlUser;
                            sqlcmd = new SqlCommand(mySql, sqlCon);
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

        private void serverTxt_TextChanged(object sender, EventArgs e)
        {
            settSave.Enabled = true;
        }

        private void portTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void showLogCheck_Click(object sender, EventArgs e)
        {
            optAppBtn.Enabled = true;
        }

        private void optAppBtn_Click(object sender, EventArgs e)
        {
            String my_querry = "Select * from Settings";
            cmd = new OleDbCommand(my_querry, accCon);
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                my_querry = "Update Settings set show_log=" + showLogCheck.Checked +
                    ",auto_start=" + autoStartCheck.Checked + ",stop_failed=" + stopFailedCheck.Checked +
                    ",show_details=" + showDetails.Checked + ",data_check=" + dataCheck.Checked +
                    ",showDevice=" + checkBox4.Checked + ",showRepA=" + checkBox5.Checked +
                    ",showRepB=" + checkBox6.Checked + ",showHist=" + checkBox7.Checked +
                    ",RepFromLog=" + checkBox8.Checked;
            }
            else
            {
                my_querry = "Insert into Settings (Server,Port,Service_name,User_Name,Pass_Word,Reading_period,show_log," +
                            "auto_stop,stop_failed,show_details,data_check,showDevice,ShowRepA,showRepB,showHist) " +
                    "Values ('" + serverTxt.Text + "'," + portNo.Text + ",'" + sidTxt.Text + "','" + userTxt.Text +
                    "','" + passTxt.Text + "'," + periodTxt.Text + "," + showLogCheck.Checked + "," + autoStartCheck.Checked +
                    "," + stopFailedCheck.Checked + "," + showDetails.Checked + "," + dataCheck.Checked +
                    "," + checkBox4.Checked + "," + checkBox5.Checked +
                    "," + checkBox6.Checked + "," + checkBox7.Checked +
                    "," + checkBox8.Checked + ")";
            }
            reader.Close();
            cmd = new OleDbCommand(my_querry, accCon);
            cmd.ExecuteNonQuery();
            optAppBtn.Enabled = false;
            MyGlobal.showLogs = showLogCheck.Checked;
            MyGlobal.autoStart = autoStartCheck.Checked;
            MyGlobal.stopFailed = stopFailedCheck.Checked;
            MyGlobal.showDetails = showDetails.Checked;
            MyGlobal.dataCheck = dataCheck.Checked;
            MyGlobal.RepFromLog = checkBox8.Checked;
        }

        private void serverTxt_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "To enter IP Address of server.";
        }

        private void serverTxt_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void portNo_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "To enter Port No. Default is 3001.";
        }

        private void sidTxt_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Enter SID or Service Name of instance oracle database.";
        }

        private void userTxt_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Enter Authorized User Name.";
        }

        private void passTxt_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Enter Password";
        }

        private void periodTxt_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Eneter the period (in minute) between the reading process.";
        }

        private void addrTxt_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "IP Address of the finger print device.";
        }

        private void portTxt_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Port No. of the finger print device. (default 3001)";
        }

        private void devId_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Device Id of the finger print device. (default 1)";
        }

        private void devType_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Finger print device type. (zkt,punch,...etc)";
        }

        private void placeTxt_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Place Id where the finger print device installed. (default 1)";
        }

        private void connPass_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Password of finger print device if setup. (default empty)";
        }

        private void showLogCheck_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "To show the current status of each finger print device.";
        }

        private void autoStartCheck_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "To start reading once the application started.";
        }

        private void stopFailedCheck_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "To stop checking the failed device.";
        }

        private void showSummary_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Show the reading result of each finger print device as a total.";
        }

        private void showDetails_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Show the reading result of each finger print device on detail.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select the location of HandPunch");
                return;
            }
            mySql = "Select * From Devices Where Place_id =" +
                comboBox1.SelectedItem.ToString().Split(' ')[0];

            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            currIpAddr = reader.GetString(1);
            currPort = reader.GetInt32(2);
            currDevId = reader.GetInt32(3);
            currDevType = reader.GetString(4).ToUpper();
            currPass = reader.GetString(8).ToUpper();
            switch (currDevType.ToUpper())
            {
                case "HANDPUNCH":
                    Cursor.Current = Cursors.WaitCursor;
                    if (ConnectPunch(currIpAddr, currPort, currDevId))
                    {
                        RSI_TIME_DATE pTime = new RSI_TIME_DATE();
                        myRdr.CmdGetTime(pTime);
                        pTime.hour = (byte)(timePicker1.Value.Hour);
                        pTime.minute = (byte)(timePicker1.Value.Minute);
                        myRdr.CmdPutTime(pTime);
                        myIP.Disconnect();
                        myIP.ResetSocket();
                    }
                    Cursor.Current = Cursors.Default;
                    break;
                case "ZKTECO":
                    Cursor.Current = Cursors.WaitCursor;
                    if (ZktConnect(currIpAddr, currPort, PlaceName))
                    {
                        zkRdr.SetDeviceTime2(currDevId, timePicker1.Value.Year,
                            timePicker1.Value.Month, timePicker1.Value.Day,
                            timePicker1.Value.Hour, timePicker1.Value.Minute,
                            timePicker1.Value.Second);
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        int errRef = 0;
                        zkRdr.GetLastError(errRef);
                        listBox1.Items.Add("Unable to connect (" + PlaceName + ") " + DateTime.Now);
                        listBox1.Update();
                        Cursor.Current = Cursors.Default;
                    }
                    Cursor.Current = Cursors.Default;
                    break;
                default:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select the location of HandPunch");
                return;
            }
            mySql = "Select * From Devices Where Place_id =" +
                comboBox2.SelectedItem.ToString().Split(' ')[0];

            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            currIpAddr = reader.GetString(1);
            currPort = reader.GetInt32(2);
            currDevId = reader.GetInt32(3);
            currDevType = reader.GetString(4).ToUpper();
            currPass = reader.GetString(8).ToUpper();
            switch (currDevType.ToUpper())
            {
                case "HANDPUNCH":
                    //Cursor.Current = Cursors.WaitCursor;
                    //RSI_TIME_DATE pDate = new RSI_TIME_DATE();
                    //if (ConnectPunch(currIpAddr, currPort, currDevId))
                    //{
                    //    myRdr.CmdGetTime(pDate);
                    //    pDate.year = (byte)int.Parse(((datePicker1.Value.Year).ToString().Substring(2, 2)));
                    //    pDate.month = (byte)(datePicker1.Value.Month);
                    //    pDate.day = (byte)(datePicker1.Value.Day);
                    //    MessageBox.Show(pDate.year.ToString());
                    //    myRdr.CmdPutTime(pDate);
                    //    myIP.Disconnect();
                    //    myIP.ResetSocket();
                    //}
                    //Cursor.Current = Cursors.Default;
                    break;
                case "ZKTECO":
                    Cursor.Current = Cursors.WaitCursor;
                    if (ZktConnect(currIpAddr, currPort, PlaceName))
                    {
                        zkRdr.SetDeviceTime2(currDevId2, timePicker1.Value.Year,
                            timePicker1.Value.Month, timePicker1.Value.Day,
                            timePicker1.Value.Hour, timePicker1.Value.Minute,
                            timePicker1.Value.Second);
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        int errRef = 0;
                        zkRdr.GetLastError(errRef);
                        listBox1.Items.Add("Unable to connect (" + PlaceName + ") " + DateTime.Now);
                        listBox1.Update();
                        Cursor.Current = Cursors.Default;
                    }
                    Cursor.Current = Cursors.Default;
                    break;
                default:
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fmt = "00";
            if (comboBox1.SelectedIndex == 0)
            {
                return;
            }
            mySql = "Select * From Devices Where Place_id =" +
                comboBox1.SelectedItem.ToString().Split(' ')[0];
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            currIpAddr = reader.GetString(1);
            currPort = reader.GetInt32(2);
            currDevId = reader.GetInt32(3);
            currDevType = reader.GetString(4).ToUpper();
            currPass = reader.GetString(8).ToUpper();
            switch (currDevType.ToUpper())
            {
                case "HANDPUNCH":
                    Cursor.Current = Cursors.WaitCursor;
                    label7.Text = "";
                    if (ConnectPunch(currIpAddr, currPort, currDevId))
                    {
                        RSI_TIME_DATE pTime = new RSI_TIME_DATE();
                        myRdr.CmdGetTime(pTime);
                        label7.Text = pTime.hour.ToString(fmt) + ":" + pTime.minute.ToString(fmt) + ":" + pTime.second.ToString(fmt);
                        myIP.Disconnect();
                        myIP.ResetSocket();
                    }
                    Cursor.Current = Cursors.Default;
                    break;
                case "ZKTECO":
                    int dwYear = 0; int dwMonth = 0; int dwDay = 0;
                    int dwHour = 0; int dwMinute = 0; int dwSecond = 0;
                    Cursor.Current = Cursors.WaitCursor;
                    label7.Text = "";
                    if (ZktConnect(currIpAddr, currPort, PlaceName))
                    {
                        zkRdr.GetDeviceTime(currDevId, ref dwYear, ref dwMonth, ref dwDay,
                                ref dwHour, ref dwMinute, ref dwSecond);
                        label7.Text = dwHour.ToString(fmt) + ":" +
                            dwMinute.ToString(fmt) + ":" + dwSecond.ToString(fmt);
                        label7.Update();
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        listBox1.Items.Add("Unable to connect (" + PlaceName + ") " + DateTime.Now);
                        listBox1.Update();
                        Cursor.Current = Cursors.Default;
                    }
                    Cursor.Current = Cursors.Default;
                    break;
                default:
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fmt = "00";
            string fmy = "2000";
            if (comboBox2.SelectedIndex == 0)
            {
                return;
            }
            mySql = "Select * From Devices Where Place_id =" +
                comboBox2.SelectedItem.ToString().Split(' ')[0];
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            currIpAddr = reader.GetString(1);
            currPort = reader.GetInt32(2);
            currDevId2 = reader.GetInt32(3);
            currDevType = reader.GetString(4).ToUpper();
            currPass = reader.GetString(8).ToUpper();
            switch (currDevType.ToUpper())
            {
                case "HANDPUNCH":
                    Cursor.Current = Cursors.WaitCursor;
                    if (ConnectPunch(currIpAddr, currPort, currDevId2))
                    {
                        RSI_TIME_DATE pDate = new RSI_TIME_DATE();
                        myRdr.CmdGetTime(pDate);
                        label8.Text = pDate.day.ToString(fmt) + "/" + pDate.month.ToString(fmt) + "/" + pDate.year.ToString(fmy);
                        myIP.Disconnect();
                        myIP.ResetSocket();
                    }
                    Cursor.Current = Cursors.Default;
                    break;
                case "ZKTECO":
                    int dwYear = 0; int dwMonth = 0; int dwDay = 0;
                    int dwHour = 0; int dwMinute = 0; int dwSecond = 0;
                    Cursor.Current = Cursors.WaitCursor;
                    if (ZktConnect(currIpAddr, currPort, PlaceName))
                    {
                        zkRdr.GetDeviceTime(currDevId, ref dwYear, ref dwMonth, ref dwDay,
                                ref dwHour, ref dwMinute, ref dwSecond);
                        label8.Text += dwYear.ToString() + "/" +
                            dwMonth.ToString(fmt) + "/" + dwDay.ToString(fmt);
                        label8.Update();
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        int errRef = 0;
                        zkRdr.GetLastError(errRef);
                        listBox1.Items.Add("Unable to connect (" + PlaceName + ") " + DateTime.Now);
                        listBox1.Update();
                        Cursor.Current = Cursors.Default;
                    }
                    Cursor.Current = Cursors.Default;
                    break;
                default:
                    break;
            }
        }

        private bool ConnectPunch(String ipAdd, int port, int devId)
        {
            myIP.SetHost(ipAdd);
            myIP.SetPortA(ushort.Parse(port.ToString()));
            CRsiHandReader myRdr1 = new CRsiHandReader(myIP, byte.Parse(devId.ToString()));
            if (TRUE == myIP.Ping())
            {
                if (MyGlobal.showLogs)
                {
                    listBox1.Items.Add("Success ping with device no: " + MyPlace);
                    listBox1.Update();
                }
                myIP.Disconnect();
                myIP.ResetSocket();
                myIP.Connect();
                if (FALSE == myIP.IsConnected())
                {
                    myIP.Disconnect();
                    myIP.ResetSocket();
                    label5.Text = "";
                    return false;
                }
                else
                {
                    if (MyGlobal.showLogs)
                    {
                        listBox1.Items.Add("Connected to device no: " + MyPlace);
                        listBox1.Update();
                    }
                    CRsiNetwork myNetwork = new CRsiNetwork(myIP);
                    myNetwork.Attach(myRdr);
                    RSI_TIME_DATE currentDateTime = new RSI_TIME_DATE();
                    myRdr.SetAddress(1);
                    myRdr.CmdGetTime(currentDateTime);        // Gets current time from reader.
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idText.Text) || string.IsNullOrEmpty(nameText.Text))
            {
                MessageBox.Show("Please fill Emp. No. and Emp. Name ", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                idText.Focus();
            }
            else
            {
                if (string.IsNullOrEmpty(shiftText.Text.Trim()))
                {
                    shiftText.Text = "0";
                }
                string my_querry = "Insert Into Employee (emp_id,emp_name,emp_shift) values('" +
                    idText.Text + "','" + nameText.Text + "'," + shiftText.Text + ")";
                cmd = new OleDbCommand(my_querry, accCon);
                cmd.ExecuteNonQuery();
                RefreshGrid2();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            idTxt.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            addrTxt.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            portTxt.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            devId.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            devType.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            placeTxt.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            placeName.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            password.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            checkBox1.Checked = bool.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());
            checkBox3.Checked = bool.Parse(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());
            checkBox3.Enabled = (devType.SelectedIndex == 1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(indexText.Text))
            {
                MessageBox.Show("Please choose the name you want to modify", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (string.IsNullOrEmpty(idText.Text) || string.IsNullOrEmpty(nameText.Text))
                {
                    MessageBox.Show("Please fill Emp. No. and Emp. Name ", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    idText.Focus();
                }
                else
                {
                    if (string.IsNullOrEmpty(shiftText.Text.Trim()))
                    {
                        shiftText.Text = "0";
                    }
                    string my_querry = "Update employee set emp_id='" + idText.Text +
                        "',emp_name='" + nameText.Text + "',emp_shift1=" + shiftText.Text +
                        " Where ID=" + Int32.Parse(indexText.Text);
                    cmd = new OleDbCommand(my_querry, accCon);
                    cmd.ExecuteNonQuery();
                    RefreshGrid2();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(indexText.Text))
            {
                MessageBox.Show("Please choose the name you want to delete", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure to delete this device?",
                    "Confirm Message",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    string my_querry = "Delete From Employee Where ID=" + Int32.Parse(indexText.Text);
                    cmd = new OleDbCommand(my_querry, accCon);
                    cmd.ExecuteNonQuery();
                    RefreshGrid2();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int Shtid;
            if (checkDefault.Checked)
            {
                Shtid = 0;
            }
            else
            {
                Shtid = Int32.Parse(comboBox3.SelectedItem.ToString().Split('-')[0]);
            }
            string sql = "Select ShiftId From ShiftsTimes Where ShiftId=" + Shtid;
            cmd = new OleDbCommand(sql, accCon);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                sql = "Update ShiftsTimes Set " +
                    "day1=" + checkDay1.Checked + ",start1='" + sTime1.Text + "',end1='" + eTime1.Text + "'," +
                    "day2=" + checkDay2.Checked + ",start2='" + sTime2.Text + "',end2='" + eTime2.Text + "'," +
                    "day3=" + checkDay3.Checked + ",start3='" + sTime3.Text + "',end3='" + eTime3.Text + "'," +
                    "day4=" + checkDay4.Checked + ",start4='" + sTime4.Text + "',end4='" + eTime4.Text + "'," +
                    "day5=" + checkDay5.Checked + ",start5='" + sTime5.Text + "',end5='" + eTime5.Text + "'," +
                    "day6=" + checkDay6.Checked + ",start6='" + sTime6.Text + "',end6='" + eTime6.Text + "'," +
                    "day7=" + checkDay7.Checked + ",start7='" + sTime7.Text + "',end7='" + eTime7.Text + "' " +
                    "Where ShiftId=" + Shtid;
                MessageBox.Show(sql);
                cmd = new OleDbCommand(sql, accCon);
                cmd.ExecuteNonQuery();
            }
            else
            {
                reader.Close();
                sql = "Insert Into ShiftsTimes (ShiftId,day1,start1,end1,day2,start2,end2," +
                    "day3,start3,end3,day4,start4,end4,day5,start5,end5,day6,start6,end6," +
                    "day7,start7,end7) " +
                    "Values(" + Shtid + "," + checkDay1.Checked + ",'" + sTime1.Text + "','" + eTime1.Text + "'," +
                    checkDay2.Checked + ",'" + sTime2.Text + "','" + eTime2.Text + "'," +
                    checkDay3.Checked + ",'" + sTime3.Text + "','" + eTime3.Text + "'," +
                    checkDay4.Checked + ",'" + sTime4.Text + "','" + eTime4.Text + "'," +
                    checkDay5.Checked + ",'" + sTime5.Text + "','" + eTime5.Text + "'," +
                    checkDay6.Checked + ",'" + sTime6.Text + "','" + eTime6.Text + "'," +
                    checkDay7.Checked + ",'" + sTime7.Text + "','" + eTime7.Text + "')";
                cmd = new OleDbCommand(sql, accCon);
                cmd.ExecuteNonQuery();
            }
            shiftApplyButt.Enabled = false;
        }

        private void checkDefault_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = !checkDefault.Checked;
            //if (checkDefault.Checked)
            //{
            comboBox3_SelectedIndexChanged(sender, null);
            //}
            shiftApplyButt.Enabled = true;
        }

        private void checkDay1_CheckedChanged(object sender, EventArgs e)
        {
            shiftApplyButt.Enabled = true;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Shtid;
            if (checkDefault.Checked)
            {
                Shtid = 0;
            }
            else
            {
                Shtid = Int32.Parse(comboBox3.SelectedItem.ToString().Split('-')[0]);
            }
            string sql = "Select * From ShiftsTimes Where ShiftId=" + Shtid;
            cmd = new OleDbCommand(sql, accCon);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                checkDay1.Checked = reader.GetBoolean(2);
                sTime1.Text = reader.GetString(3);
                eTime1.Text = reader.GetString(4);
                checkDay2.Checked = reader.GetBoolean(5);
                sTime2.Text = reader.GetString(6);
                eTime2.Text = reader.GetString(7);
                checkDay3.Checked = reader.GetBoolean(8);
                sTime3.Text = reader.GetString(9);
                eTime3.Text = reader.GetString(10);
                checkDay4.Checked = reader.GetBoolean(11);
                sTime4.Text = reader.GetString(12);
                eTime4.Text = reader.GetString(13);
                checkDay5.Checked = reader.GetBoolean(14);
                sTime5.Text = reader.GetString(15);
                eTime5.Text = reader.GetString(16);
                checkDay6.Checked = reader.GetBoolean(17);
                sTime6.Text = reader.GetString(18);
                eTime6.Text = reader.GetString(19);
                checkDay7.Checked = reader.GetBoolean(20);
                sTime7.Text = reader.GetString(21);
                eTime7.Text = reader.GetString(22);
            }
            else
            {
                times_reset();
            }
            reader.Close();
            shiftApplyButt.Enabled = true;
        }

        private void times_reset()
        {
            checkDay1.Checked = false;
            sTime1.Text = "08:00AM";
            eTime1.Text = "08:00AM";
            checkDay2.Checked = false;
            sTime2.Text = "08:00AM";
            eTime2.Text = "08:00AM";
            checkDay3.Checked = false;
            sTime3.Text = "08:00AM";
            eTime3.Text = "08:00AM";
            checkDay4.Checked = false;
            sTime4.Text = "08:00AM";
            eTime4.Text = "08:00AM";
            checkDay5.Checked = false;
            sTime5.Text = "08:00AM";
            eTime5.Text = "08:00AM";
            checkDay6.Checked = false;
            sTime6.Text = "08:00AM";
            eTime6.Text = "08:00AM";
            checkDay7.Checked = false;
            sTime7.Text = "08:00AM";
            eTime7.Text = "08:00AM";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //RSI_DATALOG myLogData = new RSI_DATALOG();
            //RSI_READER_INFO myRdrInfo = new RSI_READER_INFO();
            //RSI_TIME_DATE pTime = new RSI_TIME_DATE();
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select the location of HandPunch");
                comboBox1.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter employye no. ", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }
            string myDate = datePicker2.Value.Day.ToString("00/") +
                datePicker2.Value.Month.ToString("00/") +
                datePicker2.Value.Year.ToString("0000");
            string myTime = timePicker2.Value.Hour.ToString("00:") +
                timePicker2.Value.Minute.ToString("00:") +
                timePicker2.Value.Second.ToString("00");
            if (!checkBox2.Checked)
            {
                mySql = "Select * From Devices Where Place_id =" +
                    comboBox1.SelectedItem.ToString().Split(' ')[0];
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Read();
                currIpAddr = reader.GetString(1);
                currPort = reader.GetInt32(2);
                currDevId = reader.GetInt32(3);
                currDevType = reader.GetString(4).ToUpper();
                currPass = reader.GetString(8).ToUpper();
                PlaceName = reader.GetString(7).ToUpper();
                switch (currDevType.ToUpper())
                {
                    case "HANDPUNCH":
                        //Cursor.Current = Cursors.WaitCursor;
                        //if (ConnectPunch(currIpAddr, currPort, currDevId))
                        //{
                        //    //myLogData.format = RSI_DATALOG_FORMAT.;
                        //    myLogData.pTimestamp.day = (byte)datePicker2.Value.Day;
                        //    myLogData.pTimestamp.month = (byte)datePicker2.Value.Month;
                        //    myLogData.pTimestamp.year = byte.Parse(datePicker2.Value.ToString("yy"));
                        //    myLogData.pTimestamp.hour = (byte)timePicker2.Value.Hour;
                        //    myLogData.pTimestamp.minute = (byte)timePicker2.Value.Minute;
                        //    myLogData.pTimestamp.second = (byte)timePicker2.Value.Second;
                        //    myLogData.pOperand.SetID(textBox1.Text);
                        //    myLogData.pOperator.SetID(textBox1.Text);

                        //    //myLogData.pMenuData
                        //    //myLogData.pDepartment.CopyDataB(myLogData.pOperator);

                        //    //MessageBox.Show(myLogData.pOperator.GetID());
                        //    //return;
                        //    //myRdr.CmdGetReaderInfo(myRdrInfo);
                        //    //myRdr.CmdGetTime(pTime);

                        //    int ss = myRdr.CmdGenerateLogRecords(myLogData);
                        //    if (ss > 0)
                        //    {
                        //        MessageBox.Show("Save record done successfully " + myDate + " " + myTime, "Inform",
                        //            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show("Sorry, couldn't save data. " + ss, "Error",
                        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    }
                        //}
                        break;
                    case "ZKTECO":
                        Cursor.Current = Cursors.WaitCursor;
                        if (ZktConnect(currIpAddr, currPort, PlaceName))
                        {

                        }
                        else
                        {

                        }
                        break;
                }
            }
            else
            {
                if (CheckDatabase1())
                {
                    MyPlace = 1;
                    SendToDatabase(Int32.Parse(textBox1.Text), myDate, myTime);
                    MessageBox.Show("Save record done successfully " + myDate + " " + myTime, "Inform",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sorry, couldn't save data.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private bool CheckDatabase1()
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

        private void SendToDatabase(int myId, string pDate, string pTime)
        {
            string my_querry;
            switch (MyGlobal.ServerType)
            {
                case 0:
                    OracleCommand cmd;
                    OracleDataReader reader;
                    my_querry = "Select nvl(Max(Sign_Seq),0) From EMS_EMP_FINGERS";
                    cmd = new OracleCommand(my_querry, oraCon);
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    double NextValue = reader.GetDouble(0) + 1;
                    reader.Close();
                    my_querry = "INSERT INTO EMS_EMP_FINGERS (Emp_Id,Sign_Date,Sign_Time,Sign_Plc_Id," +
                        "Status,Dev_Loc, Sign_Seq) " +
                        "VALUES(" + myId.ToString() + ", TO_DATE('" + pDate + "','DD/MM/YYYY')," +
                        "TO_DATE('" + pDate + " " + pTime + "','DD/MM/YYYY HH24:MI:SS'), " + MyPlace.ToString() +
                        ", 0, 0," + NextValue.ToString() + ")";
                    cmd = new OracleCommand(my_querry, oraCon);
                    cmd.ExecuteNonQuery();
                    break;
                case 1:
                    SqlCommand sqlcmd;
                    my_querry = "Select nvl(Max(Sign_Seq),0) From EMS_EMP_FINGERS";
                    sqlcmd = new SqlCommand(my_querry, sqlCon);
                    my_querry = "INSERT INTO EMS_EMP_FINGERS (Emp_Id,Sign_Date,Sign_Time,Sign_Plc_Id) " +
                        "VALUES(" + myId.ToString() + ",'" + pDate + "','" + pTime + "','" +
                        MyPlace.ToString() + "')";
                    sqlcmd = new SqlCommand(my_querry, sqlCon);
                    sqlcmd.ExecuteNonQuery();
                    break;
                    //case 2:
                    //    OleDbCommand acscmd;
                    //    my_querry = "Select nvl(Max(Sign_Seq),0) From EMS_EMP_FINGERS";
                    //    sqlcmd = new SqlCommand(my_querry, sqlCon);
                    //    my_querry = "INSERT INTO EMS_EMP_FINGERS (Emp_Id,Sign_Date,Sign_Time,Sign_Plc_Id) " +
                    //        "VALUES(" + myId.ToString() + ",'" + pDate + "','" + pTime + "','" +
                    //        MyPlace.ToString() + "')";
                    //    //mySql = "Insert into FingerPrint (userId,fingerTime,FingerDate,Place_id) values('" + myId.ToString() +
                    //    //    "','" + pTime + "','" + pDate + "')";
                    //    acscmd = new OleDbCommand(mySql, accCon);
                    //    acscmd.ExecuteNonQuery();
                    //    break;
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
                    "VALUES(" + myId.ToString() + ", DateValue('" + pDate + "')," +
                    "Format('" + pDate + " " + pTime + "','dd/mm/yyyy Hh:mm:ss')," +    //, 'h: m: s'
                    MyPlace.ToString() +
                    ", 0, 0," + lastValue.ToString() + ")";
            accmd = new OleDbCommand(mySql, accCon);
            accmd.ExecuteNonQuery();
        }

        private void Setup_FormClosed(object sender, FormClosedEventArgs e)
        {
            accCon.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            idTxt.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            addrTxt.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            portTxt.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            devId.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            devType.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            placeTxt.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            placeName.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            password.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            checkBox1.Checked = bool.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());
            checkBox3.Checked = bool.Parse(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());
            checkBox3.Enabled = (devType.SelectedIndex == 1);
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fmt = "00";
            if (comboBox1.SelectedIndex == 0)
            {
                return;
            }
            mySql = "Select * From Devices Where Place_id =" +
                comboBox1.SelectedItem.ToString().Split(' ')[0];
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            currIpAddr = reader.GetString(1);
            currPort = reader.GetInt32(2);
            currDevId = reader.GetInt32(3);
            currDevType = reader.GetString(4).ToUpper();
            currPass = reader.GetString(8).ToUpper();
            switch (currDevType.ToUpper())
            {
                case "ZKTECO":
                    int dwYear = 0; int dwMonth = 0; int dwDay = 0;
                    int dwHour = 0; int dwMinute = 0; int dwSecond = 0;
                    Cursor.Current = Cursors.WaitCursor;
                    label7.Text = "";
                    if (ZktConnect(currIpAddr, currPort, PlaceName))
                    {
                        zkRdr.GetDeviceTime(currDevId, ref dwYear, ref dwMonth, ref dwDay,
                                ref dwHour, ref dwMinute, ref dwSecond);
                        label7.Text = dwHour.ToString(fmt) + ":" +
                            dwMinute.ToString(fmt) + ":" + dwSecond.ToString(fmt);
                        label7.Update();
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        listBox1.Items.Add("Unable to connect (" + PlaceName + ") " + DateTime.Now);
                        listBox1.Update();
                        Cursor.Current = Cursors.Default;
                    }
                    Cursor.Current = Cursors.Default;
                    break;
                default:
                    break;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 0)
            {
                return;
            }
            mySql = "Select * From Devices Where Place_id =" +
                comboBox4.SelectedItem.ToString().Split(' ')[0];
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            currIpAddr = reader.GetString(1);
            currPort = reader.GetInt32(2);
            currDevId = reader.GetInt32(3);
            currDevType = reader.GetString(4).ToUpper();
            currPass = reader.GetString(8).ToUpper();
            PlaceName = reader.GetString(7);
            switch (currDevType.ToUpper())
            {
                case "ZKTECO":
                    Cursor.Current = Cursors.WaitCursor;
                    label7.Text = "";
                    if (ZktConnect(currIpAddr, currPort, PlaceName))
                    {
                        getTimeButt.Enabled = true;
                        setTimeButt.Enabled = true;
                        getGrpButt.Enabled = true;
                        setGrpButt.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        toolStripStatusLabel1.Text = "";
                    }
                    else
                    {
                        getTimeButt.Enabled = false;
                        setTimeButt.Enabled = false;
                        getGrpButt.Enabled = false;
                        setGrpButt.Enabled = false;
                        toolStripStatusLabel1.Text = "Unable to connect (" + PlaceName + ") " + DateTime.Now;
                        Cursor.Current = Cursors.Default;
                    }
                    Cursor.Current = Cursors.Default;
                    break;
                default:
                    break;
            }
        }

        private void getTimeButt_Click(object sender, EventArgs e)
        {
            string myTZ = "";
            zkRdr.GetTZInfo(currDevId,Int32.Parse(timeZone.Value.ToString()),ref myTZ);
            day2Time1.Value = DateTime.ParseExact(myTZ.Substring(0, 2),"HH",null); 
            day2Time2.Value = DateTime.ParseExact(myTZ.Substring(2, 2), "mm", null);
            day2Time3.Value = DateTime.ParseExact(myTZ.Substring(4, 2), "HH", null); 
            day2Time4.Value = DateTime.ParseExact(myTZ.Substring(6, 2), "mm", null);
            day3Time1.Value = DateTime.ParseExact(myTZ.Substring(8, 2), "HH", null); 
            day3Time2.Value = DateTime.ParseExact(myTZ.Substring(10, 2), "mm", null);
            day3Time3.Value = DateTime.ParseExact(myTZ.Substring(12, 2), "HH", null); 
            day3Time4.Value = DateTime.ParseExact(myTZ.Substring(14, 2), "mm", null);
            day4Time1.Value = DateTime.ParseExact(myTZ.Substring(16, 2), "HH", null); 
            day4Time2.Value = DateTime.ParseExact(myTZ.Substring(18, 2), "mm", null);
            day4Time3.Value = DateTime.ParseExact(myTZ.Substring(20, 2), "HH", null); 
            day4Time4.Value = DateTime.ParseExact(myTZ.Substring(22, 2), "mm", null);
            day5Time1.Value = DateTime.ParseExact(myTZ.Substring(24, 2), "HH", null); 
            day5Time2.Value = DateTime.ParseExact(myTZ.Substring(26, 2), "mm", null);
            day5Time3.Value = DateTime.ParseExact(myTZ.Substring(28, 2), "HH", null); 
            day5Time4.Value = DateTime.ParseExact(myTZ.Substring(30, 2), "mm", null);
            day6Time1.Value = DateTime.ParseExact(myTZ.Substring(32, 2), "HH", null); 
            day6Time2.Value = DateTime.ParseExact(myTZ.Substring(34, 2), "mm", null);
            day6Time3.Value = DateTime.ParseExact(myTZ.Substring(36, 2), "HH", null); 
            day6Time4.Value = DateTime.ParseExact(myTZ.Substring(38, 2), "mm", null);
            day7Time1.Value = DateTime.ParseExact(myTZ.Substring(40, 2), "HH", null); 
            day7Time2.Value = DateTime.ParseExact(myTZ.Substring(42, 2), "mm", null);
            day7Time3.Value = DateTime.ParseExact(myTZ.Substring(44, 2), "HH", null); 
            day7Time4.Value = DateTime.ParseExact(myTZ.Substring(46, 2), "mm", null);
            day1Time1.Value = DateTime.ParseExact(myTZ.Substring(48, 2), "HH", null); 
            day1Time2.Value = DateTime.ParseExact(myTZ.Substring(50, 2), "mm", null);
            day1Time3.Value = DateTime.ParseExact(myTZ.Substring(52, 2), "HH", null); 
            day1Time4.Value = DateTime.ParseExact(myTZ.Substring(54, 2), "mm", null);
        }

        private void setTimeButt_Click(object sender, EventArgs e)
        {
            string myTZ =
                day2Time1.Value.ToString("HH") + day2Time2.Value.ToString("mm") +
                day2Time3.Value.ToString("HH") + day6Time4.Value.ToString("mm") +
                day3Time1.Value.ToString("HH") + day6Time2.Value.ToString("mm") +
                day3Time3.Value.ToString("HH") + day3Time4.Value.ToString("mm") +
                day4Time1.Value.ToString("HH") + day6Time2.Value.ToString("mm") +
                day4Time3.Value.ToString("HH") + day4Time4.Value.ToString("mm") +
                day5Time1.Value.ToString("HH") + day6Time2.Value.ToString("mm") +
                day5Time3.Value.ToString("HH") + day5Time4.Value.ToString("mm") +
                day6Time1.Value.ToString("HH") + day6Time2.Value.ToString("mm") +
                day6Time3.Value.ToString("HH") + day6Time4.Value.ToString("mm") +
                day7Time1.Value.ToString("HH") + day6Time2.Value.ToString("mm") +
                day7Time3.Value.ToString("HH") + day7Time4.Value.ToString("mm") +
                day1Time1.Value.ToString("HH") + day6Time2.Value.ToString("mm") +
                day1Time3.Value.ToString("HH") + day1Time4.Value.ToString("mm");
            if(zkRdr.SetTZInfo(currDevId, Int32.Parse(timeZone.Value.ToString()), myTZ))
            {
                MessageBox.Show("Time Zone value are set successfully", "Inform",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Something went wrong, nothing set", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void setGrpButt_Click(object sender, EventArgs e)
        {
            zkRdr.SSR_SetGroupTZ(currDevId, Int32.Parse(grpNum.Value.ToString()),
                Int32.Parse(shiftNum1.Value.ToString()),
                Int32.Parse(shiftNum2.Value.ToString()),
                Int32.Parse(shiftNum3.Value.ToString()), 0, 12);
        }

        private void getGrpButt_Click(object sender, EventArgs e)
        {
            int tz1=0,tz2=0,tz3=0,vHolidy=0, style=0;
            zkRdr.SSR_GetGroupTZ(currDevId, Int32.Parse(grpNum.Value.ToString()),
                ref tz1,ref tz2,ref tz3,ref vHolidy, ref style);
            shiftNum1.Value = tz1;
            shiftNum2.Value = tz2;
            shiftNum3.Value = tz3;
        }

        private void testButt_Click(object sender, EventArgs e)
        {
            if (string.Empty == apiLink.Text)
            {
                MessageBox.Show("Please enter ApiLink", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                apiLink.Focus();
                return;
            }
            if (string.Empty == fingerText.Text)
            {
                MessageBox.Show("Please enter Finger Id", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                fingerText.Focus();
                return;
            }
            TestApiLink();
        }

        private async void TestApiLink()
        {
            HttpClient client = new HttpClient();
            string myUrl = $"{apiLink.Text}{fingerText.Text}";
            var response = await client.GetAsync(myUrl);
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject responseObject = JObject.Parse(responseBody);
            if (response.IsSuccessStatusCode)
            {
                if (responseObject["finger_id"].ToString() == fingerText.Text)
                {
                    string Stud_Name = responseObject["stud_a_name"].ToString();
                    string Stud_Finger = responseObject["finger_id"].ToString();
                    string Allow_Status = responseObject["allow_status"].ToString();
                    string Status_Text = responseObject["status_text"].ToString();
                    MessageBox.Show("Student Finger : " + Stud_Finger + "\n" +
                        "Student Name   : " + Stud_Name + "\n" +
                        "Student State  : " + Allow_Status + "\n" +
                        "Status Text    : " + Status_Text, "Succes Message",

                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("This student FingerId not found", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Connection to ERP failed", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void devType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (devType.SelectedIndex == 0)
            {
                checkBox3.Checked = true;
                checkBox3.Enabled = false;
                portTxt.Text = "3001";
            }
            else
            {
                checkBox3.Checked = false;
                checkBox3.Enabled = true;
                portTxt.Text = "4370";
            }
        }

        private void rOracle_CheckedChanged(object sender, EventArgs e)
        {
            //panel8.Enabled = rOracle.Checked;
            //panel10.Enabled = rSqlServer.Checked;
            settSave.Enabled = true;
        }

        private void moveButt_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex <= 0)
            {
                MessageBox.Show("Please choose source device", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return;
            }
            if (comboBox2.SelectedIndex <= 0)
            {
                MessageBox.Show("Please choose target device", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox2.Focus();
                return;
            }
            if (comboBox1.SelectedIndex == comboBox2.SelectedIndex)
            {
                MessageBox.Show("Not allow to perform this action on the same device", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox2.Focus();
                return;
            }
            if (empTxt.Text.Trim() == "")
            {
                MessageBox.Show("Please enter user number", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                empTxt.Focus();
                return;
            }
            mySql = "Select * From Devices Where Place_id =" +
                comboBox1.SelectedItem.ToString().Split(' ')[0];
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            currIpAddr = reader.GetString(1);
            currPort = reader.GetInt32(2);
            currDevId2 = reader.GetInt32(3);
            currDevType = reader.GetString(4).ToUpper();
            currPass = reader.GetString(8).ToUpper();
            reader.Close();
            Button myButt = (Button)sender;
            switch (currDevType.ToUpper())
            {
                case "HANDPUNCH":
                    //Cursor.Current = Cursors.WaitCursor;
                    //RSI_EXT_USER_RECORD pUser = new RSI_EXT_USER_RECORD();
                    //RSI_READER_INFO pReaderInfo = new RSI_READER_INFO();
                    //if (myRdr.CmdGetExtUser(empTxt.Text, pUser) == FALSE)
                    //{
                    //    MessageBox.Show("This User Not Found!", "Error",
                    //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    empTxt.Focus();
                    //    return;
                    //}
                    //else
                    //{
                    //    mySql = "Select * From Devices Where Place_id =" +
                    //            comboBox2.SelectedItem.ToString().Split(' ')[0];
                    //    cmd = new OleDbCommand(mySql, accCon);
                    //    reader = cmd.ExecuteReader();
                    //    reader.Read();
                    //    if (ConnectPunch(reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)))
                    //    {
                    //        reader.Close();
                    //        string moveMsg = "This action will delete the user from the source device\n\n" +
                    //                         "Are you sure to move this user to the target device";
                    //        string copyMsg = "Are you sure to copy this user to the target device";

                    //        if (MessageBox.Show((myButt.Tag.ToString() == "1") ? moveMsg : copyMsg, "Confirm",
                    //                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //        {
                    //            myRdr.CmdPutUserRecord(pUser);
                    //            if (myButt.Tag.ToString() == "1")
                    //            {
                    //                mySql = "Select * From Devices Where Place_id =" +
                    //                        comboBox1.SelectedItem.ToString().Split(' ')[0];
                    //                cmd = new OleDbCommand(mySql, accCon);
                    //                reader = cmd.ExecuteReader();
                    //                reader.Read();
                    //                if (ConnectPunch(reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)))
                    //                {
                    //                    reader.Close();
                    //                    myRdr.CmdRemoveUser(empTxt.Text);
                    //                    MessageBox.Show("Move user to the target device done successfully", "Inform",
                    //                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                MessageBox.Show("Copy user to the target device done successfully", "Inform",
                    //                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //            }
                    //        }

                    //    }
                    //}
                    //Cursor.Current = Cursors.Default;
                    break;
                case "ZKTECO":
                    Cursor.Current = Cursors.WaitCursor;
                    if (ZktConnect(currIpAddr, currPort, PlaceName))
                    {
                        Cursor.Current = Cursors.Default;
                        //string iEnrollNumber = "";
                        string iName = "";
                        string iPassword = "";
                        int iPrivilege = 0;
                        bool iEnabled = false;
                        //string status = "Enable";
                        string[] tempData = new string[10];
                        int[] tempLen = new int[10];
                        string faceTemp = "";
                        int faceLen = 0;
                        zkRdr.EnableDevice(currDevId, false);
                        if (zkRdr.GetUserInfo(currDevId, Int32.Parse(empTxt.Text), iName, iPassword, iPrivilege, iEnabled))
                        {
                            for (int i = 0; i <= 9; i++)
                            {
                                zkRdr.GetUserTmpStr(currDevId, Int32.Parse(empTxt.Text), i, tempData[i], tempLen[i]);
                            }
                            zkRdr.GetUserFaceStr(currDevId, empTxt.Text, 50, faceTemp, faceLen);

                            zkRdr.EnableDevice(currDevId2, false);
                            if (zkRdr.SetUserInfo(currDevId2, Int32.Parse(empTxt.Text), iName, iPassword, iPrivilege, iEnabled))
                            {
                                for (int i = 0; i <= 9; i++)
                                {
                                    zkRdr.SetUserTmpStr(currDevId2, Int32.Parse(empTxt.Text), i, tempData[i]);
                                }
                                zkRdr.SetUserFaceStr(currDevId2, empTxt.Text, 50, faceTemp, faceLen);
                                if (myButt.Tag.ToString() == "1")
                                {
                                    zkRdr.SSR_DeleteEnrollData(currDevId, empTxt.Text, 12);
                                    MessageBox.Show("Move user to the target device done successfully", "Inform",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Copy user to the target device done successfully", "Inform",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                zkRdr.EnableDevice(currDevId, true);
                                zkRdr.EnableDevice(currDevId2, true);
                            }
                            else
                            {
                                zkRdr.EnableDevice(currDevId, true);
                                zkRdr.EnableDevice(currDevId2, true);
                            }
                        }
                        else
                        {
                            zkRdr.EnableDevice(currDevId, true);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private bool ZktConnect(string ipAddress, int port, string placeId)
        {
            MyGlobal.IsConnected = false;
            if (PingTheDevice(ipAddress))
            {
                if (MyGlobal.showLogs)
                {
                    listBox1.Items.Add("Success ping with device no: " + placeId);
                    listBox1.Update();
                }
                IPAddress ipAdd = IPAddress.Parse(ipAddress);
                Cursor.Current = Cursors.WaitCursor;
                if (string.IsNullOrEmpty(currPass.Trim()))
                {
                    currPass = "0";
                }
                zkRdr.SetCommPassword(int.Parse(currPass));
                MyGlobal.IsConnected = zkRdr.Connect_Net(ipAdd.ToString(), port);
                if (MyGlobal.IsConnected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
                int timeout = 120;
                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);
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
    }
}

