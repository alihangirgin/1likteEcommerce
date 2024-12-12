using _1likteEcommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Dtos
{
    public class CategoryDto : Dto
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
