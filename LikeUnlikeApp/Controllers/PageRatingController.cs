using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity.Owin;
using LikeUnlikeApp.Models.Rating;
using LikeUnlikeApp.Services;

namespace LikeUnlikeApp.Controllers
{
    [Authorize]
    public class PageRatingController : Controller
    {
        private static JavaScriptSerializer _serializer = new JavaScriptSerializer();
        private RatingService _ratingService;

        public PageRatingController()
        {
        }

        public PageRatingController(RatingService ratingService)
        {
            RatingService = ratingService;
        }

        public RatingService RatingService
        {
            get
            {
                return _ratingService ?? HttpContext.GetOwinContext().GetUserManager<RatingService>();
            }
            private set
            {
                _ratingService = value;
            }
        }

        //
        // GET: /PageRating/Info
        [AllowAnonymous]
        [HttpGet]
        public Task<string> Info(string key)
        {
            var resultData = RatingService.GetRatingByKey(key);

            return Task.FromResult(_serializer.Serialize(resultData));
        }

        [HttpPost]
        public Task<string> Like(string key)
        {
            var resultData = RatingService.IncreaseLikesByKey(key, System.Web.HttpContext.Current.User.Identity.Name);

            return Task.FromResult(_serializer.Serialize(resultData));
        }

        [HttpPost]
        public Task<string> UnLike(string key)
        {
            var resultData = RatingService.IncreaseDislikesByKey(key, System.Web.HttpContext.Current.User.Identity.Name);

            return Task.FromResult(_serializer.Serialize(resultData));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_ratingService != null)
                {
                    _ratingService.Dispose();
                    _ratingService = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}