using _1likteEcommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Dtos
{
    public class BasketItemDto : Dto
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public virtual ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
