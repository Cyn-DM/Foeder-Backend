namespace FoederBusiness.Dtos;

public class RefreshResult
{
    public bool IsRefreshTokenExpired  { get; set; }
    public bool isRefreshTokenFound { get; set; }
    public string AccessToken { get; set; }    
}