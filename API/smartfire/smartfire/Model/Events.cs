using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartfire.Model
{
    public partial class Events
    {
        public Events()
        {
            Measurements = new HashSet<Measurements>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        [InverseProperty("Event")]
        public virtual ICollection<Measurements> Measurements { get; set; }
    }
}
