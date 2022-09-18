
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

        public NetnaijaMovieDetail(Element e, Elements r, string Url)
        {
            this.Name = e.Select("img").First.Attr("title");
            this.Image = e.Select("img").First.Attr("src");
            this.Link = Url;
            this.Description = e.Select("p")[0].Text + " " + e.Select("p")[1].Text;
            this.youtubeTrailer = e.Select("div.video-player").First.Select("iframe").First.Attr("src");

            this.youMayLike = new List<NetnaijaVideo>();

            foreach (var video in r)
            {
                var item = NetnaijaVideo.create(video);
                if (item != null)
                    youMayLike.Add(item);

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

        public bool isSeries => this.Link?.Contains("videos/series") ?? false;

        public bool isMovie => this.Link?.Contains("videos/movies") ?? false;

        private NetnaijaVideo(Element e)
        {
            this.Name = e.Select("img").First.Attr("title");
            this.Image = e.Select("img").First.Attr("src");
            this.Link = e.GetElementsByAttribute("href").First.Attr("href");
            this.Id = Base64Encode(e.GetElementsByAttribute("href").First.Attr("href"));
        }

        public static NetnaijaVideo? create(Element e)
        {

            string Name = e.Select("img").First.Attr("title");
            string Image = e.Select("img").First.Attr("src");
            string Link = e.GetElementsByAttribute("href").First.Attr("href");

            if (Image == null)
            {
                return null;
            }
            if (Image!.Trim() == "")
            {
                return null;
            }

            return new NetnaijaVideo(e);

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }



    }
}