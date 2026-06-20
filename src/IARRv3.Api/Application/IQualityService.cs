namespace IARRv3.Api.Application;

public interface IQualityService
{
    Task Get(Guid reviewId);
}