using System;

namespace ArvanCloudLib.Setting
{
    public static class PollySetting
    {
        public static readonly TimeSpan[] RetrySetting = {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2)
        };
    }
}