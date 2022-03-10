namespace CrystalReport_Console
{
    partial class EditPayee
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
            this.payee_textBox = new System.Windows.Forms.TextBox();
            this.cheque_usage_textBox = new System.Windows.Forms.TextBox();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // payee_textBox
            // 
            this.payee_textBox.Location = new System.Drawing.Point(116, 30);
            this.payee_textBox.Name = "payee_textBox";
            this.payee_textBox.Size = new System.Drawing.Size(509, 22);
            this.payee_textBox.TabIndex = 0;
            // 
            // cheque_usage_textBox
            // 
            this.cheque_usage_textBox.Location = new System.Drawing.Point(116, 58);
            this.cheque_usage_textBox.Name = "cheque_usage_textBox";
            this.cheque_usage_textBox.Size = new System.Drawing.Size(509, 22);
            this.cheque_usage_textBox.TabIndex = 1;
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(230, 109);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(75, 23);
            this.save_button.TabIndex = 2;
            this.save_button.Text = "Save";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(373, 109);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 3;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Payee";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Cheque Usage";
            // 
            // EditPayee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 164);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.cheque_usage_textBox);
            this.Controls.Add(this.payee_textBox);
            this.Name = "EditPayee";
            this.Text = "EditPayee";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox payee_textBox;
        private System.Windows.Forms.TextBox cheque_usage_textBox;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}