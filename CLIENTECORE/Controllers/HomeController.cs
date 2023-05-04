using CLIENTECORE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;


namespace CLIENTECORE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult BadRequest()
        {
            Response.StatusCode = 400;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(IFormCollection formCollection)
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
                    Response.Cookies.Append("token", token, new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddHours(1),
                        HttpOnly = true,
                        Secure = true
                    });

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