namespace ExchangeRatesDownloaderApp.Helper
{
    public static class ControllerMessages
    {
        public const string MSG_DBWRITESQLFAIL = "Writing to db failed. - {0}";
        public const string MSG_DBWRITEHTTPFAIL = "Writing to db failed.Reading data from remote API not possible - {0}";
        public const string MSG_GETDATAFAIL = "Data cannot be shown - {0}";
        public const string MSG_GETDATASUCCESS = "Db successfully updated";
    }
}
