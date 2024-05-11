using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Usuario
    {
        public void cadUsuario(string connectionString)
        {
            string nome;
            string senha;
            string cargo;

            Console.WriteLine("\n\nDigite o nome do novo usuário\n");
            nome = Console.ReadLine();
            Console.WriteLine("\n\nDigite a senha do novo usuário\n");
            senha = Console.ReadLine();
            Console.WriteLine("\n\nDigite o cargo do novo usuário\n");
            cargo = Console.ReadLine();
            Console.WriteLine("Nome: "+nome+"\nSenha: "+senha+"\nCargo: "+cargo);
            using (SqlConnection sqlconn = new SqlConnection(connectionString))
            {
                
                string query = "INSERT INTO Db_Farmtech.dbo.Tb_usuario (nome,senha,cargo) VALUES (@Nome,@Senha,@Cargo)";
                SqlCommand cmd = new SqlCommand(query, sqlconn);
                cmd.Parameters.AddWithValue("@Nome",nome);
                cmd.Parameters.AddWithValue("@Senha",senha);
                cmd.Parameters.AddWithValue("@Cargo", cargo);
                sqlconn.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                sqlconn.Close(); 
                
                sqlconn.Open();
                query = "SELECT nome FROM Db_farmtech.dbo.Tb_usuario WHERE nome = @Nome";
                cmd.Parameters.AddWithValue("@Nome", nome);                
                int count = (int)cmd.ExecuteScalar();                
                if (count > 0)
                {
                    Console.WriteLine($"O usuário '{nome}' existe na tabela.\n\nQuantidade de encontro'{count}'");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"O usuário '{nome}' não existe na tabela.");
                }
                sqlconn.Close();
            }
        }
    }
}
