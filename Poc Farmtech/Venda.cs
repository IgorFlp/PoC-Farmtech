using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Venda
    {
        static List<Produto> produtos = new List<Produto>();
        static List<decimal> quantidades = new List<decimal>();
        //+ID: Int
        static decimal subtotal;
        static decimal frete;
        static decimal desconto;
        static decimal total;
        static string mtdPagto;
        static string cupom;

        // pdt_id
        //-Usr_nome: Char(50)

        private static Cliente slcCliente()
        {
            Cliente cliente = new Cliente();
            //Perguntar qual cliente queremos pesquisar (com o cpf)
            Console.WriteLine("Preencha o CPF do cliente");
            string cpf = Console.ReadLine();
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                sqlconn.Open();
                string query = $"select count (*) from Tb_cliente where cpf = @cpf;";
                SqlCommand cmd = new SqlCommand(query, sqlconn);
                cmd.Parameters.AddWithValue("@cpf", cpf);
                int count = (int)cmd.ExecuteScalar();
                sqlconn.Close();
                if (count > 0)
                {
                    Console.WriteLine("Cliente encontrado");
                }
                else
                {
                    Console.WriteLine("Cliente não encontrado, digite o cpf novamente");
                    slcCliente();
                }
            }
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {
                    sqlconn.Open();
                    string saida;
                    string query = $"select cpf, nome from Db_Farmtech.dbo.Tb_cliente where cpf = @cpf";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cliente.nome = reader["nome"].ToString();
                        cliente.cpf = reader["cpf"].ToString();                        
                    }
                    else
                    {
                        saida = "Não encontrado";
                    }                              
                    reader.Close();
                    sqlconn.Close();
                    return cliente;
                   
                }catch (Exception ex)
                {
                    Console.WriteLine("Erro: "+ex.Message);
                    return slcCliente();
                    
                }
            }            
        }
        private static void exibeProduto()
        {
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                sqlconn.Open();
                string query = $"select * from Db_Farmtech.dbo.Tb_produto";
                SqlCommand cmd = new SqlCommand(query, sqlconn);
                List<int> id = new List<int>();
                List<string> nome = new List<string>();
                List<string> unMed = new List<string>();
                List<decimal> preco = new List<decimal>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id.Add(Convert.ToInt32(reader["id"]));
                    nome.Add(reader["nome"].ToString());
                    unMed.Add(reader["unMedida"].ToString());
                    preco.Add(Convert.ToDecimal(reader["precoUn"]));
                }
                int index = 0;
                Console.WriteLine("{0,-5} {1,-30} {2,-2} {3,10}", "ID", "Nome", "UN", "Preço");
                foreach (int i in id)
                {

                    Console.WriteLine("{0,-5} {1,-30} {2,-5} {3,10:C}", id[index], nome[index], unMed[index], preco[index]);
                    index++;
                }
                reader.Close();
                sqlconn.Close();
            }
        }
        private static void slcProduto()
        {
            Produto saida = new Produto();         
            
            Console.WriteLine("\nSelecione o ID do produto");
            int input = Convert.ToInt32(Console.ReadLine());
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {
                    sqlconn.Open();
                    string query = $"select * from Db_Farmtech.dbo.Tb_produto where id = @id";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@id", input);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        saida.id = Convert.ToInt32(reader["id"]);
                        saida.nome = reader["nome"].ToString();
                        saida.unMedida = reader["unMedida"].ToString();
                        saida.precoUn = Convert.ToDecimal(reader["precoUn"]);
                    }
                    reader.Close();
                    sqlconn.Close();
                }catch (Exception ex)
                {
                    Console.WriteLine("Erro: "+ex.Message);
                }
            }
            bool sair;
            if (saida.unMedida == "KG")
            {
                sair = false;
                decimal qtd = 0;
                while (sair == false)
                {
                    Console.WriteLine("\nQual a quantidade comprada? em " + saida.unMedida);
                    try
                    {
                        qtd = Convert.ToDecimal(Console.ReadLine());
                        sair = true;
                    }catch (Exception ex)
                    {
                        Console.WriteLine("Preencha um valor valido");
                        sair = false;
                    }

                }
                produtos.Add(saida);
                quantidades.Add(qtd);
            }
            else
            {
                sair = false;
                int qtd = 0;
                while (sair == false)
                {
                    Console.WriteLine("\nQual a quantidade comprada? em " + saida.unMedida);
                    try
                    {
                        qtd = Convert.ToInt32(Console.ReadLine());
                        sair = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Preencha um valor inteiro");
                        sair = false;
                    }

                }                
                produtos.Add(saida);
                quantidades.Add(qtd);
            }
            Console.WriteLine("Gostaria de inserir outro produto? S/N");
            string upper;
            sair = false;
            while (sair == false) {
                string resp = Console.ReadLine();
                upper = resp.ToUpper();
                if (upper == "S")
                {
                    slcProduto();
                    sair = true;
                } else if (upper == "N") {
                    sair = true;
                }
                else
                {
                    Console.WriteLine("Escolha ente S ou N");
                    sair = false;
                }
            }

            //Deseja adicionar produto S/N while S  adicione ID do produto
            //Adicione quantidade de produto
            // criar lista de produtos e lista de quantidades
            
        }
        private static decimal calcSubtotal()
        {
            decimal subtotal = 0;
            for(int i = 0; i < produtos.Count;i++)
            {
                subtotal = subtotal + (produtos[i].precoUn * quantidades[i]);
            }
            return subtotal;
        }
        private static bool vldCupom()
        {
            return false;
        }
        public static void vender(string usrName)
        {          
            Console.WriteLine("Bem vindo ao modulo de vendas\n");
            Cliente cliente = new Cliente();
            cliente = slcCliente();
            Console.WriteLine("Nome: "+cliente.nome+"\nCPF: "+cliente.cpf);
            exibeProduto();
            slcProduto();
            for (int i = 0; i<produtos.Count;i++)
            {
                Console.WriteLine("Produto: " + produtos[i].nome + " Quantidade: "+ quantidades[i]);
            }
            subtotal = calcSubtotal();
            Console.WriteLine("Subtotal: {0,10:C} ",subtotal);

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
