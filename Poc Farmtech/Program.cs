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
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Poc_Farmtech {

    class Program
    {
        public static string connectionString = @"Data Source=DESKTOP-69PP4N9;Initial Catalog=Db_Farmtech;Integrated Security=True;";
        
        static string insiraUsr(string usrName,string query)
        {
            Console.WriteLine("Digite seu nome de usuario\n");
            usrName = Console.ReadLine();
            using (SqlConnection sqlconn = new SqlConnection(connectionString))
            {
                try
                {
                    sqlconn.Open();
                    query = $"select count (*) from Tb_usuario where nome = @Nome";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@Nome", usrName);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Console.WriteLine($"O usuário '{usrName}' existe na tabela.\n");
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
                    Console.WriteLine(ex.Message);
                    return "null";

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
                            return logar(usrName, "a", "a");                             
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

        public static void menu(string usrName)
        {
            Console.WriteLine("Iniciando menu e limpando tela");
            Thread.Sleep(3000);
            Console.Clear();

            Console.WriteLine("\n\nBem vindo: "+usrName+"\nQual ação deseja testar?\nCadastros\n0-Cadastrar Usuario\n1-Cadastrar Cliente\n2-Cadastrar Fornecedor\n3-Cadastrar Produto\n\nMovimentações\n\n4-Vender\n5-Produção\n6-Relatórios");
            try
            {
                switch (Console.ReadLine())
                {
                    case "0":
                        Usuario usr = new Usuario();
                        usr.cadUsuario(connectionString,usrName);
                        break;
                    case "1":
                        Cliente cliente = new Cliente();
                        cliente.cadCliente(usrName);
                        break;
                    case "2":                        
                        Fornecedor.cadFornecedor(usrName);
                        break;
                    case "3":
                        Produto.cadProduto(usrName);
                        break;
                    case "4":
                        Venda.vender(usrName);
                        break;
                    case "5":
                        Producao.incluirProducao(usrName);
                        break;
                    case "6":
                        Relatorios.gerarRelatorio();
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
    


            


