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
        public List<ElementInfo> Elements { get; set; }

        public ElementInfo CurrentElement
        {
            get => Elements[_currentIndex];
        }

        private int _currentIndex = 0;

        public ElementContainer(string[] paths)
        {
            Elements = new List<ElementInfo>(paths.Length);

            for (int i = 0; i < paths.Length; i++)
            {
                Elements.Add(new ElementInfo(paths[i]));
            }

        }

        public ElementInfo NextImage()
        {
            if ((_currentIndex + 1) >= Elements.Count) return Elements[_currentIndex];

            _currentIndex++;

            return Elements[_currentIndex];
        }

        public ElementInfo PrevImage()
        {
            if ((_currentIndex - 1) < 0) return Elements[_currentIndex];

            _currentIndex--;

            return Elements[_currentIndex];
        }

    }
}
