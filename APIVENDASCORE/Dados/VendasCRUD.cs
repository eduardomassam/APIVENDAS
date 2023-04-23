using APIVENDASCORE.Models;
using APIVENDASCORE.Utils;



namespace APIVENDASCORE.Dados
{
    public class VendasCRUD
    {
        //OPERAÇÕES

        //CADASTRA NOVO CLIENTE

        public static void NovoCliente(Usuario Novo)
        {
            using (var ctx = new Contexto())
            {
                Criptografia cript = new Criptografia();
                Novo.Senha = cript.RetornarMD5(Novo.Senha);
                ctx.Usuario.Add(Novo);
                ctx.SaveChanges();
            }
        }
    }
}
