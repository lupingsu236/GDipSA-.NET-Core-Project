using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class Selected
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public int quantity { get; set; }
        public virtual Product Product { get; set; }
    }
}
