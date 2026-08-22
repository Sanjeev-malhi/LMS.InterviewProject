using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Persistence.Configurations
{
    public class FailedEventConfiguration : IEntityTypeConfiguration<FailedEvents>
    {
        public void Configure(EntityTypeBuilder<FailedEvents> builder)
        {
            builder.ToTable("FailedEvent");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.QueueName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.MessageContent).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.CorrelationId).HasMaxLength(200);
        }
    }
}
