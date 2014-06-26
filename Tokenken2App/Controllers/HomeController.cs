using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace Tokenken2App.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient httpClient;
        private readonly string helloUrl;
        private readonly string itemsUrl;

        public HomeController()
        {
            this.httpClient = new HttpClient();
            var token = ConfigurationManager.AppSettings["currentToken"];
            this.httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

            this.helloUrl = ConfigurationManager.AppSettings["HelloApiUrl"];
            this.itemsUrl = ConfigurationManager.AppSettings["ItemsApiUrl"];

#if DEBUG
            //オレオレ証明書対策 デバッグ時だけ通すようにする
            ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => 
            {
                return true;
            };
#endif
        }

        public async Task<ActionResult> Index()
        {
            var ret = await httpClient.GetStringAsync(helloUrl);
            ViewBag.Hello = ret;
            return View();
        }

        [HttpGet]
        public async Task<ContentResult> Item()
        {
            if (!Request.IsAjaxRequest())
                return null;
            var items = await httpClient.GetStringAsync(itemsUrl);
            return new ContentResult() { Content = items, ContentType = "application/json", ContentEncoding = System.Text.Encoding.UTF8 };
        }

        [HttpGet]
        public ActionResult ItemList()
        {
            return View();
        }
    }
}