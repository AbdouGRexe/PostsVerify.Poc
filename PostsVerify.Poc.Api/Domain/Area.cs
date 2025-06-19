namespace PostsVerify.Poc.Api.Domain;

public class Area
{
    public int Id { get; set; }
    public string Label { get; set; }
}

public enum Areas
{
    Medecine = 1,
    Geopolitics = 2,
    ComputerScience = 3
}
