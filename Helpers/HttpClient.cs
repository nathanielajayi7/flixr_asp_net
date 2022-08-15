// using System.Net;

namespace Flixr
{
    public class MyHttpClient
    {

        string url;

        public MyHttpClient(string url)
        {
            this.url = url;
        }

        public async Task<string> Get()
        {
            //  string html = string.Empty;
            string url = this.url;
            var myClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var response = await myClient.GetAsync(url);
            var streamResponse = await response.Content.ReadAsStringAsync();
            return (streamResponse);
        }
    }
}