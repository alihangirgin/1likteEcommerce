using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Models
{
    public class Basket : Entity
    {
        public int UserId { get; set; } 
        public virtual User User { get; set; }
        public virtual ICollection<BasketItem> BasketItems { get; set; }
    }
}
