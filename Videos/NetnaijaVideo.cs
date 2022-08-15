
using Supremes.Nodes;

namespace Flixr
{
    public class NetnaijaVideo
    {

        public string? Name  { get; set; }
        public string? Image  { get; set; }
        public string? Link  { get; set; }

        private NetnaijaVideo(Element e)
        {
            this.Name = e.Select("img").First.Attr("title");
            this.Image = e.Select("img").First.Attr("src");
            this.Link = e.GetElementsByAttribute("href").First.Attr("href");
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

    }
}