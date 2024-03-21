using BibliomaticData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliomaticData.EntityConfigurations
{
    public class ArticleEntityConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {  
            builder
                .HasKey(a => a.Id);

            builder
                .Property(a => a.Id)
                .ValueGeneratedNever();

            builder
                .Ignore(a => a.Author);

            builder
                .Ignore(a => a.ArticleCommentsCount);

            builder
             .Navigation(a => a.ArticleLikes)
             .AutoInclude();

            builder
              .Navigation(a => a.ArticleDislikes)
              .AutoInclude();            

            builder
                .HasMany(a => a.ArticleLikes)
                .WithOne()
                .HasForeignKey(a => a.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(a => a.ArticleDislikes)
               .WithOne()
               .HasForeignKey(a => a.ArticleId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(a => a.ArticleComments)
               .WithOne()
               .HasForeignKey(a => a.ArticleId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
