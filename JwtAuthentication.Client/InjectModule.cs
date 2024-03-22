using Autofac;

namespace AuthJwt
{
	public class InjectModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<AuthForm>().AsSelf();
			builder.RegisterModule<JwtAuthentication.Server.InjectModule>();
			builder.RegisterModule<JwtAuthentication.AuthorizeServer.InjectModule>();
		}
	}
}