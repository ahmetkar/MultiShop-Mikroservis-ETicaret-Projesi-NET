﻿namespace MultiShop.IdentityServer.DTOs
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
