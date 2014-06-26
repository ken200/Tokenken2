using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Extensions;
using Nancy.Security;
using Nancy.Authentication.Token;
using Nancy.Responses;

namespace TokenkenAPI
{
    public class ApiTopPageModule : NancyModule
    {
        public ApiTopPageModule() : base()
        {
            Get["/"] = _ => 
            {
                return "Api Server";
            };

            Get["/error"] = _ =>
            {
                throw new Exception("何らかのエラー");
            };
        }
    }

    public class ApiSampleModule : NancyModule
    {
        public ApiSampleModule()
            : base("/api")
        {
            this.RequiresHttps();
            this.RequiresAuthentication();

            Get["/hello"] = _ =>
            {
                return string.Format("hello, now is {0}", DateTime.Now);
            };
        }
    }

    public class CategoryModule : NancyModule
    {
        private class Category
        {
            public int CateId {get;set;}
            public string CateName {get;set;}
        }

        public CategoryModule() : base("/api/category")
        {
            this.RequiresHttps();
            this.RequiresAuthentication();

            Get["/all"] = _ => 
            {
              return new JsonResponse<IEnumerable<Category>>(Enumerable.Range(1,3).Select((i) => 
              { 
                  return new Category()
                  {
                      CateId = 10001 + 1000 * i, 
                      CateName = string.Format("カテゴリー{0}", i)
                  };
              }), new DefaultJsonSerializer());
            };
        }
    }

    public class ItemModule : NancyModule
    {
        public class Item
        {
            public string ItemId { get; set; }
            public int CateId { get; set; }
            public string ItemName { get; set; }
            public string Summary { get; set; }
            public DateTime Limit { get; set; }
        }

        public ItemModule() : base("/api/item") 
        {
            this.RequiresHttps();
            this.RequiresAuthentication();

            Get["/all"] = _ => 
            {
                Func<IEnumerable<Item>> items = () =>
                {
                    return Enumerable.Range(1, 3).Select<int, IEnumerable<Item>>((i) =>
                    {
                        return Enumerable.Range(1, 10).Select((j) =>
                        {
                            return new Item()
                            {
                                ItemId = string.Format("{0}", i * 10000 + j),
                                CateId = 10001 + 1000 * i,
                                ItemName = string.Format("カテゴリ{0}のアイテム{1}", i, j),
                                Summary = string.Format("サマリー{0:00}_{1:00}", i, j),
                                Limit = DateTime.Now
                            };
                        });
                    })
                    .SelectMany(i => i);
                };

                return new JsonResponse<IEnumerable<Item>>(items(), new DefaultJsonSerializer());
            };
        }
    }
}