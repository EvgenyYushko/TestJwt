
namespace AuthJwt
{
	partial class AuthForm
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
			this.tbLogin = new System.Windows.Forms.TextBox();
			this.tbRegister = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbAction = new System.Windows.Forms.Button();
			this.btLogin = new System.Windows.Forms.Button();
			this.btRefreshToken = new System.Windows.Forms.Button();
			this.btRevokeToken = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbLogin
			// 
			this.tbLogin.Location = new System.Drawing.Point(79, 9);
			this.tbLogin.Name = "tbLogin";
			this.tbLogin.Size = new System.Drawing.Size(100, 20);
			this.tbLogin.TabIndex = 0;
			// 
			// tbRegister
			// 
			this.tbRegister.Location = new System.Drawing.Point(190, 9);
			this.tbRegister.Name = "tbRegister";
			this.tbRegister.Size = new System.Drawing.Size(85, 20);
			this.tbRegister.TabIndex = 1;
			this.tbRegister.Text = "Регистрация";
			this.tbRegister.UseVisualStyleBackColor = true;
			this.tbRegister.Click += new System.EventHandler(this.tbRegister_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Логин";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(17, 37);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Пароль";
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(79, 37);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.Size = new System.Drawing.Size(100, 20);
			this.tbPassword.TabIndex = 4;
			// 
			// tbAction
			// 
			this.tbAction.Location = new System.Drawing.Point(20, 73);
			this.tbAction.Name = "tbAction";
			this.tbAction.Size = new System.Drawing.Size(255, 50);
			this.tbAction.TabIndex = 5;
			this.tbAction.Text = "Имитация запроса на сервер";
			this.tbAction.UseVisualStyleBackColor = true;
			this.tbAction.Click += new System.EventHandler(this.tbAction_Click);
			// 
			// btLogin
			// 
			this.btLogin.Location = new System.Drawing.Point(190, 37);
			this.btLogin.Name = "btLogin";
			this.btLogin.Size = new System.Drawing.Size(85, 20);
			this.btLogin.TabIndex = 6;
			this.btLogin.Text = "Вход";
			this.btLogin.UseVisualStyleBackColor = true;
			this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
			// 
			// btRefreshToken
			// 
			this.btRefreshToken.Location = new System.Drawing.Point(281, 8);
			this.btRefreshToken.Name = "btRefreshToken";
			this.btRefreshToken.Size = new System.Drawing.Size(104, 20);
			this.btRefreshToken.TabIndex = 7;
			this.btRefreshToken.Text = "Refresh Token";
			this.btRefreshToken.UseVisualStyleBackColor = true;
			this.btRefreshToken.Click += new System.EventHandler(this.btRefreshToken_Click);
			// 
			// btRevokeToken
			// 
			this.btRevokeToken.Location = new System.Drawing.Point(281, 37);
			this.btRevokeToken.Name = "btRevokeToken";
			this.btRevokeToken.Size = new System.Drawing.Size(104, 20);
			this.btRevokeToken.TabIndex = 8;
			this.btRevokeToken.Text = "Revoke Token";
			this.btRevokeToken.UseVisualStyleBackColor = true;
			this.btRevokeToken.Click += new System.EventHandler(this.btRevokeToken_Click);
			// 
			// AuthForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(397, 133);
			this.Controls.Add(this.btRevokeToken);
			this.Controls.Add(this.btRefreshToken);
			this.Controls.Add(this.btLogin);
			this.Controls.Add(this.tbAction);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbRegister);
			this.Controls.Add(this.tbLogin);
			this.Name = "AuthForm";
			this.Text = "Jwt";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AuthForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbLogin;
		private System.Windows.Forms.Button tbRegister;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Button tbAction;
		private System.Windows.Forms.Button btLogin;
		private System.Windows.Forms.Button btRefreshToken;
		private System.Windows.Forms.Button btRevokeToken;
	}
}

