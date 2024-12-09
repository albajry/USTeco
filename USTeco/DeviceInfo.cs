using RecogSys.RdrAccess;
using System;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using zkemkeeper;

namespace USTeco
{
    public partial class DeviceInfo : Form
    {
        OleDbConnection accCon = new OleDbConnection();
        CRsiComWinsock myIP = new CRsiComWinsock();
        CZKEM zkRdr = new CZKEM();
        CRsiHandReader myRdr;
        CRsiNetwork myNetwork;
        RSI_STATUS pStatus = new RSI_STATUS();
        RSI_READER_INFO pReaderInfo = new RSI_READER_INFO();
        RSI_SETUP_DATA pSetup = new RSI_SETUP_DATA();
        RSI_EXT_SETUP_DATA xSetup = new RSI_EXT_SETUP_DATA();
        RSI_TIME_DATE pTimeDate = new RSI_TIME_DATE();
        RSI_TEMPLATE template = new RSI_TEMPLATE();
        RSI_STATUS response = new RSI_STATUS();
        //RSI_ERROR rsiError = new RSI_ERROR();
        const int TRUE = 1;
        const int FALSE = 0;
        string currIpAddr, currDevType, currPass, PlaceName,mySql;
        int currDevId, currPort;
        OleDbCommand cmd;
        string myDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UST Applications", "Handpunch");
        public DeviceInfo()
        {
            InitializeComponent();
            accCon.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + myDataPath + "\\handdb.mdb; Jet OLEDB:Database Password = Ajoset25";
            accCon.Open();
            BackColor = MyGlobal.color1;
            panel19.BackColor = MyGlobal.color2;
            mySql = "Select Place_id,Place_Name From Devices";
            comboBox1.Items.Add("Select Device");
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
                comboBox1.Items.Add(MyPlace.ToString() + "-" + PlaceName);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void getButtn_Click(object sender, EventArgs e)
        {
            string fmt = "00";
            string fmy = "2000";
           
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select device you want to read",
                    "Select Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            switch (currDevType.ToUpper())
            {
                case "HANDPUNCH":
                    if (MyGlobal.IsConnected)
                    {
                        toolStripStatusLabel1.Text = "Successful connectino to target device";
                        label6.Text = "Adapter Type";
                        textBox1.Text = "Hand Key";
                        if (myRdr.CmdGetReaderInfo(pReaderInfo) == FALSE)
                        {
                            toolStripStatusLabel1.Text = "Couldn't read information from target device";
                            clearText();
                            return;
                        }
                        myRdr.CmdGetTime(pTimeDate);
                        textBox13.Text = pReaderInfo.model.ToString().Split('_')[2];
                        try
                        {
                            textBox13.Text += " " + pReaderInfo.model.ToString().Split('_')[3];
                        }
                        catch { };
                        textBox2.Text = pReaderInfo.userCap.ToString();
                        textBox3.Text = pReaderInfo.usersEnrolled.ToString();
                        textBox4.Text = pReaderInfo.dlogCap.ToString();
                        textBox5.Text = pReaderInfo.dlogsPresent.ToString();
                        textBox6.Text = pTimeDate.hour.ToString(fmt) + ":" + pTimeDate.minute.ToString(fmt) + ":" + pTimeDate.second.ToString(fmt); ;
                        textBox7.Text = pTimeDate.day.ToString(fmt) + "/" + pTimeDate.month.ToString(fmt) + "/" + pTimeDate.year.ToString(fmy);
                        switch (((int)pReaderInfo.memory))
                        {
                            case 0:
                                textBox8.Text = "128KB";
                                break;
                            case 1:
                                textBox8.Text = "256KB";
                                break;
                            case 2:
                                textBox8.Text = "512KB";
                                break;
                            default:
                                textBox8.Text = "???KB";
                                break;
                        }
                        textBox9.Text = pReaderInfo.pPromName.Get();
                        textBox10.Text = pReaderInfo.pPromDate.Get();
                        textBox14.Text = pReaderInfo.sn.ToString();
                        textBox21.Text = pReaderInfo.adaptorType.ToString().Split('_')[1];
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Device not connected.";
                    }
                    break;
                case "ZKTECO":
                    int dwYear = 0; int dwMonth = 0; int dwDay = 0;
                    int dwHour = 0; int dwMinute = 0; int dwSecond = 0;
                    string stVersion = "";
                    label6.Text = "MAC Number";
                    clearText();
                    if (MyGlobal.IsConnected)
                    {
                        toolStripStatusLabel1.Text = "Successful connection to target device";
                        int statusValue = 0;
                        if (zkRdr.GetDeviceStatus(currDevId, 8, ref statusValue))
                        {
                            textBox2.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceStatus(currDevId, 2, ref statusValue))
                        {
                            textBox3.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceStatus(currDevId, 9, ref statusValue))
                        {
                            textBox4.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceStatus(currDevId, 6, ref statusValue))
                        {
                            textBox5.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceStatus(currDevId, 22, ref statusValue))
                        {
                            textBox15.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceStatus(currDevId, 21, ref statusValue))
                        {
                            textBox16.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceStatus(currDevId, 1, ref statusValue))
                        {
                            textBox17.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceStatus(currDevId, 3, ref statusValue))
                        {
                            textBox18.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceStatus(currDevId, 7, ref statusValue))
                        {
                            textBox19.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceInfo(currDevId, 64, ref statusValue))
                        {
                            textBox20.Text = statusValue.ToString();
                        }
                        if (zkRdr.GetDeviceTime(currDevId, ref dwYear, ref dwMonth, ref dwDay,
                            ref dwHour, ref dwMinute, ref dwSecond))
                        {
                            textBox6.Text = dwHour.ToString(fmt) + ":" +
                                dwMinute.ToString(fmt) + ":" + dwSecond.ToString(fmt);
                            textBox7.Text = dwYear + "/" +
                                dwMonth.ToString(fmt) + "/" + dwDay.ToString(fmt);
                        }
                        if (zkRdr.GetFirmwareVersion(currDevId, ref stVersion))
                        {
                            textBox9.Text = stVersion.Split(' ')[0] + " " + stVersion.Split(' ')[1];
                            textBox10.Text = stVersion.Split(' ')[2] + " " + stVersion.Split(' ')[3] +
                                " " + stVersion.Split(' ')[4];
                        }
                        string devTypeName = "";
                        if (zkRdr.GetPlatform(currDevId, ref devTypeName))
                        {
                            textBox1.Text = devTypeName;
                        }
                        string sProductCode = "";
                        if (zkRdr.GetProductCode(currDevId, out sProductCode))
                        {
                            textBox13.Text = sProductCode;
                        }
                        string sdwSerialNumber = "";
                        if (zkRdr.GetSerialNumber(currDevId, out sdwSerialNumber))
                        {
                            textBox14.Text = sdwSerialNumber;
                        }
                        string sMAC = "";
                        if (zkRdr.GetDeviceMAC(currDevId, ref sMAC))
                        {
                            textBox21.Text = sMAC;
                        }
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Device not connected. Reselect it from the list";
                    }
                    break;
            }
            Cursor.Current = Cursors.Default;
        }
        void clearText()
        {
            textBox1.Text = ""; textBox1.Update();
            textBox2.Text = ""; textBox2.Update();
            textBox3.Text = ""; textBox3.Update();
            textBox4.Text = ""; textBox4.Update();
            textBox5.Text = ""; textBox5.Update();
            textBox6.Text = ""; textBox6.Update();
            textBox7.Text = ""; textBox7.Update();
            textBox8.Text = ""; textBox8.Update();
            textBox9.Text = ""; textBox9.Update();
            textBox10.Text = ""; textBox10.Update();
            textBox13.Text = ""; textBox13.Update();
            textBox14.Text = ""; textBox14.Update();
            textBox15.Text = ""; textBox15.Update();
            textBox16.Text = ""; textBox16.Update();
            textBox17.Text = ""; textBox17.Update();
            textBox18.Text = ""; textBox18.Update();
            textBox19.Text = ""; textBox19.Update();
            textBox20.Text = ""; textBox20.Update();
            textBox21.Text = ""; textBox21.Update();
            toolStripStatusLabel1.Text = "";
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

        private bool ConnectPunch(String ipAdd, int port, int devId, String placeId)
        {
            myIP.SetHost(ipAdd);
            myIP.SetPortA(ushort.Parse(port.ToString()));
            myRdr = new CRsiHandReader(myIP, ((byte)devId));
            if (FALSE == myIP.Ping())
            {
                toolStripStatusLabel1.Text = "Unable to Ping (" + placeId + ") ";
                getButtn.BackgroundImage = Properties.Resources.logo;
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
                    getButtn.BackgroundImage = Properties.Resources.logo;
                    myIP.Disconnect();
                    myIP.ResetSocket();
                    MyGlobal.IsConnected = false;
                    return false;
                }
                else
                {
                    myNetwork = new CRsiNetwork(myIP);
                    myNetwork.Attach(myRdr);
                    RSI_TIME_DATE currentDateTime = new RSI_TIME_DATE();
                    if (myRdr.CmdGetTime(currentDateTime) == TRUE)  // Gets current time from reader.      
                    {
                        getButtn.BackgroundImage = Properties.Resources.handpunch;
                        MyGlobal.IsConnected = true;
                        textBox1.Update();
                        return true;
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Couldn't read from (" + placeId + ") ";
                        getButtn.BackgroundImage = Properties.Resources.logo;
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

                    //int fingerCount = 0;
                    //int faceCount = 0;
                    //int userCount = 0;
                    //int palmCount = 0;
                    //zkRdr.GetDeviceStatus(currDevId, 2, ref userCount);
                    //zkRdr.GetDeviceStatus(currDevId, 3, ref fingerCount);
                    //zkRdr.GetDeviceStatus(currDevId, 21, ref faceCount);
                    getButtn.BackgroundImage = Properties.Resources.zkteco;
                    toolStripStatusLabel1.Text = "";
                    Cursor.Current = Cursors.Default;
                    return true;
                }
                else
                {
                    int errRef = 0;
                    zkRdr.GetLastError(errRef);
                    toolStripStatusLabel1.Text = "Unable to connect (" + placeId + ") ";
                    Cursor.Current = Cursors.Default;
                    getButtn.BackgroundImage = Properties.Resources.logo; ;
                    getButtn.Update();
                    MyGlobal.IsConnected = false;
                    return false;
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Unable to ping (" + placeId + ") ";
                getButtn.BackgroundImage = Properties.Resources.logo;
                getButtn.Update();
                MyGlobal.IsConnected = false;
                return false;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            string mySql;
            clearText();
            if (comboBox1.SelectedIndex > 0)
            {
                mySql = "Select * From Devices Where Place_id =" +
                    comboBox1.Text.ToString().Split('-')[0];
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
                        
                        Cursor.Current = Cursors.WaitCursor;
                        ConnectPunch(currIpAddr, currPort, currDevId, PlaceName);
                        Cursor.Current = Cursors.Default;
                        break;
                    case "ZKTECO":
                        Cursor.Current = Cursors.WaitCursor;
                        ZktConnect(currIpAddr, currPort, PlaceName, currPass);
                        Cursor.Current = Cursors.Default;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                myIP.Disconnect();
                myIP.ResetSocket();
                zkRdr.Disconnect();
                getButtn.BackgroundImage = Properties.Resources.logo;
                getButtn.Update();
            }
        }
    }
}
