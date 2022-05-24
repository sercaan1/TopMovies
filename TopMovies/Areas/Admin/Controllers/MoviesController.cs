using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopMovies.Areas.Admin.Models;
using TopMovies.Models;

namespace TopMovies.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Admin/Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Admin/Movies/Create
        public IActionResult Create()
        {
            var vm = new CreateMovieViewModel() { Genres = _context.Genres.OrderBy(x => x.Name).ToList() };
            return View(vm);
        }

        // POST: Admin/Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMovieViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Movie movie = new Movie()
                {
                    Name = vm.Name,
                    Year = vm.Year,
                    Rating = vm.Rating,
                    ImageUrl = vm.ImageUrl,
                    ImdbId = vm.ImdbId,
                    Genres = new List<Genre>()
                };

                foreach (int genreId in vm.SelectedGenres)
                {
                    movie.Genres.Add(_context.Genres.Find(genreId));
                }
                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            vm.Genres = _context.Genres.OrderBy(x => x.Name).ToList();
            return View(vm);
        }

        // GET: Admin/Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.Include(x => x.Genres).FirstOrDefaultAsync(x => x.Id == id);
            EditMovieViewModel vm = new EditMovieViewModel();
            vm.ImageUrl = movie.ImageUrl;
            vm.Name = movie.Name;
            vm.Rating = movie.Rating;
            vm.ImdbId = movie.ImdbId;
            vm.Year = movie.Year;
            vm.Genres = _context.Genres.ToList();
            foreach (Genre genre in movie.Genres)
            {
                vm.SelectedGenres.Add(genre.Id);
            }
            if (movie == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        // POST: Admin/Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditMovieViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // veritabanından orjinal movieyi çek üzerindeki alanları güncelle.
                var movie = await _context.Movies.Include(x => x.Genres).FirstOrDefaultAsync(x => x.Id == id);
                if (movie == null)
                {
                    return NotFound();
                }

                movie.Name = vm.Name;
                movie.ImageUrl = vm.ImageUrl;
                movie.ImdbId = vm.ImdbId;
                movie.Rating = vm.Rating;
                movie.Year = vm.Year;
                movie.Genres.Clear();
                movie.Genres.AddRange(_context.Genres.Where(x => vm.SelectedGenres.Contains(x.Id)));

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm.Genres = await _context.Genres.ToListAsync();
            return View(vm);
        }

        // GET: Admin/Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Admin/Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
