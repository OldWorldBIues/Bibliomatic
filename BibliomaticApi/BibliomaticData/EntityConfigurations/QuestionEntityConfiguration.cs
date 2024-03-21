using BibliomaticData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliomaticData.EntityConfigurations
{
    public class QuestionEntityConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder
                .HasKey(q => q.Id);           

            builder
                .Property(q => q.Id)
                .ValueGeneratedNever();

            builder
                .Ignore(q => q.Author);

            builder
                .Ignore(q => q.QuestionCommentsCount);
          
            builder
               .Navigation(q => q.Formulas)
               .AutoInclude();

            builder
               .Navigation(q => q.Images)
               .AutoInclude();

            builder
               .Navigation(q => q.Hyperlinks)
               .AutoInclude();

            builder
               .Navigation(q => q.QuestionLikes)
               .AutoInclude();

            builder
              .Navigation(q => q.QuestionDislikes)
              .AutoInclude();           

            builder
                .HasMany(q => q.QuestionLikes)
                .WithOne()
                .HasForeignKey(q => q.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(q => q.QuestionDislikes)
               .WithOne()
               .HasForeignKey(q => q.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(q => q.QuestionComments)
               .WithOne()
               .HasForeignKey(q => q.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(q => q.Formulas)
                .WithOne()
                .HasForeignKey(f => f.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(q => q.Images)
               .WithOne()
               .HasForeignKey(f => f.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(q => q.Hyperlinks)
               .WithOne()    
               .HasForeignKey(f => f.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }    
}
