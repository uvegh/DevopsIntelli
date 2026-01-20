using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DevopsIntelli.Application.common.Interface;

public interface IBackgroundService
{
    string EnqueueJob<T>(Expression<Func<T, Task>> methodCall);
    //run after a delay

    //expression<Func> -adds to expression tree to not call func immediately
    //<T, Task>- param type= job class and async return type
    string ScheduleJob<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);
    //Run on a schedule -this is basically a cronjob
    void RecurringJob<T>( string  jobId,Expression<Func<T, Task>> methodCall, string cronExpression);
}
