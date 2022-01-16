using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestUsers.Models
{
    public class OrderDetailMetaData
    {
    }

    [MetadataType(typeof(OrderDetailMetaData))]
    public partial class OrderDetail
    {
    }
}