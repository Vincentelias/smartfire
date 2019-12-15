using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartfire.Model
{
    [Table("Connected_devices")]
    public partial class ConnectedDevices
    {
        [Key]
        public int Id { get; set; }
        [Column("Device_id")]
        public int? DeviceId { get; set; }
        [Column("Connected_device_id")]
        public int? ConnectedDeviceId { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty(nameof(Devices.ConnectedDevices))]
        public virtual Devices Device { get; set; }
    }
}
