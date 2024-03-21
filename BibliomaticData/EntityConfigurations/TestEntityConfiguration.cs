using BibliomaticData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliomaticData.EntityConfigurations
{
    public class TestEntityConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder
               .HasKey(t => t.Id);

            builder
                .Property(t => t.Id)
                .ValueGeneratedNever();

            builder
               .Ignore(t => t.Author);

            builder
                .Ignore(t => t.TestCommentsCount);

            builder
                .Ignore(t => t.TestUserScoresCount);

            builder
                .Navigation(t => t.TestQuestions)
                .AutoInclude();

            builder
               .Navigation(t => t.TestLikes)
               .AutoInclude();

            builder
              .Navigation(t => t.TestDislikes)
              .AutoInclude();           

            builder
                .HasMany(t => t.TestQuestions)
                .WithOne(tq => tq.Test)
                .HasForeignKey(tq => tq.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(t => t.TestLikes)
                .WithOne()
                .HasForeignKey(tl => tl.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(t => t.TestDislikes)
               .WithOne()
               .HasForeignKey(tdl => tdl.TestId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(t => t.TestComments)
               .WithOne()
               .HasForeignKey(tl => tl.TestId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(t => t.UserScores)
               .WithOne()
               .HasForeignKey(tl => tl.TestId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
