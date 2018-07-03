﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    public class SocketClient
    {
        private static int _port;
        private static Socket _clientSocket;

        public SocketClient(int port)
        {
            _port = port;
            _clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool ConnectToServer()
        {
            int attempts = 0;

            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    _clientSocket.Connect(IPAddress.Loopback, _port);
                }
                catch (SocketException) 
                {
                    Console.Clear();
                }
            }

            return true;
        }

        private void RequestLoop()
        {
            while (true)
            {
                SendRequest();
                Console.WriteLine(ReceiveResponse());
            }
        }

        public void SendRequest()
        {
            Console.Write("Send a request: ");
            string request = Console.ReadLine();
            SendString(request);

            if (request.ToLower() == "exit")
            {
                Exit();
            }
        }

        public string ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = _clientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return "";
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            return Encoding.ASCII.GetString(data);
        }
        
        private static void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Close();
            Environment.Exit(0);
        }
        
        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            _clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
    }
}