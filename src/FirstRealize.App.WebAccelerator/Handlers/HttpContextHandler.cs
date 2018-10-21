using FirstRealize.App.WebAccelerator.Caching;
using FirstRealize.App.WebAccelerator.Filters;
using FirstRealize.App.WebAccelerator.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace FirstRealize.App.WebAccelerator.Handlers
{
    public class HttpContextHandler
    {
        private readonly ICache _cache;

        public HttpContextHandler(
            ICache cache)
        {
            _cache = cache;
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

            var keys = new List<string>();
            for (var enumerator = context.Response.Headers.Keys.GetEnumerator(); enumerator.MoveNext();)
            {
                keys.Add(enumerator.Current.ToString());
            }

            foreach (var key in keys)
            {
                context.Response.Headers.Remove(key);
            }

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
            ICache cache,
            TimeSpan timeout)
        {
            var cacheKey = context.Request.Url.AbsoluteUri;
            var cacheResponse = HttpRuntime.Cache.Get(cacheKey) as ResponseCache;

            if (cacheResponse != null)
            {
                return;
            }

            var cacheContentFilter = context.Response.Filter as CacheContentFilter;

            if (cacheContentFilter == null)
            {
                return;
            }

            var content = cacheContentFilter.GetContent();

            cacheResponse = new ResponseCache
            {
                Headers = context.Response.Headers,
                ContentType = context.Response.ContentType,
                Charset = context.Response.Charset,
                ContentEncoding = context.Response.ContentEncoding,
                Content = content,
                StatusCode = context.Response.StatusCode,
                StatusDescription = context.Response.StatusDescription
            };

            cache.Add(
                cacheKey,
                cacheResponse,
                timeout);
        }
    }
}