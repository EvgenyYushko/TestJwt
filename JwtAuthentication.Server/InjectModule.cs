using Autofac;
using JwtAuthentication.Server.BusinessLogicLayer;
using JwtAuthentication.Server.DataAccessLayer;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace JwtAuthentication.Server
{
	public class InjectModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UserService>().As<IUserService>();
			builder.RegisterType<BookService>().As<IBookService>();
		}
	}
}
