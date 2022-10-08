using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using KnjiznicarInstanceServer.Message;
using KnjiznicarInstanceServer.MessageHandlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace KnjiznicarInstanceServer
{
    class Client
    {
        public static int DataBufferSize = 4096;
        public int Id;
        public string Username;
        public TCP Tcp;

        public Client(int id)
        {
            Id = id;
            Tcp = new TCP(id);
        }

        public class TCP
        {
            private readonly int _id;
            private NetworkStream _stream;
            private byte[] _recieveBuffer;

            public TcpClient Socket;

            public TCP(int id)
            {
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
                message.welcomeMessage = "Welcome to the server!";
                ServerSend.SendTCPData(_id, message);
            }

            private void RecieveDataCallback(IAsyncResult ar)
            {
                try
                {
                    int byteLength = _stream.EndRead(ar);
                    if(byteLength <= 0)
                    {
                        //Dissconnect();
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(_recieveBuffer, data, byteLength);

                    MemoryStream ms = new MemoryStream(data);
                    JObject dataJsonObject;
                    using (BsonReader reader = new BsonReader(ms))
                    {
                        dataJsonObject = (JObject)JToken.ReadFrom(reader);
                        MessageHandler.Instance.HandleMessage(dataJsonObject);
                    }

                    _stream.BeginRead(_recieveBuffer, 0, DataBufferSize, RecieveDataCallback, null);
                }
                catch(Exception ex)
                {
                    //Dissconnect();
                }
            }

            public void SendData(byte[] sendData)
            {
                try
                {
                    if(Socket != null)
                    {
                        _stream.BeginWrite(sendData,0, sendData.Length, null, null);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error whiel sending data to player {_id} via TCP: {ex}");
                }
            }
        }
    }
}
