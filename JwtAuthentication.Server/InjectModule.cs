using Autofac;
using JwtAuthentication.Server.BusinessLogicLayer;
using JwtAuthentication.Server.DataAccessLayes.Repositories;
using JwtAuthentication.Server.DomainLayer.Repositories;
using JwtAuthentication.Server.ServiceLayer.Services;

namespace JwtAuthentication.Server
{
	public class InjectModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<BookRepository>().As<IBookRepository>();
			builder.RegisterType<BookService>().As<IBookService>();
		}
	}
}
