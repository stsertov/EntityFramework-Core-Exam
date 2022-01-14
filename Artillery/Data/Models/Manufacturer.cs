using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    [Table("Manufacturers")]
    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Guns = new HashSet<Gun>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(40), MinLength(4)]
        public string ManufacturerName { get; set; }

        [Required]
        [StringLength(100), MinLength(10)]
        public string Founded { get; set; }

        public IEnumerable<Gun> Guns { get; set; }
    }
}
