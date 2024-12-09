
namespace USTeco
{
    partial class LogReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogReport));
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridView1 = new System.Windows.Forms.DataGridView();
            this.emp_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emp_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlaceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.atttime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.printList = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.attDateTo = new System.Windows.Forms.DateTimePicker();
            this.attDateFrom = new System.Windows.Forms.DateTimePicker();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.logRepButt = new System.Windows.Forms.Button();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.extCond = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridView1);
            this.panel1.Controls.Add(this.gridView2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(712, 389);
            this.panel1.TabIndex = 0;
            // 
            // gridView1
            // 
            this.gridView1.AllowUserToAddRows = false;
            this.gridView1.AllowUserToDeleteRows = false;
            this.gridView1.AllowUserToOrderColumns = true;
            this.gridView1.AllowUserToResizeColumns = false;
            this.gridView1.AllowUserToResizeRows = false;
            this.gridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Tahoma", 8F);
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.gridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.emp_no,
            this.emp_name});
            this.gridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridView1.Location = new System.Drawing.Point(177, 189);
            this.gridView1.MultiSelect = false;
            this.gridView1.Name = "gridView1";
            this.gridView1.ReadOnly = true;
            this.gridView1.RowHeadersVisible = false;
            this.gridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridView1.Size = new System.Drawing.Size(347, 167);
            this.gridView1.TabIndex = 96;
            this.gridView1.Visible = false;
            this.gridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridView1_CellDoubleClick);
            this.gridView1.Enter += new System.EventHandler(this.gridView1_Enter);
            this.gridView1.Leave += new System.EventHandler(this.gridView1_Leave);
            // 
            // emp_no
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.emp_no.DefaultCellStyle = dataGridViewCellStyle20;
            this.emp_no.HeaderText = "Emp Id";
            this.emp_no.Name = "emp_no";
            this.emp_no.ReadOnly = true;
            this.emp_no.Width = 70;
            // 
            // emp_name
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.emp_name.DefaultCellStyle = dataGridViewCellStyle21;
            this.emp_name.HeaderText = "Employee Name";
            this.emp_name.Name = "emp_name";
            this.emp_name.ReadOnly = true;
            this.emp_name.Width = 250;
            // 
            // gridView2
            // 
            this.gridView2.AllowUserToAddRows = false;
            this.gridView2.AllowUserToDeleteRows = false;
            this.gridView2.AllowUserToOrderColumns = true;
            this.gridView2.AllowUserToResizeColumns = false;
            this.gridView2.AllowUserToResizeRows = false;
            this.gridView2.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.gridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.PlaceName,
            this.PlcName,
            this.dataGridViewTextBoxColumn2,
            this.atttime,
            this.timeStamp});
            this.gridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridView2.Location = new System.Drawing.Point(9, 13);
            this.gridView2.MultiSelect = false;
            this.gridView2.Name = "gridView2";
            this.gridView2.ReadOnly = true;
            this.gridView2.RowHeadersVisible = false;
            dataGridViewCellStyle27.BackColor = System.Drawing.Color.Honeydew;
            this.gridView2.RowsDefaultCellStyle = dataGridViewCellStyle27;
            this.gridView2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridView2.Size = new System.Drawing.Size(694, 373);
            this.gridView2.TabIndex = 3;
            this.gridView2.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridView2_CellEnter);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Emp_id";
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle23;
            this.dataGridViewTextBoxColumn1.HeaderText = "E_ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // PlaceName
            // 
            this.PlaceName.HeaderText = "Employee Name";
            this.PlaceName.Name = "PlaceName";
            this.PlaceName.ReadOnly = true;
            this.PlaceName.Width = 180;
            // 
            // PlcName
            // 
            this.PlcName.HeaderText = "Place Name";
            this.PlcName.Name = "PlcName";
            this.PlcName.ReadOnly = true;
            this.PlcName.Width = 160;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Sign_Date";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle24;
            this.dataGridViewTextBoxColumn2.HeaderText = "Date";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // atttime
            // 
            this.atttime.DataPropertyName = "Sign_Time";
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.atttime.DefaultCellStyle = dataGridViewCellStyle25;
            this.atttime.HeaderText = "Time";
            this.atttime.Name = "atttime";
            this.atttime.ReadOnly = true;
            this.atttime.Width = 80;
            // 
            // timeStamp
            // 
            this.timeStamp.DataPropertyName = "Time_Stamp";
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.timeStamp.DefaultCellStyle = dataGridViewCellStyle26;
            this.timeStamp.HeaderText = "Reading Time";
            this.timeStamp.Name = "timeStamp";
            this.timeStamp.ReadOnly = true;
            this.timeStamp.Width = 120;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.comboBox3);
            this.panel2.Controls.Add(this.comboBox2);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.extCond);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.searchBtn);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.printList);
            this.panel2.Controls.Add(this.label20);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.attDateTo);
            this.panel2.Controls.Add(this.attDateFrom);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.logRepButt);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 395);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(712, 73);
            this.panel2.TabIndex = 60;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.textBox1.Location = new System.Drawing.Point(285, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(136, 23);
            this.textBox1.TabIndex = 95;
            // 
            // searchBtn
            // 
            this.searchBtn.BackgroundImage = global::USTeco.Properties.Resources.search;
            this.searchBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.searchBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.searchBtn.Location = new System.Drawing.Point(424, 4);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(28, 28);
            this.searchBtn.TabIndex = 94;
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(454, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 22);
            this.label1.TabIndex = 93;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label13.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(502, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(199, 22);
            this.label13.TabIndex = 91;
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(8, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(215, 24);
            this.comboBox1.TabIndex = 90;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // printList
            // 
            this.printList.BackgroundImage = global::USTeco.Properties.Resources.excel;
            this.printList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.printList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.printList.Location = new System.Drawing.Point(671, 33);
            this.printList.Name = "printList";
            this.printList.Size = new System.Drawing.Size(32, 32);
            this.printList.TabIndex = 89;
            this.printList.UseVisualStyleBackColor = true;
            this.printList.Click += new System.EventHandler(this.printList_Click);
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label20.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label20.Location = new System.Drawing.Point(204, 35);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(61, 26);
            this.label20.TabIndex = 52;
            this.label20.Text = "To Date";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Location = new System.Drawing.Point(10, 35);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(76, 26);
            this.label19.TabIndex = 49;
            this.label19.Text = "From Date";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // attDateTo
            // 
            this.attDateTo.CustomFormat = "";
            this.attDateTo.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.attDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.attDateTo.Location = new System.Drawing.Point(269, 38);
            this.attDateTo.Name = "attDateTo";
            this.attDateTo.Size = new System.Drawing.Size(101, 23);
            this.attDateTo.TabIndex = 53;
            // 
            // attDateFrom
            // 
            this.attDateFrom.CustomFormat = "";
            this.attDateFrom.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.attDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.attDateFrom.Location = new System.Drawing.Point(90, 38);
            this.attDateFrom.Name = "attDateFrom";
            this.attDateFrom.Size = new System.Drawing.Size(105, 23);
            this.attDateFrom.TabIndex = 51;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.textBox2.Location = new System.Drawing.Point(229, 6);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(50, 23);
            this.textBox2.TabIndex = 47;
            this.textBox2.Leave += new System.EventHandler(this.textBox12_Leave);
            // 
            // logRepButt
            // 
            this.logRepButt.BackgroundImage = global::USTeco.Properties.Resources.preview_on;
            this.logRepButt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.logRepButt.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.logRepButt.Location = new System.Drawing.Point(628, 33);
            this.logRepButt.Name = "logRepButt";
            this.logRepButt.Size = new System.Drawing.Size(32, 32);
            this.logRepButt.TabIndex = 45;
            this.logRepButt.UseVisualStyleBackColor = true;
            this.logRepButt.Click += new System.EventHandler(this.logRepButt_Click);
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
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "LogReport";
            this.saveFileDialog1.Filter = "Excel file|*.xlsx";
            // 
            // extCond
            // 
            this.extCond.BackColor = System.Drawing.SystemColors.Window;
            this.extCond.Enabled = false;
            this.extCond.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.extCond.Location = new System.Drawing.Point(563, 38);
            this.extCond.Name = "extCond";
            this.extCond.Size = new System.Drawing.Size(57, 23);
            this.extCond.TabIndex = 96;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(380, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 22);
            this.label2.TabIndex = 97;
            this.label2.Text = "And";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox2
            // 
            this.comboBox2.Enabled = false;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Emp_Id",
            "Sign_Plc_Id"});
            this.comboBox2.Location = new System.Drawing.Point(421, 38);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(88, 21);
            this.comboBox2.TabIndex = 98;
            // 
            // comboBox3
            // 
            this.comboBox3.Enabled = false;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "  =",
            " !=",
            "  <",
            "  >",
            "<=",
            ">="});
            this.comboBox3.Location = new System.Drawing.Point(513, 38);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(46, 21);
            this.comboBox3.TabIndex = 99;
            // 
            // LogReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 468);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log Report";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gridView2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button printList;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.DateTimePicker attDateTo;
        private System.Windows.Forms.DateTimePicker attDateFrom;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button logRepButt;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlaceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlcName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn atttime;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeStamp;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.DataGridView gridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn emp_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn emp_name;
        private System.Windows.Forms.TextBox extCond;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}