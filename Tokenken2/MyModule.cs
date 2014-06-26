using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Security;
using Nancy.Extensions;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Token;
using Tokenken2Common;

namespace Tokenken2
{
    public class MyModule : NancyModule
    {
        public MyModule()
        {
            Get["/"] = _ => 
            {
                return Response.AsRedirect("/login");
            };

            Get["/login"] = _ => 
            {
                return View["login"];
            };

            Post["/login"] = _ =>
            {
                var userGuid = UserDatabase.ValidateUser(Context.Request.Form.Username, Context.Request.Form.Password);
                if (userGuid == null)
                {
                    //リダイレクト先起点は、モジュールのルートパス起点ではないので注意すること。
                    return Context.GetRedirect("/login?error=true");
                }
                return Nancy.Authentication.Forms.ModuleExtensions.LoginAndRedirect(this, userGuid, null, "/secure");
            };

            Get["/error"] = _ => 
            {
                throw new Exception("エラー");
            };
        }
    }

    public class SecureModule : NancyModule
    {
        public SecureModule(ITokenizer tokenizer) : base("/secure") 
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                ViewBag.CurrentUser = this.Context.CurrentUser.UserName;
                return View["secure"];
            };

            Get["/token"] = _ =>
            {
                ViewBag.Token = tokenizer.Tokenize(Context.CurrentUser, Context);
                return View["token"];
            };

            Get["/logout"] = _ =>
            {
                return this.LogoutAndRedirect("/");
            };
        }
    }
}