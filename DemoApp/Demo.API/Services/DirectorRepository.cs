using Demo.API.Contracts;
using Demo.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Services
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly ApplicationDbContext _db;

        public DirectorRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Director entity)
        {
            var director = _db.Directors.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Director entity)
        {
            _db.Directors.Remove(entity);
            return await Save();
        }

        public async Task<IList<Director>> FindAll()
        {
            var directors = await _db.Directors.ToListAsync();
            return directors;
        }

        public async Task<Director> FindById(int Id)
        {
            var director = await _db.Directors.FindAsync(Id);
            return director;
        }

        public Task<bool> IsExists(int Id)
        {
            return _db.Directors.AnyAsync(Q => Q.Id == Id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Director entity)
        {
            _db.Directors.Update(entity);
            return await Save();
        }
    }
}
