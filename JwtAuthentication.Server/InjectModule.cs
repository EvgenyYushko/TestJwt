using Autofac;
using Autofac.Extras.DynamicProxy;
using JwtAuthentication.Server.BusinessLogicLayer;
using JwtAuthentication.Server.DataAccessLayes.Repositories;
using JwtAuthentication.Server.DomainLayer.Repositories;
using JwtAuthentication.Server.Interceptors;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace JwtAuthentication.Server
{
	public class InjectModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<AuthInterceptor>();

			builder.RegisterType<BookService>()
				.EnableInterfaceInterceptors()
				.InterceptedBy(typeof(AuthInterceptor))
				.As<IBookService>();

			builder.RegisterType<BookRepository>().As<IBookRepository>();
		}
	}
}