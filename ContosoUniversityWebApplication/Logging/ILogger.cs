using System;

namespace ContosoUniversityWebApplication.Logging
{
    //The interface provides three tracing levels to indicate the relative importance of logs, 
    //and one designed to provide latency information for external service calls such as database queries.

    public interface ILogger
    {

        void Information(string message);
        void Information(string fmt, params object[] vars);
        void Information(Exception exception, string fmt, params object[] vars);
        void Warning(string message);
        void Warning(string fmt, params object[] vars);
        void Warning(Exception exception, string fmt, params object[] vars);
        void Error(string message);
        void Error(string fmt, params object[] vars);
        void Error(Exception exception, string fmt, params object[] vars);

        //The TraceApi methods enable you to track the latency of each call to an external service such as SQL Database.
        void TraceApi(string componentName, string method, TimeSpan timespan);
        void TraceApi(string componentName, string method, TimeSpan timespan, string properties);
        void TraceApi(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars);


    }
}