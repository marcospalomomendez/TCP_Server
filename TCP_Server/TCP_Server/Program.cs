using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_Server
{
    internal class Program
    {
            List<TcpClient> clientes = new List<TcpClient>();
            TcpListener listerner;

            public void EjemploTCPServer()
            {
                //Servidor que escucha en cualquier IP local
                TcpListener miserverguapo = new TcpListener(IPAddress.Any, 5000);
                //abre el puerto y empieza a escuchar conexiones entrantes
                miserverguapo.Start();
                Console.WriteLine("Esperando a que se conecte mi pana...");

                //AcceptTcpClient es una llamada bloquante
                //se queda esperando a que un cliente se conecte
                //cuando alguien se conecta devuelve el tcpclient
                TcpClient cliente = miserverguapo.AcceptTcpClient();
                Console.WriteLine("Cliente conectado");

                //Obterner el flujo de datos asociado al socket de el cliente especifico
                //todo lo que escriba en flujodatos se envia al cliente
                NetworkStream flujodatos = cliente.GetStream();
                byte[] purodatachorizo = Encoding.UTF8.GetBytes("Soy el server, klk manin...");
                flujodatos.Write(purodatachorizo, 0, purodatachorizo.Length);

                //cierra la conexion con le clientardo
                cliente.Close();

                //cierra el puerto y deja de escuchar
                miserverguapo.Stop();

            }

            public void EjemploTCPServerConMultiplesClientes()
            {
                listerner = new TcpListener(IPAddress.Any, 5000);
                listerner.Start();
                Console.WriteLine("Server arriba gente");

                Thread t = new Thread(LlegaUnCliente);
                t.IsBackground = true;
                t.Start();

                while (true)
                {
                    //TcpClient clientin = listerner.AcceptTcpClient();
                    Console.Write("Escribe algo para enviar a los clientes (SALIMOS para terminar): ");
                    string palabrin = Console.ReadLine();
                    if (palabrin == "SALIMOS")
                    {
                        break;
                    }

                    byte[] data = Encoding.UTF8.GetBytes("El server escribe: " + palabrin + "\n");
                  
                    for (int i = clientes.Count - 1; i >= 0; i--)
                    {
                        NetworkStream flujo = clientes[i].GetStream();
                        flujo.Write(data, 0, data.Length);

                        //------

                        //clientes[i].Close();
                        //clientes.RemoveAt(i);
                    }
                }
            }

            public void LlegaUnCliente()
            {
                while (true)
                {
                    TcpClient cliente = listerner.AcceptTcpClient();
                    clientes.Add(cliente);
                }
            }



            public void ServerMultiHilo()
            {
                TcpListener server = new TcpListener(IPAddress.Any, 5000);
                server.Start();
                Console.WriteLine("Server operativooo");

                while (true)
                {
                    TcpClient cliente = server.AcceptTcpClient();
                    Console.WriteLine("cliente conectado");
                    clientes.Add(cliente);

                    Thread t = new Thread(ElClientardo);
                    t.IsBackground = true;
                    t.Start(cliente);
                }

            }

            public void ElClientardo(object clientin)
            {
                TcpClient cliente = (TcpClient)clientin;
                NetworkStream flujo = cliente.GetStream();
                byte[] b = new byte[1024];

                while (true)
                {
                    int lectura = flujo.Read(b, 0, b.Length);
                    if (lectura <= 0)
                    {
                        break;
                    }

                    string mensaje = Encoding.UTF8.GetString(b, 0, lectura).Trim();

                    if (mensaje.Length == 0)
                    {
                        continue;
                    }

                    Console.WriteLine(mensaje);

                    // se lo envio a todos
                    byte[] d = Encoding.UTF8.GetBytes(mensaje + "\n");
                    for (int i = 0; i < clientes.Count; i++)
                    {
                        NetworkStream stream = clientes[i].GetStream();
                        stream.Write(d, 0, d.Length);
                    }
                }


                clientes.Remove(cliente);
                cliente.Close();

            }

            static void Main(string[] args)
            {

                Program p = new Program();
                //p.ServerMultiHilo();
                p.EjemploTCPServerConMultiplesClientes();

            }
        }
    }
