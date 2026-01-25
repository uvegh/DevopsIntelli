

using DevopsIntelli.Application.common.Interface;
using hangFireLib= Hangfire;
using System.Linq.Expressions;

namespace DevopsIntelli.Infrastructure.Background;

public class BackGroundService:IBackgroundService
{
    public    string EnqueueJob<T>( Expression <Func<T,Task>> methodCall)
    {
        return Hangfire.BackgroundJob.Enqueue(methodCall);
    }

    public string ScheduleJob<T>(Expression <Func<T, Task>> methodCall, TimeSpan delay)
    {
        return Hangfire.BackgroundJob.Schedule(methodCall, delay);
    }

    public void RecurringJob<T>(string jobId, Expression<Func<T, Task>> methodCall, string cronExpression)
    {
        // Create or update recurring job
        // Cron expression examples:
        // "0 2 * * *" = Daily at 2 AM
        // "*/5 * * * *" = Every 5 minutes
        // "0 0 * * 0" = Weekly on Sunday at midnight
        hangFireLib.RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);


    }

  
}
