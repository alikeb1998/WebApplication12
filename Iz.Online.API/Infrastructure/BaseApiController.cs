﻿using System.Dynamic;
using Iz.Online.Services;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Iz.Online.API.Infrastructure
{
    [CustomAuthorization]

    public class BaseApiController : ControllerBase
    {
        //private readonly IHttpContextAccessor httpContextAccessor;
        public static string _token_;
        public BaseApiController(IHttpContextAccessor httpContextAccessor)
        {
            _token_ = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
         _token_ = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJSZXNvdXJjZXMiOlt7IlRhZyI6IkdldEFsbEFzc2V0cyIsIlVyaSI6Ii9vcmRlci9hc3NldC9hbGwiLCJJZCI6NTUsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEluc3RydW1lbnQiLCJVcmkiOiIvb3JkZXIvaW5zdHJ1bWVudHMve3F9IiwiSWQiOjU2LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxJbnN0cnVtZW50cyIsIlVyaSI6Ii9vcmRlci9pbnN0cnVtZW50cyIsIklkIjo1NywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0RGFzaGJvYXJkIiwiVXJpIjoiL3VzZXIvZGFzaGJvYXJkIiwiSWQiOjU5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRXYWxsZXQiLCJVcmkiOiIvdXNlci93YWxsZXQiLCJJZCI6NjAsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFsbE9ic2VydmVyTWVzc2FnZXMiLCJVcmkiOiIvcmxjL29ic2VydmVyLW1lc3NhZ2VzIiwiSWQiOjYxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRMaWdodFdlaWdodEluc3RydW1lbnQiLCJVcmkiOiIvb3JkZXIvaW5zdHJ1bWVudHMtbGlnaHR3ZWlnaHQiLCJJZCI6NjIsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldFRva2VuIiwiVXJpIjoiL3BheW1lbnQiLCJJZCI6MTA4LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDYWxsQmFjayIsIlVyaSI6Ii9wYXltZW50IiwiSWQiOjEwOSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsRGlzY2xhaW1lcnMiLCJVcmkiOiIvZGlzY2xhaW1lcnMvZ2V0QWxsIiwiSWQiOjExMCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQ2hhbmdlV2l0aFNlc3Npb25EaXNjbGFpbWVycyIsIlVyaSI6Ii9kaXNjbGFpbWVycy9jaGFuZ2VXaXRoU2Vzc2lvbiIsIklkIjoxMTMsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkNoYW5nZVBhc3N3b3JkU2VuZE90cCIsIlVyaSI6Ii91c2VyL2NoYW5nZS1wYXNzd29yZC9zZW5kLW90cCIsIklkIjoxMTQsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkNoYW5nZVBhc3N3b3JkQ2hlY2tPdHAiLCJVcmkiOiIvdXNlci9jaGFuZ2UtcGFzc3dvcmQvY2hlY2stb3RwIiwiSWQiOjExNSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQ2hlY2tPbldpdGhkcmF3YWwiLCJVcmkiOiIvdXNlci9jaGVja09uV2l0aGRyYXdhbCIsIklkIjoxMTYsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkluZHVjdE9uRGVwb3NpdCIsIlVyaSI6Ii91c2VyL2luZHVjdE9uRGVwb3NpdCIsIklkIjoxMTcsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkZyZWV6ZU9uV2l0aGRyYXdhbCIsIlVyaSI6Ii91c2VyL2ZyZWV6ZU9uV2l0aGRyYXdhbCIsIklkIjoxMTgsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFsbFRyYWRlc0J5UGFyYW1ldGVycyIsIlVyaSI6Ii90cmFkZS9hbGwvZmlsdGVyIiwiSWQiOjExOSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsT3JkZXJzQnlQYXJhbWV0ZXJzIiwiVXJpIjoiL29yZGVyL2FsbC9maWx0ZXIiLCJJZCI6MTIwLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRDdXN0b21lckluZm8iLCJVcmkiOiIvY3VzdG9tZXIvZ2V0Q3VzdG9tZXJJbmZvIiwiSWQiOjEyMSwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiUmVkZWVtT25XaXRoZHJhd2FsIiwiVXJpIjoiL3VzZXIvcmVkZWVtT25XaXRoZHJhd2FsIiwiSWQiOjEyMiwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiRGVkdWN0T25XaXRoZHJhd2FsIiwiVXJpIjoiL3VzZXIvZGVkdWN0T25XaXRoZHJhd2FsIiwiSWQiOjEyMywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QXNzZXRzIiwiVXJpIjoicG9ydGZvbGlvL2Fzc2V0cyIsIklkIjoxMjUsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldFRvdGFsUHJvZml0IiwiVXJpIjoicG9ydGZvbGlvL3RvdGFsUHJvZml0IiwiSWQiOjEyNiwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQWRkT3JkZXIiLCJVcmkiOiIvb3JkZXIvYWRkIiwiSWQiOjQ5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJVcGRhdGVPcmRlciIsIlVyaSI6Ii9vcmRlci91cGRhdGUve2lkOmxvbmd9IiwiSWQiOjUwLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDYW5jZWxPcmRlciIsIlVyaSI6Ii9vcmRlci9jYW5jZWwve2lkOmxvbmd9IiwiSWQiOjUxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxPcGVuT3JkZXJzIiwiVXJpIjoiL29yZGVyL2FsbC9vcGVuIiwiSWQiOjUyLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxBY3RpdmVPcmRlcnMiLCJVcmkiOiIvb3JkZXIvYWxsL2FjdGl2ZSIsIklkIjo1MywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsT3JkZXJzIiwiVXJpIjoiL29yZGVyL2FsbCIsIklkIjo1NCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiVmVyaWZ5T3JkZXIiLCJVcmkiOiIvb3JkZXIvdmVyaWZ5IiwiSWQiOjU4LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxUcmFkZXMiLCJVcmkiOiIvdHJhZGUvYWxsIiwiSWQiOjYzLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJHZXRBbGxPcGVuT2ZmZXJzIiwiVXJpIjoiL29yZGVyL29mZmVyL29wZW4iLCJJZCI6NjUsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IkdldEFjdGl2ZUlvT3JkZXJzIiwiVXJpIjoiL2lvL2FjdGl2ZSIsIklkIjo2NiwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsSW9PcmRlcnMiLCJVcmkiOiIvaW8vb3BlbiIsIklkIjo2NywiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiQWRkSW9PcmRlciIsIlVyaSI6Ii9pby9hZGQiLCJJZCI6NjgsIkRlc2NyaXB0aW9uIjpudWxsLCJLaW5kIjo0fSx7IlRhZyI6IlVwZGF0ZUlvT3JkZXIiLCJVcmkiOiIvaW8vdXBkYXRlIiwiSWQiOjY5LCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJEZWxldGVJb09yZGVyIiwiVXJpIjoiL2lvL2RlbGV0ZSIsIklkIjo3MCwiRGVzY3JpcHRpb24iOm51bGwsIktpbmQiOjR9LHsiVGFnIjoiR2V0QWxsSW9UcmFkZXMiLCJVcmkiOiIvaW8vdHJhZGVzIiwiSWQiOjcxLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH0seyJUYWciOiJDaGFuZ2VEaXNjbGFpbWVycyIsIlVyaSI6Ii9kaXNjbGFpbWVycy9jaGFuZ2UiLCJJZCI6MTEyLCJEZXNjcmlwdGlvbiI6bnVsbCwiS2luZCI6NH1dLCJTZXNzaW9uIjoiMjQ5YThiMzMtZGI5OC00MWYxLWFiYTUtZDc5MzRiNzIwNzZkIiwiQ3VzdG9tZXJJZCI6MCwiVXNlcm5hbWUiOiJzYXR0YXIiLCJFbWFpbEFkZHJlc3MiOiJzYXR0YXJAc2NlbnVzLmNvbSIsIk1vYmlsZU51bWJlciI6Iis5ODkxNjYxNTg5MzUiLCJJZCI6MjEsIlBhc3N3b3JkRXhwaXJhdGlvbiI6IjIwMjItMDgtMDhUMTY6MDA6MjkuNzk3IiwiZXhwIjoxNjUwMTk0ODUwfQ.mV98MUdn3KzIbAwsRniWwu3FMsFFnOQMeoUyc4U40KU";

            //IConfiguration _configuration = configuration;
            //var conf = _configuration.GetSection("IsDevelopment").Get<string>();
            //var tk = _configuration.GetSection("TK").Get<string>();
            //if (conf == "true")
            //{
            //    _token_ = tk;
            //}
        }

    }
}
