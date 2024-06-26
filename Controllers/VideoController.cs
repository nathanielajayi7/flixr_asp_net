// using System.Web;
// using System.Linq;
// using System.Net.Http;
using System.Collections.Specialized;
using Supremes;
using Supremes.Nodes;
using System.Text.Json;

using Newtonsoft.Json;
namespace Flixr
{


    class JsonUtil
    {
        public static string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }
    }

    public class VideoController
    {

        static int index = 3;


        public static async Task<string> searchResult(HttpContext context)
        {

            var location = ($"{context.Request.QueryString}");



            string? param = null;



            param = location.Split("?", 2).Last().Split("&").Last().Trim();


            var httpClient = new MyHttpClient(Netnaija.SearchUrl + (param != "" ?
            param!.Split("=").Last()
            :
            ""
            ))
            ;


            string rawHtml = await httpClient.Get();

            ListDictionary response = new ListDictionary();

            List<NetnaijaVideo> videoList = new List<NetnaijaVideo>();

            Document doc = Dcsoup.Parse(rawHtml);
            Elements videoFiles = doc.Select("div.search-results").First.Select("article");
            for (int i = 0; i < videoFiles.Count; i++)
            {
                var e = NetnaijaVideo.createFromSearchResult(videoFiles[i]);
                if (e != null) videoList.Add(e);
            }


            response.Add("result", videoList);
            var json = System.Text.Json.JsonSerializer.Serialize(response);

            return (
                JsonUtil.JsonPrettify(
                json));
        }

        public static async Task<string> getMovies(HttpContext context, string? query = null)
        {


            if (query != null)
            {
                throw new NotImplementedException("The query string has not been implemented");
            }
            var location = ($"{context.Request.QueryString}");



            string? param = null;



            param = location.Split("?", 2).Last().Split("&").Last().Trim();

            string? page = param?.Split("=").Last();


            var httpClient = new MyHttpClient(Netnaija.VideoUrl + (param != "" ?
            $"/page/{page}"
            :
            "")
            )
            ;



            string rawHtml = await httpClient.Get();
            // Console.WriteLine(rawHtml);

            ListDictionary response = new ListDictionary();

            List<NetnaijaVideo> videoList = new List<NetnaijaVideo>();

            Document doc = Dcsoup.Parse(rawHtml);
            Elements videoFiles = doc.Select("li[class=\"post-item tie-standard\"]");

            //print
            // Console.WriteLine(videoFiles.Count);
            //print all videos
            // Console.WriteLine(videoFiles);

            for (int i = 0; i < videoFiles.Count; i++)
            {
                //print
                Console.WriteLine(videoFiles[i]);
                var e = NetnaijaVideo.create(videoFiles[i], Netnaija.VideoUrl);
                if (e != null) videoList.Add(e);
            }


            response.Add("movies", videoList);
            response.Add("page",  (page == null || page == "") ? 1 : int.Parse(page));
            response.Add("header", videoList[index]);


            var json = System.Text.Json.JsonSerializer.Serialize(response);

            return (
                JsonUtil.JsonPrettify(
                json));

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
                var e = NetnaijaVideo.create(videoFiles[i], Netnaija.VideoUrl);
                if (e != null) videoList.Add(e);
            }


            response.Add("movies", videoList);
            // response.Add("header", videoList[index]);


            var json = System.Text.Json.JsonSerializer.Serialize(response);

            return (
                JsonUtil.JsonPrettify(
                json));

        }

        public static async Task<string> getMovieById(string base64)
        {



            string currentUrl = "";


            try
            {
                currentUrl = Base64Decode(base64);
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
            Element videoInfo = doc.Select("article[id=\"the-post\"]").First;
            Elements relatedInfo = doc.Select("div[id=\"related-posts\"]").First.Select("div.related-posts-list").First.Select("div.related-item");
            response.Add(
                "data",
            new NetnaijaMovieDetail(
                videoInfo,
                relatedInfo,
                currentUrl
            )
            );


            var json = System.Text.Json.JsonSerializer.Serialize(response);

            return (
                JsonUtil.JsonPrettify(
                json));





        }




        public static async Task<string> getSeries(HttpContext context, string? query = null)
        {

            if (query != null)
            {
                throw new NotImplementedException("The query string has not been implemented");
            }
            var location = ($"{context.Request.QueryString}");



            string? param = null;


            param = location.Split("?", 2).Last().Split("&").Last().Trim();

            string? page = param?.Split("=").Last();

            var httpClient = new MyHttpClient(Netnaija.SeriesUrl + (param != "" ?
            $"/page/{page}"
            :
            "")
            )
            ;

            ListDictionary response = new ListDictionary();

            string rawHtml = await httpClient.Get();

            List<NetnaijaVideo> videoList = new List<NetnaijaVideo>();

            Document doc = Dcsoup.Parse(rawHtml);
            Elements videoFiles = doc.Select("li[class=\"post-item tie-standard\"]");

            for (int i = 0; i < videoFiles.Count; i++)
            {
                var e = NetnaijaVideo.create(videoFiles[i], Netnaija.SeriesUrl);
                if (e != null) videoList.Add(e);
            }


            response.Add("series", videoList);
            response.Add("page", (page == null || page == "") ? 1 : int.Parse(page));
            response.Add("header", videoList[index]);


            var json = System.Text.Json.JsonSerializer.Serialize(response);

            return (
                JsonUtil.JsonPrettify(
                json));
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }


}