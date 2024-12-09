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
    public partial class BackupList : Form
    {
        public BackupList(string[] backList)
        {
            InitializeComponent();
            int index = 20;
            Height += 20 * backList.Length + 10;
            int groupZeroHeight = (backList.Length > 0) ? 40 : 20;
            groupBox1.Height = 20 * backList.Length + groupZeroHeight + 10;
            RadioButton[] nickName = new RadioButton[backList.Length];
            for (int i = 0; i < backList.Length; i++)
            {
                nickName[i] = new RadioButton();
                nickName[i].Left = 30;
                nickName[i].Top = 20 + 25 * i;
                nickName[i].Text = backList[i];
                nickName[i].Width = 290;
                nickName[i].Click+= new EventHandler(nickName_click);
                groupBox1.Controls.Add(nickName[i]);
                index += 20;
            }
            label2.Top = 84 + index;
            textBox1.Top = 81 + index;
            cancelButt.Top = 142 + index;
            deleteButt.Top = 142 + index;
            restoreButt.Top = 142 + index;
        }
        private void nickName_click(object sender, EventArgs e)
        {
            restoreButt.Enabled = true;
            deleteButt.Enabled = true;
        }

        private void restoreButt_Click(object sender, EventArgs e)
        {
        }
    }
}
