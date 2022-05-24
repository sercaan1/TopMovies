using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopMovies.Dtos;
using TopMovies.Models;
using TopMovies.Services;

namespace TopMovies.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly FavoriteService _favoriteServices;

        public FavoritesController(AppDbContext db, FavoriteService favoriteServices)
        {
            _db = db;
            _favoriteServices = favoriteServices;
        }
        public IActionResult Index()
        {
            List<int> favs = _favoriteServices.GetFavList();
            List<Movie> movies = _db.Movies.Where(x => favs.Contains(x.Id)).ToList();

            return View(movies);
        }
        public IActionResult Toggle(int movieId)
        {
            List<int> favs = _favoriteServices.GetFavList();
            bool favorited;

            if (favs.Contains(movieId))
            {
                favs.Remove(movieId);
                favorited = false;
            }
            else
            {
                favs.Add(movieId);
                favorited = true;
            }

            _favoriteServices.SaveFavList(favs);

            return Json(new FavoriteToggleResult()
            {
                Favorited = favorited
            });
        }
    }
}
