using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ServerCode;



namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                Task.Run(() => SentMessage("Aleksei", i));
            }
            Console.ReadKey();
        }


        public static void SentMessage(string From, int i, string ip = "127.0.0.1")
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            string messageText = "Say hello " + i + "  times.";
            //messageText = Console.ReadLine();
            Message message = new Message() { Text = messageText, NicknameFrom = From, NicknameTo = "Server", DateTime = DateTime.Now };
            string json = message.SerializeMessageToJson();
            var data = Encoding.UTF8.GetBytes(json);
            // byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, iPEndPoint);
            Console.WriteLine($"The message {message.Text} has been sent successfuly!");
            byte[] buffer = udpClient.Receive(ref iPEndPoint);
            var serverAnswer = Encoding.UTF8.GetString(buffer);
            Console.WriteLine($"The server answer {serverAnswer} has been recieved successfuly!");
            /*if (messageText.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("The chat session will be closed.");
                return;
            }
            Console.WriteLine($"Client has sent the message with lenght {messageSize} to the Server! Waiting for the Server feedback.");
            //Console.WriteLine($"The message {message.Text} has been sent successfuly!");
            //ServerFeedbackToClient(messageSize);*/
        }

    }

    /*public static void ServerFeedbackToClient(int msgSize)
    {
        UdpClient udpClient = new UdpClient(1234);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        Console.WriteLine("Клиент ждет обратную связь от сервера.");
        while (true)
        {
            byte[] buffer = udpClient.Receive(ref iPEndPoint);
            if (buffer == null) break;
            var messageText = Encoding.UTF8.GetString(buffer);
            int messageSize = int.Parse(JsonSerializer.Deserialize<String>(messageText));
            if (msgSize == messageSize)
            {
                Console.WriteLine("The message has been recieved by Server successfully!");
                return;
            }
        }
    }*/
}
