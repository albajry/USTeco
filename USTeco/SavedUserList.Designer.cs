
namespace USTeco
{
    partial class SavedUserList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SavedUserList));
            this.panel1 = new System.Windows.Forms.Panel();
            this.excelList = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.showList = new System.Windows.Forms.Button();
            this.duplicateButt = new System.Windows.Forms.Button();
            this.trashList = new System.Windows.Forms.Button();
            this.printList = new System.Windows.Forms.Button();
            this.previewList = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.EMP_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Emp_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Place_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Emp_Auth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMP_STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox13);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.textBox11);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.excelList);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.saveButton);
            this.panel1.Controls.Add(this.progressBar2);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.showList);
            this.panel1.Controls.Add(this.duplicateButt);
            this.panel1.Controls.Add(this.trashList);
            this.panel1.Controls.Add(this.printList);
            this.panel1.Controls.Add(this.previewList);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(651, 100);
            this.panel1.TabIndex = 82;
            // 
            // excelList
            // 
            this.excelList.BackgroundImage = global::USTeco.Properties.Resources.excel;
            this.excelList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.excelList.Location = new System.Drawing.Point(261, 63);
            this.excelList.Name = "excelList";
            this.excelList.Size = new System.Drawing.Size(35, 34);
            this.excelList.TabIndex = 93;
            this.excelList.UseVisualStyleBackColor = true;
            this.excelList.Click += new System.EventHandler(this.excelList_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::USTeco.Properties.Resources.logo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(581, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(59, 59);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 92;
            this.pictureBox1.TabStop = false;
            // 
            // saveButton
            // 
            this.saveButton.BackgroundImage = global::USTeco.Properties.Resources.restore;
            this.saveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.saveButton.Location = new System.Drawing.Point(15, 63);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(35, 34);
            this.saveButton.TabIndex = 84;
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // progressBar2
            // 
            this.progressBar2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.progressBar2.Location = new System.Drawing.Point(345, 36);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(230, 10);
            this.progressBar2.TabIndex = 86;
            this.progressBar2.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.progressBar1.Location = new System.Drawing.Point(345, 21);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(230, 10);
            this.progressBar1.TabIndex = 85;
            this.progressBar1.Visible = false;
            // 
            // showList
            // 
            this.showList.BackgroundImage = global::USTeco.Properties.Resources.list;
            this.showList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.showList.Location = new System.Drawing.Point(56, 63);
            this.showList.Name = "showList";
            this.showList.Size = new System.Drawing.Size(35, 34);
            this.showList.TabIndex = 91;
            this.showList.UseVisualStyleBackColor = true;
            this.showList.Click += new System.EventHandler(this.showList_Click);
            // 
            // duplicateButt
            // 
            this.duplicateButt.BackgroundImage = global::USTeco.Properties.Resources.newjob;
            this.duplicateButt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.duplicateButt.Location = new System.Drawing.Point(97, 63);
            this.duplicateButt.Name = "duplicateButt";
            this.duplicateButt.Size = new System.Drawing.Size(35, 34);
            this.duplicateButt.TabIndex = 87;
            this.duplicateButt.UseVisualStyleBackColor = true;
            this.duplicateButt.Click += new System.EventHandler(this.duplicateButt_Click);
            // 
            // trashList
            // 
            this.trashList.BackgroundImage = global::USTeco.Properties.Resources.delete;
            this.trashList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.trashList.Location = new System.Drawing.Point(138, 63);
            this.trashList.Name = "trashList";
            this.trashList.Size = new System.Drawing.Size(35, 34);
            this.trashList.TabIndex = 90;
            this.trashList.UseVisualStyleBackColor = true;
            this.trashList.Click += new System.EventHandler(this.trashList_Click);
            // 
            // printList
            // 
            this.printList.BackgroundImage = global::USTeco.Properties.Resources.print;
            this.printList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.printList.Location = new System.Drawing.Point(220, 63);
            this.printList.Name = "printList";
            this.printList.Size = new System.Drawing.Size(35, 34);
            this.printList.TabIndex = 88;
            this.printList.UseVisualStyleBackColor = true;
            this.printList.Click += new System.EventHandler(this.printList_Click);
            // 
            // previewList
            // 
            this.previewList.BackgroundImage = global::USTeco.Properties.Resources.preview_on;
            this.previewList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.previewList.Location = new System.Drawing.Point(179, 63);
            this.previewList.Name = "previewList";
            this.previewList.Size = new System.Drawing.Size(35, 34);
            this.previewList.TabIndex = 89;
            this.previewList.UseVisualStyleBackColor = true;
            this.previewList.Click += new System.EventHandler(this.previewList_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 26);
            this.label1.TabIndex = 75;
            this.label1.Text = "Select Device";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Lucida Sans Typewriter", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(105, 21);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(222, 25);
            this.comboBox1.TabIndex = 74;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 368);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.statusStrip1.Size = new System.Drawing.Size(651, 22);
            this.statusStrip1.TabIndex = 84;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Ivory;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EMP_ID,
            this.Emp_Name,
            this.Place_Name,
            this.Emp_Auth,
            this.EMP_STATUS});
            this.dataGridView1.Location = new System.Drawing.Point(10, 103);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(632, 264);
            this.dataGridView1.TabIndex = 83;
            // 
            // EMP_ID
            // 
            this.EMP_ID.DataPropertyName = "EMP_ID";
            this.EMP_ID.HeaderText = "No";
            this.EMP_ID.Name = "EMP_ID";
            this.EMP_ID.ReadOnly = true;
            this.EMP_ID.Width = 60;
            // 
            // Emp_Name
            // 
            this.Emp_Name.DataPropertyName = "EMP_NAME";
            this.Emp_Name.HeaderText = "Emp Name";
            this.Emp_Name.Name = "Emp_Name";
            this.Emp_Name.ReadOnly = true;
            this.Emp_Name.Width = 200;
            // 
            // Place_Name
            // 
            this.Place_Name.DataPropertyName = "PLACE_NAME";
            this.Place_Name.HeaderText = "Place Name";
            this.Place_Name.Name = "Place_Name";
            this.Place_Name.ReadOnly = true;
            this.Place_Name.Width = 150;
            // 
            // Emp_Auth
            // 
            this.Emp_Auth.DataPropertyName = "Emp_Auth";
            this.Emp_Auth.HeaderText = "Authority";
            this.Emp_Auth.Name = "Emp_Auth";
            this.Emp_Auth.ReadOnly = true;
            // 
            // EMP_STATUS
            // 
            this.EMP_STATUS.DataPropertyName = "EMP_STATUS";
            this.EMP_STATUS.HeaderText = "Status";
            this.EMP_STATUS.Name = "EMP_STATUS";
            this.EMP_STATUS.ReadOnly = true;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "UserList";
            this.saveFileDialog1.Filter = "Excel file|*.xlsx";
            // 
            // button2
            // 
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Location = new System.Drawing.Point(604, 66);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(35, 34);
            this.button2.TabIndex = 102;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox11
            // 
            this.textBox11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox11.Location = new System.Drawing.Point(371, 69);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(42, 23);
            this.textBox11.TabIndex = 103;
            // 
            // textBox13
            // 
            this.textBox13.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox13.Location = new System.Drawing.Point(467, 70);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(131, 23);
            this.textBox13.TabIndex = 105;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(421, 68);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(42, 26);
            this.label24.TabIndex = 104;
            this.label24.Text = "Name";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(314, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 26);
            this.label2.TabIndex = 106;
            this.label2.Text = "Emp ID";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SavedUserList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 390);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SavedUserList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SavedUserList";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button showList;
        private System.Windows.Forms.Button duplicateButt;
        private System.Windows.Forms.Button trashList;
        private System.Windows.Forms.Button printList;
        private System.Windows.Forms.Button previewList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMP_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Emp_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Place_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Emp_Auth;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMP_STATUS;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.Button excelList;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.Label label24;
    }
}