using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Poc_Farmtech
{
    internal class Cliente
    {
        static string validacao(string tipo)
        {
            string input;
            switch (tipo)
            {
                case "cpf":                    
                    Console.WriteLine("Bem vindo ao cadastro de Clientes\nPreencha o CPF do cliente apenas numeros.\n");
                    input = Console.ReadLine();
                    if (input.Length > 11 || input.Length < 11)
                    {
                        Console.WriteLine("CPF invalido, deve ter 11 caracteres, digite novamente");
                        return validacao("cpf");
                    }
                    else
                    {
                        using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
                        {
                            try
                            {
                                sqlconn.Open();
                                string query = $"select count (*) from Tb_cliente where cpf = @input";
                                SqlCommand cmd = new SqlCommand(query, sqlconn);
                                cmd.Parameters.AddWithValue("@input", input);                                
                                int count = (int)cmd.ExecuteScalar();
                                if (count > 0)
                                {
                                    Console.WriteLine($"O CPF '{input}' ja esta cadastrado.");
                                    sqlconn.Close();

                                    return validacao("cpf");

                                }
                                else
                                {
                                    //calculo de validação do cpf

                                    return input;
                                }


                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex.Message);
                                return validacao("cpf");
                            }
                        }
                        
                    }
            
        
                    
                //    //inserir validação de cpf aqui
                //    //transformar string em array to int
                //    //executar calculo de validação de cpf
                //    //IF correto return input.
                //    //Else errado exibe: CPF incorreto tente novamente, return validacao() invoca novamente o metodo
                //    break;
                case "nome":
                    Console.WriteLine("\nPreencha o nome do cliente\n");
                    input = Console.ReadLine();
                    if (input[1].Equals(" ") || input[1].Equals("."))
                    {
                        Console.WriteLine("Caractere invalido no inicio do nome.");
                        return validacao("nome");
                    }else if(input.Length > 50)
                    {
                        Console.WriteLine("Nome excede 50 caracteres, digite novamente abreviando");
                        return validacao("nome");
                    }
                    else
                    {
                        return input;
                    }                   
                case "telefone":
                    Console.WriteLine("\nPreencha o telefone do cliente com DDD e numero, apenas numeros 18123456789\n");
                    input = Console.ReadLine();
                    if (input.Length>11||input.Length<11)
                    {
                        Console.WriteLine("\nTamanho incorreto do telefone\n");
                        return validacao("telefone");
                    }
                    else
                    {
                        return input;
                    }
                    
                case "email":
                Console.WriteLine("\nPreencha o email do cliente\n");
                input = Console.ReadLine();
                    if (input.Length > 50)
                    {
                        Console.WriteLine("Email excede 50 caracteres");
                        return validacao("email");
                    }
                    else
                    {
                        return input;
                    }
                    
                case "dataNasc":
                Console.WriteLine("\nPreencha o data de nascimento do cliente com barras Ex: 18/07/2001\n");
                 input = Console.ReadLine();                 
                    try
                    {
                        //Declara variavel do tipo data pra receber a data convertida
                        DateTime date;

                        if (DateTime.TryParse(input, out date))
                        {
                            // Verificar se o ano está entre 1900 a 2024)
                            if (date.Year < 1900 || date.Year > 2024)
                            {
                                Console.WriteLine("Ano inválido. Por favor, digite um ano entre 1900 e 2024.");
                                return validacao("dataNasc");
                            }
                            else
                            {
                                // Exibir a data convertida
                                Console.WriteLine("Data: " + date.ToString("dd/MM/yyyy"));
                                return date.ToString("dd/MM/yyyy");
                            }                            
                        }
                        else
                        {
                            // Informar que a conversão falhou devido a um formato inválido
                            Console.WriteLine("Erro: Formato de data inválido.");
                            return validacao("dataNasc");
                        }
                        
                    }catch (Exception ex)
                    {
                        Console.WriteLine("Erro: ", ex.Message);
                        return validacao("dataNasc");
                    }                             
                // te, algum erro aqui.   VVVVVV 
                case "genero":
                Console.WriteLine("\nPreencha o genero do cliente \nM: Masculino, \nF: Feminino, \nO: Outro, \nN: Não informar\n");
                input = Console.ReadLine();
                    string upper = input.ToUpper();
                    input = upper;
                    if (input != "M" & input != "F" & input != "O"& input != "N")
                    {
                        Console.WriteLine("Insira um valor valido");
                        return validacao("genero");
                    }
                    else
                    {
                        return input;
                    }  
                    // Tb_cl_endereco
                case "rua":
                Console.WriteLine("\nPreencha a rua do cliente\n");
                input= Console.ReadLine();
                    try
                    {
                        if (input.Length > 80)
                        {
                            Console.WriteLine("Rua excede o a quantidade maxima de caracteres 80");
                            return validacao("rua");
                        }
                        else
                        {
                            Console.WriteLine("Rua: "+input);
                            return input;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro: ", ex.Message);
                        return validacao("rua");
                    }                    
                case "bairro":
                Console.WriteLine("\nPreencha o bairro do cliente\n");
                input = Console.ReadLine();
                    try {
                        if (input.Length > 30)
                        {
                            Console.WriteLine("Bairro excede o a quantidade maxima de caracteres 30");
                            return validacao("bairro");
                        }
                        else
                        {
                            Console.WriteLine("Bairro: " + input);
                            return input;
                        }
                    }
                    catch (Exception ex){ Console.WriteLine("Erro: " + ex.Message);
                        return validacao("bairro");
                    }
                    
                case "cidade":
                    Console.WriteLine("\nPreencha a cidade do cliente\n");
                    input = Console.ReadLine();
                    try
                    {
                        if (input.Length > 30)
                        {
                            Console.WriteLine("Cidade excede o a quantidade maxima de caracteres 30");
                            return validacao("cidade");
                        }
                        else
                        {
                            Console.WriteLine("Cidade: " + input);
                            return input;
                        }
                    }catch (Exception ex) { Console.WriteLine("Erro: "+ex.Message); return validacao("cidade"); }                    
                case "estado":
                    string[] estados = {
                    "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
                    "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
                    "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
                    };
                    Console.WriteLine("\nPreencha o estado do cliente\n");
                    input = Console.ReadLine();
                    upper = input.ToUpper();
                    input = upper;
                    
                    if (estados.Contains(input))
                    {
                        Console.WriteLine("Estado: "+input);
                        return input;
                    }
                    else
                    {
                        Console.WriteLine("Estado invalido, tente novamente\n");
                        return validacao("estado");
                    }                    
                case "cep":
                    Console.WriteLine("\nPreencha o CEP do cliente, apenas numeros, Ex: 19807385\n");
                    input = Console.ReadLine();
                    try
                    {
                        if (input.Length > 8 || input.Length < 8)
                        {
                            Console.WriteLine("CEP invalido, preencha novamente");
                            return validacao("cep");
                        }
                        else
                        {
                            Console.WriteLine("CEP: " + input);
                            return input;
                        }
                    } catch (Exception ex) { Console.WriteLine("Erro: "+ex.Message); return validacao("cep");}
                default:
                    Console.WriteLine("Tipo inválido, tente novamente.");
                    return validacao("tipo");
            }

            }
                
            
        
        public void cadCliente()
        {            
            string cpf = validacao("cpf");           
            string nome = validacao("nome");
            string telefone = validacao("telefone");
            string email = validacao("email");
            string dataNasc = validacao("dataNasc");
            string genero = validacao("genero");

            //endereço        
            string rua = validacao("rua");
            string bairro = validacao("bairro");
            string cidade = validacao("cidade");            
            string estado = validacao("estado");
            string cep = validacao("cep");
            
            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {
                    sqlconn.Open();
                    string query = "INSERT INTO Tb_cliente (cpf,nome,telefone,email,dataNscm,genero) VALUES (@cpf,@nome,@telefone,@email,@dataNscm,@genero)";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cpf",cpf); // Criar validação ainda
                    cmd.Parameters.AddWithValue("@nome",nome);
                    cmd.Parameters.AddWithValue("@telefone", telefone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@dataNscm", dataNasc);
                    cmd.Parameters.AddWithValue("@genero", genero);


                    sqlconn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: "+ex.Message);
                }
                
            } 
        }
        //Atributos
//- CPF : Char(11)
//- Nome : Char(50)
//- Telefone : Char(11)
//- Email : Char(50)
//- DataNasc : Date
//- Genero: Char
//- Endereço : Char(150)
        

    }
}
