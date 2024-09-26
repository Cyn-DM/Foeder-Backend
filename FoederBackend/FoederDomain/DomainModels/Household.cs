namespace FoederDomain.DomainModels;

public class Household
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<User> Users { get; set; }

    public Household(){}
}