using BibliomaticData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliomaticData.EntityConfigurations
{
    public class TestQuestionEntityConfiguration : IEntityTypeConfiguration<TestQuestion>
    {
        public void Configure(EntityTypeBuilder<TestQuestion> builder)
        {
            builder
              .HasKey(tq => tq.Id);

            builder
                .Property(tq => tq.Id)
                .ValueGeneratedNever();

            builder
                .Navigation(tq => tq.TestAnswers)
                .AutoInclude();

            builder
                .Navigation(tq => tq.Test)
                .AutoInclude();

            builder
                .HasMany(tq => tq.TestAnswers)
                .WithOne(ta => ta.TestQuestion)
                .HasForeignKey(tq => tq.TestQuestionId)
                .OnDelete(DeleteBehavior.Cascade);           
        }
    }
}
