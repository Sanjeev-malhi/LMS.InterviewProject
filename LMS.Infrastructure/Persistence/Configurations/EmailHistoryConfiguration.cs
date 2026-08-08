using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Persistence.Configurations
{
    public class EmailHistoryConfiguration : IEntityTypeConfiguration<EmailHistory>
    {
        public void Configure(EntityTypeBuilder<EmailHistory> builder)
        {
            builder.ToTable("EmailHistory");
            builder.HasKey("Id");

            builder.Property(x => x.Email)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(x => x.Subject)
            .HasMaxLength(300)
            .IsRequired();

            builder.Property(x => x.Body)
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.ProviderMessageId)
                .HasMaxLength(200);

            builder.Property(x => x.ErrorMessage)
                .HasMaxLength(1000);

            builder.Property(x => x.Status)
                .HasConversion<byte>();

            builder.Property(x => x.EmailType)
                .HasConversion<byte>();

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.Status);

            builder.HasIndex(x => x.Email);

            builder.HasIndex(x => x.CreatedOn);

        }
    }
}
