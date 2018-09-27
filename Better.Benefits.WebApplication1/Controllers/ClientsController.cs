using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Data;
using WebApplication1.Providers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ClientsController : Controller
    {
        private BenefitsDBProvider provider = new BenefitsDBProvider();

        // GET: Clients
        public async Task<ActionResult> Index()
        {
            return View(await provider.GetClientsAsync());
        }

        // GET: Clients/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var client = await provider.GetClientAsync(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "NameFirst,NameLast,NameTitle,WorkName,WorkPhone,HomePhone,CellPhone,AgentId,IdNumber,Id")] Person client)
        {
            if (ModelState.IsValid)
            {
                client.Id = Guid.NewGuid();
                await provider.NewClientAsync(client);
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var client = await provider.GetClientAsync(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "NameFirst,NameLast,NameTitle,WorkName,WorkPhone,HomePhone,CellPhone,AgentId,IdNumber,Id")] Person client)
        {
            if (ModelState.IsValid)
            {
                await provider.SavePersonAsync(client);
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var client = await provider.GetClientAsync(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            await provider.DeleteClientAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                provider.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}