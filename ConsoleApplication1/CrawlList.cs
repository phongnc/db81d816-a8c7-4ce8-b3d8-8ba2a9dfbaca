namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CrawlList")]
    public partial class CrawlList
    {
        public int CrawlListId { get; set; }

        public int CrawlSiteId { get; set; }

        public int? CrawlListItem { get; set; }

        public int? CrawlListPage { get; set; }
    }
}
