namespace WebApplication1.Models
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
        public virtual DbSet<CrawlRule> CrawlRules { get; set; }
        public virtual DbSet<CrawlSite> CrawlSites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CrawlSite>()
                .Property(e => e.CrawlSiteUrl)
                .IsUnicode(false);
        }
    }
}
