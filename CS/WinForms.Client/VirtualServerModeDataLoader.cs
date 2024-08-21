using DevExpress.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace WinForms.Client
{
    public class DataFetchResult
    {
        public List<OrderItem> Items { get; set; } = null!;
        public int TotalCount { get; set; }
    }

    // This type is instantiated each time a configuration change in the virtual server
    // mode source takes place. It receives the configuration information and handles
    // the parts of it that are relevant to the data loading process.
    public class VirtualServerModeDataLoader
    {
        public VirtualServerModeDataLoader(VirtualServerModeConfigurationInfo configurationInfo)
        {
            // For instance, let's assume the backend supports sorting for just one field
            if (configurationInfo.SortInfo?.Length > 0)
            {
                SortField = configurationInfo.SortInfo[0].SortPropertyName;
                SortAscending = !configurationInfo.SortInfo[0].IsDesc;
            }
        }

        public int BatchSize { get; set; } = 40;
        public string SortField { get; set; } = "Id";
        public bool SortAscending { get; set; } = true;

        public Task<VirtualServerModeRowsTaskResult> GetRowsAsync(VirtualServerModeRowsEventArgs e)
        {
            return Task.Run(async () =>
            {
                Debug.WriteLine($"Fetching data rows {e.CurrentRowCount} to {e.CurrentRowCount + BatchSize}, sorting by {SortField} ({(SortAscending ? "asc" : "desc")})");
                using var client = new HttpClient();
                var response = await client.GetAsync(
                    $"{System.Configuration.ConfigurationManager.AppSettings["baseUrl"]}/data/OrderItems?skip={e.CurrentRowCount}&take={BatchSize}&sortField={SortField}&sortAscending={SortAscending}");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var dataFetchResult = JsonSerializer.Deserialize<DataFetchResult>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dataFetchResult is null)
                    return new VirtualServerModeRowsTaskResult();

                var moreRowsAvailable = e.CurrentRowCount + dataFetchResult.Items.Count < dataFetchResult.TotalCount;
                Debug.WriteLine($"Returning {dataFetchResult.Items.Count} items, more rows available: {moreRowsAvailable}");
                return new VirtualServerModeRowsTaskResult(dataFetchResult.Items, moreRowsAvailable);
            }, e.CancellationToken);
        }
    }
}
