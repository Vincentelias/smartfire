using Microsoft.EntityFrameworkCore;

namespace smartfire.Model
{
    public partial class smartfireContext : DbContext
    {
        public smartfireContext()
        {
        }

        public smartfireContext(DbContextOptions<smartfireContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConnectedDevices> ConnectedDevices { get; set; }
        public virtual DbSet<Devices> Devices { get; set; }
        public virtual DbSet<EmergencyContacts> EmergencyContacts { get; set; }
        public virtual DbSet<Measurements> Measurements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:smartfire.database.windows.net,1433;Initial Catalog=smartfire;Persist Security Info=False;User ID=smartfire;Password=Thebteam1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConnectedDevices>(entity =>
            {
                entity.HasOne(d => d.Device)
                    .WithMany(p => p.ConnectedDevices)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_DeviceConnectedDevice");
            });

            modelBuilder.Entity<Devices>(entity =>
            {
                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsFire).HasDefaultValueSql("((0))");

                entity.Property(e => e.Mac).IsUnicode(false);
            });

            modelBuilder.Entity<EmergencyContacts>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.EmergencyContacts)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_DeviceEmergencyContact");
            });

            modelBuilder.Entity<Measurements>(entity =>
            {
                entity.Property(e => e.Event).IsUnicode(false);
                entity.Property(e => e.MeasuredOn).IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Measurements)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_DeviceMeasurement");


            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
