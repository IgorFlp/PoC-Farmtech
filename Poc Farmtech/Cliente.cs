using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    Console.WriteLine("Bem vindo ao cadastro de Clientes\nPreencha o CPF do cliente\n");
                    input = Console.ReadLine();
                    //inserir validação de cpf aqui
                    //transformar string em array to int
                    //executar calculo de validação de cpf
                    //IF correto return input.
                    //Else errado exibe: CPF incorreto tente novamente, return validacao() invoca novamente o metodo
                    break;
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
                    break;
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
                    break;
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
                    break;
                case "dataNasc":
                Console.WriteLine("\nPreencha o data de nascimento do cliente sem barras Ex: DDMMAAAA\n");
                input = Console.ReadLine();
                    string formatacao = "dd/MM/yyyy";
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

                            }
                            else
                            {
                                // Exibir a data convertida
                                Console.WriteLine("Data convertida: " + date.ToString("dd/MM/yyyy"));
                            }
                        }
                        else
                        {
                            // Informar que a conversão falhou devido a um formato inválido
                            Console.WriteLine("Erro: Formato de data inválido.");
                        }
                    }
}

            if (input.Length<10 || input.Length > 10)
                    {
                        Console.WriteLine("Data incorreta, tente novamente sem barras ex:21062000");

                    }else if (Int32.Parse(input) < 1 || Int32.Parse(input[7])>2)
                    {
                        Console.WriteLine("Ano incorreto");
                        return validacao("dataNasc");

                    }
                    break;
                case "genero":
                    break;
                case "rua":
                    break;
                case "bairro":
                    break;
                case "cidade":
                    break;
                case "estado":
                    break;
                case "cep":
                    break;

            }
                
            
        }
        public void cadCliente()
        {
            string[] arrInfo = { "cpf", "nome", "telefone", "email", "dataNasc", "genero", "rua", "bairro", "cidade", "estado", "cep" };
            string cpf;           
            string nome;
            string telefone;
            string email;
            string dataNasc


           
            Console.WriteLine("\nPreencha o genero do cliente M-Masculino, F-Feminino, O-Outro, N-Não informar\n");
            string genero = Console.ReadLine();
            //endereço
            Console.WriteLine("\nPreencha o nome do cliente\n");
            string rua = Console.ReadLine();
            Console.WriteLine("\nPreencha o nome do cliente\n");
            string bairro = Console.ReadLine();
            Console.WriteLine("\nPreencha o nome do cliente\n");
            string cidade = Console.ReadLine();
            Console.WriteLine("\nPreencha o nome do cliente\n");
            string estado = Console.ReadLine();
            Console.WriteLine("\nPreencha o nome do cliente\n");
            string cep = Console.ReadLine();


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
