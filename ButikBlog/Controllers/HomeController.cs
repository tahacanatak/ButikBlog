﻿using ButikBlog.Models;
using ButikBlog.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ButikBlog.Controllers
{
    public class HomeController : BaseController
    {


        public ActionResult Index(int? cid, string slug , int page = 1)
        {
            int pageSize = 5;


            ViewBag.SubTitle = "Yazılarım"; // Default
            IQueryable<Post> result = db.Posts;
            Category cat = null;

            if (cid != null)
            {
                cat = db.Categories.Find(cid);
                if (cat == null)
                {
                    return HttpNotFound();
                }
                if (cat.Slug != slug)
                {
                    return RedirectToRoute("CategoryRoute", new { cid = cid, slug = cat.Slug, page = page});
                }


                result = result.Where(x => x.CategoryId == cid);
                ViewBag.SubTitle = cat.CategoryName;
            }
            ViewBag.page = page;
            ViewBag.pageCount = Math.Ceiling( result.Count() / (decimal)pageSize);
            ViewBag.nextPage = page + 1;
            ViewBag.prevPage = page - 1;
            ViewBag.cid = cid;

            return View(result.OrderByDescending(x => x.CreationTime)
                .Skip((page - 1) * pageSize) // skip kadar atla sıradaki Take kadar göster
                .Take(pageSize)
                .ToList());

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CategoriesPartial()
        {
            return PartialView("_CategoriesPartial", db.Categories.ToList());
        }

        public ActionResult ShowPost(int id, string slug)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }// eger adresteki slug veritabaninkiyle aynı değilse dogrusuna yönlendir
            if (post.Slug != slug)
            {
                return RedirectToRoute("PostRoute", new { id = id, slug = post.Slug });
            }


            return View(post);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SendComment(SendCommentViewModel model)
        {
            if(ModelState.IsValid)
            {
                Comment comment = new Comment
                {
                    ParentId = model.ParentId,
                    PostId = model.PostId,
                    AuthorId = User.Identity.GetUserId(),
                    AuthorName = model.AuthorName,
                    AuthorEmail = model.AuthorEmail,
                    Content = model.Content,
                    CreationTime = DateTime.Now                   
                };

                db.Comments.Add(comment);
                db.SaveChanges();
                return Json(comment);
            }

            var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();
            return Json( new { Errors = errorList});
        }
    }
}