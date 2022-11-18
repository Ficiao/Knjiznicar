using KnjiznicarLoginServer.Message;
using KnjiznicarLoginServer.MessageHandlers;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Sockets;

namespace KnjiznicarLoginServer
{
    public class TCP
    {
        private readonly int _id;
        private NetworkStream _stream;
        private byte[] _recieveBuffer;
        private Client _client = null;

        public TcpClient Socket;
        public static int DataBufferSize = 4096;

        public TCP(int id, int dataBufferSize, Client client)
        {
            DataBufferSize = dataBufferSize;
            _client = client;
            _id = id;
        }

        public void Connect(TcpClient socket)
        {
            Socket = socket;
            socket.ReceiveBufferSize = DataBufferSize;
            socket.SendBufferSize = DataBufferSize;

            _stream = socket.GetStream();
            _recieveBuffer = new byte[DataBufferSize];

            _stream.BeginRead(_recieveBuffer, 0, DataBufferSize, RecieveDataCallback, null);
            ConnectedToServerMessage message = new ConnectedToServerMessage();
            message.welcomeMessage = "Welcome to the login server!";
            ServerSend.SendTCPData(_id, message);
        }

        private void RecieveDataCallback(IAsyncResult ar)
        {
            try
            {
                if(_stream == null)
                {
                    _client.Disconnect();
                    return;
                }
                int byteLength = _stream.EndRead(ar);
                if (byteLength <= 0)
                {
                    _client.Disconnect();
                    Console.WriteLine($"Error while recieving data from player {_id} via TCP: byte length was 0.");
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
                    MessageHandler.HandleMessage(_id, dataJsonObject);
                }
            }
            catch (Exception ex)
            {
                _client.Disconnect();
                Console.WriteLine($"Error while recieving data from player {_id} via TCP: {ex}");
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
                Console.WriteLine($"Error while sending data to player {_id} via TCP: {ex}");
                _client.Disconnect();
            }
        }

        public void Disconnect()
        {
            Socket?.Close();
            _stream = null;
            _recieveBuffer = null;
            Socket = null;
        }
    }
}
