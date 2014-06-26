using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Nancy;
using Nancy.Authentication.Token;
using Tokenken2Common;

namespace TokenkenAPI
{
    public class MyBootStrapper : DefaultNancyBootstrapper
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
            TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(container.Resolve<ITokenizer>()));
        }
    }
}