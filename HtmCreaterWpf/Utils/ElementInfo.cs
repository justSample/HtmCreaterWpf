using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HtmCreaterWpf.Utils
{
    public class ElementInfo
    {
        /// <summary>
        /// Путь до изображения
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Добавлять ли в файл
        /// </summary>
        public bool IsAdd { get; set; }
        /// <summary>
        /// Изобраение из PDF
        /// </summary>
        public ImageSource Image { get; set; }

        /// <summary>
        /// Конструктор :)
        /// </summary>
        /// <param name="path">Путь до изображения</param>
        public ElementInfo(string path)
        {
            Path = path;
            IsAdd = false;
            Image = GetImage(Path);
        }

        /// <summary>
        /// Возвращает изображение которое мы передаём по пути
        /// </summary>
        /// <param name="path">Путь до изображения</param>
        /// <returns></returns>
        private BitmapImage GetImage(string path)
        {
            byte[] data = File.ReadAllBytes(path);

            BitmapImage image = new BitmapImage();

            using (MemoryStream ms = new MemoryStream(data))
            {
                ms.Position = 0; //На всякий случай ставим позицию на 0

                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = ms;
                image.EndInit();
            }

            return image;
        }

    }
}
