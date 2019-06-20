using System;
using LikeUnlikeApp.Models.Rating;

namespace LikeUnlikeApp.Services
{
    public class RatingService : IDisposable
    {
        private static CacheService _cacheService;

        public RatingService(CacheService cacheService)
        {
            CacheService = cacheService;
        }

        public static CacheService CacheService
        {
            get
            {
                return _cacheService ?? new CacheService();
            }
            private set
            {
                _cacheService = value;
            }
        }


        public static RatingService Create()
        {
            return new RatingService(CacheService);
        }

        public PageRatingModel GetRatingByKey(string key)
        {
            return CacheService.FindOrCreateDataByKey(key);
        }

        public PageRatingModel IncreaseLikesByKey(string key, string userName)
        {
            return CacheService.IncreaseLikesByKey(key, userName);
        }

        public PageRatingModel IncreaseDislikesByKey(string key, string userName)
        {
            return CacheService.IncreaseUnLikesByKey(key, userName);
        }

        public void Dispose()
        {
            _cacheService?.Dispose();
        }
    }
}