﻿namespace ArvanCloudLib
{
    public static class ArvanTokenHandler
    {
        private static string _token;

        public static string GetToken()
        {
            return _token;
        }

        public static void SetToken(string token)
        {
            _token = token;
        }
        
        public static string GetCompleteToken()
        {
            return BaseArvanValue.TokenType + " " + GetToken();
        }
    }
}