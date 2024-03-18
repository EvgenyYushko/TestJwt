using System;
using System.Collections.Generic;
using JwtAuthentication.Server.DataAccessLayer.Models;

namespace JwtAuthentication.Server.ServiceLayer.Models
{
	public class ServiceUser
	{
		public string UserName { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiry { get; set; }
	}
}
