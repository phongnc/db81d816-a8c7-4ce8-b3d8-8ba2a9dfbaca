namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CrawlRule")]
    public partial class CrawlRule
    {
        public int CrawlRuleId { get; set; }

        public int? CrawlSiteId { get; set; }

        public int? CrawlParentId { get; set; }

        [StringLength(500)]
        public string CrawlRuleFor { get; set; }

        [StringLength(500)]
        public string CrawlRuleQuery { get; set; }

        [StringLength(500)]
        public string CrawlRuleTag { get; set; }

        [StringLength(500)]
        public string CrawlRuleClass { get; set; }

        [StringLength(500)]
        public string CrawlRuleStart { get; set; }

        [StringLength(500)]
        public string CrawlRuleEnd { get; set; }

        public int? CrawlRuleIndex { get; set; }

        [StringLength(500)]
        public string CrawlRuleRegex { get; set; }

        [StringLength(500)]
        public string CrawlRuleFormat { get; set; }

        [StringLength(500)]
        public string CrawlRuleReplace { get; set; }

        [StringLength(500)]
        public string CrawlRuleJson { get; set; }

        [StringLength(500)]
        public string CrawlRuleNote { get; set; }
    }
}
