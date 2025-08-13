namespace Identity.Application.Response
{
    public class AuthenticationResponse
    {
        public bool IsSuccess { get; set; }
        public string? AccessToken { get; set; }
        public string? Message { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
