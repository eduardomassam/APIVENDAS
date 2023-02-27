using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cliente.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        
        
        
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index (FormCollection Info)
        {
            //Guarda a informação durante todo a sessão do usuário
            Session["CPF"] = Info["CPF"].ToString();

            return Redirect("~/Pedidos/Listar");
        }
    }
}