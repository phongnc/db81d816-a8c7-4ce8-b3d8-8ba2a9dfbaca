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
    public class CrawlSitesController : Controller
    {
        private Model1 db = new Model1();

        // GET: CrawlSites
        public ActionResult Index()
        {
            return View(db.CrawlSites.ToList());
        }

        // GET: CrawlRules
        public ActionResult Rules(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int rule = 0; try { rule = db.CrawlRules.First(o => o.CrawlSiteId == id.Value && o.CrawlRuleFor == "Root").CrawlRuleId; } catch { }
            if (rule == 0)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "CrawlRules", new { id = rule });
        }

        // GET: CrawlSites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrawlSite crawlSite = db.CrawlSites.Find(id);
            if (crawlSite == null)
            {
                return HttpNotFound();
            }
            return View(crawlSite);
        }

        // GET: CrawlSites/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CrawlSites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CrawlSiteId,CrawlSiteUrl,CrawlSiteTitle,CrawlSiteNote")] CrawlSite crawlSite)
        {
            if (ModelState.IsValid)
            {
                db.CrawlSites.Add(crawlSite);
                db.SaveChanges();
                CrawlRule rule = new CrawlRule() { CrawlSiteId = crawlSite.CrawlSiteId, CrawlParentId = 0, CrawlRuleFor = "Root", CrawlRuleQuery = "" };
                db.CrawlRules.Add(rule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crawlSite);
        }

        // GET: CrawlSites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrawlSite crawlSite = db.CrawlSites.Find(id);
            if (crawlSite == null)
            {
                return HttpNotFound();
            }
            return View(crawlSite);
        }

        // POST: CrawlSites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CrawlSiteId,CrawlSiteUrl,CrawlSiteTitle,CrawlSiteNote")] CrawlSite crawlSite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crawlSite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crawlSite);
        }

        // GET: CrawlSites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrawlSite crawlSite = db.CrawlSites.Find(id);
            if (crawlSite == null)
            {
                return HttpNotFound();
            }
            return View(crawlSite);
        }

        // POST: CrawlSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CrawlSite crawlSite = db.CrawlSites.Find(id);
            db.CrawlSites.Remove(crawlSite);
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
