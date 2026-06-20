using System.Collections.ObjectModel;

namespace IARRv3.Api.Application;

public interface IAccuracyService
{
    Task Get(Guid id);
    Task SetValues(Collection<Guid> postIds);
}