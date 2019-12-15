using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartfire.Model
{
    public partial class Devices
    {
        public Devices()
        {
            ConnectedDevices = new HashSet<ConnectedDevices>();
            EmergencyContacts = new HashSet<EmergencyContacts>();
            Measurements = new HashSet<Measurements>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column("MAC")]
        [StringLength(255)]
        public string Mac { get; set; }
        [Column("Is_active")]
        public bool? IsActive { get; set; }
        [Column("Is_fire")]
        public bool? IsFire { get; set; }

        [InverseProperty("Device")]
        public virtual ICollection<ConnectedDevices> ConnectedDevices { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<EmergencyContacts> EmergencyContacts { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<Measurements> Measurements { get; set; }
    }
}
