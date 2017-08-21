﻿using AngleSharp.Parser.Html;
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
        Stack<CrawlLog> logs;
        public CrawlClient(string title)
        {
            site = db.CrawlSites.First(o => o.CrawlSiteTitle == title);
            rootrule = db.CrawlRules.First(o => o.CrawlSiteId == site.CrawlSiteId && o.CrawlRuleFor == "Root");
            List<CrawlRule> rules = db.CrawlRules.Where(o => o.CrawlParentId == rootrule.CrawlRuleId).ToList();
            logs = new Stack<CrawlLog>();
            if (string.IsNullOrEmpty(site.CrawlSiteUrl)) return;
            if (site.CrawlSiteUrl.Length < 2) return;
            if (rules.Count == 0) return;
            foreach (CrawlRule rule in rules) logs.Push(new CrawlLog() { CrawlLogUrl = site.CrawlSiteUrl, CrawlLogRuleId = rule.CrawlRuleId });
            while (logs.Count > 0)
                ProcessQueue();
        }
        void ProcessQueue()
        {
            var log = logs.Pop();
            var rule = db.CrawlRules.First(o => o.CrawlRuleId == log.CrawlLogRuleId);
            if (string.IsNullOrEmpty(log.CrawlLogUrl)) return;
            if (log.CrawlLogUrl.Length < 2) return;
            string content = "";
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                content = webClient.DownloadString(log.CrawlLogUrl);
                List<string> _result = RuleExcecute(rule, content);
                db.CrawlLogs.Add(new CrawlLog() {
                    CrawlLogUrl = log.CrawlLogUrl,
                    CrawlLogUrlEncode = Base64Encode(log.CrawlLogUrl),
                    CrawlLogRuleId = log.CrawlLogRuleId,
                    CrawlLogDate = DateTime.Now,
                    CrawlLogUnique = Base64Encode(log.CrawlLogUrl) + "@" + rule.CrawlRuleId.ToString()
                });
                if (_result.Count > 0)
                {
                    var subrules = db.CrawlRules.Where(o => o.CrawlParentId == log.CrawlLogRuleId);
                    for (int i = 0; i < _result.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(rule.CrawlRuleReplace))
                        {
                            try
                            {
                                string[] partOfCrawlRuleReplace = rule.CrawlRuleReplace.Split(new char[] { ',' });
                                _result[i] = _result[i].Replace(partOfCrawlRuleReplace[0], partOfCrawlRuleReplace[1]);
                            }
                            catch { }
                        }
                        if (subrules.Count() > 0)
                        {
                            try
                            {
                                foreach (CrawlRule subrule in subrules)
                                {
                                    if (!string.IsNullOrEmpty(rule.CrawlRuleJson) && rule.CrawlRuleJson.Contains("RUNONCE"))
                                    {
                                        string _urlUnique = Base64Encode(_result[i]) + "@" + subrule.CrawlRuleId.ToString();
                                        if (db.CrawlLogs.Count(o => o.CrawlLogUnique == _urlUnique) > 0) continue;
                                    }
                                    CrawlLog _log = new CrawlLog() { CrawlLogUrl = _result[i], CrawlLogRuleId = subrule.CrawlRuleId };
                                    logs.Push(_log);
                                }
                            }
                            catch (Exception ex) { }
                        }
                        else
                        {
                            db.CrawlLogs.Add(new CrawlLog()
                            {
                                CrawlLogUrl = _result[i],
                                CrawlLogUrlEncode = Base64Encode(_result[i]),
                                CrawlLogRuleId = 0,
                                CrawlLogDate = DateTime.Now,
                                CrawlLogUnique = Base64Encode(log.CrawlLogUrl) + "@0"
                            });
                        }
                    }
                }
                db.SaveChanges();
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
