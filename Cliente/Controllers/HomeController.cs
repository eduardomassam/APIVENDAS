using Cliente.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Cliente.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        public System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        public System.Web.Mvc.ActionResult BadRequest()
        {
            Response.StatusCode = 400;
            return View();
        }

        [HttpPost]
        public async Task<System.Web.Mvc.ActionResult> Index(System.Web.Mvc.FormCollection formCollection)
        {

            var usuario = new Usuario
            {
                Cpf = formCollection["CPF"],
                Senha = formCollection["Senha"]
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7259/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("api/vendas/Login", content);


                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadAsStringAsync();
                    var token = JObject.Parse(resultado)["token"].ToString();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Armazena o token em um cookie
                    var cookie = new HttpCookie("token");
                    cookie.Value = token;
                    cookie.Expires = DateTime.UtcNow.AddHours(1);
                    cookie.HttpOnly = true;
                    cookie.Secure = true;
                    Response.Cookies.Add(cookie);

                    return RedirectToAction("Listar", "Pedidos", new { cpf = usuario.Cpf });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro na autenticação, verifique cpf/senha.");
                    return View();
                    
                }
            }

        }
    }
}