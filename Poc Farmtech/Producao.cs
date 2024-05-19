using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Producao
    {
        static int id;
        static DateTime dtProd = new DateTime();
        static List<Produto> produtos = new List<Produto>();
        static List<decimal> quantidades = new List<decimal>();
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
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
                    }
                    catch (Exception ex)
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
            while (sair == false)
            {
                string resp = Console.ReadLine();
                upper = resp.ToUpper();
                if (upper == "S")
                {
                    slcProduto();
                    sair = true;
                }
                else if (upper == "N")
                {
                    sair = true;
                }
                else
                {
                    Console.WriteLine("Escolha ente S ou N");
                    sair = false;
                }
            }
        
        }
        private static void slcData()
        {
            Console.WriteLine("\nPor favor preencha o data da produção com barras Ex: 18/07/2001\n");
            string input = Console.ReadLine();
            try
            {
                //Declara variavel do tipo data pra receber a data convertida
                

                if (DateTime.TryParse(input, out dtProd))
                {
                    // Verificar se o ano está entre 1900 a 2024)
                    if (dtProd.Year < 1900 || dtProd.Year > 2024)
                    {
                        Console.WriteLine("Ano inválido. Por favor, digite um ano entre 1900 e 2024.");
                        slcData();
                    }
                    else
                    {
                        // Exibir a data convertida
                        Console.WriteLine("Data: " + dtProd);
                        
                        
                    }
                }
                else
                {
                    // Informar que a conversão falhou devido a um formato inválido
                    Console.WriteLine("Erro: Formato de data inválido.");
                    slcData();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: ", ex.Message);
                slcData();
            }
        }
        private static void gravaDB()
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                {
                    sqlconn.Open();
                    string query = $"INSERT INTO Db_Farmtech.dbo.Tb_producao (dataProd) VALUES (@dataProd); SELECT SCOPE_IDENTITY();";
                    //(cl_cpf,usr_nome,subtotal,frete,cupom,desconto,total,mtdPagto)
                    //(@cpf,@usrName,@subtotal,@frete,@cupom,@desconto,@total,@mtdPagto)
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@dataProd", dtProd);                    
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                    Console.WriteLine("Produção inserida com sucesso!");
                    cmd.Parameters.Clear();
                    sqlconn.Close();
                }
                using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                {
                    sqlconn.Open();
                    string query = $"INSERT INTO Db_Farmtech.dbo.Tb_producao_produtos (pdc_id,pdt_id,quant) VALUES (@pdc_id,@pdt_id,@quant)";
                    //(cl_cpf,usr_nome,subtotal,frete,cupom,desconto,total,mtdPagto)
                    //(@cpf,@usrName,@subtotal,@frete,@cupom,@desconto,@total,@mtdPagto)
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    for (int i = 0; i < produtos.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@pdc_id", id);
                        cmd.Parameters.AddWithValue("@pdt_id", produtos[i].id);
                        cmd.Parameters.AddWithValue("@quant", quantidades[i]);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    Console.WriteLine("Produtos inseridos na produção com sucesso!");
                    sqlconn.Close();
                }
                using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                {
                    sqlconn.Open();
                    string query = $"UPDATE Db_Farmtech.dbo.Tb_estoque SET quant = quant + @quant where pdt_id = @pdt_id";
                    //(cl_cpf,usr_nome,subtotal,frete,cupom,desconto,total,mtdPagto)
                    //(@cpf,@usrName,@subtotal,@frete,@cupom,@desconto,@total,@mtdPagto)
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    for (int i = 0; i < produtos.Count; i++)
                    {                        
                        cmd.Parameters.AddWithValue("@pdt_id", produtos[i].id);
                        cmd.Parameters.AddWithValue("@quant", quantidades[i]);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    Console.WriteLine("Estoque atualizado!");
                    sqlconn.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine("Erro: " + ex.ToString()); }
        }
        public static void incluirProducao(string usrName)
        {
            Console.WriteLine("Bem vindo a inclusão de produção\n\nProdutos");
            Venda.exibeProduto();
            Console.WriteLine("Digite o ID do produto a inserir");
            slcProduto();            
            slcData();
            gravaDB();
            Console.WriteLine("Aperte enter quando quiser voltar para o menu");
            Console.ReadLine();
            Program.menu(usrName);

        }
    }
}
