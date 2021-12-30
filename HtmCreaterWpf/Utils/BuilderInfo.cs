using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmCreaterWpf.Utils
{
    public class BuilderInfo
    {

        public static string GetCurrentYearAndQuarter()
        {
            int monthInt = int.Parse(DateTime.Now.ToString("MM"));
            int yearInt =  int.Parse(DateTime.Now.ToString("yyyy"));

            switch (monthInt)
            {

                case 1:
                case 2:
                case 3:
                    return $"{yearInt}-1";
                case 4:
                case 5:
                case 6:
                    return $"{yearInt}-2";
                case 7:
                case 8:
                case 9:
                    return $"{yearInt}-3";
                case 10:
                case 11:
                case 12:
                    return $"{yearInt}-4";
                default:
                    return $"{yearInt}-0";
            }

        }


    }
}
