using Iz.Online.DataAccess;
using Iz.Online.Files;
using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.Order;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

namespace Iz.Online.ExternalServices.Rest.Infrastructure
{
    public class BaseService
    {
        public  string apiBaseAddress = "http://192.168.72.54:8080/";
        

        private readonly IBaseRepository _baseRepository;
        public BaseService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }


        public T HttpGetRequest<T>(string RequestAddress  , string token)
        {
            try
            {

                var client = new RestClient($"{apiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", token);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception e)
            {
                _baseRepository.LogException(e);
                var t = new OmsResponseBaseModel
                {
                    clientMessage = "خطا در برقراری ارتباط با سرویس",
                    code = -1,
                    message = "خطا در برقراری ارتباط با سرویس",
                    statusCode = -1
                };
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(t));
            }
        }

        public T HttpPostRequest<T>(string RequestAddress, string SerializedObject, string token)
        {
            try
            {


                var client = new RestClient($"{apiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", token);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", SerializedObject, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<T>(response.Content);
                
            }
            catch (Exception e)
            {
                _baseRepository.LogException(e);

                var t = new OmsResponseBaseModel
                {
                    clientMessage = "خطا در برقراری ارتباط با سرویس",
                    code = -1,
                    message = "خطا در برقراری ارتباط با سرویس",
                    statusCode = -1
                };
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(t));

            }
        }



    }
}
