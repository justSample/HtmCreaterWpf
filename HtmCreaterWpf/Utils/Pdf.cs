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
        private string[] _pdfPaths;

        public Pdf(string pdfPath)
        {
            _pdfPath = pdfPath;
        }

        public Pdf(string[] pdfPaths)
        {
            _pdfPaths = pdfPaths;
        }

        /// <summary>
        /// Создаёт изображения из PDF, где 1 страница = 1 изображению
        /// </summary>
        /// <param name="outputDirPath">Путь до папки, куда сохранять изображения</param>
        public void CreateImages(string outputDirPath)
        {
            if (_pdfPaths == null)
                CreatePathsSingle(outputDirPath);
            else
                CreatePathsMulti(outputDirPath);
        }

        private void CreatePathsSingle(string outputDirPath)
        {
            using (var doc = PdfiumViewer.PdfDocument.Load(_pdfPath))
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

        private void CreatePathsMulti(string outputDirPath)
        {
            int indexCountImage = 1;

            for (int i = 0; i < _pdfPaths.Length; i++)
            {
                using (var doc = PdfiumViewer.PdfDocument.Load(_pdfPaths[i]))
                {
                    for (int j = 0; j < doc.PageCount; j++)
                    {
                        Image image = doc.Render(j, doc.PageSizes[j].Width, doc.PageSizes[j].Height, true);

                        string savePath = System.IO.Path.Combine(outputDirPath, $"{indexCountImage}.png");

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
                        indexCountImage++;
                    }
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
