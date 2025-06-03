namespace SAIdImporter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnImport = new Button();
            dataGridView1 = new DataGridView();
            openFileDialog1 = new OpenFileDialog();
            txtSearch = new TextBox();
            cmbGender = new ComboBox();
            btnViewDb = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btnImport
            // 
            btnImport.Location = new Point(12, 11);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(129, 23);
            btnImport.TabIndex = 0;
            btnImport.Text = "Import Excel";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 42);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size(589, 410);
            dataGridView1.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(284, 11);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search name...";
            txtSearch.Size = new Size(172, 23);
            txtSearch.TabIndex = 2;
            // 
            // cmbGender
            // 
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.FormattingEnabled = true;
            cmbGender.Location = new Point(462, 12);
            cmbGender.Name = "cmbGender";
            cmbGender.Size = new Size(140, 23);
            cmbGender.TabIndex = 3;
            // 
            // btnViewDb
            // 
            btnViewDb.Location = new Point(147, 10);
            btnViewDb.Name = "btnViewDb";
            btnViewDb.Size = new Size(131, 23);
            btnViewDb.TabIndex = 4;
            btnViewDb.Text = "View from Database";
            btnViewDb.UseVisualStyleBackColor = true;
            btnViewDb.Click += btnViewDb_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(613, 464);
            Controls.Add(btnViewDb);
            Controls.Add(cmbGender);
            Controls.Add(txtSearch);
            Controls.Add(dataGridView1);
            Controls.Add(btnImport);
            Name = "Form1";
            Text = "RSA ID Validator";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnImport;
        private DataGridView dataGridView1;
        private OpenFileDialog openFileDialog1;
        private TextBox txtSearch;
        private ComboBox cmbGender;
        private Button btnViewDb;
    }
}
