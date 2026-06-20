namespace IARRv3.Api.Domain.Entities;
public class User(Guid id, string fullName)
{
    public Guid Id { get; init; } = id;

    public string FullName { get; set; } = fullName;

    public double ReputationScore { get; set; }
    
    public bool Initiated { get; set; } =  false;
    
    public User GetUserCopy()
    {
        return new User(Id, FullName);
    }
}