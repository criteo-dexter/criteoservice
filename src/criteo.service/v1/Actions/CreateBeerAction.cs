using criteo.service.v1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace criteo.service.v1.Actions
{
    /// <summary>
    /// Represent the action to create a new Beer
    /// </summary>
    public class CreateBeerAction
    {
        /// <summary>
        /// Band fo the Beer
        /// </summary>
        [Required]
        [StringLength(maximumLength: 50)]
        public string Brand { get; set; }

        /// <summary>
        /// Type of the Beer
        /// </summary>
        [Required]
        public BeerType Type { get; set; }

        /// <summary>
        /// Quantity in centiliter of the Beer
        /// </summary>
        [Required]
        [Range(minimum: 25, maximum: 100)]
        public int Quantity { get; set; }
    }
}
