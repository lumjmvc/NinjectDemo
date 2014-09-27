using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NinjectWithEF.WebUI.Filters
{
    public enum LogSeverityLevel
    {
        Warning,
        Information
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class LogAttribute : FilterAttribute
    {
        // public members of LogAttribute 
        public LogSeverityLevel Level { get; set; }

        // By passing in the Level as a parameter in the constructor, a value for level MUST BE assigned 
        // when using the LogAttribute attribute on an action or controller 
        // like [Log(level:LogSeverityLevel.Warning)]
        public LogAttribute(LogSeverityLevel level)
        {
            this.Level = Level;
        }
    }
}