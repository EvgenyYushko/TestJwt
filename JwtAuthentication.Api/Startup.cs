using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthentication.AuthorizeServer.BusinessLogicLayer;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;
using JwtAuthentication.Server.BusinessLogicLayer;
using JwtAuthentication.Server.DataAccessLayes.Repositories;
using JwtAuthentication.Server.DomainLayer.Repositories;
using JwtAuthentication.Server.ServiceLayer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JwtAuthentication.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRazorPages();

			services.AddControllers();
			//services.AddAuthentication(options =>
			//{
			//	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			//});

		//	services.AddAuthentication("s");

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = "YourAuthenticationScheme";
				options.DefaultScheme = "YourAuthenticationScheme";
				options.DefaultChallengeScheme = "YourAuthenticationScheme";
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("age-policy", x => { x.RequireClaim("age"); });
			});


			services.AddTransient<IAuthenticationService, AuthenticationService>();
			services.AddSingleton<UserStorage.DataAccessLayer.UserStorage>();
			services.AddTransient<IBookRepository, BookRepository>();
			services.AddTransient<IBookService, BookService>();
			services.AddTransient<IUserService, UserService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();

				//endpoints.MapRazorPages();
			});
		}
	}
}
