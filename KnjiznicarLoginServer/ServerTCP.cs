using KnjiznicarLoginServer.Message;
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

        public ServerTCP(int dataBufferSize)
        {
            DataBufferSize = dataBufferSize;
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
                if (_stream == null)
                {
                    return;
                }
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
                    MessageHandler.HandleMessage(-1, dataJsonObject);
                }
            }
            catch (Exception ex)
            {
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
