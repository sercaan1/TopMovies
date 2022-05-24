using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopMovies.Services
{
    public class FavoriteService
    {
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-5.0#use-httpcontext-from-custom-components-1
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FavoriteService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public List<int> GetFavList()
        {
            var favs = _httpContextAccessor.HttpContext.Request.Cookies["favs"];

            if (string.IsNullOrEmpty(favs)) return new List<int>();

            try
            {
                return favs.Split('-').Select(x => Convert.ToInt32(x)).ToList();
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }
        public void SaveFavList(List<int> favorites)
        {
            string favs = string.Join('-', favorites);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("favs", favs, new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(10)
            });
        }
    }
}
