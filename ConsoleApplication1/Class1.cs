using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ConsoleApplication1
{
    class ResultList
    {
        private List<string> items;
        private List<string> pages;
        public List<string> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }
        public List<string> Pages
        {
            get
            {
                return pages;
            }
            set
            {
                pages = value;
            }
        }

        public ResultList()
        {
            Items = new List<string>();
            Pages = new List<string>();
        }
    }
    class ResultDetail
    {
        private string url;
        private string title;
        private string description;
        private string image;
        private string content;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }
    }
    class CrawlClient
    {
        Model1 db = new Model1();
        CrawlSite site;
        CrawlRule rootrule;
        Queue<CrawlLog> logs;
        public CrawlClient(string title)
        {
            site = db.CrawlSites.First(o => o.CrawlSiteTitle == title);
            rootrule = db.CrawlRules.First(o => o.CrawlSiteId == site.CrawlSiteId && o.CrawlRuleFor == "Root");
            logs = new Queue<CrawlLog>();
            CrawlLog _log = new CrawlLog();
            _log.CrawlLogUrl = site.CrawlSiteUrl;
            _log.CrawlLogRuleId = rootrule.CrawlRuleId;
            logs.Enqueue(_log);
            while (logs.Count > 0)
                ProcessQueue();
        }
        void ProcessQueue()
        {
            var log = logs.Dequeue();
            var rule = db.CrawlRules.First(o => o.CrawlRuleId == log.CrawlLogRuleId);
            var subrules = db.CrawlRules.Where(o => o.CrawlParentId == rule.CrawlRuleId).ToList();
            if (subrules.Count > 0)
            {
                string content = "";
                using (WebClient webClient = new WebClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    content = webClient.DownloadString(log.CrawlLogUrl);
                    foreach (var subrule in subrules)
                    {
                        bool _needqueue = db.CrawlRules.Count(o => o.CrawlParentId == subrule.CrawlRuleId) > 0 ? true : false;
                        List<string> _result = RuleExcecute(subrule, content);
                        for (int i = 0; i < _result.Count; i++)
                        {
                            CrawlLog _log = new CrawlLog() { CrawlLogUrl = _result[i], CrawlLogRuleId = subrule.CrawlRuleId, CrawlLogUrlEncode = Base64Encode(_result[i]) };
                            if (_needqueue) logs.Enqueue(_log);
                            db.CrawlLogs.Add(_log);
                        }
                        db.SaveChanges();
                    }
                }
            }
        }
        List<string> RuleExcecute(CrawlRule rule, string content)
        {
            List<string> result = new List<string>();
            var parser = new HtmlParser();
            var document = parser.Parse(content);
            var list = document.Body.QuerySelectorAll(rule.CrawlRuleQuery);
            if (rule.CrawlRuleFor == "List")
            {
                foreach (var item in list)
                {
                    if (rule.CrawlRuleTag == "Anchor")
                    {
                        try
                        {
                            var _a = (AngleSharp.Dom.Html.IHtmlAnchorElement)item;
                            result.Add(_a.Href);
                        }
                        catch { }
                    }
                    else if (rule.CrawlRuleTag == "Image")
                    {
                        try
                        {
                            var _a = (AngleSharp.Dom.Html.IHtmlImageElement)item;
                            result.Add(_a.Source);
                        }
                        catch { }
                    }
                }
            }
            else
            {

            }
            return result;
        }
        string Base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
