using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Usuario
    {
        public void cadUsuario(string connectionString,string usrName)
        {
            string nome = "Teste";
            string senha;
            string cargo = "teste";
            try
            {
                

                Console.WriteLine("\n\nDigite o nome do novo usuário\n");
                nome = Console.ReadLine();
                Console.WriteLine("\n\nDigite a senha do novo usuário\n");
                senha = Console.ReadLine();

                bool sair = false;

                while (sair == false)
                {
                    Console.WriteLine("\n\nDigite o cargo do novo usuário\n");
                    cargo = Console.ReadLine();
                    string[] cargos = { "Vendedor", "Produtor", "Estoquista", "Gerente de vendas", "Gerente de estoque", "Gerente de produção", "Gerente geral" };
                    if (cargos.Contains(cargo) == false)
                    {
                        Console.WriteLine("Cargo invalido, selecione entre os cargos \nVendedor\nProdutor\nEstoquista\nGerente de vendas\nGerente de estoque\nGerente de produção\nGerente geral");
                        sair = false;
                    }
                    else
                    {
                        sair = true;
                    }
                }

                //Console.WriteLine("Nome: "+nome+"\nSenha: "+senha+"\nCargo: "+cargo);
                using (SqlConnection sqlconn = new SqlConnection(connectionString))
                {
                    sqlconn.Open();
                    string query = "INSERT INTO Db_Farmtech.dbo.Tb_usuario (nome,senha,cargo) VALUES (@Nome,@Senha,@Cargo)";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@Nome", nome);
                    cmd.Parameters.AddWithValue("@Senha", senha);
                    cmd.Parameters.AddWithValue("@Cargo", cargo);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    Console.WriteLine("Cadastro ja existe no banco de dados, escolha outro nome de usuario");
                    cadUsuario(Program.connectionString, usrName);
                }
                else
                {
                    Console.WriteLine("O sistema encontrou um erro inesperado, reiniciando cadastro de usuarios.");
                    cadUsuario(Program.connectionString,usrName);
                }
            }
                using (SqlConnection sqlconn = new SqlConnection(connectionString))
                {
                    sqlconn.Open();
                    string query = $"SELECT count (*) FROM Db_farmtech.dbo.Tb_usuario WHERE nome = @Nome";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@Nome", nome);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Console.WriteLine($"O usuário '{nome}' foi criado.\n\n");
                    
                }
                    else
                    {
                        Console.WriteLine($"O usuário '{nome}' não foi criado.");
                    }
                    sqlconn.Close();
                }
            Program.menu(usrName);
        }               
                
            }
        }
    


