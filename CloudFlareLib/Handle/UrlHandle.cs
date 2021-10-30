namespace CloudFlareLib.Handle
{
    public static class UrlHandle
    {
        public static string ConcatBaseUrlWithParams(string baseAddress, string[] parameters)
        {
            return AddSlashToEnd(AddSlashToEnd(baseAddress) + string.Concat("/", parameters));
        }

        public static string ConcatBaseUrlWithParam(string baseAddress, string parameters)
        {
            return AddSlashToEnd(AddSlashToEnd(baseAddress) + parameters);
        }

        private static string AddSlashToEnd(string url)
        {
            if (url.EndsWith("/"))
            {
                return url;
            }

            return url + "/";
        }
    }
}