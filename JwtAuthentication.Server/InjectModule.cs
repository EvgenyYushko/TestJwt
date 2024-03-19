using Autofac;
using JwtAuthentication.Server.BusinessLogicLayer;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace JwtAuthentication.Server
{
	public class InjectModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
			builder.RegisterType<UserManager>().As<IUserManager>();
		}
	}
}
