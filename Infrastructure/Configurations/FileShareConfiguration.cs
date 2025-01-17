using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    public class FileShareConfiguration : IEntityTypeConfiguration<Domain.Entities.FileShare>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.FileShare> builder)
        {
            builder.HasKey(fs => fs.Id);

            builder.HasOne(fs => fs.UserFile)
               .WithMany(f => f.SharedWithUsers)
               .HasForeignKey(fs => fs.UserFileId)
               .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(fs => fs.SharedWithUser)
                .WithMany()
                .HasForeignKey(fs => fs.SharedWithUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
