using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Persistence.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("Enrollment");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TransactionId).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ErrorMessage).HasMaxLength(1000);
            builder.Property(x => x.Status).HasConversion<byte>();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.CourseId);

            builder.HasIndex(x => x.EventId).IsUnique();
            builder.HasIndex(x => x.TransactionId).IsUnique();
        }
    }
}
