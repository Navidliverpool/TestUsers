using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TestUsers.Models;

namespace TestUsers.Data.Repositories
{
    public interface IMotorcycleRepository
    {
        IQueryable<Motorcycle> GetMotorcycles();
        IQueryable<Motorcycle> GetMotorcyclesIncludeBrandsCategories();
        Task<Motorcycle> GetMotorcycleById(int? id);

        Motorcycle AddMotorcycle(Motorcycle motorcycle);
        void SaveChanges();
        Motorcycle GetMotorcycleIncludeItsDealers(int? id);
    }
}