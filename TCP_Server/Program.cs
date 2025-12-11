using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCP_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Servidor que escucha cualquier IP local
            TcpListener miserverGuapo = new TcpListener(IPAddress.Any,5000);
            //abre el puerto y empieza a escuchar conexiones entrantes
            miserverGuapo.Start();
            Console.WriteLine("Esperando a que se conecte mi pana...");

            //AcceptTcpClient es una llamada bloqueante
            //se queda esperando a que un cliente se conecte
            //cuando alguien se conecta devuelve el tcpClient
            TcpClient cliente = miserverGuapo.AcceptTcpClient();
            Console.WriteLine("Cliente conectado");

            Console.Write("Escribe lo que le quieres mandar al cliente: ");
            string mensajito = Console.ReadLine();

            //Obtener el flujo de datos asociado al socket de el clinete especifico
            //todo lo que escriba en flujodatos se envia al cliente
            NetworkStream flujodatos = cliente.GetStream();
            byte[] purodatachorizo = Encoding.UTF8.GetBytes(mensajito);
            flujodatos.Write(purodatachorizo, 0, purodatachorizo.Length);

            //cierra la conexion con el cliente
            cliente.Close();

            //cierra el puerto y deha de escuchar
            miserverGuapo.Stop();


        }
    }
}
