using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Relatorios
    {
        static DateTime dtInicial = new DateTime();
        static DateTime dtFinal = new DateTime();
        static string tipoRelatorio;
        private static void setDatas()
        {
            try
            {
                Console.WriteLine("Preencha a data de inicio do relatório");
                string input = Console.ReadLine();
                DateTime inicial = new DateTime();
                DateTime final = new DateTime();
                DateTime.TryParse(input, out inicial);

                Console.WriteLine("Preencha a data de final do relatório");
                input = Console.ReadLine();
                DateTime.TryParse(input, out final);
                
                if (final < inicial)
                {
                    Console.WriteLine("Data final é maior que a inicial, tente novamente");
                    setDatas();
                }
                else
                {
                    dtInicial = inicial;
                    dtFinal = final;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Algum erro aconteceu tente novamente.");
                Console.WriteLine("Erro: " + ex.Message);
                setDatas();    
            }
        }
        private static string slcTipoRel()
        {
            try
            {
                Console.WriteLine("Qual tipo de relatório deseja emitir?\n\n1-Vendas\n2-Produções");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        return "Vendas";
                    case "2":
                        return "Produções";
                    default:
                        Console.WriteLine("Valor invalido tente novamente");
                        slcTipoRel();
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                slcTipoRel();
            }
            return null;
        }
        private static void relVendas()
        {
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {
                    List<int> id = new List<int>();
                    List<decimal> subtotal = new List<decimal>();
                    List<string> cupom = new List<string>();
                    List<decimal> frete = new List<decimal>();
                    List<decimal> desconto = new List<decimal>();
                    List<decimal> total = new List<decimal>();
                    List<string> mtdPagto = new List<string>();
                    List<string> nomeUsuario = new List<string>();
                    List<string> entrega = new List<string>();
                    List<string> cpf = new List<string>();
                    List<DateTime> dtVenda = new List<DateTime>();
                    sqlconn.Open();
                    string query = $"select * from Tb_venda where dtVenda >= @dtInicial AND dtVenda <= @dtFinal";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@dtInicial", dtInicial);
                    cmd.Parameters.AddWithValue("@dtFinal", dtFinal);
                    SqlDataReader reader = cmd.ExecuteReader();
                                        

                    while (reader.Read())
                    {
                        id.Add(Convert.ToInt32(reader["id"]));
                        cpf.Add(reader["cl_cpf"].ToString());
                        subtotal.Add(Convert.ToDecimal(reader["subtotal"]));
                        frete.Add(Convert.ToDecimal(reader["frete"]));
                        desconto.Add(Convert.ToDecimal(reader["desconto"]));
                        total.Add(Convert.ToDecimal(reader["total"]));                        
                        cupom.Add(reader["cupom"].ToString());
                        mtdPagto.Add(reader["mtdPagto"].ToString());
                        entrega.Add(reader["entrega"].ToString());
                        nomeUsuario.Add(reader["usr_nome"].ToString());
                        dtVenda.Add(Convert.ToDateTime(reader["dtVenda"]));
                    }
                    reader.Close();                    
                    sqlconn.Close();
                    Console.WriteLine("{0,-2} {1,-13} {2,-10} {3,-10} {4,-10} {5,-10} {6,-12} {7,-12} {8,-12} {9,-20} {10,-15}", "ID", "CPF Cliente", "Subtotal", "Frete","Desconto","Total","Cupom","Mtd Pagto","Entrega","Vendedor","Data Venda");
                    for(int i = 0; i <= id.Count;i++)
                    {
                        Console.WriteLine("{0,-2} {1,-13} {2,-10:C} {3,-10:C} {4,-10:C} {5,-10:C} {6,-12} {7,-12} {8,-12} {9,-20} {10,-15}", id[i], cpf[i], subtotal[i], frete[i], desconto[i], total[i], cupom[i], mtdPagto[i], entrega[i], nomeUsuario[i], dtVenda[i].Date);
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
        }
        private static void relProdução()
        {
            List<int> id = new List<int>();
            //List<int> pdt_id = new List<int>();
            List<string> nomePdt = new List<string>();
            List<decimal> quant = new List<decimal>();
            List<DateTime> dtProd = new List<DateTime>();
             int[,] pdt_id = {};
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {                  
                    //Coleta id e data da tabela produção
                    sqlconn.Open();
                    string query = $"select * from Tb_producao where dataProd >= @dtInicial AND dataProd <= @dtFinal";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@dtInicial", dtInicial);
                    cmd.Parameters.AddWithValue("@dtFinal", dtFinal);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        id.Add(Convert.ToInt32(reader["id"]));
                        dtProd.Add(Convert.ToDateTime(reader["dataProd"]));
                    }
                
                    reader.Close();
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
                    string query = $"select * from Tb_producao_produtos where pdc_id = @id";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    for (int i = 0; i < dtProd.Count; i++)
                    {                        
                        cmd.Parameters.AddWithValue("@id", id[i]);
                        SqlDataReader reader = cmd.ExecuteReader();

                        for (int j = 0;reader.Read() == true;j++)
                        {
                            pdt_id[i,j] = Convert.ToInt32(reader["id"]);
                            dtProd.Add(Convert.ToDateTime(reader["dataProd"]));
                        }
                        cmd.Parameters.Clear();
                        reader.Close();
                    }                    
                    sqlconn.Close();
                    for (int i = 0; i <= pdt_id.Length; i++)
                    {
                        Console.WriteLine("\n\n");
                        for (int j = 0; j <= pdt_id.Length; j++)
                        {
                            Console.WriteLine("Id: "+pdt_id[i,j]);
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
            //Console.WriteLine("{0,-2} {1,-13} {2,-10} {3,-10} {4,-10} {5,-10} {6,-12} {7,-12} {8,-12} {9,-20} {10,-15}", "ID", "CPF Cliente", "Subtotal", "Frete", "Desconto", "Total", "Cupom", "Mtd Pagto", "Entrega", "Vendedor", "Data Venda");
            //for (int i = 0; i <= id.Count; i++)
            //{
            //    Console.WriteLine("{0,-2} {1,-13} {2,-10:C} {3,-10:C} {4,-10:C} {5,-10:C} {6,-12} {7,-12} {8,-12} {9,-20} {10,-15}", id[i], cpf[i], subtotal[i], frete[i], desconto[i], total[i], cupom[i], mtdPagto[i], entrega[i], nomeUsuario[i], dtVenda[i].Date);

            //}
        }
        public static void gerarRelatorio()
        {
            setDatas();
            tipoRelatorio = slcTipoRel();
            switch (tipoRelatorio)
            {
                case "Vendas":
                    relVendas();
                    break;
                case "Produções":
                    relProdução();
                    break;
                default:
                    Console.WriteLine("Opção invalida");
                    break;
            }
                        
                        
                        
                        
                   
                
            
        }
    }
}
