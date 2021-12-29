using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HtmCreaterWpf.Utils
{
    public class ElementContainer
    {
        /// <summary>
        /// Все эелементы с информацией о картинках
        /// </summary>
        public List<ElementInfo> Elements { get; set; }

        /// <summary>
        /// Получить текущий элемент
        /// </summary>
        public ElementInfo CurrentElement
        {
            get => Elements[_currentIndex];
        }

        private int _currentIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths">Массив с путями до картинок</param>
        public ElementContainer(string[] paths)
        {
            Elements = new List<ElementInfo>(paths.Length);

            for (int i = 0; i < paths.Length; i++)
            {
                Elements.Add(new ElementInfo(paths[i]));
            }

        }

        /// <summary>
        /// Поставить следующее изображение
        /// </summary>
        /// <returns></returns>
        public ElementInfo NextImage()
        {
            if ((_currentIndex + 1) >= Elements.Count) return CurrentElement;

            _currentIndex++;

            return CurrentElement;
        }

        /// <summary>
        /// Поставить предъдущее изображение
        /// </summary>
        /// <returns></returns>
        public ElementInfo PrevImage()
        {
            if ((_currentIndex - 1) < 0) return CurrentElement;

            _currentIndex--;

            return CurrentElement;
        }
    }
}
