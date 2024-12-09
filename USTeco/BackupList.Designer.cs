
namespace USTeco
{
    partial class BackupList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupList));
            this.restoreButt = new System.Windows.Forms.Button();
            this.deleteButt = new System.Windows.Forms.Button();
            this.cancelButt = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // restoreButt
            // 
            this.restoreButt.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.restoreButt.Enabled = false;
            this.restoreButt.Location = new System.Drawing.Point(300, 162);
            this.restoreButt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.restoreButt.Name = "restoreButt";
            this.restoreButt.Size = new System.Drawing.Size(78, 30);
            this.restoreButt.TabIndex = 0;
            this.restoreButt.Text = "Restore";
            this.restoreButt.UseVisualStyleBackColor = true;
            this.restoreButt.Click += new System.EventHandler(this.restoreButt_Click);
            // 
            // deleteButt
            // 
            this.deleteButt.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.deleteButt.Enabled = false;
            this.deleteButt.Location = new System.Drawing.Point(216, 162);
            this.deleteButt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deleteButt.Name = "deleteButt";
            this.deleteButt.Size = new System.Drawing.Size(78, 30);
            this.deleteButt.TabIndex = 1;
            this.deleteButt.Text = "Delete";
            this.deleteButt.UseVisualStyleBackColor = true;
            // 
            // cancelButt
            // 
            this.cancelButt.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButt.Location = new System.Drawing.Point(132, 162);
            this.cancelButt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancelButt.Name = "cancelButt";
            this.cancelButt.Size = new System.Drawing.Size(78, 30);
            this.cancelButt.TabIndex = 2;
            this.cancelButt.Text = "Cancel";
            this.cancelButt.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(30, 20);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(325, 55);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Backup";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(257, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Employee No To Restore (Empty Means All)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(300, 101);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(55, 23);
            this.textBox1.TabIndex = 6;
            // 
            // BackupList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 202);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButt);
            this.Controls.Add(this.deleteButt);
            this.Controls.Add(this.restoreButt);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackupList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup List";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button restoreButt;
        private System.Windows.Forms.Button deleteButt;
        private System.Windows.Forms.Button cancelButt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
    }
}