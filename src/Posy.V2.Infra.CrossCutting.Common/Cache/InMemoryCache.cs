using Newtonsoft.Json;
using System;
using System.Runtime.Caching;

namespace Posy.V2.Infra.CrossCutting.Common.Cache
{
    public class CacheService : ICacheService
    {
        public T Get<T>(string key)
        {
            ObjectCache cache = MemoryCache.Default;
            return cache.Contains(key) ? (T)cache[key] : default(T);
        }

        public bool IsInCache(string key)
        {
            ObjectCache cache = MemoryCache.Default;
            return cache.Contains(key);
        }

        public bool Remove(string key)
        {
            try
            {
                ObjectCache cache = MemoryCache.Default;
                cache.Remove(key);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Set<T>(string key, T value)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Add(key, value, null);
        }

        public void Set<T>(string key, T value, TimeSpan timeout)
        {
            ObjectCache cache = MemoryCache.Default;
            var policy = new CacheItemPolicy
            {
                SlidingExpiration = timeout
            };

            if (cache.Contains(key))
            {
                cache.Remove(key);
            }

            cache.Add(key, value, policy);
        }

        private static readonly object CacheLockObject = new object();

        public T GetOrSet<T>(Func<T> getItemCallback, object dependsOn, TimeSpan duration) where T : class
        {
            string cacheKey = GetCacheKey(getItemCallback, dependsOn);
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                lock (CacheLockObject)
                {
                    if (item == null)
                    {
                        item = getItemCallback();

                        if (item != null)
                            MemoryCache.Default.Add(cacheKey, item, DateTime.Now.Add(duration));
                    }
                }
            }
            return item;
        }

        private string GetCacheKey<T>(Func<T> itemCallback, object dependsOn) where T : class
        {
            var serializedDependants = JsonConvert.SerializeObject(dependsOn);
            var methodType = itemCallback.GetType();
            return methodType.FullName + serializedDependants;
        }

        public T GetOrSet<T>(Func<T> getItemCallback, string key, TimeSpan duration) where T : class
        {
            string cacheKey = key;
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                lock (CacheLockObject)
                {
                    if (item == null)
                    {
                        item = getItemCallback();

                        if (item != null)
                            MemoryCache.Default.Add(cacheKey, item, DateTime.Now.Add(duration));
                    }
                }
            }
            return item;
        }

        //public string GetOrSetKey<T>(Func<T> getItemCallback, object dependsOn) where T : class
        //{
        //    return GetCacheKey(getItemCallback, dependsOn);
        //}

        #region OPCAO 1

        //public void Remove(string key)
        //{
        //    ObjectCache cache = MemoryCache.Default;
        //    cache.Remove(key);
        //}

        //public void Store(string key, object data)
        //{
        //    ObjectCache cache = MemoryCache.Default;
        //    cache.Add(key, data, null);
        //}

        //public void Store(string key, object data, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        //{
        //    ObjectCache cache = MemoryCache.Default;
        //    var policy = new CacheItemPolicy
        //    {
        //        AbsoluteExpiration = absoluteExpiration,
        //        SlidingExpiration = slidingExpiration
        //    };

        //    if (cache.Contains(key))
        //    {
        //        cache.Remove(key);
        //    }

        //    cache.Add(key, data, policy);
        //}

        //public T Retrieve<T>(string key)
        //{
        //    ObjectCache cache = MemoryCache.Default;
        //    return cache.Contains(key) ? (T)cache[key] : default(T);
        //}

        //public void Store(string key, object data, TimeSpan slidingExpiration)
        //{
        //    ObjectCache cache = MemoryCache.Default;
        //    var policy = new CacheItemPolicy
        //    {
        //        SlidingExpiration = slidingExpiration
        //    };

        //    if (cache.Contains(key))
        //    {
        //        cache.Remove(key);
        //    }

        //    cache.Add(key, data, policy);
        //}

        //public void Dispose()
        //{

        //}

        #endregion

        //private static readonly object CacheLockObject = new object();

        //public T Get<T>(string cacheKey) where T : class
        //{
        //    T item = MemoryCache.Default.Get(cacheKey) as T;
        //    return item;
        //}

        //public T GetOrSet<T>(Func<T> getItemCallback, object dependsOn, TimeSpan duration) where T : class
        //{
        //    string cacheKey = GetCacheKey(getItemCallback, dependsOn);
        //    T item = MemoryCache.Default.Get(cacheKey) as T;
        //    if (item == null)
        //    {
        //        lock (CacheLockObject)
        //        {
        //            item = getItemCallback();
        //            MemoryCache.Default.Add(cacheKey, item, DateTime.Now.Add(duration));
        //        }
        //    }
        //    return item;
        //}

        //private string GetCacheKey<T>(Func<T> itemCallback, object dependsOn) where T : class
        //{
        //    var serializedDependants = JsonConvert.SerializeObject(dependsOn);
        //    //var methodType = itemCallback.GetType();
        //    //return methodType.FullName + serializedDependants;
        //    return serializedDependants;
        //}

        // Usage https://stackoverflow.com/questions/343899/how-to-cache-data-in-a-mvc-application
        //var order = _cache.GetOrSet(
        //    () => _session.Set<Order>().SingleOrDefault(o => o.Id == orderId)
        //    , new { id = orderId }
        //    , new TimeSpan(0, 10, 0)
        //);

        //public TValue Get<TValue>(string cacheKey, int durationInMinutes, Func<TValue> getItemCallback) where TValue : class
        //{
        //    TValue item = MemoryCache.Default.Get(cacheKey) as TValue;
        //    if (item == null)
        //    {
        //        item = getItemCallback();
        //        MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(durationInMinutes));
        //    }
        //    return item;
        //}

        //public TValue Get<TValue, TId>(string cacheKeyFormat, TId id, int durationInMinutes, Func<TId, TValue> getItemCallback) where TValue : class
        //{
        //    string cacheKey = string.Format(cacheKeyFormat, id);
        //    TValue item = MemoryCache.Default.Get(cacheKey) as TValue;
        //    if (item == null)
        //    {
        //        item = getItemCallback(id);
        //        MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(durationInMinutes));
        //    }
        //    return item;
        //}
    }
}
