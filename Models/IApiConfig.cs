namespace JWT_Example.Config
{
    public interface IApiConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string JWTAudience { get; set; }

        public string JWTSecretKey { get; set; }
        public string JWTLifeTimeInMinutes { get; set; }

        public string JWTClaimRole { get; set; }
        public string JWTIssuer { get; set; }
    }
}
