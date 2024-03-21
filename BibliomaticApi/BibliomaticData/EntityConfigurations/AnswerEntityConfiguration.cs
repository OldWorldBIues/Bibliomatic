using BibliomaticData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliomaticData.EntityConfigurations
{
    public class AnswerEntityConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .Property(a => a.Id)
                .ValueGeneratedNever();

            builder
                .Ignore(a => a.Author);

            builder
                .Ignore(a => a.AnswerCommentsCount);       
            
            builder
               .Navigation(a => a.Formulas)
               .AutoInclude();

            builder
               .Navigation(a => a.Images)
               .AutoInclude();

            builder
               .Navigation(a => a.Hyperlinks)
               .AutoInclude();

            builder
               .Navigation(a => a.AnswerLikes)
               .AutoInclude();

            builder
              .Navigation(a => a.AnswerDislikes)
              .AutoInclude();
            
            builder
                .HasMany(a => a.AnswerLikes)
                .WithOne()
                .HasForeignKey(a => a.AnswerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(a => a.AnswerDislikes)
               .WithOne()
               .HasForeignKey(a => a.AnswerId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(a => a.AnswerComments)
               .WithOne()
               .HasForeignKey(a => a.AnswerId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(a => a.Formulas)
                .WithOne()
                .HasForeignKey(f => f.AnswerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(a => a.Images)
               .WithOne()
               .HasForeignKey(f => f.AnswerId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(a => a.Hyperlinks)
               .WithOne()
               .HasForeignKey(f => f.AnswerId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
