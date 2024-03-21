using BibliomaticData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BibliomaticData.EntityConfigurations
{
    public class BaseQuestionEntityConfiguration : IEntityTypeConfiguration<BaseQuestion>
    {
        public void Configure(EntityTypeBuilder<BaseQuestion> builder)
        {
            builder
               .HasKey(bq => bq.Id);

            builder
                .Property(bq => bq.Id)
                .ValueGeneratedNever();

            builder
               .Ignore(bq => bq.AnswersCount);

            builder
                .Ignore(bq => bq.Author);

            builder
                .Navigation(bq => bq.Answers)
                .AutoInclude();

            builder
                .Navigation(bq => bq.Question)
                .AutoInclude();

            builder
                .HasOne(bq => bq.Question)
                .WithOne(q => q.BaseQuestion)
                .HasForeignKey<Question>(q => q.BaseQuestionId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder
                .HasMany(bq => bq.Answers)
                .WithOne(a => a.BaseQuestion)
                .HasForeignKey(a => a.BaseQuestionId)
                .OnDelete(DeleteBehavior.Cascade);            
        }
    }
}
