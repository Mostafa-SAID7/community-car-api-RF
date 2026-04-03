namespace CommunityCarApi.Application.Common.Interfaces;

public interface IBackgroundJobService
{
    string Enqueue<T>(System.Linq.Expressions.Expression<Action<T>> methodCall);
    string Schedule<T>(System.Linq.Expressions.Expression<Action<T>> methodCall, TimeSpan delay);
    void RecurringJob(string jobId, System.Linq.Expressions.Expression<Action> methodCall, string cronExpression);
    bool Delete(string jobId);
}
