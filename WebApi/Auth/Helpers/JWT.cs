﻿namespace WebApi.Auth.Helpers
{
    public class JWT
    {
        public required string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public required double AccessTokenDuration { get; set; }
        public required double RefreshTokenDuration { get; set; }
    }
}
