namespace TestCopilot.App.Clients
{
    public interface ICountClient
    {
        public Task<int> Next(int number);
    }

    public class CountClient : ICountClient
    {
        private readonly string _baseUrl;

        public CountClient(IConfiguration configuration)
        {
            _baseUrl = configuration["TestCopilotApi"] ?? "";
        }

        public async Task<int> Next(int number)
        {
            using var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_baseUrl)
            };
            var forecast = await httpClient.GetFromJsonAsync<int>($"Count/Next/{number}");
            return forecast;
        }
    }
}
