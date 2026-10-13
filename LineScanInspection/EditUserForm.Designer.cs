namespace LineScanInspection
{
    partial class EditUserForm
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
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.btn_saveUser = new Sunny.UI.UIButton();
            this.btn_cancelUser = new Sunny.UI.UIButton();
            this.uiAvatar1 = new Sunny.UI.UIAvatar();
            this.Txb_password = new Sunny.UI.UITextBox();
            this.Txb_account = new Sunny.UI.UITextBox();
            this.Txb_userName = new Sunny.UI.UITextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Txb_checkPassword = new Sunny.UI.UITextBox();
            this.cb_io = new Sunny.UI.UICheckBox();
            this.cb_log = new Sunny.UI.UICheckBox();
            this.cb_yield = new Sunny.UI.UICheckBox();
            this.cb_camera = new Sunny.UI.UICheckBox();
            this.cb_user = new Sunny.UI.UICheckBox();
            this.btn_all = new Sunny.UI.UIButton();
            this.uiGroupBox1.SuspendLayout();
            this.uiGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.Txb_checkPassword);
            this.uiGroupBox1.Controls.Add(this.Txb_password);
            this.uiGroupBox1.Controls.Add(this.Txb_account);
            this.uiGroupBox1.Controls.Add(this.Txb_userName);
            this.uiGroupBox1.Controls.Add(this.label4);
            this.uiGroupBox1.Controls.Add(this.label3);
            this.uiGroupBox1.Controls.Add(this.label2);
            this.uiGroupBox1.Controls.Add(this.label1);
            this.uiGroupBox1.Controls.Add(this.uiAvatar1);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 35);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.Size = new System.Drawing.Size(373, 243);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "用户信息";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.cb_user);
            this.uiGroupBox2.Controls.Add(this.btn_all);
            this.uiGroupBox2.Controls.Add(this.cb_yield);
            this.uiGroupBox2.Controls.Add(this.cb_log);
            this.uiGroupBox2.Controls.Add(this.cb_camera);
            this.uiGroupBox2.Controls.Add(this.cb_io);
            this.uiGroupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox2.Location = new System.Drawing.Point(0, 288);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.Size = new System.Drawing.Size(373, 238);
            this.uiGroupBox2.TabIndex = 0;
            this.uiGroupBox2.Text = "权限操作";
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_saveUser
            // 
            this.btn_saveUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_saveUser.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_saveUser.Location = new System.Drawing.Point(55, 539);
            this.btn_saveUser.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_saveUser.Name = "btn_saveUser";
            this.btn_saveUser.Size = new System.Drawing.Size(100, 35);
            this.btn_saveUser.TabIndex = 1;
            this.btn_saveUser.Text = "确定";
            this.btn_saveUser.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_saveUser.Click += new System.EventHandler(this.btn_saveUser_Click);
            // 
            // btn_cancelUser
            // 
            this.btn_cancelUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_cancelUser.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_cancelUser.Location = new System.Drawing.Point(221, 539);
            this.btn_cancelUser.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_cancelUser.Name = "btn_cancelUser";
            this.btn_cancelUser.Size = new System.Drawing.Size(100, 35);
            this.btn_cancelUser.TabIndex = 1;
            this.btn_cancelUser.Text = "取消";
            this.btn_cancelUser.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_cancelUser.Click += new System.EventHandler(this.btn_cancelUser_Click);
            // 
            // uiAvatar1
            // 
            this.uiAvatar1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiAvatar1.Location = new System.Drawing.Point(14, 35);
            this.uiAvatar1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiAvatar1.Name = "uiAvatar1";
            this.uiAvatar1.Size = new System.Drawing.Size(52, 48);
            this.uiAvatar1.TabIndex = 0;
            this.uiAvatar1.Text = "uiAvatar1";
            // 
            // Txb_password
            // 
            this.Txb_password.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Txb_password.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Txb_password.Location = new System.Drawing.Point(115, 166);
            this.Txb_password.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Txb_password.MinimumSize = new System.Drawing.Size(1, 16);
            this.Txb_password.Name = "Txb_password";
            this.Txb_password.Padding = new System.Windows.Forms.Padding(5);
            this.Txb_password.PasswordChar = '*';
            this.Txb_password.ShowText = false;
            this.Txb_password.Size = new System.Drawing.Size(229, 29);
            this.Txb_password.TabIndex = 6;
            this.Txb_password.Tag = "用户密码：";
            this.Txb_password.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.Txb_password.Watermark = "";
            // 
            // Txb_account
            // 
            this.Txb_account.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Txb_account.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Txb_account.Location = new System.Drawing.Point(115, 129);
            this.Txb_account.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Txb_account.MinimumSize = new System.Drawing.Size(1, 16);
            this.Txb_account.Name = "Txb_account";
            this.Txb_account.Padding = new System.Windows.Forms.Padding(5);
            this.Txb_account.ShowText = false;
            this.Txb_account.Size = new System.Drawing.Size(229, 29);
            this.Txb_account.TabIndex = 7;
            this.Txb_account.Tag = "用户账户：";
            this.Txb_account.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.Txb_account.Watermark = "";
            // 
            // Txb_userName
            // 
            this.Txb_userName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Txb_userName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Txb_userName.Location = new System.Drawing.Point(115, 93);
            this.Txb_userName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Txb_userName.MinimumSize = new System.Drawing.Size(1, 16);
            this.Txb_userName.Name = "Txb_userName";
            this.Txb_userName.Padding = new System.Windows.Forms.Padding(5);
            this.Txb_userName.ShowText = false;
            this.Txb_userName.Size = new System.Drawing.Size(229, 29);
            this.Txb_userName.TabIndex = 8;
            this.Txb_userName.Tag = "用户姓名：";
            this.Txb_userName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.Txb_userName.Watermark = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(11, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "用户密码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(11, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "用户账户：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "用户姓名：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(11, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "确认密码：";
            // 
            // Txb_checkPassword
            // 
            this.Txb_checkPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Txb_checkPassword.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Txb_checkPassword.Location = new System.Drawing.Point(115, 205);
            this.Txb_checkPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Txb_checkPassword.MinimumSize = new System.Drawing.Size(1, 16);
            this.Txb_checkPassword.Name = "Txb_checkPassword";
            this.Txb_checkPassword.Padding = new System.Windows.Forms.Padding(5);
            this.Txb_checkPassword.ShowText = false;
            this.Txb_checkPassword.Size = new System.Drawing.Size(229, 29);
            this.Txb_checkPassword.TabIndex = 6;
            this.Txb_checkPassword.Tag = "确认密码：";
            this.Txb_checkPassword.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.Txb_checkPassword.Watermark = "";
            // 
            // cb_io
            // 
            this.cb_io.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cb_io.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_io.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cb_io.Location = new System.Drawing.Point(33, 51);
            this.cb_io.MinimumSize = new System.Drawing.Size(1, 1);
            this.cb_io.Name = "cb_io";
            this.cb_io.Size = new System.Drawing.Size(118, 29);
            this.cb_io.TabIndex = 0;
            this.cb_io.Text = "IO管理";
            // 
            // cb_log
            // 
            this.cb_log.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cb_log.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_log.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cb_log.Location = new System.Drawing.Point(33, 104);
            this.cb_log.MinimumSize = new System.Drawing.Size(1, 1);
            this.cb_log.Name = "cb_log";
            this.cb_log.Size = new System.Drawing.Size(118, 29);
            this.cb_log.TabIndex = 0;
            this.cb_log.Text = "日志管理";
            // 
            // cb_yield
            // 
            this.cb_yield.Checked = true;
            this.cb_yield.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cb_yield.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_yield.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cb_yield.Location = new System.Drawing.Point(33, 160);
            this.cb_yield.MinimumSize = new System.Drawing.Size(1, 1);
            this.cb_yield.Name = "cb_yield";
            this.cb_yield.Size = new System.Drawing.Size(118, 29);
            this.cb_yield.TabIndex = 0;
            this.cb_yield.Text = "产量管理";
            // 
            // cb_camera
            // 
            this.cb_camera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cb_camera.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_camera.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cb_camera.Location = new System.Drawing.Point(199, 51);
            this.cb_camera.MinimumSize = new System.Drawing.Size(1, 1);
            this.cb_camera.Name = "cb_camera";
            this.cb_camera.Size = new System.Drawing.Size(118, 29);
            this.cb_camera.TabIndex = 0;
            this.cb_camera.Text = "相机管理";
            // 
            // cb_user
            // 
            this.cb_user.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cb_user.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_user.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cb_user.Location = new System.Drawing.Point(199, 104);
            this.cb_user.MinimumSize = new System.Drawing.Size(1, 1);
            this.cb_user.Name = "cb_user";
            this.cb_user.Size = new System.Drawing.Size(118, 29);
            this.cb_user.TabIndex = 0;
            this.cb_user.Text = "用户管理";
            // 
            // btn_all
            // 
            this.btn_all.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_all.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_all.Location = new System.Drawing.Point(199, 160);
            this.btn_all.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_all.Name = "btn_all";
            this.btn_all.Size = new System.Drawing.Size(118, 35);
            this.btn_all.TabIndex = 1;
            this.btn_all.Text = "全选/全不选";
            this.btn_all.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_all.Click += new System.EventHandler(this.uiButton3_Click);
            // 
            // EditUserForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(373, 590);
            this.Controls.Add(this.btn_cancelUser);
            this.Controls.Add(this.btn_saveUser);
            this.Controls.Add(this.uiGroupBox2);
            this.Controls.Add(this.uiGroupBox1);
            this.Name = "EditUserForm";
            this.Text = "用户添加";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            this.uiGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox uiGroupBox1;
        private Sunny.UI.UIGroupBox uiGroupBox2;
        private Sunny.UI.UIAvatar uiAvatar1;
        private Sunny.UI.UIButton btn_saveUser;
        private Sunny.UI.UIButton btn_cancelUser;
        private Sunny.UI.UITextBox Txb_checkPassword;
        private Sunny.UI.UITextBox Txb_password;
        private Sunny.UI.UITextBox Txb_account;
        private Sunny.UI.UITextBox Txb_userName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Sunny.UI.UICheckBox cb_user;
        private Sunny.UI.UIButton btn_all;
        private Sunny.UI.UICheckBox cb_yield;
        private Sunny.UI.UICheckBox cb_log;
        private Sunny.UI.UICheckBox cb_camera;
        private Sunny.UI.UICheckBox cb_io;
    }
}