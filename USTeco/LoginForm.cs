using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USTeco
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            MyGlobal.IsGuest = 0;
            comboBox1.SelectedIndex = 0;

        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (guestBox.Checked)
            {
                MyGlobal.IsGuest = 0;
                this.Close();
                Setup f2 = new Setup();
                f2.ShowDialog();
            }
            else
            {
                if (comboBox1.Text.ToUpper() == "ADMIN" && (
                    passText.Text == "" + (char)70 + (char)105 + (char)110 + (char)103 + (char)101 +
                    (char)114 + (char)64 + (char)85 + (char)115 + (char)116 ||
                    passText.Text == "" + (char)65 + (char)106 + (char)111 + (char)115 +
                    (char)101 + (char)116 + (char)50 + (char)53))
                {
                    MyGlobal.IsGuest = 1;
                    this.Close();
                    Setup f2 = new Setup();
                    f2.ShowDialog();
                }
                else if (comboBox1.Text.ToUpper() == "SERVICE" &&
                   (passText.Text == "" + (char)65 + (char)106 + (char)111 + (char)115 +
                   (char)101 + (char)116 + (char)35 + (char)50 + (char)53 +
                   (char)54 + (char)55))
                {
                    MyGlobal.IsGuest = 2;
                    this.Close();
                    Setup f2 = new Setup();
                    f2.ShowDialog();
                }
                else if (comboBox1.Text.ToUpper() == "SERVICE" &&
                    (passText.Text == "" + (char)65 + (char)106 + (char)111 + (char)115 +
                    (char)101 + (char)116 + (char)35 + (char)50 + (char)52 +
                    (char)48 + (char)48 + (char)54 + (char)50))
                {
                    MyGlobal.IsGuest = 3;
                    this.Close();
                    Setup f2 = new Setup();
                    f2.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalied Username or Password", "Login");
                }

            }

        }

        private void guestBox_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = !guestBox.Checked;
            passText.Enabled = !guestBox.Checked;
        }

        private void userText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)13)
                passText.Focus();
        }

        private void passText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)13)
                btnLogin_Click(this, null);
        }
    }
}

