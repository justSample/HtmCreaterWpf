using HtmCreaterWpf.Utils;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace HtmCreaterWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string TEMP_FOLDER_NAME = "TempImages";

        private const string TEXT_PREVIEW = "Письмо №_-ИФ_09 от";

        private string[] _pathImages;

        private string _tempDirPath;

        private ElementContainer _container;

        private bool isFirstTime = true; //Нужен для того, чтоб в первый раз до загрузки ничего не сломалось

        public MainWindow()
        {
            InitializeComponent();

            _tempDirPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), TEMP_FOLDER_NAME);

            this.Closing += (o, e) => CheckAndDeleteTempDir();

            txtBoxHtmName.Text = $"{BuilderInfo.GetCurrentYearAndQuarter()} {TEXT_PREVIEW} {DateTime.Now.ToString("dd.MM.yyyy")}(текст)";
        }

        private void BtnLoadPdfImages_Click(object sender, RoutedEventArgs e)
        {
            CreateImagesFromPdf();
            LoadImageFromFiles();

            if (isFirstTime)
            {
                BtnPrev.Click += BtnPrev_Click;
                BtnRight.Click += BtnRight_Click;
                IsAddToWord.IsEnabled = true;
                IsAddToWord.Click += IsAddToWord_Click;
                isFirstTime = false;

                this.KeyDown += (o, keyE) =>
                {
                    switch (keyE.Key)
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

            ShowMessage("Хорошее сообщение", "Импорт изображений из PDF завершен", MessageBoxIcon.Information);
        }

        /// <summary>
        /// Создание и загрузка изображений из PDF
        /// </summary>
        private void CreateImagesFromPdf()
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Pdf file (*.pdf) | *.pdf;"; ;

                var result = fd.ShowDialog();

                string pdfFilePath = fd.FileName;

                if(result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(pdfFilePath))
                {
                    CheckAndDeleteTempDir();

                    Directory.CreateDirectory(_tempDirPath);

                    new Pdf(pdfFilePath).CreateImages(_tempDirPath);
                    
                }

            }
        }

        /// <summary>
        /// Загрузка сохранённых изображений из PDF
        /// </summary>
        private void LoadImageFromFiles()
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

        /// <summary>
        /// Ставит информацию элемента
        /// </summary>
        /// <param name="elInfo">Элемент, из которого требуется поставить информацию</param>
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
            if (_container == null)
            {
                ShowMessage("Плохое сообщение", "Создание документа невозможно. Нету фотографий", MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBoxHtmName.Text))
            {
                ShowMessage("Плохое сообщение", "Наименование пустое", MessageBoxIcon.Error);
                return;
            }

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

            ShowMessage("Хорошее сообщение", "Создание документа завершено", MessageBoxIcon.Information);
        }

        private void ShowMessage(string title, string caption, MessageBoxIcon icon)
        {
            System.Windows.Forms.MessageBox.Show(caption, title, MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Проверяет наличие и удаляет временную папку с изображениями если таковая имеется
        /// </summary>
        private void CheckAndDeleteTempDir()
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
        }

    }
}
