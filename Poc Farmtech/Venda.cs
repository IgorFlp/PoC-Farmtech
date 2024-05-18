using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Venda
    {
        static int id;
        static string usrName;
        static Cliente cliente = new Cliente();
        static List<Produto> produtos = new List<Produto>();
        static List<decimal> quantidades = new List<decimal>();
        static Cupom cupom = new Cupom();
        //+ID: Int
        static string entrega;
        static decimal subtotal;
        static decimal frete;
        static decimal desconto;
        static decimal total;
        static string mtdPagto;
        

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
        private static string slcMtdPagto()
        {
            Console.WriteLine("\nSelecione o metodo de pagamento\n1-Boleto\n2-Crédito\n3-Débito\n4-Dinheiro\n5-PIX");
            string resp = Console.ReadLine();
            switch (resp)
            {
                case "1":
                    Console.WriteLine("Selecionado Boleto");
                    return "Boleto";                    
                case "2":
                    Console.WriteLine("Selecionado Crédito");
                    return "Crédito";                    
                case "3":
                    Console.WriteLine("Selecionado Débito");
                    return "Débito";                    
                case "4":
                    Console.WriteLine("Selecionado Dinheiro");
                    return "Dinheiro";                    
                case "5":
                    Console.WriteLine("Selecionado PIX");
                    return "PIX";                    
                default:
                    Console.WriteLine("Entrada invalida, tente novamente");
                    slcMtdPagto();
                    break;
            }
            return null;
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
        private static decimal calcFrete()
        {
            try
            {
                Console.WriteLine("Selecione 1-Entrega ou 2-Retirada");
                string resp = Console.ReadLine();
                switch (resp)
                {
                    case "1":
                        Console.WriteLine("Selecionado Entrega");
                        entrega = "Entrega";
                        return 15.00m;                        
                    case "2":
                        Console.WriteLine("Selecionado Retirada");
                        entrega = "Retirada";
                        return 0m;                        
                    default:
                        Console.WriteLine("Resposta invalida tente novemente");
                        calcFrete();
                        break;
                }
            } catch (Exception ex) { Console.WriteLine("Erro: " + ex.Message);}
            return 0;
        }
        private static void gravaDB()
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                {
                    sqlconn.Open();
                    string query = $"INSERT INTO Db_Farmtech.dbo.Tb_venda (cl_cpf,usr_nome,subtotal,frete,cupom,desconto,total,mtdPagto) VALUES (@cpf,@usrName,@subtotal,@frete,@cupom,@desconto,@total,@mtdPagto); SELECT SCOPE_IDENTITY();";
                    //(cl_cpf,usr_nome,subtotal,frete,cupom,desconto,total,mtdPagto)
                    //(@cpf,@usrName,@subtotal,@frete,@cupom,@desconto,@total,@mtdPagto)
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                    cmd.Parameters.AddWithValue("@usrName", usrName);
                    cmd.Parameters.AddWithValue("@subtotal", subtotal);
                    cmd.Parameters.AddWithValue("@frete", frete);
                    cmd.Parameters.AddWithValue("@cupom", cupom.nome);
                    cmd.Parameters.AddWithValue("@desconto", desconto);
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@mtdPagto", mtdPagto);
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                    Console.WriteLine("Venda inserida com sucesso!");
                    cmd.Parameters.Clear();
                    sqlconn.Close();
                }
                using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                {
                    sqlconn.Open();
                    string query = $"INSERT INTO Db_Farmtech.dbo.Tb_ven_produtos (ven_id,pdt_id,quant) VALUES (@ven_id,@pdt_id,@quant)";
                    //(cl_cpf,usr_nome,subtotal,frete,cupom,desconto,total,mtdPagto)
                    //(@cpf,@usrName,@subtotal,@frete,@cupom,@desconto,@total,@mtdPagto)
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    for (int i = 0; i < produtos.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@ven_id", id);
                        cmd.Parameters.AddWithValue("@pdt_id", produtos[i].id);
                        cmd.Parameters.AddWithValue("@quant", quantidades[i]);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }                    
                    Console.WriteLine("Produtos da venda inseridos com sucesso!");
                    sqlconn.Close();
                }                
            }
            catch (Exception ex) { Console.WriteLine("Erro: " + ex.ToString()); }
        }
        private static void confirmaVenda(string usrName)
        {
            Console.WriteLine("\nResumo da venda\n\nCliente nome: {0,5}  Cliente CPF: {1,5}",cliente.nome,cliente.cpf);
            Console.WriteLine("\nProdutos");
            Console.WriteLine("{0,-5} {1,-30} {2,-14} {3,-10} {4,-11}", "ID", "Nome", "preço Unitario","Quantidade", "Preço total");
            
            for (int index = 0; index < produtos.Count;index++)
            {
                decimal precototal = produtos[index].precoUn * quantidades[index];
                Console.WriteLine("{0,-5} {1,-30} {2,-14:C} {3,-10} {4,-11:C}", produtos[index].id, produtos[index].nome, produtos[index].precoUn, quantidades[index], precototal);
                
            }
            Console.WriteLine("\nCUPOM: "+cupom.nome+"\nEntrega: "+entrega+"\nMetodo Pagamento: "+mtdPagto);
            Console.WriteLine("\nSubtotal: {0,-8:C}\nFrete: {1,-8:C}\nDesconto: {2,-8:C}\nTotal: {3,-8:C}",subtotal,frete,desconto,total);
            Console.WriteLine("\nVendedor: "+usrName);
            Console.WriteLine("\n\nDeseja confirmar e salvar a venda? S/N");
            string resp;
            bool sair = false;
            while(sair == false)
            {
                resp = Console.ReadLine();
                string upper = resp;
                switch (upper)
                {
                    case "S":
                        gravaDB();
                        sair = true;                        
                        break;
                    case "N":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Valor invalido tente novamente");
                        sair = false;
                        break;
                }
            }
        }
        public static void vender(string usuario)
        {
            usrName = usuario;
            Console.WriteLine("Bem vindo ao modulo de vendas\n");            
            cliente = slcCliente();
            Console.WriteLine("Nome: "+cliente.nome+"\nCPF: "+cliente.cpf);
            exibeProduto();
            slcProduto();            
            subtotal = calcSubtotal();
            Console.WriteLine("\nSubtotal: {0,10:C} ",subtotal);
            bool sair = false;
            while (sair == false)
            {
                Console.WriteLine("\nGostaria de utilizar um cupom? S/N");
                string resp = Console.ReadLine();
                string upper = resp.ToUpper();
                switch (upper)
                {
                    case "S":
                        string input = Cupom.buscaCupom();
                        if (input == null)
                        {
                            sair = false;
                            while (sair == false)
                            {
                                Console.WriteLine("Gostaria de tentar outro cupom? S/N");                                
                                resp = Console.ReadLine();
                                upper = resp.ToUpper();
                                switch (upper)
                                {
                                    case "S":                                        
                                        input = Cupom.buscaCupom(); 
                                        if (input == null)
                                        {                                          
                                            sair = false;
                                        }
                                        else
                                        {
                                            sair = true;
                                        }
                                        break;
                                    case "N":
                                        sair = true;
                                        break;
                                    default:
                                        Console.WriteLine("Escolha entre S ou N");
                                        sair = false;

                                        break;
                                }
                            }

                        }
                        if(input != null)
                        {
                            Console.WriteLine("input:" + input);
                            cupom = Cupom.validaCupom(input); }
                        
                        if (cupom != null)
                        {
                            desconto = cupom.valor;
                            Console.WriteLine("Cupom aplicado com desconto de: {0:C}", desconto);
                        }
                        else
                        {
                            Console.WriteLine("Cupom inválido ou não aplicável.");
                            desconto = 0m;
                        }
                        sair = true;
                        break;
                    case "N":
                        sair = true;
                        desconto = 0m;
                        break;
                    default:
                        Console.WriteLine("Escolha entre S ou N");
                        sair=false;
                        break;
                }
            }
            Console.WriteLine("\nDesconto: {0,-5:C}",desconto);
            frete = calcFrete();
            total = subtotal + frete - desconto;
            Console.WriteLine("\nTotal:{0,-8:C} ",total);
            mtdPagto = slcMtdPagto();
            confirmaVenda(usrName);
            Console.ReadLine();
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
