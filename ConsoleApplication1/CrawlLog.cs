namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CrawlLog")]
    public partial class CrawlLog
    {
        public int CrawlLogId { get; set; }

        [StringLength(500)]
        public string CrawlLogUrl { get; set; }

        [StringLength(500)]
        public string CrawlLogUrlEncode { get; set; }

        public int? CrawlLogRuleId { get; set; }
    }
}
