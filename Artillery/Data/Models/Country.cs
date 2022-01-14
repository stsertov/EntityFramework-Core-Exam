using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    [Table("Countries")]
    public class Country
    {
        public Country()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(60), MinLength(4)]
        public string CountryName { get; set; }

        [Range(50000, 10000000)]
        public int ArmySize { get; set; }

        public IEnumerable<CountryGun> CountriesGuns { get; set; }
    }
}
