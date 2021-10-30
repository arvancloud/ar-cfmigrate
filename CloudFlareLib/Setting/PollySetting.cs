using System;

namespace CloudFlareLib.Setting
{
    public static class PollySetting
    {
        public static readonly TimeSpan[] RetrySetting = {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2)
        };
    }
}