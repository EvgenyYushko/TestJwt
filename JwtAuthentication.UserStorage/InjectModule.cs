using Autofac;
using JwtAuthentication.UserStorage.BusinessLogicLayer;
using JwtAuthentication.UserStorage.ServiceLayer.Services;

namespace JwtAuthentication.UserStorage
{
	public class InjectModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UserManager>().As<IUserManager>();
			builder.RegisterType<DataAccessLayer.UserStorage>().SingleInstance();
		}
	}
}
