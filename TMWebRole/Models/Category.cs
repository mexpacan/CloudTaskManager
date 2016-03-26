using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMWebRole.Models
{
    public class Category
    {
        /// <summary>
        /// This is the Entity representing a Category
        /// </summary>
        public int CategoryId { get; set; }

        [Required()]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Category")]
        public string Name { get; set; }
    }
}