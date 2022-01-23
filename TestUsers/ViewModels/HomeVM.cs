using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestUsers.Models;

namespace TestUsers.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Motorcycle> MotorcyclesHomeVM { get; set; }
        public IEnumerable<Brand> BrandsHomeVM { get; set; }
        public IEnumerable<Motorcycle> StreetBikesHomeVM { get; set; }
        public IEnumerable<Motorcycle> SportBikesHomeVM { get; set; }
        public IEnumerable<Motorcycle> AdventureBikesHomeVM { get; set; }

        public IEnumerable<Motorcycle> ScooterBikesHomeVM { get; set; }

    }
}