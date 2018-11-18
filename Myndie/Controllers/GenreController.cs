﻿using Myndie.DAO;
using Myndie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Myndie.Controllers
{
    public class GenreController : Controller
    {
        // GET: Genre
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult Register()
        //{
        //    if (Session["ModId"] != null)
        //    {
        //        ViewBag.Genre = new Genre();
        //        return View();
        //    }
        //    return RedirectToAction("../Home/Index");
            
        //}

        public ActionResult Index()
        {
            if (Session["ModId"] != null)
            {
                GenreDAO dao = new GenreDAO();
                UserDAO udao = new UserDAO();
                ModeratorDAO mdao = new ModeratorDAO();
                ViewBag.Mod = mdao.SearchById(int.Parse(Session["ModId"].ToString()));
                User u = udao.SearchById(int.Parse(Session["Id"].ToString()));
                ViewBag.User = u;
                ViewBag.Genre = new Genre();
                ViewBag.Genres = dao.List();
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Index");
            }
        }

        public ActionResult Validate(Genre genre)
        {
            if (ModelState.IsValid)
            {
                GenreDAO dao = new GenreDAO();
                if (dao.IsUnique(genre))
                {
                    dao.Add(genre);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            ViewBag.Genre= genre;
            return View("Index");
        }
    }
}