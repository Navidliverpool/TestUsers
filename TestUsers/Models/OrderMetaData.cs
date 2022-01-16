using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestUsers.Models
{
    public class OrderMetaData
    {

    }

    [MetadataType(typeof(OrderMetaData))]
    public partial class Order
    {
    }


}