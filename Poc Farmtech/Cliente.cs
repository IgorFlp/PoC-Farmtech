using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using System;
using System.Collections;
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
        public string cpf;
        public string nome;
        string telefone;
        string email;
        string dataNasc;
        string genero;
        string rua;
        string bairro;
        string cidade;
        string estado;
        string cep;
        
        public static string validacao(string tipo)
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
                        try
                        {
                            int[] digitosCpf = new int[input.Length];
                            for (int i = 0; i < input.Length; i++)
                            {
                                digitosCpf[i] = input[i] - '0'; // - '0' serve pra pegar o numero literal, sem ele o array recebe em asci                    
                            }
                            //Calculo de validação do cpf
                            // Do primeiro ao nono digito multiplica-se por 10 a 2, sendo o primeiro digito x 10 e o nono x 2
                            //Depois somamos todos os resultado, e dividimos por 11 guardamos o resultado e o resto
                            // Se o resto da divisão for menor que 2, então o primeiro digito apos o traço é igual a 0 (Zero).
                            // Se o resto da divisão for maior ou igual a 2, então o primeiro dígito verificador é igual a 11 menos o resto da divisão(11 - resto).
                            //USAREMOS ESSE PASSO PARA SABER SE O PRIMEIR DIGITO É VALIDO^
                            int[] passo1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                            int[] passo2 = { 11,10, 9, 8, 7, 6, 5, 4, 3, 2 };
                            int soma = 0;
                            int soma2 = 0;
                            
                            for (int i = 0;i < passo1.Length;i++)
                            {
                                soma = soma + (digitosCpf[i] * passo1[i]);
                            }
                            int divisao = soma / 11;
                            int resto = soma % 11;
                            if (resto < 2 && digitosCpf[9] == 0)
                            {                               
                                    //Segundo passo
                                    int resto2;
                                for (int i = 0; i < passo2.Length; i++)
                                {
                                    soma2 = soma2 + (digitosCpf[i] * passo2[i]);
                                }
                                resto2 = soma2 % 11;
                                if (resto2 < 2 && digitosCpf[10] == 0)
                                {
                                    Console.WriteLine("CPF VALIDO");                                 

                                }
                                else if (resto2 >= 2 && 11 - resto2 == digitosCpf[10])
                                {
                                    Console.WriteLine("CPF VALIDO");                                    
                                }
                                else
                                {   //Falha segundo digito
                                    Console.WriteLine("CPF invalido, tente novamente");
                                    return validacao("cpf");
                                }

                            }
                            else if(resto >= 2 && 11-resto == digitosCpf[9])
                            {                                                              
                                // Segundo passo
                                    int resto2;
                                    for (int i = 0; i < passo2.Length; i++)
                                    {
                                        soma2 = soma2 + (digitosCpf[i] * passo2[i]);
                                    }
                                    resto2 = soma2 % 11;
                                    if(resto2 < 2 && digitosCpf[10] == 0)
                                    {                                        
                                        Console.WriteLine("CPF VALIDO");                                    
                                                                                  
                                    }else if(resto2 >= 2 && 11-resto2 == digitosCpf[10])
                                    {
                                        Console.WriteLine("CPF VALIDO");                                    
                                }
                                else
                                {   //Falha segundo digito
                                    Console.WriteLine("CPF invalido, tente novamente");
                                    return validacao("cpf");
                                }
                                
                            }else { //Falha no primeiro digito
                                    Console.WriteLine("CPF invalido, tente novamente");
                                return validacao("cpf");
                            }
                            


                            //Apos refazemos o calculo anterior com os 10 digitos, multipliclando de 11 a 2
                            //Somamos tudo e dividimos por 11 guardamos o resultado e o resto
                            //Se o resto da divisão for menor que 2, então o dígito é igual a 0 (Zero).
                            //Se o resto da divisão for maior ou igual a 2, então o dígito é igual a 11 menos o resto da divisão(11 - resto).
                            //11 - 6 = 5   logo 5 é o nosso segundo dígito verificador.
                            //Depois 
                        }
                        catch (Exception ex) { Console.WriteLine("Erro: " + ex.Message); }
                        
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
                    Console.WriteLine("\nPor favor preencha o nome\n");
                    input = Console.ReadLine();
                    if (input[1].Equals(" ") || input[1].Equals("."))
                    {
                        Console.WriteLine("Caractere invalido no inicio do nome.");
                        return validacao("nome");
                    }
                    else if (input.Length > 50)
                    {
                        Console.WriteLine("Nome excede 50 caracteres, digite novamente abreviando");
                        return validacao("nome");
                    }
                    else
                    {
                        return input;
                    }
                case "telefone":
                    Console.WriteLine("\nPor favor preencha o telefone com DDD e numero, apenas numeros 18123456789\n");
                    input = Console.ReadLine();
                    if (input.Length > 11 || input.Length < 11)
                    {
                        Console.WriteLine("\nTamanho incorreto do telefone\n");
                        return validacao("telefone");
                    }
                    else
                    {
                        return input;
                    }

                case "email":
                    Console.WriteLine("\nPor favor preencha o email\n");
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
                    Console.WriteLine("\nPor favor preencha o data de nascimento com barras Ex: 18/07/2001\n");
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

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro: ", ex.Message);
                        return validacao("dataNasc");
                    }
                // te, algum erro aqui.   VVVVVV 
                case "genero":
                    Console.WriteLine("\nPor favor preencha o genero do cliente \nM: Masculino, \nF: Feminino, \nO: Outro, \nN: Não informar\n");
                    input = Console.ReadLine();
                    string upper = input.ToUpper();
                    input = upper;
                    if (input != "M" & input != "F" & input != "O" & input != "N")
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
                    Console.WriteLine("\nPor favor preencha a rua com nome da rua e numero Ex: Rua Jacarandá, 356\n");
                    input = Console.ReadLine();
                    try
                    {
                        if (input.Length > 80)
                        {
                            Console.WriteLine("Rua excede o a quantidade maxima de caracteres 80");
                            return validacao("rua");
                        }
                        else
                        {
                            
                            return input;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro: ", ex.Message);
                        return validacao("rua");
                    }
                case "bairro":
                    Console.WriteLine("\nPor favor preencha o bairro\n");
                    input = Console.ReadLine();
                    try
                    {
                        if (input.Length > 30)
                        {
                            Console.WriteLine("Bairro excede o a quantidade maxima de caracteres 30");
                            return validacao("bairro");
                        }
                        else
                        {
                            
                            return input;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro: " + ex.Message);
                        return validacao("bairro");
                    }

                case "cidade":
                    Console.WriteLine("\nPor favor preencha a cidade\n");
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
                            
                            return input;
                        }
                    }
                    catch (Exception ex) { Console.WriteLine("Erro: " + ex.Message); return validacao("cidade"); }
                case "estado":
                    string[] estados = {
                    "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
                    "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
                    "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
                    };
                    Console.WriteLine("\nPor favor preencha o estado\n");
                    input = Console.ReadLine();
                    upper = input.ToUpper();
                    input = upper;

                    if (estados.Contains(input))
                    {
                        
                        return input;
                    }
                    else
                    {
                        Console.WriteLine("Estado invalido, tente novamente\n");
                        return validacao("estado");
                    }
                case "cep":
                    Console.WriteLine("\nPor favor preencha o CEP, apenas numeros, Ex: 19807385\n");
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
                           
                            return input;
                        }
                    }
                    catch (Exception ex) { Console.WriteLine("Erro: " + ex.Message); return validacao("cep"); }
                default:
                    Console.WriteLine("Tipo inválido, tente novamente.");
                    return validacao("tipo");
            }

        }



        public void cadCliente(string usrName)
        {
            cpf = validacao("cpf");
            nome = validacao("nome");
            telefone = validacao("telefone");
            email = validacao("email");
            dataNasc = validacao("dataNasc");
            genero = validacao("genero");

            //endereço        
            rua = validacao("rua");
            bairro = validacao("bairro");
            cidade = validacao("cidade");
            estado = validacao("estado");
            cep = validacao("cep");

            using (SqlConnection sqlconn = new SqlConnection(Program.connectionString))
            {
                try
                {
                    sqlconn.Open();
                    string query = "INSERT INTO Tb_cliente (cpf,nome,telefone,email,dataNscm,genero) VALUES (@cpf,@nome,@telefone,@email,@dataNscm,@genero)";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cpf", cpf); // Criar validação ainda
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@telefone", telefone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@dataNscm", dataNasc);
                    cmd.Parameters.AddWithValue("@genero", genero);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();



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
                    string query = $"insert into Tb_cl_Endereco (cl_cpf,rua,bairro,cidade,estado,cep)values (@cpf,@rua,@bairro,@cidade,@estado,@cep)";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cpf", cpf);
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
                    string query = $"select count (*) from Tb_cliente where cpf = @cpf";
                    SqlCommand cmd = new SqlCommand(query, sqlconn);
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Console.WriteLine($"O CPF '{cpf}' foi cadastrado no sistema.");
                        

                    }
                    else
                    {
                        //calculo de validação do cpf
                        Console.WriteLine($"O CPF '{cpf}' NÃO foi cadastrado");
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
                        string query = $"select count (*) from Tb_cl_endereco where cl_cpf = @cpf";
                        SqlCommand cmd = new SqlCommand(query, sqlconn);
                        cmd.Parameters.AddWithValue("@cpf", cpf);
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            Console.WriteLine($"O ENDEREÇO DO CPF '{cpf}' foi cadastrado no sistema.");                          

                        }
                        else
                        {
                            //calculo de validação do cpf
                            Console.WriteLine($"O ENDEREÇO DO CPF '{cpf}' NÃO foi cadastrado");
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
