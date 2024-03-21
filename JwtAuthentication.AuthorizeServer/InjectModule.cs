using Autofac;
using JwtAuthentication.AuthorizeServer.BusinessLogicLayer;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;

namespace JwtAuthentication.AuthorizeServer
{
	public class InjectModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UserService>().As<IUserService>();
			builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
			builder.RegisterModule<UserStorage.InjectModule>();
		}
	}
}
