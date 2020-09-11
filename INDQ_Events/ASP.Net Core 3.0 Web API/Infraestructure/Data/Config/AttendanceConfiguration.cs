using ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Infraestructure.Data.Config
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasOne(ci => ci.Event)
                .WithMany()
                .HasForeignKey(ci => ci.EventId);

            //builder.HasOne(ci => ci.User)
            //    .WithMany()
            //    .HasForeignKey(ci => ci.UserId);
        }
    }
}
