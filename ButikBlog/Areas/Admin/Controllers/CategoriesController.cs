using ButikBlog.Attributes;
using ButikBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ButikBlog.Areas.Admin.Controllers
{
    [Breadcrumb("Kategoriler")]
    public class CategoriesController : AdminBaseController
    {
        // GET: Admin/Categories
    [Breadcrumb("İndeks")]
        public ActionResult Index()
        {
            return View(db.Categories.OrderByDescending(x => x.CategoryName).ToList());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            if (category.Posts.Count > 0)
            {
                return Json(new { success = false, error = "Silmek istediğiniz kategoride yazılar bulunduğundan silinememektedir." });
            }
            db.Categories.Remove(category);
            db.SaveChanges();


            return Json(new { success = true });
        }

        [Breadcrumb("Yeni")]
        public ActionResult New()
        {
            return View("Edit", new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Yeni")]
        public ActionResult New(Category model)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(model);
                db.SaveChanges();
                TempData["successMessage"] = "Yeni kategori başarıyla oluşturuldu";

                return RedirectToAction("Index");
            }
            return View("Edit", model);
        }

        [Breadcrumb("Düzenle")]
        public ActionResult Edit(int id)
        {
            return View("Edit", db.Categories.Find(id));
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Düzenle")]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["successMessage"] = "Kategori ismi başarıyla değiştirildi";
                return RedirectToAction("Index");
            }

            return View("Edit", model);
        }

    }
}