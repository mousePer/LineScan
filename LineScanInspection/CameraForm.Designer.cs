namespace LineScanInspection
{
    partial class CameraForm
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
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uiGroupBox4 = new Sunny.UI.UIGroupBox();
            this.uiTextBox2 = new Sunny.UI.UITextBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiTextBox1 = new Sunny.UI.UITextBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.uiButton3 = new Sunny.UI.UIButton();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.btn_open = new Sunny.UI.UIButton();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiGroupBox3 = new Sunny.UI.UIGroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uiGroupBox1.SuspendLayout();
            this.uiGroupBox4.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.listView1);
            this.uiGroupBox1.Controls.Add(this.uiGroupBox4);
            this.uiGroupBox1.Controls.Add(this.uiPanel1);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.Size = new System.Drawing.Size(1115, 609);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "相机管理";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(378, 121);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(737, 488);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "...";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "日志时间";
            this.columnHeader2.Width = 113;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "日志信息";
            this.columnHeader3.Width = 533;
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.Controls.Add(this.uiTextBox2);
            this.uiGroupBox4.Controls.Add(this.uiLabel2);
            this.uiGroupBox4.Controls.Add(this.uiTextBox1);
            this.uiGroupBox4.Controls.Add(this.uiLabel1);
            this.uiGroupBox4.Controls.Add(this.uiButton1);
            this.uiGroupBox4.Controls.Add(this.uiButton3);
            this.uiGroupBox4.Controls.Add(this.uiButton2);
            this.uiGroupBox4.Controls.Add(this.btn_open);
            this.uiGroupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox4.Location = new System.Drawing.Point(378, 32);
            this.uiGroupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox4.Size = new System.Drawing.Size(737, 89);
            this.uiGroupBox4.TabIndex = 1;
            this.uiGroupBox4.Text = "相机操作";
            this.uiGroupBox4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiTextBox2
            // 
            this.uiTextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTextBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTextBox2.Location = new System.Drawing.Point(425, 38);
            this.uiTextBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTextBox2.MinimumSize = new System.Drawing.Size(1, 16);
            this.uiTextBox2.Name = "uiTextBox2";
            this.uiTextBox2.Padding = new System.Windows.Forms.Padding(5);
            this.uiTextBox2.ShowText = false;
            this.uiTextBox2.Size = new System.Drawing.Size(97, 29);
            this.uiTextBox2.TabIndex = 2;
            this.uiTextBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTextBox2.Watermark = "";
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(380, 43);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(66, 23);
            this.uiLabel2.TabIndex = 1;
            this.uiLabel2.Text = "下限：";
            // 
            // uiTextBox1
            // 
            this.uiTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.uiTextBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTextBox1.Location = new System.Drawing.Point(276, 38);
            this.uiTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTextBox1.MinimumSize = new System.Drawing.Size(1, 16);
            this.uiTextBox1.Name = "uiTextBox1";
            this.uiTextBox1.Padding = new System.Windows.Forms.Padding(5);
            this.uiTextBox1.ShowText = false;
            this.uiTextBox1.Size = new System.Drawing.Size(97, 29);
            this.uiTextBox1.TabIndex = 2;
            this.uiTextBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiTextBox1.Watermark = "";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(233, 43);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(66, 23);
            this.uiLabel1.TabIndex = 1;
            this.uiLabel1.Text = "上限：";
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(634, 35);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(100, 35);
            this.uiButton1.TabIndex = 0;
            this.uiButton1.Text = "Halcon";
            this.uiButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // uiButton3
            // 
            this.uiButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Location = new System.Drawing.Point(529, 35);
            this.uiButton3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton3.Name = "uiButton3";
            this.uiButton3.Size = new System.Drawing.Size(100, 35);
            this.uiButton3.TabIndex = 0;
            this.uiButton3.Text = "获取当前照片";
            this.uiButton3.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Click += new System.EventHandler(this.uiButton3_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Location = new System.Drawing.Point(127, 35);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(100, 35);
            this.uiButton2.TabIndex = 0;
            this.uiButton2.Text = "选择照片";
            this.uiButton2.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // btn_open
            // 
            this.btn_open.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_open.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_open.Location = new System.Drawing.Point(7, 35);
            this.btn_open.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(100, 35);
            this.btn_open.TabIndex = 0;
            this.btn_open.Text = "打开相机";
            this.btn_open.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.uiGroupBox3);
            this.uiPanel1.Controls.Add(this.uiGroupBox2);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(0, 32);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(378, 577);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Controls.Add(this.pictureBox2);
            this.uiGroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiGroupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox3.Location = new System.Drawing.Point(0, 193);
            this.uiGroupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox3.Size = new System.Drawing.Size(378, 384);
            this.uiGroupBox3.TabIndex = 1;
            this.uiGroupBox3.Text = "作业图像";
            this.uiGroupBox3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(0, 32);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(378, 352);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.pictureBox1);
            this.uiGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.Size = new System.Drawing.Size(378, 340);
            this.uiGroupBox2.TabIndex = 0;
            this.uiGroupBox2.Text = "实时图像";
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(378, 308);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // CameraForm
            // 
            this.AllowShowTitle = false;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1115, 609);
            this.Controls.Add(this.uiGroupBox1);
            this.Name = "CameraForm";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.ShowTitle = false;
            this.Text = "CameraForm";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox4.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.uiGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox uiGroupBox1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIGroupBox uiGroupBox3;
        private Sunny.UI.UIGroupBox uiGroupBox2;
        private System.Windows.Forms.ListView listView1;
        private Sunny.UI.UIGroupBox uiGroupBox4;
        private Sunny.UI.UITextBox uiTextBox2;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox uiTextBox1;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIButton uiButton3;
        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIButton btn_open;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private Sunny.UI.UIButton uiButton1;
    }
}