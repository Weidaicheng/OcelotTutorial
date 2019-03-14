using System;
using System.Collections.Generic;
using System.Linq;
using Ocelot.Cache;

namespace OcelotGetway
{
    public class MyCache : IOcelotCache<CachedResponse>
    {
        private static Dictionary<string, CacheObj> _cacheObjs = new Dictionary<string, CacheObj>();
        
        public void Add(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            if (!_cacheObjs.ContainsKey($"{region}_{key}"))
            {
                _cacheObjs.Add($"{region}_{key}", new CacheObj()
                {
                    ExpireTime = DateTime.Now.Add(ttl),
                    Response = value
                });
            }
        }

        public CachedResponse Get(string key, string region)
        {
            if (!_cacheObjs.ContainsKey($"{region}_{key}")) return null;
            
            var cacheObj = _cacheObjs[$"{region}_{key}"];
            if (cacheObj != null && cacheObj.ExpireTime >= DateTime.Now)
            {
                return cacheObj.Response;
            }

            _cacheObjs.Remove($"{region}_{key}");
            return null;

        }

        public void ClearRegion(string region)
        {
            var keysToRemove = _cacheObjs.Where(c => c.Key.StartsWith($"{region}_"))
                .Select(c => c.Key)
                .ToList();
            foreach (var key in keysToRemove)
            {
                _cacheObjs.Remove(key);
            }

        }

        public void AddAndDelete(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            if (_cacheObjs.ContainsKey($"{region}_{key}"))
            {
                _cacheObjs.Remove($"{region}_{key}");
            }
            
            _cacheObjs.Add($"{region}_{key}", new CacheObj()
            {
                ExpireTime = DateTime.Now.Add(ttl),
                Response = value
            });
        }
    }

    public class CacheObj
    {
        public DateTime ExpireTime { get; set; }

        public CachedResponse Response { get; set; }
    }
}