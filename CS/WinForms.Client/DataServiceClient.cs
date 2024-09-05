using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace WinForms.Client
{
    public static class DataServiceClient
    {
        static DataServiceClient()
        {
            string? url = System.Configuration.ConfigurationManager.AppSettings["baseUrl"];
            if (string.IsNullOrEmpty(url))
                throw new InvalidOperationException("The 'baseUrl' configuration setting is missing.");
            baseUrl = url;
        }

        static string baseUrl;

        static HttpClient CreateClient()
        {
            return new HttpClient();
        }

        public static async Task<DataFetchResult?> GetOrderItemsAsync(int skip, int take, string sortField, bool sortAscending)
        {
            using var client = CreateClient();
            var response = await client.GetAsync(
                $"{baseUrl}/data/OrderItems?skip={skip}&take={take}&sortField={sortField}&sortAscending={sortAscending}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var dataFetchResult = JsonSerializer.Deserialize<DataFetchResult>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return dataFetchResult;
        }

        static OrderItem? AsOrderItem(this string responseBody)
        {
            return JsonSerializer.Deserialize<OrderItem>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public static async Task<OrderItem?> GetOrderItemAsync(int id)
        {
            using var client = CreateClient();
            var response = await client.GetAsync($"{baseUrl}/data/OrderItem/{id}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody.AsOrderItem();
        }

        public static async Task<OrderItem?> CreateOrderItemAsync(OrderItem orderItem)
        {
            using var client = CreateClient();
            var response = await client.PostAsync($"{baseUrl}/data/OrderItem",
                new StringContent(JsonSerializer.Serialize(orderItem), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody.AsOrderItem();
        }

        public static async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            using var client = CreateClient();
            var response = await client.PutAsync($"{baseUrl}/data/OrderItem/{orderItem.Id}",
                new StringContent(JsonSerializer.Serialize(orderItem), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
        }

        public static async Task<bool> DeleteOrderItemAsync(int id)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.DeleteAsync($"{baseUrl}/data/OrderItem/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
