using System.Collections.ObjectModel;
using IARRv3.Api.Domain.Entities;

namespace IARRv3.Api.Domain;

public record UserData(Collection<Post> Posts, Collection<Rating> Ratings);