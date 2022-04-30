using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izi.Online.ViewModels.ShareModels
{
    public class ResultModel<T>
    {
        public ResultModel(T model , bool isSuccess = true, string message = "", int statusCode = 200)
        {
            if (isSuccess)
            {
                Message = "با موفقیت انجام شد";
                StatusCode = statusCode;

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
        public ResultModel(T model,  int statusCode, string message = "خطای پیش بینی نشده")
        {
            Message =message;
            Model = model;
            IsSuccess = false;
            StatusCode = statusCode;
        }    
        public ResultModel()
        {

        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Model { get; set; }
    }

}
