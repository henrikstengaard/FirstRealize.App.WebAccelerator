using FirstRealize.App.WebAccelerator.Caching;
using FirstRealize.App.WebAccelerator.Filters;
using FirstRealize.App.WebAccelerator.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace FirstRealize.App.WebAccelerator.Handlers
{
    public class WebAcceleratorHandler
    {
        private static Lazy<WebAcceleratorHandler> _current =
            new Lazy<WebAcceleratorHandler>(() =>
            {
                return new WebAcceleratorHandler(
                    HttpRuntimeCache.Current);
            }, true);

        private readonly ICache _cache;

        public WebAcceleratorHandler(
            ICache cache)
        {
            _cache = cache;
        }

        public static WebAcceleratorHandler Current
        {
            get
            {
                return _current.Value;
            }
        }

        public void GetResponseFromCache(
            HttpContext context)
        {
            var cacheKey = context.Request.Url.AbsoluteUri;

            var responseCache = HttpRuntime.Cache.Get(cacheKey) as ResponseCache;

            // add cache content filter, if response is not cached
            if (responseCache == null)
            {
                context.Response.Filter =
                    new CacheContentFilter(context.Response.Filter);
                return;
            }

            context.Response.Clear();

            // remove existing headers from response
            var keys = new List<string>();
            for (var enumerator = context.Response.Headers.Keys.GetEnumerator(); enumerator.MoveNext();)
            {
                keys.Add(enumerator.Current.ToString());
            }
            foreach (var key in keys)
            {
                context.Response.Headers.Remove(key);
            }

            // add response cache to response and end/return response
            context.Response.Headers.Add(
                responseCache.Headers);
            context.Response.ContentType = 
                responseCache.ContentType;
            context.Response.Charset = 
                responseCache.Charset;
            context.Response.ContentEncoding = 
                responseCache.ContentEncoding;
            context.Response.OutputStream.Write(
                responseCache.Content, 0, responseCache.Content.Length);
            context.Response.StatusCode = 
                responseCache.StatusCode;
            context.Response.StatusDescription = 
                responseCache.StatusDescription;
            context.Response.End();
        }

        public void AddResponseToCache(
            HttpContext context,
            TimeSpan timeout)
        {
            // get cache key
            var cacheKey = context.Request.Url.AbsoluteUri;

            // get response cache. return, if response is already cached
            var responseCache = HttpRuntime.Cache.Get(cacheKey) as ResponseCache;
            if (responseCache != null)
            {
                return;
            }

            // get cache content filter. return if null
            var cacheContentFilter = context.Response.Filter as CacheContentFilter;
            if (cacheContentFilter == null)
            {
                return;
            }

            // get content from cache content filter
            var content = cacheContentFilter.GetContent();

            // create and add response cache
            responseCache = new ResponseCache
            {
                Headers = context.Response.Headers,
                ContentType = context.Response.ContentType,
                Charset = context.Response.Charset,
                ContentEncoding = context.Response.ContentEncoding,
                Content = content,
                StatusCode = context.Response.StatusCode,
                StatusDescription = context.Response.StatusDescription
            };
            _cache.Add(
                cacheKey,
                responseCache,
                timeout);
        }
    }
}