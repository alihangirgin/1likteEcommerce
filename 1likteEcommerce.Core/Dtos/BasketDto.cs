using _1likteEcommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Dtos
{
    public class BasketDto : Dto
    {
        public string UserId { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
    }
}
