using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestUsers.Models;

namespace TestUsers.Controllers
{
    public class Item
    {
        private Motorcycle product = new Motorcycle();
        private int quantity;

        public Item()
        {

        }

        public Item(Motorcycle product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }

        public Motorcycle Product 
        {
            get { return product; }
            set { product = value; } 
        }
        public int Quantity 
        {
            get { return quantity; }
            set { quantity = value; }
        }
    }
}