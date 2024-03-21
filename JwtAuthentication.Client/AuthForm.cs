using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Model;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;
using JwtAuthentication.Server.ServiceLayer.Models;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace AuthJwt
{
	public partial class AuthForm : Form
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly IUserService _userService;
		private readonly IBookService _bookService;

		private LoginResponse User { get; set; }

		public AuthForm(IAuthenticationService authenticationService
			, IUserService userService
			, IBookService bookService)
		{
			_authenticationService = authenticationService;
			_userService = userService;
			_bookService = bookService;
			InitializeComponent();
		}

		private void tbRegister_Click(object sender, EventArgs e)
		{
			try
			{
				var regModel = new RegistrationModel
				{
					Username = tbLogin.Text,
					Password = tbPassword.Text,
					Email = "default@mail.ru"
				};

				var res = Task.Run(async () => await _userService.Register(regModel)).Result;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.InnerException.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btLogin_Click(object sender, EventArgs e)
		{
			try
			{
				var loginModel = new LoginModel
				{
					Username = tbLogin.Text,
					Password = tbPassword.Text
				};

				User = Task.Run(async () => await _authenticationService.Login(loginModel)).Result;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.InnerException.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void tbAction_Click(object sender, EventArgs e)
		{
			try
			{
				var bookContent = Task.Run(async ()=> await _bookService.ReadBook(User?.AccessToken)).Result;
				MessageBox.Show(bookContent.Title, "Книга", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.InnerException.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btRefreshToken_Click(object sender, EventArgs e)
		{
			try
			{
				var refreshModel = new RefreshModel
				{
					AccessToken = User.AccessToken,
					RefreshToken = User.RefreshToken
				};

				User = Task.Run(async () => await _authenticationService.Refresh(refreshModel)).Result;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.InnerException.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btRevokeToken_Click(object sender, EventArgs e)
		{
			try
			{
				var res = Task.Run(async () => await _authenticationService.Revoke(User.AccessToken)).Result;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.InnerException.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
