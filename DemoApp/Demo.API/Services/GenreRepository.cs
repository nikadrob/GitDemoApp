using Demo.API.Contracts;
using Demo.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Services
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _db;

        public GenreRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Genre entity)
        {
            var genre = _db.Genres.AddAsync(entity);
            return await Save();
        }
        public async Task<bool> Delete(Genre entity)
        {
            _db.Genres.Remove(entity);
            return await Save();
        }

        public async Task<IList<Genre>> FindAll()
        {
            var genres = await _db.Genres.ToListAsync();
            return genres;
        }

        public async Task<Genre> FindById(int Id)
        {
            var genre = await _db.Genres.FindAsync(Id);
            return genre;
        }

        public Task<bool> IsExists(int Id)
        {
            return _db.Genres.AnyAsync(Q => Q.Id == Id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Genre entity)
        {
            _db.Genres.Update(entity);
            return await Save();
        }
    }
}
