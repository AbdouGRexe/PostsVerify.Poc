using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application;

public interface IAccuracyService
{
    Task Get(int id);
    Task SetValues(Collection<int> postIds);
}