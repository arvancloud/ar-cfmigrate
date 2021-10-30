using System;

namespace CfMigrate.Setting
{
    public static class PollySetting
    {
        public static readonly TimeSpan[] RetrySetting = new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2)
        };
    }
}