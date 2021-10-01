namespace gfoidl.DataGridView.Test
{
	partial class Form1
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
      this.gfDataGridView1 = new gfoidl.Windows.Forms.gfDataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.gfDataGridView1)).BeginInit();
      this.SuspendLayout();
      // 
      // gfDataGridView1
      // 
      this.gfDataGridView1.AllowUserToOrderColumns = true;
      this.gfDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gfDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gfDataGridView1.Location = new System.Drawing.Point(0, 0);
      this.gfDataGridView1.Name = "gfDataGridView1";
      this.gfDataGridView1.Size = new System.Drawing.Size(507, 202);
      this.gfDataGridView1.TabIndex = 0;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(507, 202);
      this.Controls.Add(this.gfDataGridView1);
      this.Name = "Form1";
      this.Text = "Form1";
      ((System.ComponentModel.ISupportInitialize)(this.gfDataGridView1)).EndInit();
      this.ResumeLayout(false);

		}

		#endregion

		private gfoidl.Windows.Forms.gfDataGridView gfDataGridView1;
  }
}

