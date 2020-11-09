using Demo.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Contracts
{
    interface IMovieRepository : IRepositoryBase<Movie>
    {
    }
}
