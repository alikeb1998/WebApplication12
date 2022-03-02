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

        public string apiBaseAddress = "http://192.168.72.54:8080/";

        private readonly IBaseRepository _baseRepository;
        

        public BaseService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
           
        }

        //public string getToken()
        //{

        //    //var client = new RestClient($@"http://192.168.72.112:5554/V1/User/token/get");
        //    //client.Timeout = -1;
        //    //var request = new RestRequest(Method.GET);
        //    //IRestResponse response = client.Execute(request);
        //    //var token = JsonConvert.DeserializeObject<string>(response.Content);
        //    var res = Iz.Online.Files.ShareValue.Token;
        //    return res;
        //}

        public T HttpGetRequest<T>(string RequestAddress)
        {
            try
            {

                var client = new RestClient($"{apiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", Token);
                IRestResponse response = client.Execute(request);

                var setting = new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore};
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

        public T HttpPostRequest<T>(string RequestAddress, string SerializedObject)
        {
            try
            {

                var client = new RestClient($"{apiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", Token);
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

        public T HttpPutRequest<T>(string RequestAddress, string SerializedObject)
        {
            try
            {


                var client = new RestClient($"{apiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Authorization", ShareValue.Token);
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

        public T HttpDeleteRequest<T>(string RequestAddress, string SerializedObject)
        {
            try
            {


                var client = new RestClient($"{apiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.DELETE);
                request.AddHeader("Authorization", ShareValue.Token);
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

        public string GetOmsToken(string token)
        {
            return _baseRepository.GetOmsToken(token);
        }

        public bool LocalTokenIsValid(string token)
        {
            return _baseRepository.LocalTokenIsValid(token);
        }

    }


    public class BaseService2
    {
        public string apiBaseAddress = "http://192.168.72.54:8080/";
        public string _Id { get; set; }
        private readonly IBaseRepository _baseRepository;

        public BaseService2(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public string getToken()
        {

            var client = new RestClient($@"http://192.168.72.112:5554/V1/User/token/get");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var token = JsonConvert.DeserializeObject<string>(response.Content);

            return token;
        }

        public T HttpGetRequest<T>(string RequestAddress)
        {
            try
            {

                var client = new RestClient($"{apiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", getToken());
                IRestResponse response = client.Execute(request);

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

    }
}
