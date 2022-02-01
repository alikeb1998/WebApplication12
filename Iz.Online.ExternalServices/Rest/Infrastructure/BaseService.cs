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
        private readonly string apiBaseAddress = "http://192.168.72.54:8080/";
        private readonly string authorization = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJSZXNvdXJjZXMiOlt7IlRhZyI6IkdldEFsbEFzc2V0cyIsIlVyaSI6Ii9vcmRlci9hc3NldC9hbGwiLCJJZCI6NTUsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEluc3RydW1lbnQiLCJVcmkiOiIvb3JkZXIvaW5zdHJ1bWVudHMve3F9IiwiSWQiOjU2LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxJbnN0cnVtZW50cyIsIlVyaSI6Ii9vcmRlci9pbnN0cnVtZW50cyIsIklkIjo1NywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0RGFzaGJvYXJkIiwiVXJpIjoiL3VzZXIvZGFzaGJvYXJkIiwiSWQiOjU5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRXYWxsZXQiLCJVcmkiOiIvdXNlci93YWxsZXQiLCJJZCI6NjAsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFsbE9ic2VydmVyTWVzc2FnZXMiLCJVcmkiOiIvcmxjL29ic2VydmVyLW1lc3NhZ2VzIiwiSWQiOjYxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRMaWdodFdlaWdodEluc3RydW1lbnQiLCJVcmkiOiIvb3JkZXIvaW5zdHJ1bWVudHMtbGlnaHR3ZWlnaHQiLCJJZCI6NjIsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldFRva2VuIiwiVXJpIjoiL3BheW1lbnQiLCJJZCI6MTA4LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDYWxsQmFjayIsIlVyaSI6Ii9wYXltZW50IiwiSWQiOjEwOSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsRGlzY2xhaW1lcnMiLCJVcmkiOiIvZGlzY2xhaW1lcnMvZ2V0QWxsIiwiSWQiOjExMCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQ2hhbmdlV2l0aFNlc3Npb25EaXNjbGFpbWVycyIsIlVyaSI6Ii9kaXNjbGFpbWVycy9jaGFuZ2VXaXRoU2Vzc2lvbiIsIklkIjoxMTMsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkNoYW5nZVBhc3N3b3JkU2VuZE90cCIsIlVyaSI6Ii91c2VyL2NoYW5nZS1wYXNzd29yZC9zZW5kLW90cCIsIklkIjoxMTQsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkNoYW5nZVBhc3N3b3JkQ2hlY2tPdHAiLCJVcmkiOiIvdXNlci9jaGFuZ2UtcGFzc3dvcmQvY2hlY2stb3RwIiwiSWQiOjExNSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQWRkT3JkZXIiLCJVcmkiOiIvb3JkZXIvYWRkIiwiSWQiOjQ5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJVcGRhdGVPcmRlciIsIlVyaSI6Ii9vcmRlci91cGRhdGUve2lkOmxvbmd9IiwiSWQiOjUwLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDYW5jZWxPcmRlciIsIlVyaSI6Ii9vcmRlci9jYW5jZWwve2lkOmxvbmd9IiwiSWQiOjUxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxPcGVuT3JkZXJzIiwiVXJpIjoiL29yZGVyL2FsbC9vcGVuIiwiSWQiOjUyLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxBY3RpdmVPcmRlcnMiLCJVcmkiOiIvb3JkZXIvYWxsL2FjdGl2ZSIsIklkIjo1MywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsT3JkZXJzIiwiVXJpIjoiL29yZGVyL2FsbCIsIklkIjo1NCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiVmVyaWZ5T3JkZXIiLCJVcmkiOiIvb3JkZXIvdmVyaWZ5IiwiSWQiOjU4LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxUcmFkZXMiLCJVcmkiOiIvdHJhZGUvYWxsIiwiSWQiOjYzLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxPcGVuT2ZmZXJzIiwiVXJpIjoiL29yZGVyL29mZmVyL29wZW4iLCJJZCI6NjUsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFjdGl2ZUlvT3JkZXJzIiwiVXJpIjoiL2lvL2FjdGl2ZSIsIklkIjo2NiwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsSW9PcmRlcnMiLCJVcmkiOiIvaW8vb3BlbiIsIklkIjo2NywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQWRkSW9PcmRlciIsIlVyaSI6Ii9pby9hZGQiLCJJZCI6NjgsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IlVwZGF0ZUlvT3JkZXIiLCJVcmkiOiIvaW8vdXBkYXRlIiwiSWQiOjY5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJEZWxldGVJb09yZGVyIiwiVXJpIjoiL2lvL2RlbGV0ZSIsIklkIjo3MCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsSW9UcmFkZXMiLCJVcmkiOiIvaW8vdHJhZGVzIiwiSWQiOjcxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDaGFuZ2VEaXNjbGFpbWVycyIsIlVyaSI6Ii9kaXNjbGFpbWVycy9jaGFuZ2UiLCJJZCI6MTEyLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH1dLCJTZXNzaW9uIjoiNTNmNTNiYzgtMjFmZC00ZTViLWI4YjEtNmM2NDNlMWU3ZTY0IiwiVXNlcm5hbWUiOiJzYXR0YXIiLCJFbWFpbEFkZHJlc3MiOiJzYXR0YXJAc2NlbnVzLmNvbSIsIk1vYmlsZU51bWJlciI6Iis5ODkxNjYxNTg5MzUiLCJJZCI6MjEsIlBhc3N3b3JkRXhwaXJhdGlvbiI6IjIwMjItMDgtMDhUMTY6MDA6MjkuNzk3IiwiZXhwIjoxNjQzNTgwMDAwfQ.2Hai1PjKnY0IaIFJlX2uczSgwdDgHMRiYZn6knWr_qo";

        private readonly IBaseRepository _baseRepository;
        public BaseService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }


        public T HttpGetRequest<T>(string RequestAddress)
        {
            try
            {

                var client = new RestClient($"{apiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", authorization);
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

        public T HttpPostRequest<T>(string RequestAddress, string SerializedObject)
        {
            try
            {


                var client = new RestClient($"{apiBaseAddress}{RequestAddress}");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", authorization);
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
