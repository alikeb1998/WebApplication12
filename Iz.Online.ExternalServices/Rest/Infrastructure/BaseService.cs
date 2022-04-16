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
        public string Token { get; set; }

        private readonly string ApiBaseAddress = "http://192.168.72.54:8080/";
        

        private readonly IBaseRepository _baseRepository;
        private readonly ServiceProvider provider;

        public BaseService(IBaseRepository baseRepository, ServiceProvider provider =ServiceProvider.Oms)
        {
            _baseRepository = baseRepository;
            if (provider == ServiceProvider.BackOffice)
                //ApiBaseAddress = "http://192.168.72.112:8091/";
                ApiBaseAddress = "http://127.0.0.1:5069/";
        }

        public  async Task<T> HttpGetRequest<T>(string RequestAddress, int port = 1)
        {
            try
            {
                var client = new RestClient($"{ApiBaseAddress}{RequestAddress}");
             
                
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", Token);
                IRestResponse response =  await client.ExecuteAsync(request);

                if (string.IsNullOrEmpty(response.Content))
                    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(new OmsResponseBaseModel
                    {
                        clientMessage = "خطا در برقراری ارتباط با سرویس",
                        code = 500,
                        message = "خطا در برقراری ارتباط با سرویس",
                        statusCode = 500
                    }));

                var setting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };


                return JsonConvert.DeserializeObject<T>(response.Content, setting);
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

        public async Task<T> HttpPostRequest<T>(string RequestAddress, string SerializedObject)
        {
            try
            {
                var client = new RestClient($"{ApiBaseAddress}{RequestAddress}");

                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", SerializedObject, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (string.IsNullOrEmpty(response.Content))
                    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(new OmsResponseBaseModel
                    {
                        clientMessage = "خطا در برقراری ارتباط با سرویس",
                        code = 500,
                        message = "خطا در برقراری ارتباط با سرویس",
                        statusCode = 500
                    }));

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

        public async Task<T> HttpPutRequest<T>(string RequestAddress, string SerializedObject,  int port = 1)
        {
            try
            {
                var client = new RestClient($"{ApiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Authorization", Token);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", SerializedObject, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (string.IsNullOrEmpty(response.Content))
                    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(new OmsResponseBaseModel
                    {
                        clientMessage = "خطا در برقراری ارتباط با سرویس",
                        code = 500,
                        message = "خطا در برقراری ارتباط با سرویس",
                        statusCode = 500
                    }));

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

        public async Task<T> HttpDeleteRequest<T>(string RequestAddress, string SerializedObject, int port = 1)
        {
            try
            {
                var client = new RestClient($"{ApiBaseAddress}{RequestAddress}");
 
                client.Timeout = -1;
                var request = new RestRequest(Method.DELETE);
                request.AddHeader("Authorization", Token);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", SerializedObject, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (string.IsNullOrEmpty(response.Content))
                    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(new OmsResponseBaseModel
                    {
                        clientMessage = "خطا در برقراری ارتباط با سرویس",
                        code = 500,
                        message = "خطا در برقراری ارتباط با سرویس",
                        statusCode = 500
                    }));

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
