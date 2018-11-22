﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Myndie.DAO;
using Myndie.Models;

namespace Myndie.Controllers
{
    public class SellController : Controller
    {
        // GET: Sell
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            CartDAO cardao = new CartDAO();
            IList<Cart> cart = cardao.GetUserCart(int.Parse(Session["Id"].ToString()));
            if (Session["Id"] != null && cart.Count() != 0)
            {
                UserDAO udao = new UserDAO();
                CountryDAO cdao = new CountryDAO();
                User u = udao.SearchById(int.Parse(Session["Id"].ToString()));
                ViewBag.User = u;
                ViewBag.CountryUser = cdao.SearchById(u.CountryId);
                ViewBag.Cart = cardao.SearchCartUser(int.Parse(Session["Id"].ToString()));
                ViewBag.FullCart = cart;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Sell()
        {
            CartDAO cdao = new CartDAO();
            IList<Cart> cart = cdao.GetUserCart(int.Parse(Session["Id"].ToString()));
            if (Session["Id"] != null && cart.Count() != 0)
            {
                SellDAO dao = new SellDAO();
                Sell s = new Sell();
                s.UserId = int.Parse(Session["Id"].ToString());
                s.Date = DateTime.Now;
                s.TotalPrice = cdao.GetSubtotal(s.UserId);
                dao.Add(s);
                foreach (var c in cart)
                {
                    SellItemDAO sidao = new SellItemDAO();
                    SellItem si = new SellItem();
                    si.ApplicationId = c.ApplicationId;
                    si.PriceItem = c.Price;
                    si.SellId = s.Id;
                    sidao.Add(si);
                    cdao.Remove(c);
                }
                return RedirectToAction("Library", "User");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Sales()
        {
            try
            {
                if (Session["ModId"] != null)
                {
                    SellDAO dao = new SellDAO();
                    dao.Get7DaysSells();
                    UserDAO udao = new UserDAO();
                    User u = udao.SearchById(int.Parse(Session["Id"].ToString()));
                    ViewBag.User = u;
                    return View();
                }
                return View();
            }
            catch
            {
                return RedirectToAction("../Home/Index");
            }
        }
    }
}