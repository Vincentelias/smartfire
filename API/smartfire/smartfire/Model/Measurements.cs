using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartfire.Model
{
    public partial class Measurements
    {
        [Key]
        public int Id { get; set; }
        [Column("Device_id")]
        public int? DeviceId { get; set; }
        [Column("Event_id")]
        public int? EventId { get; set; }
        public double? Temperature { get; set; }
        [Column("Measured_on")]
        
        public DateTime MeasuredOn { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty(nameof(Devices.Measurements))]
        public virtual Devices Device { get; set; }
        [ForeignKey(nameof(EventId))]
        [InverseProperty(nameof(Events.Measurements))]
        public virtual Events Event { get; set; }
    }
}
