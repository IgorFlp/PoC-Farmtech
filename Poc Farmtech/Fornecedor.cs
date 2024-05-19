using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Fornecedor
    {
        public static string validaforn(string tipo,string usrName)
        {
            string input;
            
            switch (tipo)
            {
                case "cnpj":
                    Console.WriteLine("Inclua por favor o CNPJ apenas numeros\n");
                    input = Console.ReadLine();

                    if (input.Length > 14 || input.Length < 14)
                    {
                        Console.WriteLine("CNPJ invalido, deve ter 14 caracteres, digite novamente");
                        return validaforn("cnpj", usrName);
                    }
                    else
                    {
                        using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                        {
                            try
                            {
                                sqlconn.Open();
                                string query = $"select count (*) from Tb_fornecedor where cnpj = @input";
                                SqlCommand cmd = new SqlCommand(query, sqlconn);
                                cmd.Parameters.AddWithValue("@input", input);
                                int count = (int)cmd.ExecuteScalar();
                                if (count > 0)
                                {
                                    Console.WriteLine($"O CNPJ '{input}' ja esta cadastrado.");
                                    sqlconn.Close();

                                    return validaforn("cnpj", usrName);

                                }
                                else
                                {                                  

                                    return input;
                                }


                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex.Message);
                                return validaforn("cnpj",usrName);
                            }
                        }

                    }                    
                case "razSocial":
                    Console.WriteLine("\nPor favor preencha a Razão social\n");
                    input = Console.ReadLine();
                    if (input[1].Equals(" ") || input[1].Equals("."))
                    {
                        Console.WriteLine("Caractere invalido no inicio do nome.");
                        return validaforn("razSocial", usrName);
                    }
                    else if (input.Length > 50)
                    {
                        Console.WriteLine("Razão social excede 50 caracteres, digite novamente abreviando");
                        return validaforn("razSocial", usrName);
                    }
                    else
                    {
                        return input;
                    }                    
                case "nomeFant":
                    Console.WriteLine("\nPor favor preencha a Nome fantasia\n");
                    input = Console.ReadLine();
                    if (input[1].Equals(" ") || input[1].Equals("."))
                    {
                        Console.WriteLine("Caractere invalido no inicio do nome.");
                        return validaforn("nomeFant", usrName);
                    }
                    else if (input.Length > 50)
                    {
                        Console.WriteLine("Nome fantasia excede 50 caracteres, digite novamente abreviando");
                        return validaforn("nomeFant", usrName);
                    }
                    else
                    {
                        return input;
                    }
                default:
                    Console.WriteLine("Tipo inválido, tente novamente.");
                    cadFornecedor(usrName);
                    return "Invalido";                    
            }
            

        }
        public static void cadFornecedor(string usrName)
        {
            Console.WriteLine("Bem vindo ao cadastro de Fornecedores\n");
            string cnpj = validaforn("cnpj", usrName);            
            string razSocial = validaforn("razSocial", usrName);
            string nomeFant = validaforn("nomeFant", usrName);
            string telefone = Cliente.validacao("telefone");
            string email = Cliente.validacao("email");
            //endereços
            string rua = Cliente.validacao("rua");
            string bairro = Cliente.validacao("bairro");
            string cidade = Cliente.validacao("cidade");
            string estado = Cliente.validacao("estado");
            string cep = Cliente.validacao("cep");
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {

                    sqlconn.Open();
                    string query = $"insert into Tb_fornecedor (cnpj,razaoSocial,nomeFantasia,telefone,email)values (@cnpj,@razSocial,@nomeFant,@telefone,@email)";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cnpj", cnpj);
                    cmd.Parameters.AddWithValue("@razSocial", razSocial);
                    cmd.Parameters.AddWithValue("@nomeFant", nomeFant);
                    cmd.Parameters.AddWithValue("@telefone", telefone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    sqlconn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: "+ex.Message);   
                }

            }
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {
                    sqlconn.Open();
                    string query = $"insert into Tb_frn_Endereco (frn_cnpj,rua,bairro,cidade,estado,cep)values (@cnpj,@rua,@bairro,@cidade,@estado,@cep)";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cnpj", cnpj);
                    cmd.Parameters.AddWithValue("@rua", rua);
                    cmd.Parameters.AddWithValue("@bairro", bairro);
                    cmd.Parameters.AddWithValue("@cidade", cidade);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@cep", cep);
                    cmd.ExecuteNonQuery();
                    sqlconn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {
                    sqlconn.Open();
                    string query = $"select count (*) from Tb_fornecedor where cnpj = @cnpj";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cnpj", cnpj);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Console.WriteLine($"O CNPJ '{cnpj}' foi cadastrado no sistema.");
                        

                    }
                    else
                    {
                        
                        Console.WriteLine($"O CNPJ '{cnpj}' NÃO foi cadastrado");
                    }
                    sqlconn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }

            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {
                    sqlconn.Open();
                    string query = $"select count (*) from Tb_frn_endereco where frn_cnpj = @cnpj";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cnpj", cnpj);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Console.WriteLine($"O ENDEREÇO DO CNPJ '{cnpj}' foi cadastrado no sistema.");

                    }
                    else
                    {
                        
                        Console.WriteLine($"O ENDEREÇO DO CNPJ '{cnpj}' NÃO foi cadastrado");
                    }
                    sqlconn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }


            }
            
            Console.WriteLine("Aperte enter quando quiser voltar para o menu");
            Console.ReadLine();
            Program.menu(usrName);
        }
    }
}
