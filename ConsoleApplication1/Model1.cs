namespace ConsoleApplication1
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<CrawlDetail> CrawlDetails { get; set; }
        public virtual DbSet<CrawlList> CrawlLists { get; set; }
        public virtual DbSet<CrawlLog> CrawlLogs { get; set; }
        public virtual DbSet<CrawlRule> CrawlRules { get; set; }
        public virtual DbSet<CrawlSite> CrawlSites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CrawlLog>()
                .Property(e => e.CrawlLogUrlEncode)
                .IsUnicode(false);

            modelBuilder.Entity<CrawlLog>()
                .Property(e => e.CrawlLogUnique)
                .IsUnicode(false);

            modelBuilder.Entity<CrawlLog>()
                .Property(e => e.CrawlLogType)
                .IsUnicode(false);

            modelBuilder.Entity<CrawlLog>()
                .Property(e => e.CrawlLogDate)
                .HasPrecision(0);

            modelBuilder.Entity<CrawlSite>()
                .Property(e => e.CrawlSiteUrl)
                .IsUnicode(false);
        }
    }
}
