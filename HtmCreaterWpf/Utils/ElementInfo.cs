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

        public string Path { get; set; }
        public bool IsAdd { get; set; }
        public ImageSource Image { get; set; }

        public ElementInfo(string path)
        {
            Path = path;
            IsAdd = false;
            Image = GetImage(Path);
        }

        private BitmapImage GetImage(string path)
        {
            byte[] data = File.ReadAllBytes(path);

            BitmapImage image = new BitmapImage();

            using (MemoryStream ms = new MemoryStream(data))
            {
                ms.Position = 0;

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
