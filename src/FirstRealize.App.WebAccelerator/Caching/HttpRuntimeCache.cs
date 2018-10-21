using System;
using System.Web;
using System.Web.Caching;

namespace FirstRealize.App.WebAccelerator.Caching
{
    public class HttpRuntimeCache : ICache
    {
        private static Lazy<HttpRuntimeCache> _current =
            new Lazy<HttpRuntimeCache>(() =>
            {
                return new HttpRuntimeCache();
            }, true);

        public static HttpRuntimeCache Current
        {
            get
            {
                return _current.Value;
            }
        }

        public void Add(
            string key,
            object value,
            TimeSpan timeout)
        {
            HttpRuntime.Cache.Insert(
                key,
                value,
                null,
                Cache.NoAbsoluteExpiration,
                timeout);
        }

        public object Get(
            string key)
        {
            return HttpRuntime.Cache.Get(key);
        }
    }
}