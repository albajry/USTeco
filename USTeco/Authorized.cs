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
    public partial class Authorized : Form
    {
        public Authorized()
        {
            InitializeComponent();
            MyGlobal.IsGuest = 0;
            comboBox1.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.ToUpper() == "SERVICE" && (
                    (passText.Text == "" + (char)65 + (char)106 + (char)111 + (char)115 +
                    (char)101 + (char)116 + (char)35 + (char)50 + (char)53 +
                    (char)54 + (char)55)))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.No;
            }
            this.Close();
        }

        private void passText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(this, null);
            }
        }
    }
}
