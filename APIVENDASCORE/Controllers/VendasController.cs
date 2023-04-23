using APIVENDASCORE.Dados;
using APIVENDASCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace APIVENDASCORE.Controllers
{
    [ApiController]
    [Route("api/vendas/")]
    [Produces("application/json")]
    public class VendasController : ControllerBase
    {   
        //CADASTRA UM NOVO CLIENTE

        [HttpPost("NovoCliente")]
        [SwaggerOperation(Summary = "Cadastra um novo cliente")]
        public IActionResult IncluirCliente([FromBody] Usuario Novo)
        {
            try
            {
                VendasCRUD.NovoCliente(Novo);
                return Ok("Cliente cadastrado com sucesso!");
            }
            catch (Exception err)
            {
                return BadRequest($"Erro ao cadastrar cliente: {err.Message}");
            }
        }
    }
}
