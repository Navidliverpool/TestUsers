﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestUsers.Models;

namespace TestUsers.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Motorcycle> MotorcyclesHomeVM { get; set; }
        public IEnumerable<Brand> BrandsHomeVM { get; set; }
    }
}