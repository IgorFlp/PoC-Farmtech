using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Cupom
    {
        public string nome;
        public DateTime dtValid = new DateTime();
        public decimal valor;

        public static string buscaCupom()
        {
            Console.WriteLine("Preencha o nome do cupom");
            string input = Console.ReadLine();
            if (input != null)
            {
                using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                {
                    sqlconn.Open();
                    string query = $"select count (*) from Tb_cupom where nome = @nome";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@nome", input);
                    int count = (int)cmd.ExecuteScalar();
                    sqlconn.Close();
                    if (count > 0)
                    {
                        Console.WriteLine("Cupom encontrado no banco de dados");
                        return input;

                    }
                    else
                    {
                        Console.WriteLine("Cupom inexistente");
                        return null;
                    }

                }
            }else { return null; }
            
        }
        public static Cupom validaCupom(string input)
        {
            DateTime hoje = DateTime.Now;
            //Console.WriteLine("hoje:" + hoje);
            Cupom cupom = new Cupom();
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString)){                   
                
                    try{
                        sqlconn.Open();
                        string query = $"select * from Db_Farmtech.dbo.Tb_cupom where nome = @nome";
                        SqlCommand cmd = new SqlCommand(query, sqlconn);
                        cmd.Parameters.AddWithValue("@nome", input);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            cupom.nome = Convert.ToString(reader["nome"]);
                            cupom.dtValid = Convert.ToDateTime(reader["dtValid"]);
                            cupom.valor = Convert.ToDecimal(reader["valor"]);
                        }
                        reader.Close();
                        sqlconn.Close();

                    if (cupom.dtValid >= hoje)
                        {

                            Cupom cupomValido = new Cupom();
                            cupomValido = cupom;
                            //Console.WriteLine("Cupom valido de valor: ", cupom.valor);
                            //Console.WriteLine("Nome: {0,-10},validad: {1,-10}, valor: {2,-10} ", cupom.valor, cupom.dtValid, cupom.valor);
                            return cupomValido;
                        }
                        else
                        {
                           // Console.WriteLine("Nome: {0,-10},validad: {1,-10}, valor: {2,-10} ", cupom.valor, cupom.dtValid, cupom.valor);
                            Console.WriteLine("Cupom fora da data de validade, nenhum desconto aplicado");
                            bool sair = false;
                            while (sair == false)
                            {
                                Console.WriteLine("Gostaria de tentar outro cupom? S/N");
                                string resp = Console.ReadLine();
                                string upper = resp.ToUpper();
                                switch (upper)
                                {
                                    case "S":
                                        buscaCupom();
                                        break;
                                    case "N":
                                        sair = true;
                                    cupom.nome = "Nenhum";
                                    cupom.valor = 0;
                                    return cupom;                                    
                                    default:
                                        Console.WriteLine("Escolha entre S ou N");
                                        sair = false;
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cupom não existe, reiniciando validação cupom");
                        buscaCupom();

                    }
                }
            cupom.nome = "Nenhum";
            cupom.valor = 0;
            return cupom;
            }
            
        }
    }

        
    

