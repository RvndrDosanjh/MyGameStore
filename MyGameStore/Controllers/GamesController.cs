using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyGameStore;

namespace MyGameStore.Controllers
{
    public class GamesController : Controller
    {
        private GameStoreEntities db = new GameStoreEntities();

        // GET: Games
        public async Task<ActionResult> Index()
        {
            var games = db.Games.Include(g => g.Category).Include(g => g.Company);
            return View(await games.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = await db.Games.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GameId,Name,GameFile,PhotoFile,UploadedAt,CompanyId,CategoryId,ReleaseDate,Description")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.PhotoFile.SaveAs(Server.MapPath($"\\photos\\{game.PhotoFile.FileName}"));
                game.GameFile.SaveAs(Server.MapPath($"\\files\\{game.GameFile.FileName}"));
                game.Photo = game.PhotoFile.FileName;
                game.Filename = game.GameFile.FileName;
                db.Games.Add(game);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", game.CategoryId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", game.CompanyId);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = await db.Games.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", game.CategoryId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", game.CompanyId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GameId,Name,GameFile,PhotoFile,UploadedAt,CompanyId,CategoryId,ReleaseDate,Description")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.PhotoFile.SaveAs(Server.MapPath($"\\photos\\{game.PhotoFile.FileName}"));
                game.GameFile.SaveAs(Server.MapPath($"\\files\\{game.GameFile.FileName}"));
                game.Photo = game.PhotoFile.FileName;
                game.Filename = game.GameFile.FileName;
                db.Entry(game).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", game.CategoryId);
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "CompanyName", game.CompanyId);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = await db.Games.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Game game = await db.Games.FindAsync(id);
            db.Games.Remove(game);
            await db.SaveChangesAsync();
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
