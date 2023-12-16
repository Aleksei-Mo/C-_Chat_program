using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;


namespace ServerCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => Server());
            Console.ReadKey();
        }

        public static void Server()
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Сервер ждет сообщение от клиента");
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            while (true)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                var messageText = Encoding.UTF8.GetString(buffer);
                if (messageText.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("The Server has recieved the shutdown keyword 'Exit' from Client, chat session will be closed.");
                    cancelTokenSource.Cancel();
                    cancelTokenSource.Dispose();
                }
                Task.Run(() =>
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("The session has been canceled.");
                        return;
                    }
                    Message message = Message.DeserializeFromJson(messageText);
                    message.Print();
                    byte[] clientReply = Encoding.UTF8.GetBytes("Message has been recieved.");
                    udpClient.Send(clientReply, clientReply.Length, iPEndPoint);
                    Console.WriteLine($"Sent.");

                }, token);
            }
        }
    }
}
