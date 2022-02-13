using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Files
{
    public static class EnumHelper
    {
        public static string OrderStates(int key)
        {
            switch (key)
            {
                case 1:
                    return "انجام شد"; 
                
                
                default:
                   return "نامشخص"; 


            }
        }
    }
}
