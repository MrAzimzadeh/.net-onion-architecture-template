namespace MyApp.Persistence.Database.Configurations
{
    public class ExternalAuthConfiguration
    {
        public GoogleAuthConfig? Google { get; set; }
        public MicrosoftAuthConfig? Microsoft { get; set; }
    }

    public class GoogleAuthConfig
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }

    public class MicrosoftAuthConfig
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string TenantId { get; set; } = null!;
    }
}
