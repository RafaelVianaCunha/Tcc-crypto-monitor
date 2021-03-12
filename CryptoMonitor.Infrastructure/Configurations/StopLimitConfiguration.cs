using CryptoMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoMonitor.Infrastructure.Configurations
{
    public class StopLimitConfiguration : IEntityTypeConfiguration<StopLimit>
    {
        public void Configure(EntityTypeBuilder<StopLimit> builder)
        {
            builder.ToTable("StopLimits");
            
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Stop);
            builder.Property(x => x.DeletedAt);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}