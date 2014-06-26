using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses;

namespace Tokenken2Api
{
    public class NotFoundHandler : IStatusCodeHandler
    {
        private class NotFoundModel
        {
            public string Message { get; private set; }

            public NotFoundModel() : this("404") { }

            public NotFoundModel(string msg) { this.Message = msg; }
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = new JsonResponse<NotFoundModel>(new NotFoundModel(), new DefaultJsonSerializer());
            response.StatusCode = statusCode;
            context.Response = response; 
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }
    }

    public class InternalServerErrorHandler : IStatusCodeHandler
    {
        private class InternalServerErrorModel
        {
            public string Message { get; private set; }

            public InternalServerErrorModel() : this("500") { }

            public InternalServerErrorModel(string msg) { this.Message = msg; }
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var response = new JsonResponse<InternalServerErrorModel>(new InternalServerErrorModel(), new DefaultJsonSerializer());
            response.StatusCode = statusCode;
            context.Response = response;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.InternalServerError;
        }
    }
}