using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Model;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace AuthJwt
{
	public partial class AuthForm : Form
	{
 //dsds
		private readonly IAuthenticationService _authenticationService;
		private readonly IBookService _bookService;

		private UserClient User { get; set; }

		public AuthForm(IAuthenticationService authenticationService
			, IBookService bookService)
		{
			_authenticationService = authenticationService;
			_bookService = bookService;
			InitializeComponent();
		}

		private void tbRegister_Click(object sender, EventArgs e)
		{
			try
			{
				var userClient = new UserClient
				{
					Username = tbLogin.Text,
					Password = tbPassword.Text,
					Email = "default@mail.ru"
				};

				var res = Task.Run(async () => await _authenticationService.Register(userClient)).Result;
				MessageBox.Show($"Пользователь \"{userClient.Username}\" зарегистрирован", "Сообщение",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
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
				var userClient = new UserClient
				{
					Username = tbLogin.Text,
					Password = tbPassword.Text
				};

				var userServer = Task.Run(async () => await _authenticationService.Login(userClient)).Result;

				UserServerToClient(userServer, userClient);

				User = userClient;

				MessageBox.Show($"Пользователь \"{userClient.Username}\" успешно вошёл в систему", "Сообщение",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
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
				if (User.Expiration <= DateTime.Now)
				{
					var userServer = Task.Run(async () => await _authenticationService.Refresh(User)).Result;
					UserServerToClient(userServer, User);
				}

				var bookContent = Task.Run(async () => await _bookService.ReadBook(464318, User?.AccessToken)).Result;
				MessageBox.Show(bookContent.Title, "Получена Книга", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
				var userClient = new UserClient
				{
					AccessToken = User.AccessToken,
					RefreshToken = User.RefreshToken
				};

				var userServer = Task.Run(async () => await _authenticationService.Refresh(userClient)).Result;

				UserServerToClient(userServer, userClient);

				User = userClient;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.InnerException.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btRevokeToken_Click(object sender, EventArgs e)
		{
			Logoff(User);
		}

		private void AuthForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Logoff(User);
		}

		private void Logoff(UserClient userClient)
		{
			if (userClient is null)
			{
				return;
			}

			try
			{
				var res = Task.Run(async () => await _authenticationService.Revoke(userClient.AccessToken)).Result;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.InnerException.Message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void UserServerToClient(UserServer userServer, UserClient userClient)
		{
			userClient.AccessToken = userServer.AccessToken;
			userClient.RefreshToken = userServer.RefreshToken;
			userClient.Expiration = userServer.Expiration;
		}
	}
}
