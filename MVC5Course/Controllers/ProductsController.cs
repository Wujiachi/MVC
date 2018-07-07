using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using Omu.ValueInjecter;

namespace MVC5Course.Controllers
{
    public class ProductsController : Controller
    {
        private FabricsEntities1 db = new FabricsEntities1();

        // GET: Products
        public ActionResult Index()
        {
            var data = db.Product.Take(10).ToList();
            return View(data);
        }

        public ActionResult Index2()
        {
            var data = db.Product.OrderByDescending(p => p.ProductId).Select(p => new ProductViewModel() {
                ProductId = p.ProductId,
                Price = p.Price,
                ProductName = p.ProductName,
                Active = p.Active,
                Stock = p.Stock
            });
            return View(data);
        }

        public ActionResult AddData()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddData(ProductViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }
            Product one = new Product();
            one.ProductId = data.ProductId;
            one.Price = data.Price;
            one.Stock = data.Stock;
            one.ProductName = data.ProductName;
            one.Active = true;
            db.Product.Add(one);
            db.SaveChanges();

            return RedirectToAction("Index2");
        }


        public ActionResult edit1(int id)
        {
            var data = db.Product.Find(id);
            return View(data);
        }

        [HttpPost]
        public ActionResult edit1(int id, ProductViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }
            var one = db.Product.Find(id);
            one.InjectFrom(data);
            //one.Active = data.Active;
            //one.Price = data.Price;
            //one.ProductName = data.ProductName;
            //one.Stock = one.Stock;
            
            db.SaveChanges();

            return RedirectToAction("Index2");
        }

        public ActionResult remove1(int id)
        {
            var data = db.Product.Find(id);
            return View(data);
        }

        [HttpPost]
        public ActionResult remove1(int id, Product data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }
            var one = db.Product.Find(id);
            db.Product.Remove(one);
            db.SaveChanges();

            return RedirectToAction("Index2");
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
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
