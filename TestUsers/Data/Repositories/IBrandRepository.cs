using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUsers.Models;

namespace TestUsers.Data.Repositories
{
    public interface IBrandRepository
    {
        IQueryable<Brand> GetBrands();
    }
}
