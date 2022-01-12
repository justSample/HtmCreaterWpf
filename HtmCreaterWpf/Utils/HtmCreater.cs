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

        /// <summary>
        /// </summary>
        /// <param name="pathImages">Массив путей до всех файлов</param>
        /// <param name="outputDir">Путь до папки, куда всё будет сохраняться</param>
        /// <param name="fileName">Наименование файла</param>
        public HtmCreater(string[] pathImages, string outputDir, string fileName)
        {
            _pathImages = pathImages;
            _outputDir = outputDir;
            _fileName = fileName;
        }

        /// <summary>
        /// Создаёт HTM файл по заданным в конструкторе путям
        /// </summary>
        public void CreateHtm()
        {
            string filesDir = Path.Combine(_outputDir, ($"{_fileName}.files")); //Получаем путь до папки, где будут хранится файлы

            Directory.CreateDirectory(filesDir); //Создаём папку (Её точно быть не может)

            HtmlDocument doc = new HtmlDocument();
            
            //Ниже создаётся структура

            HtmlNode html = doc.DocumentNode.AppendChild(doc.CreateElement("html"));
            HtmlNode head = html.AppendChild(doc.CreateElement("head"));
            HtmlNode meta = head.AppendChild(doc.CreateElement("meta"));
            meta.Attributes.Add("http-equiv", "Content-Type");
            meta.Attributes.Add("content", "text/html; charset=utf-8");
            HtmlNode body = html.AppendChild(doc.CreateElement("body"));

            for (int i = 0; i < _pathImages.Length; i++)
            {
                string filesDirPathImage = Path.Combine(filesDir, $"{i + 1}.png"); //Получаем будущий путь до изображения в .files
                File.Copy(_pathImages[i], filesDirPathImage, true); //Копируем изображение от туда, где сохранили и сохраняем по пути выше
                HtmlNode img = doc.CreateElement("img"); 

                img.Attributes.Add("style", "display: block; margin: 5px auto;"); //Центрирование изображения
                img.Attributes.Add("src", $"{new DirectoryInfo(filesDir).Name}/{new FileInfo(filesDirPathImage).Name}"); //Задаём путь относительно того, где он находится, чтоб запускался нормально у всех

                body.AppendChild(img);
            }

            doc.Save(Path.Combine(_outputDir, $"{_fileName}.htm"), Encoding.UTF8); //Сохраняем по пути до вывода с расширением htm

        }


    }
}
