﻿using System.Dynamic;
using Iz.Online.Services;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Iz.Online.API.Infrastructure
{
    [CustomAuthorization]
    //
    // [EnableCors("CustomCors")]

    public class BaseApiController : ControllerBase
    {
        //private readonly IHttpContextAccessor httpContextAccessor;
        public static string _token_;
        public BaseApiController(IHttpContextAccessor httpContextAccessor)
        {
          _token_ = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
          _token_ = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJSZXNvdXJjZXMiOlt7IlRhZyI6IkdldEFsbEFzc2V0cyIsIlVyaSI6Ii9vcmRlci9hc3NldC9hbGwiLCJJZCI6NTUsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEluc3RydW1lbnQiLCJVcmkiOiIvb3JkZXIvaW5zdHJ1bWVudHMve3F9IiwiSWQiOjU2LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxJbnN0cnVtZW50cyIsIlVyaSI6Ii9vcmRlci9pbnN0cnVtZW50cyIsIklkIjo1NywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0RGFzaGJvYXJkIiwiVXJpIjoiL3VzZXIvZGFzaGJvYXJkIiwiSWQiOjU5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRXYWxsZXQiLCJVcmkiOiIvdXNlci93YWxsZXQiLCJJZCI6NjAsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFsbE9ic2VydmVyTWVzc2FnZXMiLCJVcmkiOiIvcmxjL29ic2VydmVyLW1lc3NhZ2VzIiwiSWQiOjYxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRMaWdodFdlaWdodEluc3RydW1lbnQiLCJVcmkiOiIvb3JkZXIvaW5zdHJ1bWVudHMtbGlnaHR3ZWlnaHQiLCJJZCI6NjIsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldFRva2VuIiwiVXJpIjoiL3BheW1lbnQiLCJJZCI6MTA4LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDYWxsQmFjayIsIlVyaSI6Ii9wYXltZW50IiwiSWQiOjEwOSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsRGlzY2xhaW1lcnMiLCJVcmkiOiIvZGlzY2xhaW1lcnMvZ2V0QWxsIiwiSWQiOjExMCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQ2hhbmdlV2l0aFNlc3Npb25EaXNjbGFpbWVycyIsIlVyaSI6Ii9kaXNjbGFpbWVycy9jaGFuZ2VXaXRoU2Vzc2lvbiIsIklkIjoxMTMsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkNoYW5nZVBhc3N3b3JkU2VuZE90cCIsIlVyaSI6Ii91c2VyL2NoYW5nZS1wYXNzd29yZC9zZW5kLW90cCIsIklkIjoxMTQsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkNoYW5nZVBhc3N3b3JkQ2hlY2tPdHAiLCJVcmkiOiIvdXNlci9jaGFuZ2UtcGFzc3dvcmQvY2hlY2stb3RwIiwiSWQiOjExNSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQ2hlY2tPbldpdGhkcmF3YWwiLCJVcmkiOiIvdXNlci9jaGVja09uV2l0aGRyYXdhbCIsIklkIjoxMTYsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkluZHVjdE9uRGVwb3NpdCIsIlVyaSI6Ii91c2VyL2luZHVjdE9uRGVwb3NpdCIsIklkIjoxMTcsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkZyZWV6ZU9uV2l0aGRyYXdhbCIsIlVyaSI6Ii91c2VyL2ZyZWV6ZU9uV2l0aGRyYXdhbCIsIklkIjoxMTgsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkFkZE9yZGVyIiwiVXJpIjoiL29yZGVyL2FkZCIsIklkIjo0OSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiVXBkYXRlT3JkZXIiLCJVcmkiOiIvb3JkZXIvdXBkYXRlL3tpZDpsb25nfSIsIklkIjo1MCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQ2FuY2VsT3JkZXIiLCJVcmkiOiIvb3JkZXIvY2FuY2VsL3tpZDpsb25nfSIsIklkIjo1MSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsT3Blbk9yZGVycyIsIlVyaSI6Ii9vcmRlci9hbGwvb3BlbiIsIklkIjo1MiwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsQWN0aXZlT3JkZXJzIiwiVXJpIjoiL29yZGVyL2FsbC9hY3RpdmUiLCJJZCI6NTMsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFsbE9yZGVycyIsIlVyaSI6Ii9vcmRlci9hbGwiLCJJZCI6NTQsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IlZlcmlmeU9yZGVyIiwiVXJpIjoiL29yZGVyL3ZlcmlmeSIsIklkIjo1OCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsVHJhZGVzIiwiVXJpIjoiL3RyYWRlL2FsbCIsIklkIjo2MywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsT3Blbk9mZmVycyIsIlVyaSI6Ii9vcmRlci9vZmZlci9vcGVuIiwiSWQiOjY1LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBY3RpdmVJb09yZGVycyIsIlVyaSI6Ii9pby9hY3RpdmUiLCJJZCI6NjYsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFsbElvT3JkZXJzIiwiVXJpIjoiL2lvL29wZW4iLCJJZCI6NjcsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkFkZElvT3JkZXIiLCJVcmkiOiIvaW8vYWRkIiwiSWQiOjY4LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJVcGRhdGVJb09yZGVyIiwiVXJpIjoiL2lvL3VwZGF0ZSIsIklkIjo2OSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiRGVsZXRlSW9PcmRlciIsIlVyaSI6Ii9pby9kZWxldGUiLCJJZCI6NzAsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFsbElvVHJhZGVzIiwiVXJpIjoiL2lvL3RyYWRlcyIsIklkIjo3MSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQ2hhbmdlRGlzY2xhaW1lcnMiLCJVcmkiOiIvZGlzY2xhaW1lcnMvY2hhbmdlIiwiSWQiOjExMiwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9XSwiU2Vzc2lvbiI6ImYxZWJhMThhLTk2M2QtNGJlNC04NzRmLTg3ZGJhMzNmZWMxOSIsIlVzZXJuYW1lIjoic2F0dGFyIiwiRW1haWxBZGRyZXNzIjoic2F0dGFyQHNjZW51cy5jb20iLCJNb2JpbGVOdW1iZXIiOiIrOTg5MTY2MTU4OTM1IiwiSWQiOjIxLCJQYXNzd29yZEV4cGlyYXRpb24iOiIyMDIyLTA4LTA4VDE2OjAwOjI5Ljc5NyIsImV4cCI6MTY0Njc0Njg4M30.8L1IDxiFDbanopbTZ3umh7epp-uko1AHBPufv06HOoI";
        }

    }
}
