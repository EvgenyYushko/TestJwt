using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;

namespace JwtAuthentication.Server.Interceptors
{
	public class AuthInterceptor : IInterceptor
	{
		private readonly IAuthenticationService _authenticationService;

		public AuthInterceptor(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		// Эксперемент по проверке токена :-)
		public void Intercept(IInvocation invocation)
		{
			var token = "";

			if (invocation.Arguments is { Length: > 0 })
			{
				var argumentsNames = invocation.Method.GetParameters();
				for (var i = 0; i < invocation.Arguments.Length; i++)
				{
					if (argumentsNames[i].Name == "acessToken")
					{
						object argument = GetArgument(invocation.Arguments[i]);
						token = argument.ToString();
						break;
					}
				}
			}

			if (!Task.Run(async ()=> await _authenticationService.CheckToken(token)).Result)
			{
				throw new Exception("Unauthorized");
			}

			invocation.Proceed();
		}

		private static string GetArgument(object argument)
		{
			if (argument is null)
			{
				return string.Empty;
			}

			if (argument.GetType().IsGenericType && argument is IEnumerable listArgument)
			{
				var itemsList = new List<string>();
				var listStringResult = new StringBuilder();
				listStringResult.Append($"Type: {argument.GetType()}\n");
				var type = string.Empty;
				foreach (var item in listArgument)
				{
					if (string.IsNullOrEmpty(type))
					{
						type = item.GetType().ToString();
					}
					itemsList.Add(item.ToString());
				}
				listStringResult.Append($"Count: {itemsList.Count}");
				foreach (var item in itemsList.Where(item => item != type))
				{
					listStringResult.Append($"\n{item}");
				}
				return listStringResult.ToString();
			}

			return argument.ToString();
		}
	}
}
