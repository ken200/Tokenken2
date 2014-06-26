using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Token;
using Tokenken2Common;

namespace Tokenken2
{
    public class MyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            container.Register<ITokenizer>(new Tokenizer((config) =>
            {
                config.AdditionalItems();
                config.KeyExpiration(() => new TimeSpan(DateTime.MaxValue.Ticks));
                config.TokenExpiration(() => new TimeSpan(DateTime.MaxValue.AddDays(-1).Ticks));
                config.WithKeyCache(new DBTokenKeyStore(ConfigurationManager.AppSettings["tokenDbConnection"]));
            }));
        }

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            var formsAuthConfiguration = new FormsAuthenticationConfiguration()
            {
                RedirectUrl = "/login", //認証失敗時のリダイレクト先
                UserMapper = container.Resolve<IUserMapper>()
            };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);  //フォーム認証の有効化
        }
    }
}