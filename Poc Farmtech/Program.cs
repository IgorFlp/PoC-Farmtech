// See https://aka.ms/new-console-template for more information
using Poc_Farmtech;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Poc_Farmtech {

    class Program
    {
        static string connectionString = @"Data Source=DESKTOP-69PP4N9;Initial Catalog=Db_Farmtech;Integrated Security=True;";
        
        static string insiraUsr(string usrName,string query)
        {
            Console.WriteLine("Digite seu nome de usuario");
            usrName = Console.ReadLine();
            using (SqlConnection sqlconn = new SqlConnection(connectionString))
            {
                try
                {
                    sqlconn.Open();
                    Console.WriteLine("logou");
                    query = $"select count (*) from Db_farmtech.dbo.Tb_usuario where nome = @Nome";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@Nome", usrName);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Console.WriteLine($"O usuário '{usrName}' existe na tabela.");
                        sqlconn.Close();
                        
                        return logar(usrName,"a","a");
                        
                    }
                    else
                    {
                        Console.WriteLine($"O usuário '{usrName}' não foi encontrado na tabela\n TENTE NOVAMENTE\n");

                        return insiraUsr("a", "a");                        
                    }
                }
                catch (Exception ex)
                {
                    return "null";
                    Console.WriteLine(ex.Message);
                    
                }
            }

        }

        static string logar(string usrName,string usrSenha, string query)
        {
            using (SqlConnection sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();
                Console.WriteLine("Digite a senha");
                usrSenha = Console.ReadLine();                
                query = "SELECT nome, senha FROM Db_farmtech.dbo.Tb_usuario WHERE nome = @Nome";
                SqlCommand cmd = new SqlCommand(query, sqlconn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Nome", usrName);
                Console.WriteLine("Nome: "+usrName+"\nSenha: "+usrSenha);
                SqlDataReader leitor = cmd.ExecuteReader();

                if (leitor.Read())
                {
                    string nomeUsuario = leitor["nome"].ToString();
                    string senhaUsuario = leitor["senha"].ToString();

                    // Verificar se o password fornecido corresponde ao password do usuário
                    if (usrSenha == senhaUsuario)
                    {
                        Console.WriteLine($"Login bem-sucedido para o usuário '{nomeUsuario}'.");
                        sqlconn.Close();
                        return usrName;
                    }
                    else
                    {
                        sqlconn.Close();                        
                        Console.WriteLine($"Senha incorreta para o usuário '{nomeUsuario}'\n TENTE NOVAMENTE.");                        
                        {                            
                            logar(usrName, "a", "a");
                            return "null";
                        }
                        
                    }
                }
                else
                {
                    Console.WriteLine($"Usuário '{usrName}' não encontrado.");
                    return "null";

                }
                leitor.Close();                
            }
        }

        static void menu(string nome)
        {
            Console.WriteLine("\n\nBem vindo: "+nome+"\nQual ação deseja testar?\n\n0-Cadastrar Usuario\n1-Cadastrar Cliente\n2-Cadastrar Fornecedor\n3-Cadastrar Produto\n");
            try
            {
                switch (Console.ReadLine())
                {
                    case "0":
                        Usuario usr = new Usuario();
                        usr.cadUsuario(connectionString);
                        break;
                    case "1":
                        Cliente cliente = new Cliente();
                        cliente.cadCliente();
                        break;
                }
            }
            catch(Exception ex)
            {
                  Console.WriteLine("Exception: "+ex.Message);
            }
        }
        static void Main(string[] args)
        {

            Console.WriteLine("Bem vindo a Poc do sistema Farmtech\n\nPara iniciar o teste vamos iniciar o login\n\n");
            string usrName = insiraUsr("a","a");            
            menu(usrName);

            

        }
    }
}
    


            


