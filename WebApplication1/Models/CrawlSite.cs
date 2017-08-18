namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CrawlSite")]
    public partial class CrawlSite
    {
        public int CrawlSiteId { get; set; }

        [StringLength(500)]
        public string CrawlSiteUrl { get; set; }

        [StringLength(500)]
        public string CrawlSiteTitle { get; set; }

        public string CrawlSiteNote { get; set; }
    }
}
