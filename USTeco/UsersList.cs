using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using RecogSys.RdrAccess;
using zkemkeeper;
using System.Net;
using System.Net.NetworkInformation;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Reflection.Emit;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace USTeco
{

    public partial class UsersList : Form
    {
        //int numberOfItemsPrintedSoFar;
        //int numberOfItemsPerPage;
        //int currentPageNo;
        //int currentPageCount;
        //private int iCanSaveTmp;
        const int TRUE = 1;
        const int FALSE = 0;
        OleDbCommand cmd;
        OleDbDataReader reader;
        OleDbConnection accCon = new OleDbConnection();
        CRsiComWinsock myIP = new CRsiComWinsock(); // to hold the IP address of ZkTeco device.
        CRsiHandReader myRdr;// = new CRsiHandReader(); // to receive retrieved log of ZkTeco device.
        CRsiNetwork myNetwork;
        CZKEM zkRdr = new CZKEM();
        CZKEM zkRdr2 = new CZKEM();
        RSI_READER_INFO pReaderInfo = new RSI_READER_INFO();
        RSI_STATUS response = new RSI_STATUS();
        string PlaceName;
        int PlaceNo;
        struct ZktUserInfo
        {
            public string empId;
            public string name;
            public int authority;
            public string cardNo;
            public bool status;
            public int group;
            public string[] rTemplate;
            public int[] flag;
            public string sTemplate;
            public string pTemplate;
        }
        ZktUserInfo[] zktUserInfo;
        string mySql;
        string currIpAddr, currDevType, currPass;
        int currPort, currDevId, newDevId2;
        string[] backupList = new string[] { };
        string[,] userData;
        string[,] sTmpData;
        string[,] rTmpData;
        string[,] pTmpData;
        int[,] rFlagData;
        string[,] ZkError = new string[11, 2];
        string myDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UST Applications", "Handpunch");
        public UsersList()
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            int MyPlace;
            InitializeComponent();
            accCon.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + myDataPath + "\\handdb.mdb; Jet OLEDB:Database Password = Ajoset25";
            accCon.Open();
            mySql = "Select Place_id,Place_Name From Devices";
            comboBox1.Items.Add("All Devices");
            comboBox3.Items.Add("All Devices");
            BackColor = MyGlobal.color2;
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
                comboBox1.Items.Add(MyPlace.ToString() + " " + PlaceName);
                comboBox3.Items.Add(MyPlace.ToString() + " " + PlaceName);
            }
            comboBox1.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            toolStripProgressBar1.Width = 200;
            reader.Close();

        }

        private bool ConnectPunch(String ipAdd, int port, int devId, String placeId)
        {
            myIP.SetHost(ipAdd);
            myIP.SetPortA(ushort.Parse(port.ToString()));
            myRdr = new CRsiHandReader(myIP, ((byte)devId));
            if (FALSE == myIP.Ping())
            {
                toolStripStatusLabel1.Text = "Unable to Ping (" + placeId + ") ";
                pictureBox1.BackgroundImage = Properties.Resources.logo;
                return false;
            }
            else
            {
                myIP.Disconnect();
                myIP.ResetSocket();
                myIP.Connect();
                myIP.IsConnected();
                if (FALSE == myIP.IsConnected())
                {
                    toolStripStatusLabel1.Text = "Unable to connect (" + placeId + ") ";
                    pictureBox1.BackgroundImage = Properties.Resources.logo;
                    myIP.Disconnect();
                    myIP.ResetSocket();
                    MyGlobal.IsConnected = false;
                    return false;
                }
                else
                {
                    pictureBox1.BackgroundImage = Properties.Resources.handpunch;
                    myNetwork = new CRsiNetwork(myIP);
                    myNetwork.Attach(myRdr);
                    RSI_TIME_DATE currentDateTime = new RSI_TIME_DATE();
                    if (myRdr.CmdGetTime(currentDateTime) == TRUE)  // Gets current time from reader.      
                    {
                        switch (myRdr.GetFamily())
                        {
                            case RSI_FAMILY.RSI_FAMILY_UNKNOWN:
                                label5.Text = "Unknown";
                                break;
                            case RSI_FAMILY.RSI_FAMILY_HANDPUNCH:
                            case RSI_FAMILY.RSI_FAMILY_HANDKEY:
                                myRdr.CmdGetReaderInfo(pReaderInfo);
                                if ((int)pReaderInfo.model == (int)RSI_MODEL.RSI_MODEL_HP4K)
                                {
                                    label5.Text = "Hand Punch 4K";
                                }
                                else
                                {
                                    label5.Text = "Hand Punch 3K";
                                }
                                break;
                            case RSI_FAMILY.RSI_FAMILY_FINGERKEY:
                                label5.Text = "Finger Print";
                                break;
                            case RSI_FAMILY.RSI_FAMILY_ACU:
                                label5.Text = "ACU";
                                break;
                            case RSI_FAMILY.RSI_FAMILY_BPU:
                                label5.Text = "BPU";
                                break;
                        }
                        MyGlobal.IsConnected = true;
                        label5.Update();
                        return true;
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Couldn't read from (" + placeId + ") ";
                        pictureBox1.BackgroundImage = Properties.Resources.logo;
                        MyGlobal.IsConnected = false;
                        return false;
                    }
                }
            }
        }

        private bool ZktConnect(string ipAddress, int port, string placeId, string password)
        {
            if (PingTheDevice(ipAddress))
            {
                IPAddress ipAdd = IPAddress.Parse(ipAddress);
                Cursor.Current = Cursors.WaitCursor;
                if (password.Trim() != "0")
                {
                    zkRdr.SetCommPassword(int.Parse(password));
                }
                else
                {
                    zkRdr.SetCommPassword(0);
                }
                zkRdr.Disconnect();
                MyGlobal.IsConnected = zkRdr.Connect_Net(ipAdd.ToString(), port);
                if (MyGlobal.IsConnected)
                {

                    int fingerCount = 0;
                    int faceCount = 0;
                    int userCount = 0;
                    //int palmCount = 0;
                    zkRdr.GetDeviceStatus(currDevId, 2, ref userCount);
                    zkRdr.GetDeviceStatus(currDevId, 3, ref fingerCount);
                    zkRdr.GetDeviceStatus(currDevId, 21, ref faceCount);
                    UsrCount.Text = "Users : " + userCount.ToString();
                    pictureBox1.BackgroundImage = Properties.Resources.zkteco;
                    string sProductCode = "";
                    if (zkRdr.GetProductCode(currDevId, out sProductCode))
                    {
                        label5.Text = sProductCode;
                    }
                    toolStripStatusLabel1.Text = "";
                    label5.Update();
                    Cursor.Current = Cursors.Default;
                    return true;
                }
                else
                {
                    int errRef = 0;
                    zkRdr.GetLastError(errRef);
                    toolStripStatusLabel1.Text = "Unable to connect (" + placeId + ") ";
                    Cursor.Current = Cursors.Default;
                    pictureBox1.BackgroundImage = Properties.Resources.logo; ;
                    pictureBox1.Update();
                    label5.Text = "";
                    label5.Update();
                    MyGlobal.IsConnected = false;
                    return false;
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Unable to ping (" + placeId + ") ";
                pictureBox1.BackgroundImage = Properties.Resources.logo;
                pictureBox1.Update();
                label5.Text = "";
                label5.Update();
                MyGlobal.IsConnected = false;
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

        private void backupButt_Click(object sender, EventArgs e)
        {
            int backupId = 0;
            int i = 0;
            if (comboBox1.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select device you want to backup", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (gridView1.RowCount <= 0)
            {
                MessageBox.Show("Please load user data first", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            mySql = "Select * From UserBackup";
            cmd = new OleDbCommand(mySql, accCon);
            try
            {
                reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch
            {
                mySql = "Create Table UserBackup (id AUTOINCREMENT PRIMARY KEY, devId Integer," +
                    "backupId Integer, backDate DateTime, empId Integer, empName Text,authority Text," +
                    "cardNo Text, status YesNo, fingTemp0 Text,fingTemp1 Text," +
                    "fingTemp2 Text,fingTemp3 Text,fingTemp4 Text, fingTemp5 Text,"+
                    "fingTemp6 Text,fingTemp7 Text,fingTemp8 Text, fingTemp9 Text," +
                    "faceTemp Text)";
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
            mySql = "Select Max(IIF(backupId IS NOT NULL, backupId, 0))+1 From UserBackup " +
                    "Where devId=" + comboBox1.Text.Split(' ')[0];
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    backupId = Int32.Parse(reader.GetValue(0).ToString());
                }
                catch
                {
                    backupId = 1;
                }
            }
            switch (currDevType)
            {
                case "HANDPUNCH":
                    Cursor.Current = Cursors.WaitCursor;
                    for(int j=0; j < userData.GetLength(0); j++)
                    {
                        if (userData[j, 0] == null)
                        {
                            i++;
                            continue;
                        }
                        mySql = "Insert Into UserBackup (devId,backupId,backDate,empId," +
                            "empName,authority,cardNo,status,fingTemp0," +
                            "fingTemp1,fingTemp2,fingTemp3,fingTemp4,fingTemp5,fingTemp6," +
                            "fingTemp7,fingTemp8,fingTemp9,faceTemp) Values (" +
                            comboBox1.Text.Split(' ')[0] + "," + backupId + ",Now()," +
                            userData[j,0] + ",'" + userData[j, 1] + "','" + userData[j, 2] + "','" +
                            userData[j, 3] + "',0,'" + userData[j, 4] +"','"+
                            userData[j, 5] + "','" + userData[j, 6] + "','" +
                            userData[j, 7] + "','" + userData[j, 8] + "','" +
                            userData[j, 9] + "','" + userData[j, 10] + "','" +
                            userData[j, 11] + "','"+ userData[j, 12] + "','','')";
                        cmd = new OleDbCommand(mySql, accCon);
                        cmd.ExecuteNonQuery();
                        i++;
                    }
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Data backup done successfully", "Inform",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "ZKTECO":
                    zkRdr.EnableDevice(currDevId, false);
                    Cursor.Current = Cursors.WaitCursor;
                    foreach (ZktUserInfo zRec in zktUserInfo)
                    {
                        mySql = "Insert Into UserBackup (devId,backupId,backDate,empId," +
                            "empName,authority,cardNo,status,fingTemp0," +
                            "fingTemp1,fingTemp2,fingTemp3,fingTemp4,fingTemp5,fingTemp6," +
                            "fingTemp7,fingTemp8,fingTemp9,faceTemp) Values (" +
                            comboBox1.Text.Split(' ')[0]+","+backupId+ ",Now()," +
                            zRec.empId + ",'" + zRec.name + "','" + zRec.authority + "','" + 
                            zRec.cardNo + "'," + zRec.status + ",'" +
                            rTmpData[i, 0] + "','" + rTmpData[i, 1] + "','" +
                            rTmpData[i, 2] + "','" + rTmpData[i, 3] + "','" +
                            rTmpData[i, 4] + "','" + rTmpData[i, 5] + "','" +
                            rTmpData[i, 6] + "','" + rTmpData[i, 7] + "','" +
                            rTmpData[i, 8] + "','" + rTmpData[i, 9] + "','" +
                            zRec.sTemplate + "')";
                        cmd = new OleDbCommand(mySql, accCon);
                        cmd.ExecuteNonQuery();
                        i++;
                    }
                    Cursor.Current = Cursors.Default;
                    zkRdr.EnableDevice(currDevId, true);
                    MessageBox.Show("Data backup done successfully", "Inform",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void restoreButt_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select device you want to restore", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            mySql = "Select Distinct backupId,Format(backDate,'yyyy/mm/dd hh:nn') as 'backDate' " +
                "From UserBackup Where devId=" + comboBox1.Text.Split(' ')[0];
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            string devId = comboBox1.Text.Split(' ')[0];
            List<string> myList = new List<string>();
            while (reader.Read())
            {
                myList.Add("Backup ( "+ reader.GetValue(0) + " ) on " + reader.GetValue(1));
            }
   
            backupList = myList.ToArray();
            string backupNo = ShowBackupDialog(backupList);
            string backId;
            string empId;
            string idCond;
            if (backupNo.Split(',')[0] == "Done")
            {
                backId = backupNo.Split(',')[1];
                if (MessageBox.Show("Are you sure to delete this backup?", "Warning",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    mySql = "Delete From UserBackup Where devId=" + devId +
                        " And backupId=" + backId;
                    cmd = new OleDbCommand(mySql, accCon);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Selected backup deleted successfully", "Inform",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (backupNo != "")
            {
                backId = backupNo.Split(',')[0];
                empId = backupNo.Split(',')[1];
                if(empId != "")
                {
                    idCond = " And empId=" + empId;
                }
                else
                {
                    idCond = "";
                }
                switch (currDevType.ToUpper())
                {
                    case "HANDPUNCH":
                        if (MyGlobal.IsConnected)
                        {
                            if (MessageBox.Show("Are you sure you want to restore all user information?\n\n" +
                                "This will overwite all current user information. Continue?", "Confirm",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                RSI_EXT_USER_RECORD pUser = new RSI_EXT_USER_RECORD();
                                RSI_USER_RECORD xUser = new RSI_USER_RECORD();
                                myRdr.CmdEnterIdleMode();
                                myRdr.CmdGetReaderInfo(pReaderInfo);
                                Cursor.Current = Cursors.WaitCursor;
                                mySql = "Select * From UserBackup Where devId=" + devId +
                                    " And backupId=" + backId + idCond;
                                cmd = new OleDbCommand(mySql, accCon);
                                reader = cmd.ExecuteReader();
                                bool hasRec = false;
                                while (reader.Read())
                                {
                                    hasRec = true;
                                    pUser.pID.SetID(reader.GetInt32(4).ToString());
                                    pUser.pXUD.pName.Set(reader.GetString(5));
                                    pUser.rejectThreshold = 1;
                                    pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                    xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                    xUser.pID.SetID(reader.GetInt32(4).ToString());
                                    xUser.rejectThreshold = 1;
                                    switch (reader.GetString(6))
                                    {
                                        case "0":
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                            break;
                                        case "1":
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                            break;
                                        case "2":
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                            break;
                                        case "3":
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                            break;
                                        case "4":
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_ENROLLMENT;
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_ENROLLMENT;
                                            break;
                                        case "5":
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                            break;
                                    }
                                    pUser.timeZone = byte.Parse(reader.GetString(7));
                                    byte[] temp = new byte[9];

                                    for (int col = 0; col < 9; col++)
                                    {
                                        temp[col] = byte.Parse(reader.GetString(col + 9));
                                    }
                                    pUser.pTemplateVector.Set(temp);
                                    xUser.pTemplateVector.Set(temp);
                                    myRdr.CmdGetReaderInfo(pReaderInfo);
                                    if ((int)pReaderInfo.model == (int)RSI_MODEL.RSI_MODEL_HP4K)
                                    {
                                        myRdr.CmdPutExtUser(pUser);
                                    }
                                    else
                                    {
                                        myRdr.CmdPutUserRecord(xUser);
                                    }
                                }
                                myRdr.CmdExitIdleMode();
                                MyGlobal.IsConnected = false;
                                getUserList.Enabled = false;
                                backupButt.Enabled = false;
                                restoreButt.Enabled = false;
                                addUser.Enabled = false;
                                Cursor.Current = Cursors.Default;
                                if (hasRec)
                                {
                                    MessageBox.Show("Data restore done successfully.\nThe device may restart.", "Inform",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }
                                else
                                {
                                    MessageBox.Show("Thers is no data to restore.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Device is not connected!", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;
                    case "ZKTECO":
                        if (MyGlobal.IsConnected)
                        {
                            if (MessageBox.Show("Are you sure you want to restore all user information?\n\n" +
                                "This will overwite all current user information. Continue?", "Confirm",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                zkRdr.EnableDevice(currDevId, false);
                                mySql = "Select * From UserBackup Where devId=" + devId +
                                    " And backupId=" + backId;
                                cmd = new OleDbCommand(mySql, accCon);
                                reader = cmd.ExecuteReader();
                                bool hasRec = false;
                                while (reader.Read())
                                {
                                    hasRec = true;
                                    if (zkRdr.SSR_SetUserInfo(currDevId, reader.GetInt32(4).ToString(), reader.GetString(5),
                                        "", Int32.Parse(reader.GetString(6)), reader.GetBoolean(8)))
                                    {
                                        if (reader.GetString(7).Trim() != "")
                                        {
                                            zkRdr.SetStrCardNumber(reader.GetString(7));
                                        }
                                        for (int i = 0; i <= 9; i++)
                                        {
                                            try
                                            {
                                                zkRdr.SetUserTmpExStr(currDevId, reader.GetInt32(4).ToString(), i,
                                                    1, reader.GetString(9+i));
                                            }
                                            catch { }
                                        }
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(reader.GetString(19)))
                                            {
                                                zkRdr.SSR_DelUserTmpExt(currDevId, reader.GetInt32(4).ToString(), 111);
                                                if (!zkRdr.SetUserFaceStr(currDevId, reader.GetInt32(4).ToString(),
                                                    50, reader.GetString(19), reader.GetString(19).Length))
                                                {
                                                    int err = 0;
                                                    zkRdr.GetLastError(err);
                                                    MessageBox.Show(GetErrorMessage(err), "Error",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                            }
                                        }
                                        catch { };
                                    }
                                    else
                                    {
                                        int err = 0;
                                        zkRdr.GetLastError(err);
                                        MessageBox.Show(GetErrorMessage(err), "Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                zkRdr.RefreshData(currDevId);
                                zkRdr.EnableDevice(currDevId, true);
                                MyGlobal.IsConnected = false;
                                getUserList.Enabled = false;
                                backupButt.Enabled = false;
                                restoreButt.Enabled = false;
                                addUser.Enabled = false;
                                Cursor.Current = Cursors.Default;
                                if (hasRec)
                                {
                                    MessageBox.Show("Data restore done successfully.\nThe device may restart.", "Inform",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    zkRdr.RestartDevice(currDevId);
                                }
                                else
                                {
                                    MessageBox.Show("Thers is no data to restore.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Device is not connected!", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;
                }
            }
        }

        private void GetZkAllUserInfo(int iMachineNumber)
        {
            string iEnrollNumber = "";
            string iName = "";
            string iPassword = "";
            int iPrivilege = 0;
            bool iEnabled = false;
            string status = "Enable";
            int userCount = 0;
            string[] row;
            int iFaceIndex = 50;        //the only possible parameter value
            int iPalmIndex = 10;
            int iLength = 0; ;          //initialize the length(cannot be zero)
            int fFlag = 0;
            userData = new string[currDevId, 13];
            zkRdr.GetDeviceStatus(currDevId, 2, ref userCount);
            rTmpData = new string[userCount, 10];
            rFlagData = new int[userCount, 10];
            sTmpData = new string[userCount, 2];
            pTmpData = new string[userCount, 2];
            zktUserInfo = new ZktUserInfo[userCount];
            for (int i = 0; i < userCount; i++)
            {
                zktUserInfo[i].rTemplate = new string[10];
                zktUserInfo[i].flag = new int[10];
            }
            toolStripProgressBar1.Maximum = userCount;
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Visible = true;
            userCount = 0;
            zkRdr.ReadAllUserID(currDevId);
            zkRdr.ReadAllTemplate(currDevId);
            while (zkRdr.SSR_GetAllUserInfo(currDevId, out iEnrollNumber, out iName,
                out iPassword, out iPrivilege, out iEnabled))
            {
                string strAuth = "";
                toolStripProgressBar1.Value += 1;
                statusStrip1.Update();
                switch (iPrivilege)
                {
                    case 0:
                        strAuth = "None";
                        break;
                    case 1:
                        strAuth = "Enroll";
                        break;
                    case 2:
                        strAuth = "Admin";
                        break;
                    case 3:
                        strAuth = "Super";
                        break;
                }
                if (iEnabled)
                {
                    status = "Enable";
                }
                else
                {
                    status = "Disable";
                };
                string cardNum;
                zkRdr.GetStrCardNumber(out cardNum);
                if (cardNum.Trim() == "0") cardNum = "";
                try
                {
                    mySql = "Select Emp_Name From Employee Where EMP_ID='" + iEnrollNumber.TrimStart('0') + "'";
                    cmd = new OleDbCommand(mySql, accCon);
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    iName = reader.GetString(0);
                    reader.Close();
                }
                catch
                {
                    iName = "غير معروف";
                }
                int myGroup = 0;
                zkRdr.GetUserGroup(currDevId, Int32.Parse(iEnrollNumber), ref myGroup);
                row = new string[] { iEnrollNumber, iName, strAuth, cardNum, status, myGroup.ToString() };
                gridView1.Rows.Add(row);
                zktUserInfo[userCount].empId = iEnrollNumber;
                zktUserInfo[userCount].name = iName;
                zktUserInfo[userCount].authority = iPrivilege;
                zktUserInfo[userCount].cardNo = cardNum;
                zktUserInfo[userCount].status = iEnabled;
                zktUserInfo[userCount].group = myGroup;
                try
                {
                    bool alreadyHas = false;
                    rTmpData[userCount, 0] = iEnrollNumber;
                    for (int iFingerIndex = 0; iFingerIndex < 10; iFingerIndex++)
                    {
                        rTmpData[userCount, iFingerIndex] = "";
                        zkRdr.SSR_GetUserTmpStr(currDevId, iEnrollNumber, iFingerIndex,
                            out rTmpData[userCount, iFingerIndex], out iLength);
                        //zkRdr.GetUserTmpExStr(currDevId, iEnrollNumber.ToString(), iFingerIndex,
                        //   out rFlagData[userCount, iFingerIndex],
                        //   out rTmpData[userCount, iFingerIndex], out iLength);
                        zktUserInfo[userCount].rTemplate[iFingerIndex] = rTmpData[userCount, iFingerIndex];
                        zktUserInfo[userCount].flag[iFingerIndex] = rFlagData[userCount, iFingerIndex];
                        if (!alreadyHas && !string.IsNullOrEmpty(rTmpData[userCount, iFingerIndex]))
                        {
                            alreadyHas = true;
                            fFlag = rFlagData[userCount, iFingerIndex];
                        }
                    }
                    if (alreadyHas)
                    {
                        if (fFlag == 1)
                        {
                            gridView1.Rows[userCount].Cells[6].Value = Properties.Resources.tap_on_16;
                        }
                        else
                        {
                            gridView1.Rows[userCount].Cells[6].Value = Properties.Resources.tap_th_16;
                        }
                    }
                    else
                    {
                        gridView1.Rows[userCount].Cells[6].Value = Properties.Resources.tap_off_16;
                    }

                }

                catch { };
                try
                {
                    //zkRdr.GetUserTmpExStr
                    pTmpData[userCount, 0] = iEnrollNumber;
                    pTmpData[userCount, 1] = "";
                    zkRdr.SSR_GetUserTmpStr(currDevId, iEnrollNumber, iPalmIndex,
                            //out fFlag,
                            out pTmpData[userCount, 1], out iLength);
                    zktUserInfo[userCount].pTemplate = pTmpData[userCount, 1];
                    //zktUserInfo[userCount].flag[iFingerIndex] = rFlagData[userCount, iFingerIndex];
                    if (string.IsNullOrEmpty(pTmpData[userCount, 1]))
                    {
                        gridView1.Rows[userCount].Cells[8].Value = Properties.Resources.palm_off_16;
                    }
                    else
                    {
                        gridView1.Rows[userCount].Cells[8].Value = Properties.Resources.palm_on_16;
                    }
                    
                }
                catch { }
                try
                {
                    iLength = 0;
                    sTmpData[userCount, 0] = iEnrollNumber;
                    sTmpData[userCount, 1] = null;
                    zkRdr.GetUserFaceStr(currDevId, iEnrollNumber, iFaceIndex, ref sTmpData[userCount, 1], ref iLength);
                    zktUserInfo[userCount].sTemplate = sTmpData[userCount, 1];
                    if (string.IsNullOrEmpty(sTmpData[userCount, 1]))
                    {
                        gridView1.Rows[userCount].Cells[7].Value = Properties.Resources.face_off_16;
                    }
                    else
                    {
                        gridView1.Rows[userCount].Cells[7].Value = Properties.Resources.face_on_16;
                    }
                }
                catch { };
                gridView1.FirstDisplayedScrollingRowIndex = gridView1.RowCount - 1;
                gridView1.Update();
                userCount++;
                
            }
            zkRdr.EnableDevice(currDevId, true);
            toolStripProgressBar1.Visible = false;
        }

        private void addUser_Click(object sender, EventArgs e)
        {
            RSI_EXT_USER_RECORD xUser = new RSI_EXT_USER_RECORD();
            RSI_USER_RECORD pUser = new RSI_USER_RECORD();
            RSI_PROMPT uPrompt = new RSI_PROMPT();
            RSI_LAST_TEMPLATE lastTemp = new RSI_LAST_TEMPLATE();
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select target device",
                    "Select Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox11.Text.Equals(""))
            {
                MessageBox.Show("Please enter Id Number", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox11.Focus();
                return;
            }
            else if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Authority Level", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox2.Focus();
                return;
            }
            switch (currDevType)
            {
                case "HANDPUNCH":
                    if (MyGlobal.IsConnected)
                    {
                        if (MessageBox.Show("Please ask user who will be registring to stand in " +
                                                "front of HandPunch Device. User will be instructed " +
                                                "to place his hand and remove it 3 times. Continue?", "Confirm",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            myRdr.CmdBeep(50, 3);
                            uPrompt = 0;
                            string[] row;
                            response.rslts_rdy = 0;
                            myRdr.CmdEnterIdleMode();
                            myRdr.CmdEnrollUser(uPrompt, response);
                            System.Threading.Thread.Sleep(20000);
                            myRdr.CmdGetStatus(response);
                            if (response.rslts_rdy == 4)
                            {
                                myRdr.CmdGetTemplate(lastTemp);
                                xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                xUser.rejectThreshold = 1;
                                xUser.pID.SetID(textBox11.Text);
                                xUser.pXUD.pName.Set(textBox13.Text);
                                xUser.pTemplateVector.Set(lastTemp.pTemplateVector.Get());
                                pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                pUser.pID.SetID(textBox11.Text);
                                pUser.pTemplateVector.Set(lastTemp.pTemplateVector.Get());
                                pUser.rejectThreshold = 1;
                                switch (comboBox2.Text)
                                {
                                    case "None":
                                        xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                        pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                        break;
                                    case "Service":
                                        xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                        pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                        break;
                                    case "Setup":
                                        xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                        pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                        break;
                                    case "Manage":
                                        xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                        pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                        break;
                                    case "Enroll":
                                        xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_ENROLLMENT;
                                        pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_ENROLLMENT;
                                        break;
                                    case "Security":
                                        xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                        pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                        break;
                                }
                                System.Threading.Thread.Sleep(4000);
                                try
                                {
                                    myRdr.CmdGetReaderInfo(pReaderInfo);
                                    if ((int)pReaderInfo.model == (int)RSI_MODEL.RSI_MODEL_HP4K)
                                    {
                                        myRdr.CmdPutExtUser(xUser);
                                    }
                                    else
                                    {
                                        myRdr.CmdPutUserRecord(pUser);
                                    }
                                    row = new string[] { textBox11.Text, textBox13.Text, comboBox2.Text, "0" };
                                    gridView1.Rows.Add(row);
                                    textBox11.Text = "";
                                    textBox13.Text = "";
                                    comboBox2.SelectedIndex = 0;
                                    MessageBox.Show("User was added successfully", "Inform",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                catch
                                {
                                    MessageBox.Show("Sorry, add new user failed", "Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                myRdr.CmdExitIdleMode();
                            }
                        }
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Device not connected.";
                    }
                    break;
                case "ZKTECO":
                    if (MessageBox.Show("Are you sure to add this Id Number?", "Confirm",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        if (MyGlobal.IsConnected)
                        {
                            zkRdr.EnableDevice(currDevId, false);
                            string pLevel = "";
                            switch (comboBox2.SelectedIndex)
                            {
                                case 0:
                                    pLevel = "None";
                                    break;
                                case 1:
                                    pLevel = "Enroll";
                                    break;
                                case 2:
                                    pLevel = "Admin";
                                    break;
                                case 3:
                                    pLevel = "Super";
                                    break;
                            }
                            if (zkRdr.SSR_SetUserInfo(currDevId, textBox11.Text, textBox13.Text, "", comboBox2.SelectedIndex, true))
                            {
                                string[] row = new string[] { textBox11.Text, textBox13.Text, pLevel, "False" };
                                gridView1.Rows.Add(row);
                                //gridView1.Rows[gridView1.RowCount - 1].Cells[4].Style.BackColor = Color.Red;
                                textBox11.Text = "";
                                textBox13.Text = "";
                                comboBox2.SelectedIndex = 0;
                                MessageBox.Show("User was added successfully", "Inform",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                int errCode = 0;
                                zkRdr.GetLastError(ref errCode);
                                MessageBox.Show("Sorry, User Add failed. " + GetErrorMessage(errCode), "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            zkRdr.EnableDevice(currDevId, true);
                            toolStripStatusLabel1.Text = "";
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Device not connected";
                        }
                        Cursor.Current = Cursors.Default;
                    }
                    break;
            }
        }

        private void gridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RSI_LAST_TEMPLATE lastTemp = new RSI_LAST_TEMPLATE();
            RSI_EXT_USER_RECORD xUser = new RSI_EXT_USER_RECORD();
            RSI_USER_RECORD pUser = new RSI_USER_RECORD();
            RSI_PROMPT uPrompt = new RSI_PROMPT();
            RSI_EXT_USER_DATA xData = new RSI_EXT_USER_DATA();
            switch (e.ColumnIndex)
            {
                case 6:
                case 7:
                case 8:
                    switch (currDevType.ToUpper())
                    {
                        case "HANDPUNCH":
                            if (MessageBox.Show("Please ask user who will be registring to stand in " +
                                                "front of HandPunch Device. User will be instructed " +
                                                "to place his hand and remove it 3 times. Continue?", "Confirm",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                myRdr.CmdBeep(50, 3);
                                uPrompt = 0;
                                response.rslts_rdy = 0;
                                myRdr.CmdEnterIdleMode();
                                myRdr.CmdEnrollUser(uPrompt, response);
                                System.Threading.Thread.Sleep(20000);
                                myRdr.CmdGetStatus(response);
                                if (response.rslts_rdy == 4)
                                {
                                    myRdr.CmdGetTemplate(lastTemp);
                                    xUser.pTemplateVector.Set(lastTemp.pTemplateVector.Get());
                                    pUser.pTemplateVector.Set(lastTemp.pTemplateVector.Get());
                                    xUser.pID.SetID(gridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                                    pUser.pID.SetID(gridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                                    xUser.pXUD.pName.Set(gridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                                    switch (gridView1.Rows[e.RowIndex].Cells[2].Value)
                                    {
                                        case "None":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                            break;
                                        case "Service":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                            break;
                                        case "Setup":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                            break;
                                        case "Manage":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                            break;
                                        case "Enroll":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_ENROLLMENT;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                            break;
                                        case "Security":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                            break;
                                    }
                                    myRdr.CmdGetReaderInfo(pReaderInfo);
                                    if ((int)pReaderInfo.model == (int)RSI_MODEL.RSI_MODEL_HP4K)
                                    {
                                        myRdr.CmdPutExtUser(xUser);
                                    }
                                    else
                                    {
                                        myRdr.CmdPutUserRecord(pUser);
                                    }
                                    MessageBox.Show("Handpunch Template Saved Successfully");
                                }
                                else
                                {
                                    MessageBox.Show("Handpunch Template Failed");
                                }
                                myRdr.CmdExitIdleMode();
                            }
                            break;
                        case "ZKTECO":
                            try
                            {
                                string myId = gridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                                int idwErrorCode = 0;
                                int iFingerIndex = 0;
                                int iFlag = 0;
                                zkRdr.EnableDevice(currDevId, false);
                                zkRdr.CancelOperation();
                                switch (e.ColumnIndex)
                                {
                                    case 6:
                                        iFingerIndex = 6;       //To Enroll Finger Print
                                        iFlag = 1;
                                        zkRdr.SSR_DelUserTmpExt(currDevId, myId, iFingerIndex);//If the specified index of user's templates has existed ,delete it first.(SSR_DelUserTmp is also available sometimes)
                                        break;
                                    case 7:
                                        iFingerIndex = 111;     //To Enroll Face Print
                                        iFlag = 1;
                                        zkRdr.DelUserFace(currDevId, myId, 50);
                                        break;
                                    case 8:
                                        iFingerIndex = 10;      //To Enroll Palm Print
                                        iFlag = 50;
                                        break;
                                }
                                
                                if (zkRdr.StartEnrollEx(myId, iFingerIndex, iFlag))
                                {
                                    MessageBox.Show("Start to Enroll a new User,UserID=" + myId + " FingerID=" + iFingerIndex.ToString() + " Flag=" + iFlag.ToString(), "Start");
                                    zkRdr.StartIdentify();      //After enrolling templates,you should let the device into the 1:N verification condition
                                }
                                else
                                {
                                    zkRdr.GetLastError(ref idwErrorCode);
                                    MessageBox.Show(GetErrorMessage(idwErrorCode));
                                }
                                zkRdr.EnableDevice(currDevId, true);
                            }
                            catch
                            {
                                MessageBox.Show("Device Not Support", "Inform",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            break;
                    }
                    break;
                case 9:
                    if (MessageBox.Show("Are you sure to delete this ID?", "Confirm",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        switch (currDevType)
                        {
                            case "HANDPUNCH":
                                myRdr.CmdRemoveUser(gridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                                gridView1.Rows.RemoveAt(e.RowIndex);
                                MessageBox.Show("Delete ID done successfully", "inform",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            case "ZKTECO":
                                Cursor.Current = Cursors.WaitCursor;
                                zkRdr.EnableDevice(currDevId, false);
                                zkRdr.SSR_DeleteEnrollData(currDevId,
                                    gridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), 12);
                                gridView1.Rows.RemoveAt(e.RowIndex);
                                zkRdr.EnableDevice(currDevId, true);
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Delete ID done successfully", "inform",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                        }
                    }
                    break;
                case 10:
                    switch (currDevType.ToUpper())
                    {
                        case "HANDPUNCH":
                            myRdr.CmdEnterIdleMode();
                            Cursor.Current = Cursors.WaitCursor;
                            myRdr.CmdGetExtUser(gridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), xUser);
                            myRdr.CmdGetUser(gridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), pUser);
                            byte[] xTemplate = xUser.pTemplateVector.Get();
                            byte[] pTemplate = pUser.pTemplateVector.Get();
                            xUser.pID.SetID(gridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            xUser.pXUD.pName.Set(gridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                            xUser.pTemplateVector.Set(xTemplate);
                            pUser.pID.SetID(gridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            pUser.pTemplateVector.Set(pTemplate);
                            switch (gridView1.Rows[e.RowIndex].Cells[2].Value)
                            {
                                case "None":
                                    xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                    pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                    break;
                                case "Service":
                                    xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                    pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                    break;
                                case "Setup":
                                    xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                    pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                    break;
                                case "Manage":
                                    xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                    pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                    break;
                                case "Enroll":
                                    xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_ENROLLMENT;
                                    pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_ENROLLMENT;
                                    break;
                                case "Security":
                                    xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                    pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                    break;
                            }
                            System.Threading.Thread.Sleep(4000);
                            myRdr.CmdGetReaderInfo(pReaderInfo);
                            if ((int)pReaderInfo.model == (int)RSI_MODEL.RSI_MODEL_HP4K)
                            {
                                myRdr.CmdPutExtUser(xUser);
                            }
                            else
                            {
                                myRdr.CmdPutUserRecord(pUser);
                            }
                            myRdr.CmdExitIdleMode();
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Changes Saved Successfully");
                            break;
                        case "ZKTECO":
                            Cursor.Current = Cursors.WaitCursor;
                            zkRdr.EnableDevice(currDevId, false);
                            int pLevel = 0;
                            string empNum = gridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                            string empName = gridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                            string sLevel = gridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                            bool empEnable = true;
                            int useGrpId = 0;
                            switch (sLevel)
                            {
                                case "None":
                                    pLevel = 0;
                                    break;
                                case "Enroll":
                                    pLevel = 1;
                                    break;
                                case "Admin":
                                    pLevel = 2;
                                    break;
                                case "Super":
                                    pLevel = 3;
                                    break;
                            }
                            switch (gridView1.Rows[e.RowIndex].Cells[4].Value.ToString())
                            {
                                case "Enable":
                                    empEnable = true;
                                    useGrpId = Int32.Parse(gridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                                    break;
                                case "Disable":
                                    empEnable = false;
                                    useGrpId = 2;
                                    break;
                            }
                            if (gridView1.Rows[e.RowIndex].Cells[3].Value.ToString() != "")
                            {
                                zkRdr.SetStrCardNumber(gridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                            }
                            if (zkRdr.SSR_SetUserInfo(currDevId, empNum, empName, "", pLevel, empEnable))
                            {
                                if(!zkRdr.SetUserGroup(currDevId, Int32.Parse(empNum), useGrpId))
                                {
                                    MessageBox.Show("This group not found or device not support group contol", "Warning",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                MessageBox.Show("User was modified successfully", "Inform",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                int errCode = 0;
                                zkRdr.GetLastError(ref errCode);
                                MessageBox.Show("Sorry, user modifying failed. " + GetErrorMessage(errCode), "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            zkRdr.EnableDevice(currDevId, true);
                            Cursor.Current = Cursors.Default;
                            break;
                    }
                    break;
                case 11:
                case 12:
                    if (comboBox3.SelectedIndex == 0 || comboBox3.SelectedIndex == comboBox1.SelectedIndex)
                    {
                        MessageBox.Show("Please Select Target Device From List", "Warninig",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        comboBox3.Focus();
                    }
                    else
                    {
                        DialogResult answer = MessageBox.Show("Are you sure to " + (e.ColumnIndex == 10 ? "Move" : "Copy") + " this ID to target device?", "Confirm",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (answer == DialogResult.Yes)
                        {
                            switch (currDevType.ToUpper())
                            {
                                case "HANDPUNCH":
                                    xUser.pID.SetID(userData[e.RowIndex, 0]);
                                    pUser.pID.SetID(userData[e.RowIndex, 0]);
                                    xUser.pXUD.pName.Set(userData[e.RowIndex, 1]);
                                    switch (userData[e.RowIndex, 2])
                                    {
                                        case "0":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_NONE;
                                            break;
                                        case "1":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SERVICE;
                                            break;
                                        case "2":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SETUP;
                                            break;
                                        case "3":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_MANAGEMENT;
                                            break;
                                        case "4":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_ENROLLMENT;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_ENROLLMENT;
                                            break;
                                        case "5":
                                            xUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                            pUser.authorityLevel = RSI_AUTHORITY_LEVEL.RSI_AUTHORITY_SECURITY;
                                            break;
                                    }
                                    xUser.timeZone = byte.Parse(userData[e.RowIndex, 3]);
                                    pUser.timeZone = byte.Parse(userData[e.RowIndex, 3]);
                                    byte[] temp = new byte[9];
                                    for (int col = 0; col < 9; col++)
                                    {
                                        temp[col] = byte.Parse(userData[e.RowIndex, col + 4]);
                                    }
                                    xUser.pTemplateVector.Set(temp);
                                    pUser.pTemplateVector.Set(temp);
                                    mySql = "Select * From Devices Where Place_id =" +
                                        comboBox3.Text.ToString().Split(' ')[0];
                                    OleDbCommand cmd = new OleDbCommand(mySql, accCon);
                                    OleDbDataReader reader = cmd.ExecuteReader();
                                    reader.Read();
                                    if (currDevType == reader.GetString(4).ToUpper())
                                    {
                                        PlaceName = reader.GetString(7);
                                        string newIpAddr = reader.GetString(1);
                                        int newPort = reader.GetInt32(2);
                                        int newDevId = reader.GetInt32(3);
                                        reader.Close();
                                        if (ConnectPunch(newIpAddr, newPort, newDevId, PlaceName))
                                        {
                                            myRdr.CmdGetReaderInfo(pReaderInfo);
                                            if ((int)pReaderInfo.model == (int)RSI_MODEL.RSI_MODEL_HP4K)
                                            {
                                                if (myRdr.CmdPutExtUser(xUser) == TRUE)
                                                {
                                                    ConnectPunch(currIpAddr, currPort, currDevId, PlaceName);
                                                    if (e.ColumnIndex == 10)
                                                    {
                                                        myRdr.CmdRemoveUser(userData[e.RowIndex, 0]);
                                                    }
                                                    MessageBox.Show((e.ColumnIndex == 10 ? "Move" : "Copy") + " ID done successfully", "inform",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Operation failed, please try again", "Error",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                            }
                                            else
                                            {
                                                if (myRdr.CmdPutUserRecord(pUser) == TRUE)
                                                {
                                                    ConnectPunch(currIpAddr, currPort, currDevId, PlaceName);
                                                    if (e.ColumnIndex == 10)
                                                    {
                                                        myRdr.CmdRemoveUser(userData[e.RowIndex, 0]);
                                                    }
                                                    MessageBox.Show((e.ColumnIndex == 10 ? "Move" : "Copy") + " ID done successfully", "inform",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Operation failed, please try again", "Error",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Couldn't connect to the target device", "Error",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    else
                                    {
                                        reader.Close();
                                        MessageBox.Show("The target device type must be the same as the source device", "Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    break;
                                case "ZKTECO":
                                    if (MyGlobal.IsConnected2)
                                    {
                                        string iEnrollNumber = gridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                                        string iName = "";
                                        string iPassword = "";
                                        int iPrivilege = 0;
                                        bool iEnabled = false;
                                        string iTmpData = "";
                                        int iTmpLength = 0;
                                        string cardNum;
                                        //zkRdr.EnableDevice(newDevId, false);
                                        //zkRdr2.EnableDevice(newDevId2, false);
                                        zkRdr.ReadAllUserID(currDevId);
                                        zkRdr.ReadAllTemplate(currDevId);
                                        zkRdr.SSR_GetUserInfo(currDevId, iEnrollNumber, out iName,
                                                out iPassword, out iPrivilege, out iEnabled);
                                        zkRdr.GetStrCardNumber(out cardNum);
                                        if (zkRdr2.SSR_SetUserInfo(newDevId2, iEnrollNumber, iName,
                                            iPassword, iPrivilege, iEnabled))
                                        {
                                            zkRdr2.SetStrCardNumber(cardNum);
                                            try
                                            {
                                                for (int fingerIndex = 0; fingerIndex < 10; fingerIndex++)
                                                {
                                                    iTmpData = "";
                                                    iTmpLength = 0;
                                                    zkRdr.SSR_GetUserTmpStr(currDevId, iEnrollNumber,
                                                        fingerIndex, out iTmpData, out iTmpLength);
                                                    if (!string.IsNullOrEmpty(iTmpData))
                                                    {
                                                        zkRdr2.SSR_DelUserTmpExt(newDevId2, iEnrollNumber, fingerIndex);
                                                        zkRdr2.SetUserTmpExStr(newDevId2, iEnrollNumber,
                                                            1, fingerIndex, iTmpData);
                                                    }
                                                }
                                            }
                                            catch { };
                                            iTmpData = "";
                                            iTmpLength = 0;
                                            try
                                            {
                                                zkRdr.GetUserFaceStr(currDevId, iEnrollNumber, 50, ref iTmpData, ref iTmpLength);
                                                if (!string.IsNullOrEmpty(iTmpData))
                                                {
                                                    zkRdr2.DelUserFace(newDevId2, iEnrollNumber, 50);
                                                    if (!zkRdr2.SetUserFaceStr(newDevId2, iEnrollNumber,
                                                        50, iTmpData, iTmpLength))
                                                    {
                                                        int err = 0;
                                                        zkRdr2.GetLastError(err);
                                                        MessageBox.Show(GetErrorMessage(err));
                                                    }
                                                }
                                            }
                                            catch { };
                                        }
                                        if (e.ColumnIndex == 10)
                                        {
                                            zkRdr.SSR_DeleteEnrollData(currDevId, iEnrollNumber, 12);
                                        }
                                        //zkRdr.EnableDevice(newDevId, true);
                                        zkRdr2.RefreshData(newDevId2);
                                        zkRdr2.EnableDevice(newDevId2, true);
                                        MessageBox.Show((e.ColumnIndex == 10 ? "Move" : "Copy") + " user to the target device done.", "Alert",
                                             MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string iName , iPass , strAuth ="" , iCardNo , status ;
            string[] iTempData = new string[10];
            int iPrivilege , iTempLength, iFaceIndex = 50;
            bool iEnable;
            string[] row;
            byte[] zeroTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            RSI_EXT_USER_RECORD rUser = new RSI_EXT_USER_RECORD();
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Please choose device from list", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            gridView1.Rows.Clear();
            Cursor.Current = Cursors.WaitCursor;
            switch (currDevType)
            {
                case "HANDPUNCH":
                    if (myRdr.CmdGetExtUser(textBox11.Text, rUser) == 0)
                    {
                        switch (((int)rUser.authorityLevel))
                        {
                            case 0:
                                strAuth = "None";
                                break;
                            case 1:
                                strAuth = "Service";
                                break;
                            case 2:
                                strAuth = "Setup";
                                break;
                            case 3:
                                strAuth = "Manage";
                                break;
                            case 4:
                                strAuth = "Enroll";
                                break;
                            case 5:
                                strAuth = "Security";
                                break;
                        }
                        iName = rUser.pXUD.pName.Get();
                        if (string.IsNullOrEmpty(iName))
                        {
                            try
                            {
                                mySql = "Select Emp_Name From Employee Where EMP_ID='" + textBox11.Text + "'";
                                cmd = new OleDbCommand(mySql, accCon);
                                reader = cmd.ExecuteReader();
                                reader.Read();
                                iName = reader.GetString(0);
                                reader.Close();
                            }
                            catch
                            {
                                iName = "غير معروف";
                            }
                        }
                        row = new string[] { textBox11.Text, iName, strAuth, "", "", };
                        gridView1.Rows.Add(row);
                        if (Enumerable.SequenceEqual(zeroTemp, rUser.pTemplateVector.Get()))
                        {
                            gridView1.Rows[0].Cells[8].Value = Properties.Resources.palm_off_16;
                        }
                        else
                        {
                            gridView1.Rows[0].Cells[8].Value = Properties.Resources.palm_on_16;
                        }
                        gridView1.Rows[0].Cells[7].Value = Properties.Resources.face_off_16;
                        gridView1.Rows[0].Cells[6].Value = Properties.Resources.tap_off_16;
                    }
                    else
                    {
                        MessageBox.Show("No employee data for this number.\nOr this device not support this feature", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "ZKTECO":
                    zkRdr.EnableDevice(currDevId, false);
                    if (zkRdr.SSR_GetUserInfo(currDevId, textBox11.Text, out iName, out iPass, out iPrivilege, out iEnable))
                    {
                        zkRdr.GetStrCardNumber(out iCardNo);
                        switch (iPrivilege)
                        {
                            case 0:
                                strAuth = "None";
                                break;
                            case 1:
                                strAuth = "Enroll";
                                break;
                            case 2:
                                strAuth = "Admin";
                                break;
                            case 3:
                                strAuth = "Super";
                                break;
                        }
                        try
                        {
                            mySql = "Select Emp_Name From Employee Where EMP_ID='" + textBox11.Text + "'";
                            cmd = new OleDbCommand(mySql, accCon);
                            reader = cmd.ExecuteReader();
                            reader.Read();
                            iName = reader.GetString(0);
                            reader.Close();
                        }
                        catch
                        {
                            iName = "غير معروف";
                        }
                        if (iEnable)
                        {
                            status = "Enable";
                        }
                        else
                        {
                            status = "Disable";
                        };
                        row = new string[] { textBox11.Text, iName, strAuth, iCardNo, status, };
                        gridView1.Rows.Add(row);
                        for (int i = 0; i < 10; i++)
                        {
                            iTempLength = 0;
                            zkRdr.SSR_GetUserTmpStr(currDevId, textBox11.Text, i, out iTempData[i], out iTempLength);
                            if (iTempLength > 0)
                            {
                                gridView1.Rows[0].Cells[6].Value = Properties.Resources.tap_on_16;
                                break;
                            }
                            else
                            {
                                gridView1.Rows[0].Cells[6].Value = Properties.Resources.tap_off_16;
                            }
                        }
                        iTempLength = 0;
                        iTempData[0] = "";
                        zkRdr.GetUserFaceStr(currDevId, textBox11.Text, iFaceIndex, iTempData[0], iTempLength);
                        if (string.IsNullOrEmpty(iTempData[0]))
                        {
                            gridView1.Rows[0].Cells[7].Value = Properties.Resources.face_off_16;
                        }
                        else
                        {
                            gridView1.Rows[0].Cells[7].Value = Properties.Resources.face_on_16;
                        }
                        gridView1.Rows[0].Cells[8].Value = Properties.Resources.palm_off_16;
                    }
                    else
                    {
                        MessageBox.Show("No employee data for this number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    zkRdr.EnableDevice(currDevId, true);
                    break;
            }
            Cursor.Current = Cursors.Default;
            gridView1.Enabled = false;
            backupButt.Enabled = false;
            saveButton.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox11.Text.Trim() != "")
            {
                for (int i = 0; i < gridView1.Rows.Count; i++)
                {
                    if (gridView1.Rows[i].Cells[0].Value.Equals(textBox11.Text))
                    {
                        gridView1.Rows[i].Cells[0].Selected = true;
                        break;
                    }
                }
            }
            else
            {
                if (textBox13.Text.Trim() != "")
                {
                    for (int i = 0; i < gridView1.Rows.Count; i++)
                    {
                        if (gridView1.Rows[i].Cells[1].Value.ToString().Contains(textBox13.Text))
                        {
                            gridView1.Rows[i].Cells[1].Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            MyToolTip.Show("To cancel current operation", button1);
        }

        private void addUser_MouseHover(object sender, EventArgs e)
        {
            MyToolTip.Show("To add a new user to the device", addUser);
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            MyToolTip.Show("To search user in the list", button2);
        }

        private void getUserList_MouseHover(object sender, EventArgs e)
        {
            MyToolTip.Show("To load users infomation from device", getUserList);
        }

        private void backupButt_MouseHover(object sender, EventArgs e)
        {
            MyToolTip.Show("To backup users information to the disk", backupButt);
        }

        private void restoreButt_MouseHover(object sender, EventArgs e)
        {
            MyToolTip.Show("To restore users information from disk", restoreButt);
        }

        private void unclockButt_MouseHover(object sender, EventArgs e)
        {
            MyToolTip.Show("To open the connected door", unclockButt);
        }

        private void restartButt_MouseHover(object sender, EventArgs e)
        {
            MyToolTip.Show("To restart current device", restartButt);
        }

        private void powerButt_MouseHover(object sender, EventArgs e)
        {
            MyToolTip.Show("To shutdown current device", powerButt);
        }

        private void UsersList_FormClosing(object sender, FormClosingEventArgs e)
        {
            zkRdr.Disconnect();
            zkRdr2.Disconnect();
        }

        private void unlockButt(object sender, EventArgs e)
        {
            if (currDevType == "ZKTECO")
            {
                int hasFun = 0;
                zkRdr.GetACFun(ref hasFun);
                if (hasFun > 0)
                {
                    zkRdr.ACUnlock(currDevId, 10);
                }
                else
                {
                    MessageBox.Show("This device has no access to door", "Inform",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("This kind of device has no AC Lock function", "Inform",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void restart_Click(object sender, EventArgs e)
        {
            if (currDevType == "ZKTECO")
            {
                if (MessageBox.Show("Are you sure to continue restart device?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    zkRdr.RestartDevice(currDevId);
                    zkRdr.Disconnect();
                    pictureBox1.BackgroundImage = Properties.Resources.logo;
                    pictureBox1.Update();
                    label5.Text = "";
                    label5.Update();
                    getUserList.Enabled = false;
                    backupButt.Enabled = false;
                    restoreButt.Enabled = false;
                    addUser.Enabled = false;
                    unclockButt.Enabled = false;
                    restartButt.Enabled = false;
                    powerButt.Enabled = false;

                }
            }
            else
            {
                MessageBox.Show("This kind of device has no restart function", "Inform",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void power_Click(object sender, EventArgs e)
        {
            if (currDevType == "ZKTECO")
            {
                if (MessageBox.Show("Are you sure to continue shutdown device?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    
                    zkRdr.PowerOffDevice(currDevId);
                    zkRdr.Disconnect();
                    pictureBox1.BackgroundImage = Properties.Resources.logo;
                    pictureBox1.Update();
                    label5.Text = "";
                    label5.Update();
                    getUserList.Enabled = false;
                    backupButt.Enabled = false;
                    restoreButt.Enabled = false;
                    addUser.Enabled = false;
                    unclockButt.Enabled = false;
                    restartButt.Enabled = false;
                    powerButt.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("This kind of device has no power off function", "Inform",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileName = new FileInfo(saveFileDialog1.FileName);
                using (var package = new ExcelPackage())
                {
                    using (var worksheet = package.Workbook.Worksheets.Add("Attendance"))
                    {
                        worksheet.Cells[1, 1].Value = comboBox1.Text +
                                " Users List";
                        worksheet.Cells["A1:C1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:C1"].Merge = true;
                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.Font.Size = 16;
                        worksheet.Cells[1, 1, 1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1, 1, 3].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        worksheet.Cells[3, 1, 3, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[3, 1, 3, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Column(2).Width = 26;
                        worksheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(3).Width = 18;
                        for (int j = 0; j < 3; j++)
                        {
                            worksheet.Cells[3, j + 1].Value = gridView1.Columns[j].HeaderText;
                        }
                        for (int i = 2; i < gridView1.Rows.Count + 2; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                worksheet.Cells[i + 2, j + 1].Value = gridView1.Rows[i - 2].Cells[j].Value?.ToString() ?? "";
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

        private void adminButt_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("This process will delete all admins from this device.\n\nThe menu will be open for anyone.\n\nAre you sure to continue?", "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                zkRdr.ClearAdministrators(currDevId);
                MessageBox.Show("All admins in this device have been deleted.\n\nThe menu is open, any one can acccess the menu.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                MyGlobal.IsConnected2 = false;
                mySql = "Select * From Devices Where Place_id =" +
                                            comboBox3.Text.ToString().Split(' ')[0];
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Read();
                if (currDevType == reader.GetString(4).ToUpper())
                {
                    PlaceNo = reader.GetInt32(6);
                    PlaceName = reader.GetString(7);
                    string newIpAddr = reader.GetString(1);
                    int newPort = reader.GetInt32(2);
                    newDevId2 = reader.GetInt32(3);
                    string newPass = reader.GetString(8).ToUpper();
                    reader.Close();
                    if (newPass.Trim() != "0")
                    {
                        zkRdr2.SetCommPassword(int.Parse(newPass));
                    }
                    else
                    {
                        zkRdr2.SetCommPassword(0);
                    }
                    if (currDevType.ToUpper() == "HANDPUNCH")
                    {
                        MyGlobal.IsConnected2 = ConnectPunch(newIpAddr, newPort, newDevId2, PlaceName);
                    }
                    else
                    {
                        MyGlobal.IsConnected2 = zkRdr2.Connect_Net(newIpAddr, newPort);
                    }
                    if (MyGlobal.IsConnected2)
                    {
                        label31.ForeColor = Color.Green;
                    }
                    else
                    {
                        label31.ForeColor = Color.Red;
                    }
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("The target device type must be the same as the source device", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MyGlobal.IsConnected2 = false;
                zkRdr2.Disconnect();
                label31.ForeColor = Color.Red;
            }
            Cursor.Current = Cursors.Default;
        }

        private void getUserList_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select device you want to read",
                    "Select Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            gridView1.Rows.Clear();
            gridView1.Enabled = true;
            switch (currDevType)
            {
                case "HANDPUNCH":
                    if (MyGlobal.IsConnected)
                    {
                        int RecordSize = 77;  // for HP4K or 16 for others
                        if (myRdr.CmdGetReaderInfo(pReaderInfo) == FALSE)
                        {
                            toolStripStatusLabel1.Text = "Couldn't read target device";
                            return;
                        }
                        userData = new string[pReaderInfo.usersEnrolled, 13];
                        switch (((int)pReaderInfo.model))
                        {
                            case ((int)RSI_MODEL.RSI_MODEL_HP2K):
                                RecordSize = 16;
                                break;
                            case ((int)RSI_MODEL.RSI_MODEL_HP3K):
                                RecordSize = 16;
                                break;
                            case ((int)RSI_MODEL.RSI_MODEL_HP4K):
                                RecordSize = 77;
                                break;
                            default:
                                RecordSize = 16;
                                break;
                        }
                        CRsiDataBank DataBank = new CRsiDataBank();
                        ushort BankNo = 0;
                        int UserCount = 0;
                        int NumberOfBanks = 10; //for HP4K512  
                        int UsersPerBank = (4096 / RecordSize) - 1;
                        byte[] zeroTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        string[] row;
                        Cursor.Current = Cursors.WaitCursor;
                        myRdr.CmdEnterIdleMode();
                        int RowNo = 0;
                        while (BankNo < NumberOfBanks)
                        {
                            int rslt = myRdr.CmdGetDataBank(BankNo, DataBank);
                            if (rslt == FALSE)
                            {
                                return;
                            }
                            for (int RecNo = 0; RecNo < pReaderInfo.usersEnrolled; RecNo++)
                            {
                                string name = "";
                                string id = "";
                                string auth = "";
                                string zone = "";
                                byte[] temp = new byte[9];
                                try
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        id += DataBank.Get()[i + (RecordSize * RecNo)].ToString("X2");
                                    }
                                }
                                catch { }
                                if (id == "FFFFFFFFFF" || id == "")
                                {
                                    BankNo++;
                                    continue;
                                }
                                try
                                {
                                    for (int i = 5; i < 14; i++)
                                    {
                                        temp[i - 5] = DataBank.Get()[i + (RecordSize * RecNo)];
                                    }
                                }catch { }
                                try
                                {
                                    for (int i = 14; i < 15; i++)
                                    {
                                        auth += DataBank.Get()[i + (RecordSize * RecNo)];
                                    }
                                }
                                catch { }
                                try
                                {
                                    for (int i = 15; i < 16; i++)
                                    {
                                        zone += DataBank.Get()[i + (RecordSize * RecNo)];
                                    }
                                }
                                catch { }
                                try
                                {
                                    if (RecordSize == 77)
                                    {
                                        for (int i = 30; i < 46; i++)
                                        {
                                            name += (char)DataBank.Get()[i + (RecordSize * RecNo)];
                                        }
                                    }
                                }
                                catch { }
                                string strAuth = "";
                                switch (int.Parse(auth))
                                {
                                    case 0:
                                        strAuth = "None";
                                        break;
                                    case 1:
                                        strAuth = "Service";
                                        break;
                                    case 2:
                                        strAuth = "Setup";
                                        break;
                                    case 3:
                                        strAuth = "Manage";
                                        break;
                                    case 4:
                                        strAuth = "Enroll";
                                        break;
                                    case 5:
                                        strAuth = "Security";
                                        break;
                                }
                                try
                                {
                                    mySql = "Select Emp_Name From Employee Where EMP_ID='" + id.TrimStart('0') + "'";
                                    cmd = new OleDbCommand(mySql, accCon);
                                    reader = cmd.ExecuteReader();
                                    reader.Read();
                                    name = reader.GetString(0);
                                    reader.Close();
                                }
                                catch
                                {
                                    name = "غير معروف";
                                }
                                row = new string[] { id.TrimStart('0'), name, strAuth, zone };
                                gridView1.Rows.Add(row);
                                userData[RowNo, 0] = id.TrimStart('0');
                                userData[RowNo, 1] = name;
                                userData[RowNo, 2] = auth;
                                userData[RowNo, 3] = zone;
                                for (int i = 0; i < 9; i++)
                                {
                                    userData[RowNo, i + 4] = temp[i].ToString();
                                }
                                gridView1.Rows[RowNo].Cells[6].Value = Properties.Resources.tap_off_16;
                                gridView1.Rows[RowNo].Cells[7].Value = Properties.Resources.face_off_16;
                                if (Enumerable.SequenceEqual(zeroTemp, temp))
                                {
                                    gridView1.Rows[RowNo].Cells[8].Value = Properties.Resources.palm_off_16;
                                }
                                else
                                {
                                    gridView1.Rows[RowNo].Cells[8].Value = Properties.Resources.palm_on_16;
                                }
                                RowNo++;
                                UserCount++;
                                UsrCount.Text = "Users " + UserCount;
                                UsrCount.Update();
                            }
                            BankNo++;
                        }
                        myRdr.CmdExitIdleMode();
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Device not connected.";
                    }
                    break;
                case "ZKTECO":
                    Cursor.Current = Cursors.WaitCursor;
                    zkRdr.EnableDevice(currDevId, false);
                    zkRdr.ReadAllUserID(currDevId);//read all the user information to the memory
                    if (MyGlobal.IsConnected)
                    {
                        GetZkAllUserInfo(currDevId);
                        toolStripStatusLabel1.Text = "";
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Device not connected.";
                    }
                    zkRdr.EnableDevice(currDevId, true);
                    Cursor.Current = Cursors.Default;
                    break;
            }
            backupButt.Enabled = true;
            saveButton.Enabled = true;
        }

        private string GetErrorMessage(int errorCode)
        {
            string ErrorMessage = "Not Found";
            
            for (int i = 0; i <= 10; i++)
            {
                if (ZkError[i, 0] == errorCode.ToString())
                {
                    ErrorMessage = ZkError[i, 1];
                    break;
                }
            }
            return ErrorMessage;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            gridView1.Rows.Clear();
            if (comboBox1.SelectedIndex > 0)
            {
                mySql = "Select * From Devices Where Place_id =" +
                    comboBox1.Text.ToString().Split(' ')[0];
                cmd = new OleDbCommand(mySql, accCon);
                reader = cmd.ExecuteReader();
                reader.Read();
                PlaceName = reader.GetString(7);
                currIpAddr = reader.GetString(1);
                currPort = reader.GetInt32(2);
                currDevId = reader.GetInt32(3);
                currDevType = reader.GetString(4).ToUpper();
                currPass = reader.GetString(8).ToUpper();
                reader.Close();
                switch (currDevType.ToUpper())
                {
                    case "HANDPUNCH":
                        comboBox2.Items.Clear();
                        comboBox2.Items.Add("None");
                        comboBox2.Items.Add("Service");
                        comboBox2.Items.Add("Setup");
                        comboBox2.Items.Add("Mange");
                        comboBox2.Items.Add("Enroll");
                        comboBox2.Items.Add("Security");
                        level.Items.Clear();
                        level.Items.Add("None");
                        level.Items.Add("Service");
                        level.Items.Add("Setup");
                        level.Items.Add("Mange");
                        level.Items.Add("Enroll");
                        level.Items.Add("Security");
                        gridView1.Columns[3].HeaderText = "Time Zone";
                        userStatus.Items.Clear();
                        Cursor.Current = Cursors.WaitCursor;
                        ConnectPunch(currIpAddr, currPort, currDevId, PlaceName);
                        getUserList.Enabled = MyGlobal.IsConnected;
                        //backupButt.Enabled = MyGlobal.IsConnected;
                        restoreButt.Enabled = MyGlobal.IsConnected;
                        addUser.Enabled = MyGlobal.IsConnected;
                        adminButt.Enabled = false;
                        unclockButt.Enabled = false;
                        Cursor.Current = Cursors.Default;
                        break;
                    case "ZKTECO":
                        comboBox2.Items.Clear();
                        comboBox2.Items.Add("None");
                        comboBox2.Items.Add("Enroll");
                        comboBox2.Items.Add("Admin");
                        comboBox2.Items.Add("Super");
                        level.Items.Clear();
                        level.Items.Add("None");
                        level.Items.Add("Enroll");
                        level.Items.Add("Admin");
                        level.Items.Add("Super");
                        gridView1.Columns[3].HeaderText = "Card Number";
                        userStatus.Items.Clear();
                        userStatus.Items.Add("Enable");
                        userStatus.Items.Add("Disable");
                        Cursor.Current = Cursors.WaitCursor;
                        ZktConnect(currIpAddr, currPort, PlaceName, currPass);
                        getUserList.Enabled = MyGlobal.IsConnected;
                        //backupButt.Enabled = MyGlobal.IsConnected;
                        restoreButt.Enabled = MyGlobal.IsConnected;
                        addUser.Enabled = MyGlobal.IsConnected;
                        unclockButt.Enabled = MyGlobal.IsConnected;
                        restartButt.Enabled = MyGlobal.IsConnected; ;
                        powerButt.Enabled = MyGlobal.IsConnected; ;
                        adminButt.Enabled = MyGlobal.IsConnected;   
                        Cursor.Current = Cursors.Default;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //myIP.Disconnect();
                //myIP.ResetSocket();
                zkRdr.Disconnect();
                pictureBox1.BackgroundImage = Properties.Resources.logo;
                pictureBox1.Update();
                label5.Text = "";
                label5.Update();
                getUserList.Enabled = false;
                backupButt.Enabled = false;
                restoreButt.Enabled = false;
                addUser.Enabled = false;
                unclockButt.Enabled = false;
                restartButt.Enabled = false;
                powerButt.Enabled = false;
                adminButt.Enabled = false;
            }
        }

        public string ShowBackupDialog(string[] backList)
        {
            string myAnswer = "";
            int index = 20 + 25 * backList.Length;
            int groupZeroHeight = (backList.Length > 0) ? 40 : 20;
            Form prompt = new Form()
            {
                Width = 400,
                Height = 230 + backList.Length * 20,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Backup List",
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = MyGlobal.color2,
                TopMost = true,
                MaximizeBox = false,
                MinimizeBox = false,
                Font = new Font("MS Reference Sans Serif", 9, FontStyle.Regular),
            };
            GroupBox group = new GroupBox()
            {
                Text = "Choose Backup",
                Left = 30,
                Top = 20,
                Width = 325,
                Height = 20 * backList.Length + groupZeroHeight + 20,
            };
            Button confirmation = new Button()
            {
                Text = "Restore",
                Left = 300,
                Top = 100 + index,
                Width = 78,
                Height = 30,
                Enabled = false,
                TabIndex = 2,
                DialogResult = DialogResult.OK
            };
            Button deletetion = new Button()
            {
                Text = "Delete",
                Left = 216,
                Top = 100 + index,
                Width = 78,
                Height = 30,
                Enabled = false,
                TabIndex = 1, 
                DialogResult = DialogResult.Yes
            };
            Button cancelation = new Button()
            {
                Text = "Cancel",
                Left = 132,
                Top = 100 + index,
                Width = 78,
                Height = 30,
                TabIndex = 0,
                DialogResult = DialogResult.Cancel
            };
            index = 20;
            RadioButton[] nickName = new RadioButton[backList.Length];
            for (int i = 0; i < backList.Length; i++)
            {
                nickName[i] = new RadioButton();
                nickName[i].Left = 30;
                nickName[i].Top = 20 + 25 * i;
                nickName[i].Text = backList[i];
                nickName[i].Width = 290;
                nickName[i].Click += new EventHandler(nickName_click);
                group.Controls.Add(nickName[i]);
                index += 25;
            }
            System.Windows.Forms.Label emoNo = new System.Windows.Forms.Label()
            {
                Left = 30,
                Top = 58 + index,
                Width = 260,
                Text = "Restore Employee No (Empty Means All)"
            };
            TextBox textBox = new TextBox() {
                Left = 308, 
                Top = 55 + index, 
                Width = 50
            };
            prompt.Controls.Add(group);
            prompt.Controls.Add(emoNo);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(deletetion);
            prompt.Controls.Add(cancelation);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancelation;
            void nickName_click(object sender, EventArgs e)
            {
                confirmation.Enabled = true;
                deletetion.Enabled = true;
            }
            switch (prompt.ShowDialog())
            {
                case DialogResult.OK:
                    foreach (Control ctrl in group.Controls)
                    {
                        RadioButton selected = (RadioButton)ctrl;
                        if (selected.Checked)
                        {
                            myAnswer = selected.Text.Split(' ')[2];
                            if (textBox.Text.Trim() != "")
                            {
                                myAnswer += "," + textBox.Text;
                            }
                            else
                            {
                                myAnswer += ",0";
                            }
                            break;
                        }
                        else
                        {
                            myAnswer = "";
                        }
                        
                    }
                    break;
                case DialogResult.Yes:
                    foreach (Control ctrl in group.Controls)
                    {
                        RadioButton selected = (RadioButton)ctrl;
                        if (selected.Checked)
                        {
                            myAnswer = "Done," + selected.Text.Split(' ')[2];
                            break;
                        }
                        else
                        {
                            myAnswer = "";
                        }
                    }
                    break;
                case DialogResult.Cancel:
                    myAnswer = "";
                    break;
            }
            return myAnswer;
        }
    }

    [Serializable]
    public class ZktObject
    {
        public string empId;
        public string name;
        public int authority;
        public string cardNo;
        public bool status;
        public string[] rTemplate = new string[10];
        public string sTemplate;
        public int[] flag = new int[10];
        public string pTemplate;
    }

}
