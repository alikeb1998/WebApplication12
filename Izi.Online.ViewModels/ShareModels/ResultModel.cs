using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.ShareModels
{
    public class ResultModel<T>
    {
        public ResultModel(T model , bool isSuccess = true, string message = "", int statusCode = 1)
        {
            if (isSuccess)
            {
                Message = "با موفقیت انجام شد";
                StatusCode = 200;

            }
            else
            {
                if (string.IsNullOrEmpty(message))
                    Message = "خطای پیش بینی نشده"; 
                else 
                    Message = message;
                StatusCode = statusCode;
            }
            Model = model;
            IsSuccess = isSuccess;

        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Model { get; set; }
    }

}
