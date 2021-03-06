﻿using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        SiteNote siteNote;
        Stack<CrawlLog> logs;

        public CrawlClient(string title)
        {
            try { site = db.CrawlSites.First(o => o.CrawlSiteTitle == title); }
            catch { return; }
            if (string.IsNullOrEmpty(site.CrawlSiteUrl)) return;
            if (site.CrawlSiteUrl.Length < 2) return;
            List<CrawlRule> rules = new List<CrawlRule>();
            try
            {
                var ruleRoot = db.CrawlRules.First(o => o.CrawlSiteId == site.CrawlSiteId && o.CrawlRuleFor == "Root");
                rules = db.CrawlRules.Where(o => o.CrawlParentId == ruleRoot.CrawlRuleId).ToList();
            }
            catch { }
            if (rules.Count == 0) return;
            siteNote = new SiteNote();
            try
            {
                CrawlRule ruleMenu = rules.First(o => o.CrawlRuleFor == "Menu");
                CrawlRule rulePage = rules.First(o => o.CrawlRuleFor == "Page");
                List<CrawlRule> otherRules = rules.Where(o => (o.CrawlRuleFor != "Menu") && (o.CrawlRuleFor != "Page") && (o.CrawlRuleFor != "File")).ToList();
                try
                {
                    if (rulePage.CrawlRuleNote.Contains("AUTOINC"))
                    {
                        siteNote.IsAutoInc = true;
                        siteNote.UrlFormat = rulePage.CrawlRuleFormat;
                    }
                    else if (rulePage.CrawlRuleNote.Contains("MANUAL"))
                    {
                        siteNote.IsAutoInc = false;
                        siteNote.UrlFormat = rulePage.CrawlRuleFormat;
                    }
                }
                catch { }
                using (WebClient webClient = new WebClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    string content = webClient.DownloadString(site.CrawlSiteUrl);
                    siteNote.MenuList = RuleExecute(ruleMenu, content);
                    siteNote.CurrentPage = 0;
                    siteNote.CurrentMenu = 0;
                    siteNote.NextPageString = "";
                    string crawlLogUrl = siteNote.OnNext(true);

                    logs = new Stack<CrawlLog>();
                    foreach (CrawlRule rule in otherRules)
                        if (!IsChecked(crawlLogUrl, rule.CrawlRuleId))
                            logs.Push(new CrawlLog() { CrawlLogUrl = crawlLogUrl, CrawlLogRuleId = rule.CrawlRuleId });
                    try
                    {
                        CrawlRule ruleFile = rules.First(o => o.CrawlRuleFor == "File");
                        logs.Push(new CrawlLog() { CrawlLogUrl = site.CrawlSiteUrl, CrawlLogRuleId = ruleFile.CrawlRuleId });
                    }
                    catch { }
                }
                while (logs.Count > 0)
                    try { ProcessQueue(); } catch { }
            }
            catch (Exception ex) { }
        }
        void ProcessQueue()
        {
            var log = logs.Pop();
            if (IsChecked(log.CrawlLogUrl, log.CrawlLogRuleId.Value)) return;
            var rule = db.CrawlRules.First(o => o.CrawlRuleId == log.CrawlLogRuleId);
            if (string.IsNullOrEmpty(log.CrawlLogUrl)) return;
            if (log.CrawlLogUrl.Length < 2) return;
            string content = "";
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                try { content = webClient.DownloadString(log.CrawlLogUrl); } catch { }
                List<string> _result = new List<string>();
                if (rule.CrawlRuleFor == "File")
                    _result = RuleExecute(rule.CrawlRuleFormat, log.CrawlLogUrl, content);
                else
                    _result = RuleExecute(rule, content);
                if (!IsChecked(log.CrawlLogUrl, log.CrawlLogRuleId.Value))
                {
                    db.CrawlLogs.Add(new CrawlLog()
                    {
                        CrawlLogUrl = log.CrawlLogUrl,
                        CrawlLogUrlEncode = Base64Encode(log.CrawlLogUrl),
                        CrawlLogRuleId = log.CrawlLogRuleId,
                        CrawlLogDate = DateTime.Now,
                        CrawlLogUnique = Base64Encode(log.CrawlLogUrl) + "@" + rule.CrawlRuleId.ToString()
                    });
                    Download(log.CrawlLogUrl);
                }
                if (_result.Count > 0)
                {
                    if (rule.CrawlRuleFor == "Paging")
                    {
                        string str = siteNote.OnNext(_result.Count > 0);
                        if (!string.IsNullOrEmpty(str))
                            if (!IsChecked(log.CrawlLogUrl, log.CrawlLogRuleId.Value))
                                logs.Push(new CrawlLog() { CrawlLogUrl = str, CrawlLogRuleId = rule.CrawlRuleId });
                    }
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
                                    CrawlLog _log = new CrawlLog() { CrawlLogUrl = _result[i], CrawlLogRuleId = subrule.CrawlRuleId };
                                    if (!IsChecked(log.CrawlLogUrl, log.CrawlLogRuleId.Value)) logs.Push(_log);
                                }
                            }
                            catch (Exception ex) { }
                        }
                        else
                        {
                            if (rule.CrawlRuleFor == "File")
                            {
                                CrawlLog _log = new CrawlLog() { CrawlLogUrl = _result[i], CrawlLogRuleId = rule.CrawlRuleId };
                                if (!IsChecked(_log.CrawlLogUrl, _log.CrawlLogRuleId.Value))
                                    logs.Push(_log);
                            }
                            else
                            if (!IsChecked(_result[i], 0))
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

                try { db.SaveChanges(); } catch (Exception ex) { }
            }
        }
        List<string> RuleExecute(CrawlRule rule, string content)
        {
            List<string> result = new List<string>();
            try
            {
                var parser = new HtmlParser();
                var document = parser.Parse(content);
                var list = document.Body.QuerySelectorAll(rule.CrawlRuleQuery);
                if (rule.CrawlRuleClass == "List")
                {
                    foreach (var item in list)
                    {
                        if (rule.CrawlRuleTag == "Anchor")
                        {
                            try
                            {
                                var _a = (AngleSharp.Dom.Html.IHtmlAnchorElement)item;
                                result.Add(_a.Href.Replace(@"about://", ""));
                            }
                            catch { }
                        }
                        else if (rule.CrawlRuleTag == "Image")
                        {
                            try
                            {
                                var _a = (AngleSharp.Dom.Html.IHtmlImageElement)item;
                                result.Add(_a.Source.Replace(@"about://", ""));
                            }
                            catch { }
                        }
                    }
                }
                else
                {

                }
            }
            catch { }
            return result;
        }
        List<string> RuleExecute(string baseurl, string url, string content)
        {
            string urlRegex = @"(\\)?(\""|\')+([\w\.\/\s-]+)[\w,\s-]+\.[\w]{2,5}(\\)?(\""|\')+";
            Regex regex = new Regex(urlRegex);
            MatchCollection matches= regex.Matches(content);
            List<string> results = new List<string>();
            try
            {
                Uri absolute = new Uri(url);
                for (int i = 0; i < matches.Count; i++)
                {
                    string str = matches[i].Value;
                    str = str.Replace("\"", "").Replace("'", "").Replace("/./", "/../");
                    if (str.StartsWith("./")) str = "." + str;
                    Uri result = new Uri(absolute, str);
                    if (str.StartsWith(".") || str.StartsWith("/"))
                        results.Add(result.ToString());
                    else
                        results.Add(baseurl + str);
                }
            }
            catch { }
            return results;
        }
        bool IsChecked(string crawlLogUrl, int crawlLogRuleId)
        {
            try
            {
                string unique = Base64Encode(crawlLogUrl) + "@" + crawlLogRuleId.ToString();
                if (db.CrawlLogs.Count(o => o.CrawlLogUnique == unique) > 0) return true;
            }
            catch { }
            if (IsIgnore(crawlLogUrl)) return true;
            return false;
        }
        bool IsIgnore(string crawlLogUrl)
        {
            try
            {
                var uri = new Uri(crawlLogUrl);
                if (uri.Host.Contains("google") || uri.Host.Contains("gstatic"))
                    return true;
            }
            catch { }
            return false;
        }
        void Download(string crawlLogUrl)
        {
            try
            {
                WebClient webClient = new WebClient();
                new System.IO.FileInfo(crawlLogUrl.Replace("http://","D:\\S\\").Replace("https://", "D:\\S\\")).Directory.Create();
                string fileName = crawlLogUrl.Replace("http://", "D:\\S\\").Replace("https://", "D:\\S\\");
                if (fileName.EndsWith("/"))
                { string content = webClient.DownloadString(new Uri(crawlLogUrl));
                    System.IO.StreamWriter file = new System.IO.StreamWriter(fileName += "index.html");
                    file.WriteLine(content);
                    file.Close();
                }
                else
                    webClient.DownloadFileAsync(new Uri(crawlLogUrl), fileName);
            }
            catch (Exception ex) { }
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
