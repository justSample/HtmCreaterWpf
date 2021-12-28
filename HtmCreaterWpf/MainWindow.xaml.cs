using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmCreaterWpf.Utils;

namespace HtmCreaterWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string TEMP_FOLDER_NAME = "TempImages"; // Нужно

        private string[] _pathImages;

        private string _tempDirPath;

        private ElementContainer _container;

        public MainWindow()
        {
            InitializeComponent();

            _tempDirPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), TEMP_FOLDER_NAME);

            this.Closing += (o, e) =>
            {
                if (Directory.Exists(_tempDirPath))
                {
                    string[] filesPaths = Directory.GetFiles(_tempDirPath);

                    for (int i = 0; i < filesPaths.Length; i++)
                    {
                        File.Delete(filesPaths[i]);
                    }

                    Directory.Delete(_tempDirPath);
                }
            };
            this.KeyDown += (o, e) =>
            {
                switch (e.Key)
                {
                    case Key.NumPad4:
                        SetElement(_container.PrevImage());
                        break;
                    case Key.NumPad6:
                        SetElement(_container.NextImage());
                        break;
                    case Key.NumPad5:
                        _container.CurrentElement.IsAdd = !_container.CurrentElement.IsAdd;
                        IsAddToWord.IsChecked = _container.CurrentElement.IsAdd;
                        break;
                }
            };
        }

        private void BtnLoadPdfImages_Click(object sender, RoutedEventArgs e)
        {
            LoadImagesFromPdf();
            ReadImageFiles();
        }

        private void LoadImagesFromPdf()
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {

                fd.Filter = "Pdf file (*.pdf) | *.pdf;"; ;

                var result = fd.ShowDialog();

                string pdfFilePath = fd.FileName;

                if(result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(pdfFilePath))
                {
                    if (Directory.Exists(_tempDirPath))
                    {
                        string[] filesPaths = Directory.GetFiles(_tempDirPath);

                        for (int i = 0; i < filesPaths.Length; i++)
                        {
                            File.Delete(filesPaths[i]);
                        }

                        Directory.Delete(_tempDirPath);
                    }

                    Directory.CreateDirectory(_tempDirPath);

                    new Pdf(pdfFilePath).CreateImages(_tempDirPath);
                    
                }

            }
        }

        private void ReadImageFiles()
        {
            _pathImages = Directory.GetFiles(_tempDirPath, "*.png", SearchOption.TopDirectoryOnly);
            _pathImages = _pathImages.OrderBy(p =>
            {
                return int.Parse(string.Join("", new FileInfo(p).Name.Where(ch => char.IsDigit(ch)).ToArray()));
            })
            .ToArray();

            _container = new ElementContainer(_pathImages);

            SetElement(_container.CurrentElement);
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            SetElement(_container.PrevImage());
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            SetElement(_container.NextImage());
        }

        private void SetElement(ElementInfo elInfo)
        {
            IsAddToWord.IsChecked = elInfo.IsAdd;
            ImgHolst.Source = elInfo.Image;
        }

        private void IsAddToWord_Click(object sender, RoutedEventArgs e)
        {
            _container.CurrentElement.IsAdd = !_container.CurrentElement.IsAdd;
        }

        private void BtnCreateHtm_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog browserDialog = new FolderBrowserDialog())
            {
                var result = browserDialog.ShowDialog();
                string selectedPath = browserDialog.SelectedPath;

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(selectedPath))
                {
                    string[] imgPaths = _container.Elements.Where(el => el.IsAdd).Select(p => p.Path).ToArray();

                    new HtmCreater(imgPaths, selectedPath, txtBoxHtmName.Text).CreateHtm();

                }
            }
        }
    }
}
