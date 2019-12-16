namespace LabMcuForm
{
	partial class CMcuFormAVR8BitsFuseAndLockForm
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.cMcuFormAVR8BitsFuseAndLockControl1 = new LabMcuForm.CMcuFormAVR8Bits.CMcuFormAVR8BitsFuseAndLockControl();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(13, 307);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(105, 307);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// cMcuFormAVR8BitsFuseAndLockControl1
			// 
			this.cMcuFormAVR8BitsFuseAndLockControl1.Location = new System.Drawing.Point(13, 13);
			this.cMcuFormAVR8BitsFuseAndLockControl1.MaximumSize = new System.Drawing.Size(515, 278);
			this.cMcuFormAVR8BitsFuseAndLockControl1.mCMcuFunc = null;
			this.cMcuFormAVR8BitsFuseAndLockControl1.MinimumSize = new System.Drawing.Size(515, 278);
			this.cMcuFormAVR8BitsFuseAndLockControl1.Name = "cMcuFormAVR8BitsFuseAndLockControl1";
			this.cMcuFormAVR8BitsFuseAndLockControl1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.cMcuFormAVR8BitsFuseAndLockControl1.Size = new System.Drawing.Size(515, 278);
			this.cMcuFormAVR8BitsFuseAndLockControl1.TabIndex = 2;
			// 
			// CMcuFormAVR8BitsFuseAndLockForm
			// 
			this.ClientSize = new System.Drawing.Size(539, 345);
			this.Controls.Add(this.cMcuFormAVR8BitsFuseAndLockControl1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "CMcuFormAVR8BitsFuseAndLockForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private CMcuFormAVR8Bits.CMcuFormAVR8BitsFuseAndLockControl cMcuFormAVR8BitsFuseAndLockControl1;
	}
}