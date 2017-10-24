using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardScoring.Logging
{
    //TODO make this a more robust wrapper
    public static class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogInfo(object message)
        {
            log.Info(message);
        }
        public static void LogInfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public static void LogInfo(object message, Exception ex)
        {
            log.Info(message, ex);
        }

        public static void LogError(object message)
        {
            log.Error(message);
        }
        public static void LogErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public static void LogError(object message, Exception ex)
        {
            log.Error(message, ex);
        }
    }
}
