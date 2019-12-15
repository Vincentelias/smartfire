using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartfire.Model
{
    [Table("Emergency_contacts")]
    public partial class EmergencyContacts
    {
        [Key]
        public int Id { get; set; }
        [Column("Device_id")]
        public int? DeviceId { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [Column("phone_number")]
        [StringLength(255)]
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty(nameof(Devices.EmergencyContacts))]
        public virtual Devices Device { get; set; }
    }
}
