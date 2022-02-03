﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestUsers.Models;

namespace TestUsers.ViewModels
{
    public class ShoppingCartVM
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}