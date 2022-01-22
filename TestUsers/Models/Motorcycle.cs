//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestUsers.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Motorcycle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Motorcycle()
        {
            this.Dealers = new HashSet<Dealer>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int MotorcycleId { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public Nullable<int> BrandId { get; set; }
        public byte[] Image { get; set; }
        public string Type { get; set; }
        public Nullable<int> CategoryId { get; set; }
    
        public virtual Brand Brand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dealer> Dealers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Category Category { get; set; }
    }
}
