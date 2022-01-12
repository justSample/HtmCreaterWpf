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

        public event Action OnChangeImage;

        /// <summary>
        /// Все эелементы с информацией о картинках
        /// </summary>
        public List<ElementInfo> Elements { get; set; }

        /// <summary>
        /// Получить текущий элемент
        /// </summary>
        public ElementInfo CurrentElement => Elements[CurrentIndex];
        
        public int CurrentIndex { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths">Массив с путями до картинок</param>
        public ElementContainer(string[] paths)
        {
            CurrentIndex = 0;

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
            if ((CurrentIndex + 1) >= Elements.Count) return CurrentElement;

            CurrentIndex++;

            OnChangeImage?.Invoke();

            return CurrentElement;
        }

        /// <summary>
        /// Поставить предъдущее изображение
        /// </summary>
        /// <returns></returns>
        public ElementInfo PrevImage()
        {
            if ((CurrentIndex - 1) < 0) return CurrentElement;

            CurrentIndex--;

            OnChangeImage?.Invoke();

            return CurrentElement;
        }
    }
}
