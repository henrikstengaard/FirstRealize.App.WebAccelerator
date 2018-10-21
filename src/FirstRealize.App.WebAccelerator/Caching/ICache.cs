using System;

namespace FirstRealize.App.WebAccelerator.Caching
{
    public interface ICache
    {
        void Add(
            string key,
            object value,
            TimeSpan timeout);
        object Get(
            string key);
    }
}