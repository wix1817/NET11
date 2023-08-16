﻿using BusinessLayerInterfaces.BusinessModels;
using BusinessLayerInterfaces.UserServices;

namespace GamerShop.Services
{
	public class AuthService : IAuthService
	{
		private IHomeServices _homeServices;
		private IHttpContextAccessor _httpContextAccessor;

		public AuthService(IHomeServices homeServices, IHttpContextAccessor httpContextAccessor)
		{
			_homeServices = homeServices;
			_httpContextAccessor = httpContextAccessor;
		}

		public UserBlm GetCurrentUser()
		{
			var idStr = _httpContextAccessor
				.HttpContext
				.User
				.Claims
				.First(x => x.Type == "Id")
				.Value;
			var id = int.Parse(idStr);
			var user = _homeServices.GetUserById(id);
			return user;
		}
	}
}
