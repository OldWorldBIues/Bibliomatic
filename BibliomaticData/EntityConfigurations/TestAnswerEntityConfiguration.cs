using BibliomaticData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibliomaticData.EntityConfigurations
{
    public class TestAnswerEntityConfiguration : IEntityTypeConfiguration<TestAnswer>
    {
        public void Configure(EntityTypeBuilder<TestAnswer> builder)
        {
            builder
               .HasKey(ta => ta.Id);

            builder
                .Navigation(tq => tq.TestQuestion)
                .AutoInclude();

            builder
               .Property(ta => ta.Id)               
               .ValueGeneratedNever();           
        }
    }
}
