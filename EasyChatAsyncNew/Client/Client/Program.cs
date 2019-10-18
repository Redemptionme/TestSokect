using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client
{
	class Program
	{	
		static void Main(string[] args)
		{
            for (int i = 0; i < 10; i++) 
            {
                TCP_ClientAsync client = new TCP_ClientAsync();
                client.name = "端" + i;
                client.Start();
            }
            

            while (true) ;
		}         
	}
}