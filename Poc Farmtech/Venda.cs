using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Venda
    {
        private static string slcCliente()
        {
            //Perguntar qual cliente queremos pesquisar (com o cpf)
            //Retornar o nome do cliente
            //
            return "a";
        }
        private static Produto slcProduto()
        {
            Produto saida = new Produto();
            //Deseja adicionar produto S/N while S  adicione ID do produto
            //Adicione quantidade de produto
            // criar lista de produtos e lista de quantidades
            return saida;
        }
        private static decimal slcSubtotal()
        {
            return 0;
        }
        private static bool vldCupom()
        {
            return false;
        }
        public static void vender()
        {
            Console.WriteLine("Bem vindo ao modulo de vendas\n");
        }
        //Sequencia slcCliente -> slcProduto repetidamente-> calcSubtotal -> main "entrega ou retirada?" if entrega frete = 10,00 ->//
        //main "Mtd pagamento" -> main "Deseja preencher cupom? S/N"-> vldcupom desconto = cp_valor ->
        //main calcula total "subtotal + frete - desconto" -> imprime resumo da venda.
        
        //+ID: Int
        //-Pdt_ID: Int
        //-Cl_CPF: Char(11)
        //-Qnt prodt: Decimal
        //-Subtotal: Double
        //-Frete: Double
        //-Cupom: Char(10)
        //-Desconto: Double
        //-Total: Double
        //-Mtd Pagto: Char(20)
        //-Usr_nome: Char(50)
    }
}
