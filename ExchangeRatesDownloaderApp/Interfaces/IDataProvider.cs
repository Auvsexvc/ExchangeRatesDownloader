﻿using ExchangeRatesDownloaderApp.Models;

namespace ExchangeRatesDownloaderApp.Interfaces
{
    public interface IDataProvider
    {
        string[] NbpTablesBidAsk { get; }
        string[] NbpTablesMid { get; }

        Task<IEnumerable<ExchangeTableDto>> GetTablesAsync();
    }
}