using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class RuleBreadcrumbModel
    {
        int _Id;
        string _Title;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        public List<RuleBreadcrumbModel> getRuleBreadcrumb()
        {
            List<RuleBreadcrumbModel> result = new List<RuleBreadcrumbModel>();
            Model1 db = new Model1();
            int _id = Id;
            while (_id > 0)
            {
                var obj = db.CrawlRules.Where(o => o.CrawlRuleId == _id).First();
                result.Add(new RuleBreadcrumbModel() { Id = _id, Title = obj.CrawlRuleFor });
                _id = obj.CrawlParentId.Value;
            }
            return result;
        }
    }
}