using Newtonsoft.Json;
using Posy.V2.Infra.CrossCutting.Common.Cache.Configuration;
using ServiceStack.Redis;
using System;

namespace Posy.V2.Infra.CrossCutting.Common.Cache
{
    public class RedisCacheProvider : ICacheService
    {
        RedisEndpoint _endPoint;

        public RedisCacheProvider()
        {
            var rc = RedisConfigurationManager.Config;

            _endPoint = new RedisEndpoint(
                RedisConfigurationManager.Config.Host,
                RedisConfigurationManager.Config.Port,
                RedisConfigurationManager.Config.Password,
                RedisConfigurationManager.Config.DatabaseID);
        }

        public void Set<T>(string key, T value)
        {
            this.Set(key, value, TimeSpan.Zero);
        }

        public void Set<T>(string key, T value, TimeSpan timeout)
        {
            using (RedisClient client = new RedisClient(_endPoint))
            {
                client.As<T>().SetValue(key, value, timeout);
            }
        }

        public T Get<T>(string key)
        {
            T result = default(T);

            using (RedisClient client = new RedisClient(_endPoint))
            {
                var wrapper = client.As<T>();

                result = wrapper.GetValue(key);
            }

            return result;
        }

        public bool Remove(string key)
        {
            bool removed = false;

            using (RedisClient client = new RedisClient(_endPoint))
            {
                removed = client.Remove(key);
            }

            return removed;
        }

        public bool IsInCache(string key)
        {
            bool isInCache = false;

            using (RedisClient client = new RedisClient(_endPoint))
            {
                isInCache = client.ContainsKey(key);
            }

            return isInCache;
        }

        private static readonly object CacheLockObject = new object();

        public T GetOrSet<T>(Func<T> getItemCallback, object dependsOn, TimeSpan duration) where T : class
        {
            using (RedisClient client = new RedisClient(_endPoint))
            {
                var wrapper = client.As<T>();

                string cacheKey = GetCacheKey(getItemCallback, dependsOn);
                T item = wrapper.GetValue(cacheKey) as T;
                if (item == null)
                {
                    lock (CacheLockObject)
                    {
                        if (item == null)
                        {
                            item = getItemCallback();

                            if (item != null)
                                client.As<T>().SetValue(cacheKey, item, duration);
                        }
                    }
                }
                return item;
            }
        }

        private string GetCacheKey<T>(Func<T> itemCallback, object dependsOn) where T : class
        {
            var serializedDependants = JsonConvert.SerializeObject(dependsOn);
            var methodType = itemCallback.GetType();
            return methodType.FullName + serializedDependants;
        }

        public T GetOrSet<T>(Func<T> getItemCallback, string key, TimeSpan duration) where T : class
        {
            using (RedisClient client = new RedisClient(_endPoint))
            {
                var wrapper = client.As<T>();

                string cacheKey = key;
                T item = wrapper.GetValue(cacheKey) as T;
                if (item == null)
                {
                    lock (CacheLockObject)
                    {
                        if (item == null)
                        {
                            item = getItemCallback();

                            if (item != null)
                                client.As<T>().SetValue(cacheKey, item, duration);
                        }
                    }
                }
                return item;
            }
        }

        //public string GetOrSetKey<T>(Func<T> getItemCallback, object dependsOn) where T : class
        //{
        //    return GetCacheKey(getItemCallback, dependsOn);
        //}
    }
}
