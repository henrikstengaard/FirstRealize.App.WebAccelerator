using FirstRealize.App.WebAccelerator.Caching;
using FirstRealize.App.WebAccelerator.Handlers;
using System;
using System.Web;

namespace FirstRealize.App.WebAccelerator
{
    public class WebAcceleratorModule : IHttpModule
    {
        private readonly TimeSpan _timeout;
        private readonly ICache _cache;
        private readonly HttpContextHandler _httpContextHandler;

        public WebAcceleratorModule()
        {
            _timeout = TimeSpan.FromSeconds(60);
            _cache = HttpRuntimeCache.Current;
            _httpContextHandler = new HttpContextHandler(
                _cache);
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            _httpContextHandler
                .GetResponseFromCache(context);
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            _httpContextHandler
                .AddResponseToCache(context, _cache, _timeout);
        }
    }
}