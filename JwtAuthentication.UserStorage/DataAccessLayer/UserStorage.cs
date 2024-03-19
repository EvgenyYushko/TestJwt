using System.Collections.Generic;
using JwtAuthentication.UserStorage.DomainLayer.Models;

namespace JwtAuthentication.UserStorage.DataAccessLayer
{
	public class UserStorage
	{
		public Dictionary<string, UserDbo> Users = new ();
	}
}
