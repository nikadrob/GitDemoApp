using Demo.API.Contracts;
using Demo.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _db;

        public MovieRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Movie entity)
        {
            var movie = _db.Movies.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Movie entity)
        {
            _db.Movies.Remove(entity);
            return await Save();
        }

        public async Task<IList<Movie>> FindAll()
        {
            var movies = await _db.Movies.ToListAsync();
            return movies;
        }

        public async Task<Movie> FindById(int Id)
        {
            var movie = await _db.Movies.FindAsync(Id);
            return movie;
        }

        public Task<bool> IsExists(int Id)
        {
            return _db.Movies.AnyAsync(Q => Q.Id == Id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Movie entity)
        {
            _db.Movies.Update(entity);
            return await Save();
        }
    }
}
