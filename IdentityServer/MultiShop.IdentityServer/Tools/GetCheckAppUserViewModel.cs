﻿namespace MultiShop.IdentityServer.Tools
{
    public class GetCheckAppUserViewModel
    {
        public GetCheckAppUserViewModel()
        {
            
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool IsExist { get; set; }

    }
}
