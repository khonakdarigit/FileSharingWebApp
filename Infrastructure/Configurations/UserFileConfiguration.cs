using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Configurations
{
    public class UserFileConfiguration : IEntityTypeConfiguration<UserFile>
    {
        public void Configure(EntityTypeBuilder<UserFile> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(f => f.FilePath)
                .IsRequired();

            builder.Property(f => f.ContentType)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(f => f.UploadedBy)
                .WithMany(u => u.UserFiles)
                .HasForeignKey(f => f.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
