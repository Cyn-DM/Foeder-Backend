using System.ComponentModel.DataAnnotations;

namespace FoederDomain.DomainModels;

public class RefreshToken
{
    [Required, Key]
    public string Token { get; set; }
    [Required]
    public User User { get; set; }
    [Required]
    public DateTime ExpirationDate { get; set; }
}