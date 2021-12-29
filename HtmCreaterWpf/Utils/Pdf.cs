using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace HtmCreaterWpf.Utils
{
    
    public class Pdf
    {
        private string _pdfPath;

        public Pdf(string pdfPath)
        {
            _pdfPath = pdfPath;
        }

        /// <summary>
        /// Создаёт изображения из PDF, где 1 страница = 1 изображению
        /// </summary>
        /// <param name="outputDirPath">Путь до папки, куда сохранять изображения</param>
        public void CreateImages(string outputDirPath)
        {
            using(var doc = PdfiumViewer.PdfDocument.Load(_pdfPath))
            {

                for (int i = 0; i < doc.PageCount; i++)
                {
                    Image image = doc.Render(i, doc.PageSizes[i].Width, doc.PageSizes[i].Height, true);

                    string savePath = System.IO.Path.Combine(outputDirPath, $"{i}.png");

                    System.Drawing.Imaging.Encoder myEncoder;
                    EncoderParameter myEncoderParameter;
                    EncoderParameters myEncoderParameters;
                    ImageCodecInfo myImageCodecInfo;

                    // Get an ImageCodecInfo object that represents the JPEG codec.
                    myImageCodecInfo = GetEncoderInfo("image/png");

                    // for the Quality parameter category.
                    myEncoder = System.Drawing.Imaging.Encoder.Quality;

                    myEncoderParameters = new EncoderParameters(1);

                    myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    image.Save(savePath, myImageCodecInfo, myEncoderParameters);

                }
               
            }
        }

        /// <summary>
        /// Получает тип кодирования изображения
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}
