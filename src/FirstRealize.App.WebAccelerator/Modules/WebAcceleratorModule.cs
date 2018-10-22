using FirstRealize.App.WebAccelerator.Handlers;
using System;
using System.Web;

namespace FirstRealize.App.WebAccelerator.Modules
{
    public class WebAcceleratorModule : IHttpModule
    {
        private readonly TimeSpan _timeout;
        private readonly WebAcceleratorHandler _httpContextHandler;

        public WebAcceleratorModule()
        {
            _timeout = TimeSpan.FromSeconds(60);
            _httpContextHandler = WebAcceleratorHandler.Current;
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
                .AddResponseToCache(context, _timeout);
        }
    }
}