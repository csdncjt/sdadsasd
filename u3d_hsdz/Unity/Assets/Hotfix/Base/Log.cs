// #define ENABLE_HOTFIX_LOG
using System;

namespace ETHotfix
{
    public static class Log
    {
#if ENABLE_HOTFIX_LOG
        private const bool enableLog = true;
#else
        private const bool enableLog = false;
#endif

        public static void Warning(string msg)
        {
            if (enableLog)
                ETModel.Log.Warning(msg);
        }

        public static void Info(string msg)
        {
            if (enableLog)
                ETModel.Log.Info(msg);
        }

        public static void Error(Exception e)
        {
            if (enableLog)
                ETModel.Log.Error(e.ToStr());
        }

        public static void Error(string msg)
        {
            if (enableLog)
                ETModel.Log.Error(msg);
        }

        public static void Debug(string msg)
        {
            if (enableLog)
                ETModel.Log.Debug(msg);
        }

        public static void Msg(object msg)
        {
            if (enableLog)
                Debug(Dumper.DumpAsString(msg));
        }

        // internal static void Debug(PieChart vpipChart)
        // {
        //     throw new NotImplementedException();
        // }
    }
}