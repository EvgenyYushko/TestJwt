using System;
using System.Windows.Forms;
using Autofac;

namespace AuthJwt
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var builder = new ContainerBuilder();
			builder.RegisterModule<InjectModule>();

			var container = builder.Build();

			try
			{
				Application.Run(container.Resolve<AuthForm>());
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}
	}
}
