using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Files
{
    public static class EnumHelper
    {
        public static string OrderStates(string key)
        {
            switch (key)
            {
                case "1":
                    return "انجام شد";


                default:
                    return "نامشخص";


            }
        }
        public static string InstrumentGroupStates(string key)
        {
            switch (key)
            {
                case "0":
                    return "نامشخص";
                    break;
                case "1":
                    return "آغاز مشاوره";
                    break;
                case "2":
                    return "پیش گشایش";
                    break;
                case "3":
                    return "درحال گشایش";
                    break;
                case "4":
                    return "مداخله قبل از بازار";
                    break;
                case "5":
                    return "معاملات پیوسته";
                    break;
                case "6":
                    return "معاملات در قیمت اخرین معامله";
                    break;
                case "7":
                    return "مداخله ناظر بازار";
                    break;
                case "8":
                    return "پایان مشاوره";
                    break;
                case "9":
                    return "پایان مشاوره";
                    break;
                case "10":
                    return "ممنوع";
                    break;
                case "11":
                    return "وقفه";
                    break;


                default:
                    return "نامشخص";
                    break;


            }
        }
        public static string InstrumentStates(string key)
        {
            switch (key)
            {
                case "0":
                    return "نامشخص";
                    break;
                case "1":
                    return "مجاز";
                    break;
                case "2":
                    return "مجاز متوقف";
                    break;
                case "3":
                    return "مجاز محفوظ";
                    break;
                case "4":
                    return "مجاز مسدود";
                    break;
                case "5":
                    return "ممنوع";
                    break;
                case "6":
                    return "ممنون متوقف";
                    break;
                case "7":
                    return "ممنوع محفوظ";
                    break;
                case "8":
                    return "ممنوع مسدود";
                    break;


                default:
                    return "نامشخص";
                    break;


            }
        }
    }
}
