
namespace USTeco
{
    partial class AttendReport
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttendReport));
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridView4 = new System.Windows.Forms.DataGridView();
            this.dayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkout = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label39 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.repDateTo = new System.Windows.Forms.DateTimePicker();
            this.repDateFrom = new System.Windows.Forms.DateTimePicker();
            this.repEmpNo = new System.Windows.Forms.TextBox();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.repButtLeave = new System.Windows.Forms.Button();
            this.repButtPrn = new System.Windows.Forms.Button();
            this.repButtShow = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridView4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 372);
            this.panel1.TabIndex = 0;
            // 
            // gridView4
            // 
            this.gridView4.AllowUserToAddRows = false;
            this.gridView4.AllowUserToDeleteRows = false;
            this.gridView4.AllowUserToOrderColumns = true;
            this.gridView4.AllowUserToResizeColumns = false;
            this.gridView4.AllowUserToResizeRows = false;
            this.gridView4.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridView4.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridView4.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dayName,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.checkout,
            this.delay});
            this.gridView4.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridView4.EnableHeadersVisualStyles = false;
            this.gridView4.Location = new System.Drawing.Point(12, 12);
            this.gridView4.MultiSelect = false;
            this.gridView4.Name = "gridView4";
            this.gridView4.ReadOnly = true;
            this.gridView4.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.gridView4.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.gridView4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridView4.Size = new System.Drawing.Size(509, 357);
            this.gridView4.TabIndex = 3;
            // 
            // dayName
            // 
            this.dayName.DataPropertyName = "dayName";
            this.dayName.HeaderText = "Day Name";
            this.dayName.Name = "dayName";
            this.dayName.ReadOnly = true;
            this.dayName.Width = 120;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "SignDate";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn7.HeaderText = "Date";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Check_In";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn8.HeaderText = "Check In";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // checkout
            // 
            this.checkout.DataPropertyName = "Check_Out";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkout.DefaultCellStyle = dataGridViewCellStyle4;
            this.checkout.HeaderText = "Check Out";
            this.checkout.Name = "checkout";
            this.checkout.ReadOnly = true;
            // 
            // delay
            // 
            this.delay.DataPropertyName = "dayNo";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.delay.DefaultCellStyle = dataGridViewCellStyle5;
            this.delay.HeaderText = "Delay";
            this.delay.Name = "delay";
            this.delay.ReadOnly = true;
            this.delay.Width = 60;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.repButtLeave);
            this.panel2.Controls.Add(this.label39);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.label33);
            this.panel2.Controls.Add(this.repButtPrn);
            this.panel2.Controls.Add(this.label34);
            this.panel2.Controls.Add(this.label35);
            this.panel2.Controls.Add(this.repDateTo);
            this.panel2.Controls.Add(this.repDateFrom);
            this.panel2.Controls.Add(this.repEmpNo);
            this.panel2.Controls.Add(this.repButtShow);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 378);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(534, 77);
            this.panel2.TabIndex = 60;
            // 
            // label39
            // 
            this.label39.BackColor = System.Drawing.SystemColors.Info;
            this.label39.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label39.ForeColor = System.Drawing.Color.Red;
            this.label39.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label39.Location = new System.Drawing.Point(449, 6);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(71, 23);
            this.label39.TabIndex = 93;
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(9, 38);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(41, 26);
            this.label18.TabIndex = 92;
            this.label18.Text = "Num";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.SystemColors.Info;
            this.label33.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label33.ForeColor = System.Drawing.Color.Blue;
            this.label33.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label33.Location = new System.Drawing.Point(11, 6);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(440, 24);
            this.label33.TabIndex = 91;
            this.label33.Text = "Read finger print from database as check in and check out";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label34.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label34.Location = new System.Drawing.Point(264, 38);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(27, 26);
            this.label34.TabIndex = 52;
            this.label34.Text = "To";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label35.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label35.Location = new System.Drawing.Point(109, 38);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(41, 26);
            this.label35.TabIndex = 49;
            this.label35.Text = "From";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // repDateTo
            // 
            this.repDateTo.CustomFormat = "";
            this.repDateTo.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.repDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.repDateTo.Location = new System.Drawing.Point(292, 41);
            this.repDateTo.Name = "repDateTo";
            this.repDateTo.Size = new System.Drawing.Size(100, 23);
            this.repDateTo.TabIndex = 53;
            // 
            // repDateFrom
            // 
            this.repDateFrom.CustomFormat = "";
            this.repDateFrom.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.repDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.repDateFrom.Location = new System.Drawing.Point(150, 41);
            this.repDateFrom.Name = "repDateFrom";
            this.repDateFrom.Size = new System.Drawing.Size(100, 23);
            this.repDateFrom.TabIndex = 51;
            // 
            // repEmpNo
            // 
            this.repEmpNo.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.repEmpNo.Location = new System.Drawing.Point(53, 41);
            this.repEmpNo.Name = "repEmpNo";
            this.repEmpNo.Size = new System.Drawing.Size(47, 23);
            this.repEmpNo.TabIndex = 47;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
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
            // repButtLeave
            // 
            this.repButtLeave.BackgroundImage = global::USTeco.Properties.Resources.chrome;
            this.repButtLeave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.repButtLeave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.repButtLeave.Location = new System.Drawing.Point(450, 38);
            this.repButtLeave.Name = "repButtLeave";
            this.repButtLeave.Size = new System.Drawing.Size(32, 32);
            this.repButtLeave.TabIndex = 94;
            this.repButtLeave.UseVisualStyleBackColor = true;
            this.repButtLeave.Click += new System.EventHandler(this.repButtLeave_Click);
            this.repButtLeave.MouseHover += new System.EventHandler(this.addExitForm_MouseHover);
            // 
            // repButtPrn
            // 
            this.repButtPrn.BackgroundImage = global::USTeco.Properties.Resources.print;
            this.repButtPrn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.repButtPrn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.repButtPrn.Location = new System.Drawing.Point(488, 36);
            this.repButtPrn.Name = "repButtPrn";
            this.repButtPrn.Size = new System.Drawing.Size(32, 32);
            this.repButtPrn.TabIndex = 89;
            this.repButtPrn.UseVisualStyleBackColor = true;
            this.repButtPrn.Click += new System.EventHandler(this.repButtPrn_Click);
            this.repButtPrn.MouseHover += new System.EventHandler(this.repButtPrn_MouseHover);
            // 
            // repButtShow
            // 
            this.repButtShow.BackgroundImage = global::USTeco.Properties.Resources.preview_on;
            this.repButtShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.repButtShow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.repButtShow.Location = new System.Drawing.Point(412, 38);
            this.repButtShow.Name = "repButtShow";
            this.repButtShow.Size = new System.Drawing.Size(32, 32);
            this.repButtShow.TabIndex = 45;
            this.repButtShow.UseVisualStyleBackColor = true;
            this.repButtShow.Click += new System.EventHandler(this.repButtShow_Click);
            this.repButtShow.MouseHover += new System.EventHandler(this.repButtShow_MouseHover);
            // 
            // AttendReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 455);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AttendReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Attendance  Report";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gridView4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button repButtPrn;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.DateTimePicker repDateTo;
        private System.Windows.Forms.DateTimePicker repDateFrom;
        private System.Windows.Forms.TextBox repEmpNo;
        private System.Windows.Forms.Button repButtShow;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.Button repButtLeave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn checkout;
        private System.Windows.Forms.DataGridViewTextBoxColumn delay;
    }
}