using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Files
{
    public static class ExceptionHandler
    {
        //private static void HandleException(Exception ex)
        //{

        //    var dbModel = ex.ToExceptionLog();

        //    context.ExceptionLogs.Add(dbModel);

        //    context.SaveChanges();
        //}

        public static string ExceptionToString(Exception ex)
        {
            StringBuilder str = new StringBuilder(ex.Message);

            if (ex.InnerException != null)
            {
                str.AppendLine("---------------");
                str.AppendLine(ExceptionToString(ex.InnerException));
            }

            return str.ToString();
        }

        //public static ExceptionLog ToExceptionLog(Exception ex)
        //{
        //    return new ExceptionLog()
        //    {
        //        Date = DateTime.Now,
        //        Message = ExceptionToString(ex),
        //        StackTrace = ex.StackTrace,
        //    };
        //}

    }
}
