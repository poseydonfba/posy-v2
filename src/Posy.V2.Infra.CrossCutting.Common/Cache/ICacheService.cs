using System;

namespace Posy.V2.Infra.CrossCutting.Common.Cache
{
    // https://www.codeproject.com/Articles/1120038/%2FArticles%2F1120038%2FA-simple-Csharp-cache-component-using-Redis-as-pro
    public interface ICacheService
    {
        void Set<T>(string key, T value);

        void Set<T>(string key, T value, TimeSpan timeout);

        T Get<T>(string key);

        bool Remove(string key);

        bool IsInCache(string key);

        T GetOrSet<T>(Func<T> getItemCallback, object dependsOn, TimeSpan duration) where T : class;
        T GetOrSet<T>(Func<T> getItemCallback, string key, TimeSpan duration) where T : class;
        //string GetOrSetKey<T>(Func<T> getItemCallback, object dependsOn) where T : class;
    }


    //    public interface ICacheService : IDisposable
    //    {
    //        //TValue Get<TValue>(string cacheKey, Func<TValue> getItemCallback) where TValue : class;
    //        //TValue Get<TValue, TId>(string cacheKeyFormat, TId id, Func<TId, TValue> getItemCallback) where TValue : class;

    //        //T Get<T>(string cacheKey) where T : class;
    //        //T GetOrSet<T>(Func<T> getItemCallback, object dependsOn, TimeSpan duration) where T : class;

    //        void Remove(string key);
    //        void Store(string key, object data);
    //        void Store(string key, object data, TimeSpan slidingExpiration);
    //        void Store(string key, object data, DateTime absoluteExpiration, TimeSpan slidingExpiration);
    //        T Retrieve<T>(string key);
    //    }
}
