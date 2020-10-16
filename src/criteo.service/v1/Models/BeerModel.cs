using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace criteo.service.v1.Models
{
    /// <summary>
    /// Represent a single Beer
    /// </summary>
    public class BeerModel
    {
        /// <summary>
        /// Uniquer identifier of the Beer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Band fo the Beer
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Type of the Beer
        /// </summary>
        public BeerType Type { get; set; }

        /// <summary>
        /// Quantity in centiliter of the Beer
        /// </summary>
        public int Quantity { get; set; }
    }
}
