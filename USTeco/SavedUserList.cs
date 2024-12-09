using RecogSys.RdrAccess;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using zkemkeeper;


namespace USTeco
{
    public partial class SavedUserList : Form
    {
        int numberOfItemsPrintedSoFar = 0;
        int numberOfItemsPerPage = 0;
        int currentPageNo = 1;
        int currentPageCount = 1;
        const int TRUE = 1;
        const int FALSE = 0;
        OleDbConnection accCon = new OleDbConnection();
        CRsiComWinsock myIP = new CRsiComWinsock(); // to hold the IP address of ZkTeco device.
        CRsiHandReader myRdr;// = new CRsiHandReader(); // to receive retrieved log of ZkTeco device.
        CRsiNetwork myNetwork;
        RSI_READER_INFO pReaderInfo = new RSI_READER_INFO();
        RSI_STATUS response = new RSI_STATUS();
        CZKEM zkRdr = new CZKEM();
        string PlaceName;
        //int PlaceNo;
        string mySql;
        string currIpAddr, currDevType, currPass;
        int currPort, currDevId;
        string myDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UST Applications", "Handpunch");
        public SavedUserList()
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
            int MyPlace;
            InitializeComponent();
            accCon.ConnectionString = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + myDataPath + "\\handdb.mdb; Jet OLEDB:Database Password = Ajoset25";
            accCon.Open();
            mySql = "Select Place_id,Place_Name From Devices";
            comboBox1.Items.Add("All Devices");
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
            }
            comboBox1.SelectedIndex = 0;
            reader.Close();
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(saveButton, "Update List of Users From Specific Device");
            ToolTip1.SetToolTip(showList, "Show List of User From Database");
            ToolTip1.SetToolTip(duplicateButt, "Show List of Douplicate Users From Database");
            ToolTip1.SetToolTip(trashList, "Delete List of Specific Device From Database");
            ToolTip1.SetToolTip(previewList, "Prieview List");
            ToolTip1.SetToolTip(printList, "Print List");
            ToolTip1.SetToolTip(excelList, "Send List To Excel Sheet");
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
                pictureBox1.Update();
                return false;
            }
            else
            {
                myIP.Disconnect();
                myIP.ResetSocket();
                myIP.Connect();
                if (FALSE == myIP.IsConnected())
                {
                    toolStripStatusLabel1.Text = "Unable to connect (" + placeId + ") ";
                    myIP.Disconnect();
                    myIP.ResetSocket();
                    pictureBox1.BackgroundImage = Properties.Resources.logo;
                    pictureBox1.Update();
                    return false;
                }
                else
                {
                    myNetwork = new CRsiNetwork(myIP);
                    myNetwork.Attach(myRdr);
                    RSI_TIME_DATE currentDateTime = new RSI_TIME_DATE();
                    if (myRdr.CmdGetTime(currentDateTime) == TRUE)  // Gets current time from reader.      
                    {
                        pictureBox1.BackgroundImage = Properties.Resources.handpunch;
                        pictureBox1.Update();
                        return true;
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Couldn't read from (" + placeId + ") ";
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
                zkRdr.Disconnect();
                MyGlobal.IsConnected = zkRdr.Connect_Net(ipAdd.ToString(), port);
                if (MyGlobal.IsConnected)
                {
                    int fingerCount = 0;
                    int faceCount = 0;
                    //int palmCount = 0;
                    zkRdr.GetDeviceStatus(currDevId, 3, ref fingerCount);
                    zkRdr.GetDeviceStatus(currDevId, 21, ref faceCount);

                    pictureBox1.BackgroundImage = Properties.Resources.zkteco;
                    pictureBox1.Update();
                    Cursor.Current = Cursors.Default;
                    return true;
                }
                else
                {
                    int errRef = 0;
                    zkRdr.GetLastError(errRef);
                    toolStripStatusLabel1.Text = "Unable to connect (" + placeId + ") ";
                    Cursor.Current = Cursors.Default;
                    pictureBox1.BackgroundImage = Properties.Resources.logo;
                    pictureBox1.Update();
                    return false;
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Unable to ping (" + placeId + ") ";
                pictureBox1.BackgroundImage = Properties.Resources.logo;
                pictureBox1.Update();
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
        private void saveButton_Click(object sender, EventArgs e)
        {
            int MyPlace;
            string MyMessage = "";
            OleDbCommand cmd;
            OleDbDataReader reader;
            string my_query;
            my_query = "Select Count(*) From Devices";
            cmd = new OleDbCommand(my_query, accCon);
            reader = cmd.ExecuteReader();
            reader.Read();
            int recordCount = reader.GetInt32(0);
            reader.Close();
            progressBar1.Maximum = recordCount;
            progressBar1.Value = 0;
            progressBar1.Update();
            progressBar2.Update();
            progressBar1.Visible = true;
            progressBar2.Visible = true;
            if (comboBox1.SelectedIndex == 0)
            {
                my_query = "Delete From Users";
                mySql = "Select * From Devices Order By ID";
            }
            else
            {
                string PlaceName = comboBox1.SelectedItem.ToString().Split(' ')[0];
                my_query = "Delete From Users where Place_id ='" + PlaceName + "'";
                mySql = "Select * From Devices Where Place_id =" + PlaceName;
            }
            cmd = new OleDbCommand(my_query, accCon);
            cmd.ExecuteNonQuery();
            cmd = new OleDbCommand(mySql, accCon);
            reader = cmd.ExecuteReader();
            Cursor.Current = Cursors.WaitCursor;
            while (reader.Read())
            {
                progressBar1.Value += 1;
                progressBar1.Update();
                currPass = reader.GetString(8).ToUpper();
                MyPlace = reader.GetInt32(6);
                PlaceName = reader.GetString(7);
                currIpAddr = reader.GetString(1);
                currPort = reader.GetInt32(2);
                currDevId = reader.GetInt32(3);
                currDevType = reader.GetString(4).ToUpper();
                currPass = reader.GetString(8).ToUpper();
                try
                {
                    PlaceName = reader.GetString(7);
                }
                catch
                {
                    PlaceName = "";
                }
                switch (currDevType)
                {
                    case "HANDPUNCH":
                        if (ConnectPunch(currIpAddr, currPort, currDevId, PlaceName))
                        {
                            int RecordSize;  // for HP4K or 16 for others
                            if (myRdr.CmdGetReaderInfo(pReaderInfo) == FALSE)
                            {
                                MyMessage += PlaceName + " Couldn't read.\n\n";
                                return;
                            }
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
                            int NumberOfBanks = 10; //for HP4K512  
                            int UsersPerBank = (4096 / RecordSize) - 1;
                            progressBar2.Maximum = Math.Abs(4096 / RecordSize) + 1;
                            Cursor.Current = Cursors.WaitCursor;
                            myRdr.CmdEnterIdleMode();
                            while (BankNo < NumberOfBanks)
                            {
                                int rslt = myRdr.CmdGetDataBank(BankNo, DataBank);
                                if (rslt == FALSE)
                                {
                                    return;
                                }
                                for (int RecNo = 0; RecNo <= UsersPerBank; RecNo++)
                                {
                                    if (progressBar2.Value < progressBar2.Maximum)
                                    {
                                        progressBar2.Value += 1;
                                        progressBar2.Update();
                                    }
                                    string name = "";
                                    string id = "";
                                    string auth = "";
                                    string zone = "";
                                    for (int i = 0; i < 5; i++)
                                    {
                                        id += DataBank.Get()[i + (RecordSize * RecNo)].ToString("X2");
                                    }
                                    if (id == "FFFFFFFFFF")
                                    {
                                        BankNo++;
                                        continue;
                                    }
                                    for (int i = 14; i < 15; i++)
                                    {
                                        auth += DataBank.Get()[i + (RecordSize * RecNo)];
                                    }
                                    for (int i = 15; i < 16; i++)
                                    {
                                        zone += DataBank.Get()[i + (RecordSize * RecNo)];
                                    }
                                    if (RecordSize == 77)
                                    {
                                        for (int i = 30; i < 46; i++)
                                        {
                                            name += (char)DataBank.Get()[i + (RecordSize * RecNo)];
                                        }
                                    }
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
                                    saveUserList(MyPlace.ToString(), id.TrimStart('0'), "", strAuth, zone);
                                }
                                BankNo++;
                            }
                            myRdr.CmdExitIdleMode();
                            progressBar2.Value = 0;
                            progressBar2.Update();
                        }
                        else
                        {
                            MyMessage += PlaceName + " not connected.\n\n";
                        }
                        break;
                    case "ZKTECO":
                        Cursor.Current = Cursors.WaitCursor;
                        if (ZktConnect(currIpAddr, currPort, PlaceName, currPass))
                        {
                            string iEnrollNumber = "";
                            string iName = "";
                            string iPassword = "";
                            int iPrivilege = 0;
                            bool iEnabled = false;
                            string status = "Enable";
                            int userCount = 0;
                            zkRdr.GetDeviceStatus(currDevId, 2, ref userCount);
                            progressBar2.Maximum = userCount;
                            progressBar2.Value = 0;
                            zkRdr.EnableDevice(currDevId, false);
                            zkRdr.ReadAllUserID(currDevId);
                            while (zkRdr.SSR_GetAllUserInfo(currDevId, out iEnrollNumber, out iName,
                                out iPassword, out iPrivilege, out iEnabled))
                            {
                                progressBar2.Value += 1;
                                progressBar2.Update();
                                string strAuth = "";
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
                                saveUserList(MyPlace.ToString(), iEnrollNumber.TrimStart('0'), iName, strAuth, status);
                            }
                            zkRdr.EnableDevice(currDevId, true);
                        }
                        else
                        {
                            MyMessage += PlaceName + " not connected.\n\n";
                        }
                        Cursor.Current = Cursors.Default;
                        break;
                }
            }
            progressBar1.Visible = false;
            progressBar2.Visible = false;
            Cursor.Current = Cursors.Default;
            showList_Click(this, null);
            if (string.IsNullOrEmpty(MyMessage))
            {
                MyMessage = "All Device read successfully";
                MessageBox.Show(MyMessage, "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(MyMessage, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void showList_Click(object sender, EventArgs e)
        {
            string cond = "";
            //dataGridView1.Rows.Clear();
            //dataGridView1.Update();
            //dataGridView1.BringToFront();
            if (comboBox1.SelectedIndex == 0)
            {
                cond = "";
            }
            else
            {
                cond = " And U.Place_id='" +
                    comboBox1.SelectedItem.ToString().Split(' ')[0] + "' ";
            }
            mySql = "SELECT E.EMP_ID,E.Emp_Name ,D.Place_Name, U.Emp_Auth, U.Emp_Status " +
                     "From Users U, Employee E, Devices D " +
                     "WHERE U.EMP_ID = E.EMP_ID  " +
                     "AND U.Place_id = Cstr(D.Place_id) " + cond +
                     "Order by D.Place_id,E.EMP_ID";
            using (OleDbDataAdapter dadapter = new OleDbDataAdapter(mySql, accCon))
            {
                DataTable table = new DataTable();
                dadapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            //cmd = new OleDbCommand(mySql, accCon);
            //reader = cmd.ExecuteReader();
            //while (reader.Read()) {
            //    string[] row1 = new string[] {reader.GetString(0),
            //        reader.GetString(1),reader.GetString(2),reader.GetString(3)
            //    };
            //    dataGridView1.Rows.Add(row1);
            //    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            //    dataGridView1.Update();
            //}
            //reader.Close();
            //cmd = new OleDbCommand(mySql, accCon);
            //reader = cmd.ExecuteReader();
            //reader.Read();
        }

        private void duplicateButt_Click(object sender, EventArgs e)
        {
            mySql = "SELECT E.EMP_ID,E.Emp_Name ,D.Place_Name ,U.Emp_Auth, U.Emp_Status " +
                 "From Users U, Employee E, Devices D " +
                 "WHERE U.EMP_ID = E.EMP_ID  " +
                 "AND U.Place_id = Cstr(D.Place_id) " +
                 "and exists(select 0 from Users as s " +
                 "where s.ID_KEy <> U.ID_KEY and s.EMP_ID = U.EMP_ID) order by 1";

            using (OleDbDataAdapter dadapter = new OleDbDataAdapter(mySql, accCon))
            {
                DataTable table = new DataTable();
                dadapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            //cmd = new OleDbCommand(mySql, accCon);
            //reader = cmd.ExecuteReader();
            //reader.Read();
        }

        private void trashList_Click(object sender, EventArgs e)
        {
            OleDbCommand cmd;
            //string msg;
            if (comboBox1.SelectedIndex == 0)
            {
                mySql = "Delete From Users";
            }
            else
            {
                mySql = "Delete From Users Where Place_Id ='" +
                    comboBox1.SelectedItem.ToString().Split(' ')[0] + "'";
            }
            if (MessageBox.Show("Are you sure you want to clear all specific user?", "Confirm",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cmd = new OleDbCommand(mySql, accCon);
                cmd.ExecuteNonQuery();
            }
        }
        void saveUserList(string placeId, string id, string name, string auth, string status)
        {
            string my_query = "";
            OleDbCommand cmd;
            my_query = "Insert Into Users (PLACE_ID,EMP_ID,EMP_NAME,EMP_AUTH," +
                "EMP_STATUS) values('" + placeId + "','" + id + "','" + name + "','" + auth + "','" + status + "')";
            cmd = new OleDbCommand(my_query, accCon);
            cmd.ExecuteNonQuery();
        }
        private void previewList_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void excelList_Click(object sender, EventArgs e)
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
                            worksheet.Cells[3, j + 1].Value = dataGridView1.Columns[j].HeaderText;
                        }
                        for (int i = 2; i < dataGridView1.Rows.Count + 2; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                worksheet.Cells[i + 2, j + 1].Value = dataGridView1.Rows[i - 2].Cells[j].Value?.ToString() ?? "";
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

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Selected = false;
            }
            if (textBox11.Text.Trim() != "")
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.Equals(textBox11.Text))
                    {
                        dataGridView1.Rows[i].Cells[0].Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        break;
                    }
                }
            }
            else
            {
                if (textBox13.Text.Trim() != "")
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[1].Value.ToString().Contains(textBox13.Text))
                        {
                            dataGridView1.Rows[i].Cells[1].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void printList_Click(object sender, EventArgs e)
        {
            PrintDialog PrintDialog1 = new PrintDialog();
            PrintDialog1.AllowSomePages = true;
            PrintDialog1.ShowHelp = true;
            PrintDialog1.Document = printDocument1;
            DialogResult result = PrintDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string g;
            string curdhead = "Finger Print User List";
            e.Graphics.DrawString(curdhead, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 350, 50);
            string l1 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l1, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, 100);
            g = "Emp No.";
            e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 80, 130);
            g = "Emp Name";
            e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 150, 130);
            g = "Place Name";
            e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 350, 130);
            g = "Authority";
            e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 500, 130);
            g = "Status";
            e.Graphics.DrawString(g, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 650, 130);
            string l2 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, 150);
            int height = 165;
            double num = dataGridView1.Rows.Count / 39.0;
            currentPageCount = Convert.ToInt32(Math.Ceiling(num));
            for (int l = numberOfItemsPrintedSoFar; l < dataGridView1.Rows.Count; l++)
            {
                numberOfItemsPerPage++;
                if (numberOfItemsPerPage <= 39)
                {
                    numberOfItemsPrintedSoFar++;
                    if (numberOfItemsPrintedSoFar <= dataGridView1.Rows.Count)
                    {
                        height += dataGridView1.Rows[0].Height;
                        e.Graphics.DrawString(dataGridView1.Rows[l].Cells[0].FormattedValue.ToString(),
                            dataGridView1.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(80, height,
                            dataGridView1.Columns[0].Width, dataGridView1.Rows[0].Height));
                        e.Graphics.DrawString(dataGridView1.Rows[l].Cells[1].FormattedValue.ToString(),
                            dataGridView1.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(150, height,
                            500, dataGridView1.Rows[0].Height));
                        e.Graphics.DrawString(dataGridView1.Rows[l].Cells[2].FormattedValue.ToString(),
                            dataGridView1.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(350, height,
                            500, dataGridView1.Rows[0].Height));
                        e.Graphics.DrawString(dataGridView1.Rows[l].Cells[3].FormattedValue.ToString(),
                            dataGridView1.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(500, height,
                            500, dataGridView1.Rows[0].Height));
                        e.Graphics.DrawString(dataGridView1.Rows[l].Cells[4].FormattedValue.ToString(),
                            dataGridView1.Font = new Font("Book Antiqua", 8), Brushes.Black, new RectangleF(650, height,
                            500, dataGridView1.Rows[0].Height));
                    }
                    else
                    {
                        e.HasMorePages = false;
                    }
                }
                else
                {
                    e.Graphics.DrawString(currentPageNo.ToString() + " of " + currentPageCount.ToString(),
                    dataGridView1.Font = new Font("Book Antiqua", 8),
                    Brushes.Black, new RectangleF(430, 1050,
                    500, dataGridView1.Rows[0].Height));
                    currentPageNo++;
                    numberOfItemsPerPage = 0;
                    e.HasMorePages = true;
                    return;

                }


            }
            e.Graphics.DrawString(currentPageNo.ToString() + " of " + currentPageCount.ToString(),
            dataGridView1.Font = new Font("Book Antiqua", 8),
            Brushes.Black, new RectangleF(430, 1050,
            500, dataGridView1.Rows[0].Height));
            numberOfItemsPerPage = 0;
            numberOfItemsPrintedSoFar = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataReader reader;
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
                pictureBox1.BackgroundImage = Properties.Resources.logo;
                pictureBox1.Update();
            }
        }
    }
}
