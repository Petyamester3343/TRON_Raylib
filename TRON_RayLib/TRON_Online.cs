using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Numerics;

namespace TRON_RayLib
{
    internal class TRON_Online
    {
        TcpListener? server;
        TcpClient client;
        NetworkStream stream;

        public void StartServer()
        {
            try
            {
                server = new(IPAddress.Any, 5000);
                server.Start();
                MessageBox.Show("Server started, waiting for connection...");
                client = server.AcceptTcpClient();
                stream = client.GetStream();
                MessageBox.Show("Client has successfully connected!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Disconnect();
            }
        }

        public void Connect(string ip)
        {
            try
            {
                client = new TcpClient(ip, 5000);
                stream = client.GetStream();
                MessageBox.Show($"Connecting {ip}...");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}");
                Disconnect();

            }
        }

        public void SendData(Vector2 pos)
        {
            try
            {
                string data = $"{pos.X},{pos.Y}";
                byte[] toSend = Encoding.ASCII.GetBytes(data);
                stream.Write(toSend, 0, toSend.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending data: {ex.Message}");
                Disconnect();
            }

        }

        public Vector2 RecieveData()
        {
            try
            {
                byte[] toRead = new byte[1024];
                int bytesRead = stream.Read(toRead, 0, toRead.Length);
                string recievedData = Encoding.ASCII.GetString(toRead, 0, bytesRead);

                // parse into Vector2
                string[] posArray = recievedData.Split(',');
                return new Vector2(float.Parse(posArray[0]), float.Parse(posArray[1]));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error recieving data: {ex.Message}");
                Disconnect();
                return Vector2.Zero;
            }
        }

        public void StopServer()
        {
            if (server != null)
            {
                server.Stop();
                MessageBox.Show("Server stopped!");
                MainMenuForm obj = new();
                obj.Show();
            }
        }

        public void Disconnect()
        {
            stream.Close();
            client.Close();
            MessageBox.Show("Disconnected from server!");
            MainMenuForm obj = new();
            obj.Show();
        }
    }
}
