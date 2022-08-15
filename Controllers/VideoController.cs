using System.Collections.Specialized;
using Supremes;
using Supremes.Nodes;
using System.Text.Json;

namespace Flixr
{

    public class VideoController
    {

        static int index = 3;

        public static async Task<string> getMovies(string? query = null)
        {

            if (query != null)
            {
                throw new NotImplementedException("The query string has not been implemented");
            }

            var httpClient = new MyHttpClient(Netnaija.VideoUrl);

            string rawHtml = await httpClient.Get();

            ListDictionary response = new ListDictionary();

            List<NetnaijaVideo> videoList = new List<NetnaijaVideo>();

            Document doc = Dcsoup.Parse(rawHtml);
            Elements videoFiles = doc.Select("div.video-files").First.Select("article");
            for (int i = 0; i < videoFiles.Count; i++)
            {
                var e = NetnaijaVideo.create(videoFiles[i]);
                if (e != null) videoList.Add(e);
            }


            response.Add("movies", videoList);
            response.Add("header", videoList[index]);

            var json = JsonSerializer.Serialize(response);

            return (json);

        }

        public static async Task<string> getSeries(string? query = null)
        {

            if (query != null)
            {
                throw new NotImplementedException("The query string has not been implemented");
            }

            var httpClient = new MyHttpClient(Netnaija.SeriesUrl);

            ListDictionary response = new ListDictionary();

            string rawHtml = await httpClient.Get();

            List<NetnaijaVideo> videoList = new List<NetnaijaVideo>();

            Document doc = Dcsoup.Parse(rawHtml);
            Elements videoFiles = doc.Select("div.video-files").First.Select("article");
            for (int i = 0; i < videoFiles.Count; i++)
            {
                var e = NetnaijaVideo.create(videoFiles[i]);
                if (e != null) videoList.Add(e);
            }


            response.Add("series", videoList);
            response.Add("header", videoList[index]);

            var json = JsonSerializer.Serialize(response);


            return (json);

        }

    }
}