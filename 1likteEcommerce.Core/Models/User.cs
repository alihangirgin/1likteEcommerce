using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Models
{
    public class User  : IdentityUser
    {
        public int BasketId { get; set; }
        public virtual Basket Basket { get; set; }
        public string? PhotoPath { get; set; }
    }
}
