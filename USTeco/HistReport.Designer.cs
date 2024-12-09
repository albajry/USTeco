
namespace USTeco
{
    partial class HistReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridView3 = new System.Windows.Forms.DataGridView();
            this.label38 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ComboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.logDateTo = new System.Windows.Forms.DateTimePicker();
            this.label21 = new System.Windows.Forms.Label();
            this.logRepButt = new System.Windows.Forms.Button();
            this.logDateFrom = new System.Windows.Forms.DateTimePicker();
            this.label22 = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.radioButt2 = new System.Windows.Forms.RadioButton();
            this.radioButt1 = new System.Windows.Forms.RadioButton();
            this.logSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.devname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fingcount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridView3);
            this.panel1.Controls.Add(this.label38);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 375);
            this.panel1.TabIndex = 0;
            // 
            // gridView3
            // 
            this.gridView3.AllowUserToAddRows = false;
            this.gridView3.AllowUserToResizeColumns = false;
            this.gridView3.AllowUserToResizeRows = false;
            this.gridView3.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridView3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.gridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.logSeq,
            this.dataGridViewTextBoxColumn3,
            this.devname,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewComboBoxColumn2,
            this.fingcount,
            this.TimeStamp});
            this.gridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridView3.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridView3.EnableHeadersVisualStyles = false;
            this.gridView3.Location = new System.Drawing.Point(0, 25);
            this.gridView3.MultiSelect = false;
            this.gridView3.Name = "gridView3";
            this.gridView3.RowHeadersVisible = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.Honeydew;
            this.gridView3.RowsDefaultCellStyle = dataGridViewCellStyle16;
            this.gridView3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridView3.Size = new System.Drawing.Size(704, 350);
            this.gridView3.TabIndex = 47;
            // 
            // label38
            // 
            this.label38.BackColor = System.Drawing.SystemColors.Info;
            this.label38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label38.Dock = System.Windows.Forms.DockStyle.Top;
            this.label38.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label38.ForeColor = System.Drawing.Color.Blue;
            this.label38.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label38.Location = new System.Drawing.Point(0, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(704, 25);
            this.label38.TabIndex = 92;
            this.label38.Text = "Show the history of the reading count from ";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ComboBox1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.logDateTo);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.logRepButt);
            this.panel2.Controls.Add(this.logDateFrom);
            this.panel2.Controls.Add(this.label22);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 404);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(704, 46);
            this.panel2.TabIndex = 61;
            // 
            // ComboBox1
            // 
            this.ComboBox1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox1.FormattingEnabled = true;
            this.ComboBox1.Location = new System.Drawing.Point(12, 9);
            this.ComboBox1.Name = "ComboBox1";
            this.ComboBox1.Size = new System.Drawing.Size(182, 24);
            this.ComboBox1.TabIndex = 93;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::USTeco.Properties.Resources.print;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(661, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 32);
            this.button1.TabIndex = 89;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // logDateTo
            // 
            this.logDateTo.CustomFormat = "";
            this.logDateTo.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.logDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.logDateTo.Location = new System.Drawing.Point(377, 12);
            this.logDateTo.Name = "logDateTo";
            this.logDateTo.Size = new System.Drawing.Size(100, 23);
            this.logDateTo.TabIndex = 58;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label21.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label21.Location = new System.Drawing.Point(350, 9);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(29, 26);
            this.label21.TabIndex = 57;
            this.label21.Text = "To";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logRepButt
            // 
            this.logRepButt.BackgroundImage = global::USTeco.Properties.Resources.preview_on;
            this.logRepButt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.logRepButt.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.logRepButt.Location = new System.Drawing.Point(622, 6);
            this.logRepButt.Name = "logRepButt";
            this.logRepButt.Size = new System.Drawing.Size(32, 32);
            this.logRepButt.TabIndex = 54;
            this.logRepButt.UseVisualStyleBackColor = true;
            this.logRepButt.Click += new System.EventHandler(this.logRepButt_Click);
            // 
            // logDateFrom
            // 
            this.logDateFrom.CustomFormat = "";
            this.logDateFrom.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.logDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.logDateFrom.Location = new System.Drawing.Point(245, 12);
            this.logDateFrom.Name = "logDateFrom";
            this.logDateFrom.Size = new System.Drawing.Size(100, 23);
            this.logDateFrom.TabIndex = 56;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.label22.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label22.Location = new System.Drawing.Point(202, 9);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(41, 26);
            this.label22.TabIndex = 55;
            this.label22.Text = "From";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // radioButt2
            // 
            this.radioButt2.AutoSize = true;
            this.radioButt2.Location = new System.Drawing.Point(311, 381);
            this.radioButt2.Name = "radioButt2";
            this.radioButt2.Size = new System.Drawing.Size(139, 17);
            this.radioButt2.TabIndex = 62;
            this.radioButt2.Text = "Based On Reading Date";
            this.radioButt2.UseVisualStyleBackColor = true;
            // 
            // radioButt1
            // 
            this.radioButt1.AutoSize = true;
            this.radioButt1.Checked = true;
            this.radioButt1.Location = new System.Drawing.Point(142, 381);
            this.radioButt1.Name = "radioButt1";
            this.radioButt1.Size = new System.Drawing.Size(128, 17);
            this.radioButt1.TabIndex = 63;
            this.radioButt1.TabStop = true;
            this.radioButt1.Text = "Based On Event Date";
            this.radioButt1.UseVisualStyleBackColor = true;
            // 
            // logSeq
            // 
            this.logSeq.DataPropertyName = "id";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.logSeq.DefaultCellStyle = dataGridViewCellStyle10;
            this.logSeq.HeaderText = "No";
            this.logSeq.Name = "logSeq";
            this.logSeq.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "devId";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn3.HeaderText = "DevNo";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 50;
            // 
            // devname
            // 
            this.devname.DataPropertyName = "devName";
            this.devname.HeaderText = "Device Name";
            this.devname.Name = "devname";
            this.devname.Width = 180;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "rDate";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewTextBoxColumn4.HeaderText = "Date";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.DataPropertyName = "rTime";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewComboBoxColumn2.DefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewComboBoxColumn2.HeaderText = "Time";
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            this.dataGridViewComboBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // fingcount
            // 
            this.fingcount.DataPropertyName = "rCount";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.fingcount.DefaultCellStyle = dataGridViewCellStyle14;
            this.fingcount.HeaderText = "Count";
            this.fingcount.Name = "fingcount";
            this.fingcount.Width = 50;
            // 
            // TimeStamp
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TimeStamp.DefaultCellStyle = dataGridViewCellStyle15;
            this.TimeStamp.HeaderText = "Reading Time";
            this.TimeStamp.Name = "TimeStamp";
            this.TimeStamp.Width = 130;
            // 
            // HistReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 450);
            this.Controls.Add(this.radioButt1);
            this.Controls.Add(this.radioButt2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HistReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "History Report";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gridView3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker logDateTo;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button logRepButt;
        private System.Windows.Forms.DateTimePicker logDateFrom;
        private System.Windows.Forms.Label label22;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.ComboBox ComboBox1;
        private System.Windows.Forms.RadioButton radioButt2;
        private System.Windows.Forms.RadioButton radioButt1;
        private System.Windows.Forms.DataGridViewTextBoxColumn logSeq;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn devname;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewComboBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn fingcount;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeStamp;
    }
}