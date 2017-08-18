namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CrawlDetail")]
    public partial class CrawlDetail
    {
        public int CrawlDetailId { get; set; }

        public int CrawlSiteId { get; set; }

        public int? CrawlDetailUrl { get; set; }

        public int? CrawlDetailTitle { get; set; }

        public int? CrawlDetailDescription { get; set; }

        public int? CrawlDetailImage { get; set; }

        public int? CrawlDetailContent { get; set; }
    }
}
