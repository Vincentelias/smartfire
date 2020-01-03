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
        
        public string? Event { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? Co_percentage{ get; set; }
        public double? Gas_percentage { get; set; }


        [Column("Measured_on")]
        public DateTime MeasuredOn { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty(nameof(Devices.Measurements))]
        public virtual Devices Device { get; set; }
    }
}
