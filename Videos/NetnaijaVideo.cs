
using System.Linq;
using System.Net.Http.Headers;
using Supremes.Nodes;

namespace Flixr
{

    public class NetnaijaMovieDetail
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string SubtitleLink => Link + "/download-sub";
        public string DownloadLink => Link + "/download";
        public string? youtubeTrailer { get; set; }

        public string? ImdbLink { get; set; }

        public List<NetnaijaVideo> youMayLike { get; set; }
        public List<Category> categories { get; set; }

        public NetnaijaMovieDetail(Element e, Elements r, string Url)
        {
            var _url = new Uri(Url);
            var _BaseUrl = _url.GetLeftPart(UriPartial.Authority);
            this.Name = e.Select("h1.post-title").Text.Trim().Split("(", 2).First().Trim();
            this.Image = _BaseUrl + e.Select("img.wp-post-image").First.Attr("src");
            this.Link = Url;
            this.Description = e.Select("div.entry-content").First.Select("p").First.Text;
            this.youtubeTrailer = "https" + e.Select("iframe").Last.Attr("src");
            this.youMayLike = new List<NetnaijaVideo>();
            this.categories = new List<Category>();

            foreach (var video in r)
            {
                var item = NetnaijaVideo.createFromData(
                    Image: _BaseUrl + video.Select("img").First.Attr("src"),
                    Link: _BaseUrl + video.Select("a").First.Attr("href"),
                    Name: video.Select("h3.post-title").First.Select("a").Text.Trim()
                );
                if (item != null)
                    youMayLike.Add(item);

            }


            Elements cats = e.Select("div.entry-header").First.Select("a.post-cat");

            foreach (var cat in cats)
            {
                var url = new Uri(Url);
                var _catUrl = url.GetLeftPart(UriPartial.Authority) + cat.Attr("href");
                this.categories.Add(new Category(cat.Text, _catUrl));
            }

            try
            {
                this.ImdbLink = e.Select("blockquote.quote-content").Select("p").Last.Select("a").First.Attr("href");
            }
            catch
            {

            }
            // this.Id = Base64Encode(e.GetElementsByAttribute("href").First.Attr("href"));
        }
    }
    public class NetnaijaVideo
    {

        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }
        public string? Id { get; set; }

        public bool isSeries => this.Link?.Contains("series.") ?? false;

        public bool isMovie => !this.isSeries;

        private NetnaijaVideo(Element e, string SourceUrl)
        {
            this.Name = e.Select("h2.post-title").First.Select("a").Text.Trim().Split("(", 2).First().Trim();
            this.Image = SourceUrl + e.Select("img").First.Attr("src");
            this.Link = SourceUrl + e.Select("a").First.Attr("href");
            this.Id = Base64Encode(this.Link);
        }

        private NetnaijaVideo(string Name, string Image, string Link)
        {
            this.Name = Name.Split("(", 2).First().Trim();
            this.Image = Image;
            this.Link = Link;
            this.Id = Base64Encode(Link);

        }

        public static NetnaijaVideo? create(Element e, string SourceUrl)
        {

            string Name = e.Select("h2.post-title").First.Select("a").Text.Trim();
            string Image = Netnaija.VideoUrl + e.Select("img").First.Attr("src");
            string Link = Netnaija.VideoUrl + e.Select("a").First.Attr("href");

            if (Image == null)
            {
                return null;
            }
            if (Image!.Trim() == "")
            {
                return null;
            }

            return new NetnaijaVideo(e, SourceUrl);

        }

        public static NetnaijaVideo? createFromData(

            string Image, string Link, string Name
        )
        {


            if (Image == null)
            {
                return null;
            }
            if (Image!.Trim() == "")
            {
                return null;
            }

            return new NetnaijaVideo(
                Name, Image, Link
            );

        }

        public static NetnaijaVideo? createFromSearchResult(Element e)
        {
            // String title = e.getElementsByAttribute("href").first().text().split(":", 2)[1].trim();
            string? Name = null;
            string? Image = null;
            string? Link = null;
            try
            {
                Name = e.GetElementsByAttribute("href").First.Text;
                Image = e.Select("img").First.Attr("src");
                Link = e.GetElementsByAttribute("href").First.Attr("href");

            }
            catch
            {
                return null;
            }

            if (Image == null)
            {
                return null;
            }
            if (Image!.Trim() == "")
            {
                return null;
            }

            if (!Name.ToLower().StartsWith("movie:") && !Name.ToLower().StartsWith("series:"))
            {

                return null;

            }

            Name = Name.Split(":", 2).Last().Trim();

            // string Id = Base64Encode(Link!);

            return new NetnaijaVideo(Name, Image, Link);

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }



    }
}

public class Category
{

    public string Name { get; set; }
    public string Link { get; set; }

    //constructor

    public Category(string Name, string Link)
    {
        this.Name = Name;
        this.Link = Link;
    }

}