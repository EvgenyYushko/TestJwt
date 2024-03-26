using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthentication.Api.Attributes
{
	public class OptionalAuthorizeAttribute : AuthorizeAttribute
	{
		private readonly bool _authorize;

		public OptionalAuthorizeAttribute()
		{
			_authorize = true;
		}

		public OptionalAuthorizeAttribute(bool authorize)
		{
			_authorize = authorize;
		}


		//protected override bool AuthorizeCore(HttpContextBase httpContext)
		//{
		//	if(!_authorize)
		//		return true;

		//	return base.AuthorizeCore(httpContext);
		//}
	}
}
