using System;
using System.Threading.Tasks;

namespace PostsVerify.Poc.Api.Application;

public interface IQualityService
{
    Task Get(Guid reviewId);
}