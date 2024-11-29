using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FoederDomain.DomainModels;

public class HouseholdInvite
{
    public Guid Id { get; set; }
    [JsonIgnore, Required]
    public Household Household { get; set; }
    [Required]
    public User InvitedUser { get; set; }
    public bool? IsAccepted { get; set; } 
}