using System.Collections.ObjectModel;

namespace PostsVerify.Poc.Api.Domain.Values;

public record UserData(Collection<Post> Posts, Collection<Rating> Ratings);