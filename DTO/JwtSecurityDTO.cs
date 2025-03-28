namespace Education.DTO
{
    public class JwtSecurityDto
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; } 
    }
}
