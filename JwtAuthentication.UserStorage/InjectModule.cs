using Autofac;

namespace JwtAuthentication.UserStorage
{
	public class InjectModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<DataAccessLayer.UserStorage>().SingleInstance();
		}
	}
}
