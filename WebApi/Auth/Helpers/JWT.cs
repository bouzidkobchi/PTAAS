namespace WebApi.Auth.Helpers
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public required double DurationInDays { get; set; }
    }
}
