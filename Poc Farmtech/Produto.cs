using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Produto
    {
        public static List<string> frnCnpj = new List<string>();
        public int id;
        public string nome;
        public string unMedida;
        public decimal precoUn;

        public static int gravaDB(string tabela, string nome, string unMedida, decimal precoUn, int id)
        {
            try
            {
                switch (tabela)
                {
                    case "produto":
                        using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                        {
                            sqlconn.Open();
                            string query = $"INSERT INTO Db_Farmtech.dbo.Tb_produto (nome, unMedida, precoUn) VALUES (@nome, @unMedida, @precoUn); SELECT SCOPE_IDENTITY();";
                            SqlCommand cmd = new SqlCommand(query, sqlconn);
                            cmd.Parameters.AddWithValue("@nome", nome);
                            cmd.Parameters.AddWithValue("@unMedida", unMedida);
                            cmd.Parameters.AddWithValue("@precoUn", precoUn);
                            id = Convert.ToInt32(cmd.ExecuteScalar());
                            sqlconn.Close();
                            Console.WriteLine("Produto cadastrado com sucesso!");
                            return id;

                        }

                    case "fornPdt":
                        using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                        {
                            sqlconn.Open();
                            string query = $"insert into Db_Farmtech.dbo.Tb_pdt_fornecedores (pdt_id,frn_cnpj) values (@pdt_id,@frn_cnpj)";
                            SqlCommand cmd = new SqlCommand(query, sqlconn);
                            foreach (string s in frnCnpj)
                            {
                                cmd.Parameters.AddWithValue("@pdt_id", id);
                                cmd.Parameters.AddWithValue("@frn_cnpj", s);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            sqlconn.Close();
                            Console.WriteLine("Fornecedores vinculados ao produto com sucesso!");
                            return 0;
                        }

                    default:
                        return 0;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exceção: " + ex.Message);
                return 0;
            }
        }
        public static void fornPdt()
        {
            Console.WriteLine("\nPreencha o CNPJ do fornecedor");
            string input = Console.ReadLine();

            SqlConnection sqlconn = new SqlConnection(Program.connectionString);
            sqlconn.Open();
            string query = $"select count (*) from Tb_fornecedor where cnpj = @cnpj";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@cnpj", input);

            int count = (int)cmd.ExecuteScalar();
            sqlconn.Close();
            if (count > 0)
            {
                Produto.frnCnpj.Add(input);
                Console.WriteLine("\nGostaria de outro fornecedor? S: sim, N: não");
                string resp = Console.ReadLine();
                string upper = resp.ToUpper();
                resp = upper;

                switch (resp)
                {
                    case "S":
                        for (int i = 0; i < Produto.frnCnpj.Count(); i++)
                        {
                            Console.WriteLine(Produto.frnCnpj[i]);
                        }
                        fornPdt();
                        break;


                    case "N":
                        //gravaDB();
                        break;
                    default:
                        Console.WriteLine("\nOpção invalida Por favor escolha uma entre S e N");
                        fornPdt();
                        break;
                }

            }
            else
            {
                Console.WriteLine("\nCnpj inexistente, o que deseja tentar novamente?\nS: Sim\nN: Não, registrar sem fornecedor");

                string resp = Console.ReadLine();
                string upper = resp.ToUpper();
                resp = upper;
                switch (resp)
                {
                    case "S":
                        fornPdt();
                        break;
                    case "N":
                        // gravaDB(); 
                        break;
                    default:
                        Console.WriteLine("Resposta invalida, voltando ao cadastro de CNPJ");
                        fornPdt();
                        break;
                }

            }
        }

        public static void cadProduto(string usrName)
        {
             Produto produto = new Produto();
            produto.id = 0;
            produto.nome = "teste";
            produto.unMedida = "ts";
            produto.precoUn = 0.0m;
            


            Console.WriteLine("Bem vindo ao cadastro de produtos\n");
            bool sair = false;
            while (sair == false)
            {
                //Console.WriteLine("\nPor favor, preencha o nome do produto");
                produto.nome = Cliente.validacao("nome"); ;
                if (produto.nome == null)
                {
                    sair = false;
                }
                else
                {

                    sair = true;
                }
            }
            sair = false;
            while (sair == false)
            {
                Console.WriteLine("\nPor favor, preencha a unidade de medida do produto KG ou UN");
                produto.unMedida = Console.ReadLine();
                string toupper = produto.unMedida.ToUpper();
                produto.unMedida = toupper;
                switch (produto.unMedida)
                {
                    case "KG":
                        sair = true;
                        break;
                    case "UN":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Valor invalido preencha KG ou UN");
                        sair = false;
                        break;
                }
            }
            sair = false;
            while (sair == false)
            {
                Console.WriteLine("\nPor favor, preencha o preço do produto Ex: 6,94");
                try
                {
                    string input = Console.ReadLine();
                    if (decimal.TryParse(input, out produto.precoUn))
                    {
                        sair = true;
                    }
                    else
                    {
                        sair = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro:" + ex.Message + "\n\nTente novamente com um valor valido");
                }
            }


            sair = false;
            while (sair == false)
            {
                Console.WriteLine("\nGostaria de vincular o produto a um fornecedor? S: sim, N: não");
                string resp = Console.ReadLine();
                string upper = resp.ToUpper();
                resp = upper;
                switch (resp)
                {
                    case "S":
                        fornPdt();
                        sair = true;
                        break;
                    case "N":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Valor invalido preencha S ou N");
                        sair = false;
                        break;
                }
            }

            produto.id = gravaDB("produto", produto.nome, produto.unMedida, produto.precoUn, produto.id);
            if (frnCnpj.Count > 0) { gravaDB("fornPdt", produto.nome, produto.unMedida, produto.precoUn, produto.id); }
            Program.menu(usrName);
        }
    }
}
