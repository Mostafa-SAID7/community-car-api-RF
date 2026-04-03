using CommunityCarApi.Application.Common.Interfaces;
using Hangfire;

namespace CommunityCarApi.Infrastructure.Services.Background;

public class BackgroundJobService : IBackgroundJobService
{
    public string Enqueue<T>(System.Linq.Expressions.Expression<Action<T>> methodCall)
    {
        return BackgroundJob.Enqueue(methodCall);
    }

    public string Schedule<T>(System.Linq.Expressions.Expression<Action<T>> methodCall, TimeSpan delay)
    {
        return BackgroundJob.Schedule(methodCall, delay);
    }

    public void RecurringJob(string jobId, System.Linq.Expressions.Expression<Action> methodCall, string cronExpression)
    {
        Hangfire.RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);
    }

    public bool Delete(string jobId)
    {
        return BackgroundJob.Delete(jobId);
    }
}
