using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LikeUnlikeApp.Models.Rating;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LikeUnlikeApp.Services
{
    public class CacheService : IDisposable
    {
        private static Dictionary<string, List<string>> _tempCacheLikes = new Dictionary<string, List<string>>();

        private static Dictionary<string, List<string>> _tempCacheUnLikes = new Dictionary<string, List<string>>();

        private object _locker = new object();
        

        public static CacheService Create()
        {
            return new CacheService();
        }

        public PageRatingModel FindOrCreateDataByKey(string key)
        {
            lock (_locker)
            {
                return FindOrCreateDataByKeyInternal(key);
            }
        }

        public PageRatingModel IncreaseLikesByKey(string key, string user)
        {
            lock (_locker)
            {
                if (!_tempCacheLikes.ContainsKey(key))
                {
                    _tempCacheLikes.Add(key, new List<string>());
                }

                if (!_tempCacheLikes[key].Contains(user))
                {
                    _tempCacheLikes[key].Add(user);
                }

                if (!_tempCacheUnLikes.ContainsKey(key))
                {
                    _tempCacheUnLikes.Add(key, new List<string>());
                }

                if (_tempCacheUnLikes[key].Contains(user))
                {
                    _tempCacheUnLikes[key].Remove(user);
                }

                return FindOrCreateDataByKeyInternal(key);
            }
        }

        public PageRatingModel IncreaseUnLikesByKey(string key, string user)
        {
            lock (_locker)
            {
                if (!_tempCacheUnLikes.ContainsKey(key))
                {
                    _tempCacheUnLikes.Add(key, new List<string>());
                }

                if (!_tempCacheUnLikes[key].Contains(user))
                {
                    _tempCacheUnLikes[key].Add(user);
                }

                if (!_tempCacheLikes.ContainsKey(key))
                {
                    _tempCacheLikes.Add(key, new List<string>());
                }

                if (_tempCacheLikes[key].Contains(user))
                {
                    _tempCacheLikes[key].Remove(user);
                }
                
                return FindOrCreateDataByKeyInternal(key);
            }
        }

        private PageRatingModel FindOrCreateDataByKeyInternal(string key)
        {
            if (!_tempCacheLikes.ContainsKey(key))
            {
                _tempCacheLikes.Add(key, new List<string>());
            }

            if (!_tempCacheUnLikes.ContainsKey(key))
            {
                _tempCacheUnLikes.Add(key, new List<string>());
            }

            return new PageRatingModel
            {
                LikesNumber = _tempCacheLikes[key].Count,
                UnLikesNumber = _tempCacheUnLikes[key].Count
            };
        }

        private PageRatingModel InitDefaultPageRatingModel()
        {
            return new PageRatingModel
            {
                LikesNumber = 0,
                UnLikesNumber = 0
            };
        }

        public void Dispose()
        {
            //_tempCacheDictionary = null;
        }
    }
}