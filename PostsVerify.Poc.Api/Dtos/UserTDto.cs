namespace PostsVerify.Poc.Api.Dtos;

public class UserTDto
{
    public int Id { get; init; }
    public string Label { get; init; }
    public string Area { get; set; }
    public byte? Score { get; set; }
}
