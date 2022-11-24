using KnjiznicarDataModel.Message;
using KnjiznicarLoginServer.MessageHandlers;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Sockets;

namespace KnjiznicarLoginServer
{
    public class ServerTCP
    {
        private NetworkStream _stream;
        private byte[] _recieveBuffer;

        public TcpClient Socket;
        public static int DataBufferSize = 4096;

        private Action _reconnect;

        public ServerTCP(int dataBufferSize, Action reconnect)
        {
            DataBufferSize = dataBufferSize;
            _reconnect = reconnect;
        }

        public void Connect(TcpClient socket)
        {
            Socket = socket;
            socket.ReceiveBufferSize = DataBufferSize;
            socket.SendBufferSize = DataBufferSize;

            _stream = socket.GetStream();
            _recieveBuffer = new byte[DataBufferSize];

            _stream.BeginRead(_recieveBuffer, 0, DataBufferSize, RecieveDataCallback, null);
        }

        private void RecieveDataCallback(IAsyncResult ar)
        {
            try
            {
                int byteLength = _stream.EndRead(ar);
                if (byteLength <= 0)
                {
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(_recieveBuffer, data, byteLength);

                _stream.BeginRead(_recieveBuffer, 0, DataBufferSize, RecieveDataCallback, null);
                MemoryStream ms = new MemoryStream(data);
                JObject dataJsonObject;
                using (BsonReader reader = new BsonReader(ms))
                {
                    dataJsonObject = (JObject)JToken.ReadFrom(reader);
                    MessageHandlers.MessageHandlers.HandleMessage("", dataJsonObject, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recieving data from server: {ex}");
                Reconnect();
            }
        }

        public void SendData(byte[] sendData)
        {
            try
            {
                if (Socket != null)
                {
                    _stream.BeginWrite(sendData, 0, sendData.Length, null, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending data to server: {ex}");
                Reconnect();
            }
        }

        public void Reconnect()
        {
            System.Diagnostics.Process.Start("D:\\UnityProjects\\Knjiznicar\\KnjiznicarLoginServer\\KnjiznicarLoginServer\\bin\\Debug\\net5.0\\KnjiznicarLoginServer.exe");
            Environment.Exit(0);
        }
    }
}
