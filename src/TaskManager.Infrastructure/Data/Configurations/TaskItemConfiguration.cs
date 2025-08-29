using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Infrastructure.Data.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(t => t.Id);
        
        // Configuration de TaskTitle comme propriété possédée (owned type)
        builder.OwnsOne(t => t.Title, titleBuilder =>
        {
            titleBuilder.Property(t => t.Value)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("Title");
        });

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Priority)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.DueDate);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();
    }
}