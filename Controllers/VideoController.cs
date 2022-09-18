using System.Web;
using System.Linq;
using System.Net.Http;
using System.Collections.Specialized;
using Supremes;
using Supremes.Nodes;
using System.Text.Json;
namespace Flixr
{

    public class VideoController
    {

        static int index = 3;


        public static async Task<string> searchResult(HttpContext context)
        {
            return "";
        }

        public static async Task<string> getMovies(HttpContext context, string? query = null)
        {


            if (query != null)
            {
                throw new NotImplementedException("The query string has not been implemented");
            }

            var param = HttpUtility.ParseQueryString(context.Request.Path).Get("page");

            var httpClient = new MyHttpClient(Netnaija.VideoUrl + (param == null ?
            $"/page/{param!}"
            :
            "")
            )
            ;

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

        public static async Task<string> getPopularMovies(string? query = null)
        {

            if (query != null)
            {
                throw new NotImplementedException("The query string has not been implemented");
            }

            var httpClient = new MyHttpClient(Netnaija.PopularMoviesUrl);

            string rawHtml = await httpClient.Get();

            ListDictionary response = new ListDictionary();

            List<NetnaijaVideo> videoList = new List<NetnaijaVideo>();

            Document doc = Dcsoup.Parse(rawHtml);
            Elements videoFiles = doc.Select("div.trending-list").First.Select("article");
            for (int i = 0; i < videoFiles.Count; i++)
            {
                var e = NetnaijaVideo.create(videoFiles[i]);
                if (e != null) videoList.Add(e);
            }


            response.Add("movies", videoList);
            // response.Add("header", videoList[index]);

            var json = JsonSerializer.Serialize(response);

            return (json);

        }

        public static async Task<string> getMovieById(string base64)
        {



            string currentUrl = "";


            try
            {
                currentUrl = Base64Decode(base64.Split("movies/").Last());
            }
            catch
            {
                throw new Exception("cannot decode movie id");
            }

            Console.WriteLine(currentUrl);

            var httpClient = new MyHttpClient(currentUrl);

            ListDictionary response = new ListDictionary();

            string rawHtml = await httpClient.Get();

            Document doc = Dcsoup.Parse(rawHtml);
            Element videoInfo = doc.Select("article.post-body").First;
            Elements relatedInfo = doc.Select("div.related-posts").First.Select("div.rp-list").First.Select("article.rp-one");
            response.Add(
                "data",
            new NetnaijaMovieDetail(
                videoInfo,
                relatedInfo,
                currentUrl

            )
            );

            var json = JsonSerializer.Serialize(response);


            return (json);





        }




        public static async Task<string> getSeries(HttpContext context, string? query = null)
        {

            if (query != null)
            {
                throw new NotImplementedException("The query string has not been implemented");
            }

            var param = HttpUtility.ParseQueryString(context.Request.Path).Get("page");

            var httpClient = new MyHttpClient(Netnaija.SeriesUrl + (param == null ?
            $"/page/{param!}"
            :
            ""
            ));

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

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }


}