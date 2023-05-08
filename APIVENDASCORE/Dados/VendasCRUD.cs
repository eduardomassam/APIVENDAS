using APIVENDASCORE.Models;
using APIVENDASCORE.Services;
using APIVENDASCORE.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace APIVENDASCORE.Dados
{
    public class VendasCRUD
    {



        public static LoginResult Login(Usuario Login)
        {
            using (var ctx = new Contexto())
            {
                Criptografia cript = new Criptografia();
                var Cpf = Login.Cpf;

                string senhaCriptografada = ctx.Usuario
                    .Where(u => u.Cpf == Cpf)
                    .Select(u => u.Senha)
                    .FirstOrDefault();

                bool Autenticado = cript.ComparaMD5(Login.Senha, senhaCriptografada);

                if (senhaCriptografada == null || Autenticado == false)
                {
                    throw new ArgumentException("Usuário ou senha inválidos");
                }

                var user = ctx.Usuario
                    .Where(u => u.Cpf == Cpf)
                    .Select(u => u.Tipo)
                    .FirstOrDefault();

                var roles = new List<string>();
                if (user == 0)
                {
                    roles.Add("Autenticado");
                }
                else if (user == 1)
                {
                    roles.Add("Administrador");
                    roles.Add("Default");
                }

                var token = TokenServices.GenerateToken(Login, roles.ToArray());
                Login.Senha = "";

                var result = new LoginResult
                {

                    token = token

                };

                return result;
            }
        }

        public static LoginResult LoginVendedor(Vendedor Login)
        {
            using (var ctx = new Contexto())
            {
                Criptografia cript = new Criptografia();
                var Cnpj = Login.Cnpj;

                string senhaCriptografada = ctx.Vendedor
                    .Where(u => u.Cnpj == Cnpj)
                    .Select(u => u.Senha)
                    .FirstOrDefault();

                bool Autenticado = cript.ComparaMD5(Login.Senha, senhaCriptografada);

                if (senhaCriptografada == null || Autenticado == false)
                {
                    throw new ArgumentException("Usuário ou senha inválidos");
                }

                var user = ctx.Vendedor
                    .Where(u => u.Cnpj == Cnpj)
                    .Select(u => u.Tipo)
                    .FirstOrDefault();

                var roles = new List<string>();
                if (user == 0)
                {
                    roles.Add("Autenticado");
                }
                else if (user == 1)
                {
                    roles.Add("Administrador");
                    roles.Add("Default");
                }

                var token = TokenServices.GenerateTokenVendedor(Login, roles.ToArray());
                Login.Senha = "";

                var result = new LoginResult
                {
                    token = token
                };

                return result;

            }
        }

        public static LoginResult LoginTransportadora(Transportadora Login)
        {
            using (var ctx = new Contexto())
            {
                Criptografia cript = new Criptografia();
                var Cnpj = Login.Cnpj;

                string senhaCriptografada = ctx.Transportadora
                    .Where(u => u.Cnpj == Cnpj)
                    .Select(u => u.Senha)
                    .FirstOrDefault();

                bool Autenticado = cript.ComparaMD5(Login.Senha, senhaCriptografada);

                if (senhaCriptografada == null || Autenticado == false)
                {
                    throw new ArgumentException("Usuário ou senha inválidos");
                }

                var user = ctx.Transportadora
                    .Where(u => u.Cnpj == Cnpj)
                    .Select(u => u.Tipo)
                    .FirstOrDefault();

                var roles = new List<string>();
                if (user == 0)
                {
                    roles.Add("Autenticado");
                }
                else if (user == 1)
                {
                    roles.Add("Administrador");
                    roles.Add("Default");
                }

                var token = TokenServices.GenerateTokenTransportadora(Login, roles.ToArray());
                Login.Senha = "";

                var result = new LoginResult
                {
                    token = token
                };

                return result;
            }
        }

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



        //PESQUISAS

        //RETORNA um pedido em especifico a partir de seu codigo
        public static Pedidos BuscarPedido(int Codigo)
        {
            using (var ctx = new Contexto())
            {
                return ctx.Pedidos.FirstOrDefault(p => p.Cod.Equals(Codigo));
            }
        }


        //RETORNA todo o historico de um pedido a partir de seu codigo
        public static IEnumerable<HistPedido> ListarHistorico(int Codigo)
        {
            using (var ctx = new Contexto())
            {
                var Pesquisa = (from A in ctx.HistPedido where A.CodPed == Codigo select A).ToList();

                return Pesquisa;

            }
        }


        //RETORNA Data Entrega
        public static Nullable<DateTime> DataEntrega(int Codigo)
        {
            using (var ctx = new Contexto())
            {
                // var Pesquisa = (from A in ctx.HistPedido where A.CodPed == Codigo select A).ToList();
                var Pesquisa = ctx.HistPedido.ToList().Select(i => i.DataOcorrencia);
                var x = Pesquisa.LastOrDefault();
                //var y = x.DataOcorrencia;
                return x;
            }
        }

        //RETORNA todos os pedidos
        public static IEnumerable<Pedidos> ListarPedidos()
        {
            using (var ctx = new Contexto())
            {
                return ctx.Pedidos.ToList();
            }
        }

        //RETORNA todos os pedidos de um determinado CPF
        public static IEnumerable<Pedidos> ListarPedidosCPF(string Codigo)
        {
            using (var ctx = new Contexto())
            {
                var Pesquisa = (from A in ctx.Pedidos where A.CPF == Codigo select A).ToList();
                //var Pesquisa = (from A in ctx.Pedidos 
                //                join B in ctx.HistPedido
                //                on A.Cod equals B.CodPed
                //                where A.CPF == Codigo select A).ToList();
                return Pesquisa;
            }
        }

        //RETORNA todos os pedidos de um determinado STATUS
        public static IEnumerable<Pedidos> ListarPedidosStatus(int status)
        {
            using (var ctx = new Contexto())
            {
                var Pesquisa = (from A in ctx.Pedidos where A.Status == status select A).ToList();
                return Pesquisa;
            }
        }

        //RETORNA o total de novos Pedidos
        public static string TotalPedidosFeitos()
        {
            using (var ctx = new Contexto())
            {
                var Pesquisa = (from A in ctx.Pedidos where A.Status == (int)Enum.StatusPedido.FEITO select A).Count().ToString();
                return Pesquisa;
            }
        }

        // OPERACOES

        //INCLUI um Novo Historico (Pedido 1=>N Historicos)
        public static void NovoHistorico(HistPedido Novo)
        {
            using (var ctx = new Contexto())
            {
                ctx.HistPedido.Add(Novo);
                ctx.SaveChanges();
            }
        }

        //INCLUI um historico a partir da mudança de status ou outro evento
        public static void IncluirHistorico(int CodPedido, string Obs)
        {
            HistPedido NovoHist = new HistPedido();
            NovoHist.CodPed = CodPedido;
            NovoHist.NroSeq = 0; //CAMPO INCREMENTAL
            NovoHist.DataOcorrencia = DateTime.Now;
            NovoHist.Obs = Obs;

            NovoHistorico(NovoHist);
        }

        //INCLUI um NOVO pedido. Imediatamente inclui o 1º Historico
        public static void NovoPedidoCliente(Pedidos Novo)
        {
            using (var ctx = new Contexto())
            {
                ctx.Pedidos.Add(Novo);
                ctx.SaveChanges();
            }
            IncluirHistorico(Novo.Cod, "[CLIENTE] Pedido Incluido");
        }

        //Altera um pedido
        public static void AlterarPedido(Pedidos Alterado)
        {
            using (var ctx = new Contexto())
            {
                ctx.Entry<Pedidos>(Alterado).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        //AUXILIARES
        public static string UltimoCodigoIncluido()
        {
            using (var ctx = new Contexto())
            {
                var Pesquisa = ctx.Pedidos.Max(a => a.Cod).ToString();
                return Pesquisa;
            }
        }

        //ALTERA O STATUS DE UM PEDIDO a partir da classe status
        //IMEDIATAMENTE após a mudança, incluir um historico
        public static void AlterarStatusPedido(Status Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = Info.NovoStatus;
            AlterarPedido(Alt);

            IncluirHistorico(Alt.Cod, Info.Obs);
        }

        //FAZ a avaliacao de um pedido
        public static void AvaliarPedido(AvaliacaoPedidos Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = (int)Enum.StatusPedido.AVALIADO;
            AlterarPedido(Alt);

            Info.Avaliacao = "[CLIENTE] Avaliação: " + Info.Avaliacao;
            IncluirHistorico(Alt.Cod, Info.Avaliacao);

        }

        //Cliente cancelar pedido não enviado ainda
        public static void CancelarPedido(AvaliacaoPedidos Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = (int)Enum.StatusPedido.CANCELADO;
            AlterarPedido(Alt);

            Info.Avaliacao = "[CLIENTE] Motivo do Cancelamento: " + Info.Avaliacao;

            IncluirHistorico(Alt.Cod, Info.Avaliacao);
        }

        //Devolver pedido dentro de 7 dias da entrega
        public static void DevolverPedido(AvaliacaoPedidos Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = (int)Enum.StatusPedido.DEVOLVIDO_CLIENTE;
            AlterarPedido(Alt);

            Info.Avaliacao = "[CLIENTE] Motivo da Devolução: " + Info.Avaliacao;

            IncluirHistorico(Alt.Cod, Info.Avaliacao);
        }

        //Transportadora coletar o pedido com o cliente para devolução ao vendedor
        public static void DevolverPedidoTransportadora(Status Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = (int)Enum.StatusPedido.DEVOLVIDO_TRANSPORTADORA;
            AlterarPedido(Alt);

            IncluirHistorico(Alt.Cod, Info.Obs);
        }

        //Transportadora entregar o pedido devolvivo ao vendedor
        public static void DevolverPedidoVendedor(Status Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = (int)Enum.StatusPedido.DEVOLVIDO_VENDEDOR;
            AlterarPedido(Alt);

            IncluirHistorico(Alt.Cod, Info.Obs);
        }

        //Vendedor aceitar a devolução
        public static void DevolverPedidoVendedorAceite(Status Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = (int)Enum.StatusPedido.DEVOLVIDO_COM_SUCESSO;
            AlterarPedido(Alt);

            IncluirHistorico(Alt.Cod, Info.Obs);
        }

    }
}
