using BibliomaticData.Models;
using BibliomaticData.Models.AttachmentInfo;
using Microsoft.EntityFrameworkCore;

namespace BibliomaticData.AppContext
{
    public class BibliomaticAppContext : DbContext
    {
        public BibliomaticAppContext(DbContextOptions<BibliomaticAppContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BibliomaticAppContext).Assembly);

            modelBuilder.Entity<ArticleLike>(e => e.Property(al => al.Id).ValueGeneratedNever());
            modelBuilder.Entity<ArticleDislike>(e => e.Property(adl => adl.Id).ValueGeneratedNever());
            modelBuilder.Entity<ArticleComment>(e => e.Property(ac => ac.Id).ValueGeneratedNever());
            modelBuilder.Entity<ArticleComment>(e => e.Ignore(ac => ac.Author));

            modelBuilder.Entity<AnswerLike>(e => e.Property(al => al.Id).ValueGeneratedNever());
            modelBuilder.Entity<AnswerDislike>(e => e.Property(adl => adl.Id).ValueGeneratedNever());
            modelBuilder.Entity<AnswerComment>(e => e.Property(ac => ac.Id).ValueGeneratedNever());
            modelBuilder.Entity<AnswerComment>(e => e.Ignore(ac => ac.Author));

            modelBuilder.Entity<QuestionLike>(e => e.Property(ql => ql.Id).ValueGeneratedNever());
            modelBuilder.Entity<QuestionDislike>(e => e.Property(qdl => qdl.Id).ValueGeneratedNever());
            modelBuilder.Entity<QuestionComment>(e => e.Property(qc => qc.Id).ValueGeneratedNever());
            modelBuilder.Entity<QuestionComment>(e => e.Ignore(qc => qc.Author));

            modelBuilder.Entity<TestLike>(e => e.Property(tl => tl.Id).ValueGeneratedNever());
            modelBuilder.Entity<TestDislike>(e => e.Property(tdl => tdl.Id).ValueGeneratedNever());
            modelBuilder.Entity<TestComment>(e => e.Property(tc => tc.Id).ValueGeneratedNever());
            modelBuilder.Entity<TestComment>(e => e.Ignore(tc => tc.Author));
            modelBuilder.Entity<UserScore>(e => e.Property(us => us.Id).ValueGeneratedNever());
            modelBuilder.Entity<UserScore>(e => e.Ignore(tc => tc.User));
        }        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries();            

            foreach (var entry in entries)
            {                
                if (entry.Entity is BaseTrackedEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:                            
                            trackable.UpdatedAt = DateTime.Now;
                            entry.Property("CreatedAt").IsModified = false;
                            break;

                        case EntityState.Added:                            
                            trackable.CreatedAt = DateTime.Now;
                            trackable.UpdatedAt = DateTime.Now;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleLike> ArticleLikes { get; set; }
        public DbSet<ArticleDislike> ArticleDislikes { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<TestLike> TestLikes { get; set; }
        public DbSet<TestDislike> TestDislikes { get; set; }
        public DbSet<TestComment> TestComments { get; set; }
        public DbSet<UserScore> UserScores { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<TestAnswer> TestAnswers { get; set; }        

        public DbSet<BaseQuestion> BaseQuestions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionLike> QuestionLikes { get; set; }
        public DbSet<QuestionDislike> QuestionDislikes { get; set; }
        public DbSet<QuestionComment> QuestionComments { get; set; }
        public DbSet<QuestionImageInfo> QuestionImages { get; set; }
        public DbSet<QuestionFormulaInfo> QuestionFormulas { get; set; }
        public DbSet<QuestionHyperlinkInfo> QuestionHyperlinks { get; set; }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerLike> AnswerLikes { get; set; }
        public DbSet<AnswerDislike> AnswerDislikes { get; set; }
        public DbSet<AnswerComment> AnswerComments { get; set; }
        public DbSet<AnswerImageInfo> AnswerImages { get; set; }
        public DbSet<AnswerFormulaInfo> AnswerFormulas { get; set; }
        public DbSet<AnswerHyperlinkInfo> AnswerHyperlinks { get; set; }
    }
}
