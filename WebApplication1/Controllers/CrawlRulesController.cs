using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CrawlRulesController : Controller
    {
        private Model1 db = new Model1();

        // GET: CrawlRules
        public ActionResult Index(int? id)
        {
            if (id == null) id = 0;
            ViewBag.CrawlRuleId = id;
            return View(db.CrawlRules.Where(o => o.CrawlParentId == id).ToList());
        }

        // GET: CrawlRules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrawlRule crawlRule = db.CrawlRules.Find(id);
            if (crawlRule == null)
            {
                return HttpNotFound();
            }
            return View(crawlRule);
        }

        // GET: CrawlRules/Create
        public ActionResult Create(int? id)
        {
            if (id == null) id = 0;
            ViewBag.CrawlParentId = id;
            return View();
        }

        // POST: CrawlRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CrawlRuleId,CrawlSiteId,CrawlParentId,CrawlRuleFor,CrawlRuleQuery,CrawlRuleTag,CrawlRuleClass,CrawlRuleStart,CrawlRuleEnd,CrawlRuleIndex,CrawlRuleRegex,CrawlRuleFormat,CrawlRuleReplace,CrawlRuleJson,CrawlRuleNote")] CrawlRule crawlRule)
        {
            if (ModelState.IsValid)
            {
                db.CrawlRules.Add(crawlRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crawlRule);
        }

        // GET: CrawlRules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrawlRule crawlRule = db.CrawlRules.Find(id);
            if (crawlRule == null)
            {
                return HttpNotFound();
            }
            return View(crawlRule);
        }

        // POST: CrawlRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CrawlRuleId,CrawlSiteId,CrawlParentId,CrawlRuleFor,CrawlRuleQuery,CrawlRuleTag,CrawlRuleClass,CrawlRuleStart,CrawlRuleEnd,CrawlRuleIndex,CrawlRuleRegex,CrawlRuleFormat,CrawlRuleReplace,CrawlRuleJson,CrawlRuleNote")] CrawlRule crawlRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crawlRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crawlRule);
        }

        // GET: CrawlRules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrawlRule crawlRule = db.CrawlRules.Find(id);
            if (crawlRule == null)
            {
                return HttpNotFound();
            }
            return View(crawlRule);
        }

        // POST: CrawlRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CrawlRule crawlRule = db.CrawlRules.Find(id);
            db.CrawlRules.Remove(crawlRule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
