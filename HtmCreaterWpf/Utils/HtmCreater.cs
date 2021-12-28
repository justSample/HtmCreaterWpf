using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HtmCreaterWpf.Utils
{
    public class HtmCreater
    {

        private string[] _pathImages;
        private string _outputDir;
        private string _fileName;

        public HtmCreater(string[] pathImages, string outputDir, string fileName)
        {
            _pathImages = pathImages;
            _outputDir = outputDir;
            _fileName = fileName;
        }

        public void CreateHtm()
        {
            string filesDir = Path.Combine(_outputDir, ($"{_fileName}.files"));

            Directory.CreateDirectory(filesDir);

            HtmlDocument doc = new HtmlDocument();
            
            HtmlNode html = doc.DocumentNode.AppendChild(doc.CreateElement("html"));
            HtmlNode head = html.AppendChild(doc.CreateElement("head"));
            HtmlNode meta = head.AppendChild(doc.CreateElement("meta"));
            meta.Attributes.Add("charset", "windows-1251");
            HtmlNode body = html.AppendChild(doc.CreateElement("body"));

            for (int i = 0; i < _pathImages.Length; i++)
            {
                string filesDirPathImage = Path.Combine(filesDir, $"{i + 1}.png");
                File.Copy(_pathImages[i], filesDirPathImage, true);
                HtmlNode img = doc.CreateElement("img");

                img.Attributes.Add("style", "display: block; margin: 5px auto;");
                img.Attributes.Add("src", $"{new DirectoryInfo(filesDir).Name}/{new FileInfo(filesDirPathImage).Name}");

                body.AppendChild(img);
            }

            doc.Save(Path.Combine(_outputDir, $"{_fileName}.htm"));

        }


    }
}
