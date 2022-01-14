using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    [Table("Shells")]
    public class Shell
    {
        public Shell()
        {
            this.Guns = new HashSet<Gun>();
        }
        public int Id { get; set; }

        [Range(2d, 1680d)]
        public double ShellWeight { get; set; }

        [Required]
        [StringLength(30), MinLength(4)]
        public string Caliber { get; set; }

        public IEnumerable<Gun> Guns { get; set; }
    }
}
