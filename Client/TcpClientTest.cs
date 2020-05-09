using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TcpClientTest {
        
    private const int  portNum = 20016 ;

    static public void Main()
    {

        TcpClient tcpClient = new TcpClient();
        try
        {
            tcpClient.Connect("25.150.152.13", portNum);
            NetworkStream networkStream = tcpClient.GetStream();

            if (networkStream.CanWrite && networkStream.CanRead)
            {

                String DataToSend = "";
                new Thread(() =>
                {
                    while (DataToSend != "quit")
                    {

                        Console.WriteLine("\nType a text to be sent:");
                        DataToSend = Console.ReadLine();
                        if (DataToSend.Length == 0) break;

                        Byte[] sendBytes = Encoding.ASCII.GetBytes(DataToSend);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);

                        // Reads the NetworkStream into a byte buffer.
                        //byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                        //int BytesRead = networkStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);


                        // Returns the data received from the host to the console.
                        //string returndata = Encoding.ASCII.GetString(bytes, 0, BytesRead);
                        //Console.WriteLine("This is what the host returned to you: \r\n{0}", returndata);
                    }
                    networkStream.Close();
                    tcpClient.Close();
                }).Start();

                new Thread(() =>
                {
                    while (true)
                    {
                        byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                        int BytesRead = networkStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);
                        if (bytes.Length > 0)
                        {
                            string returndata = Encoding.ASCII.GetString(bytes, 0, BytesRead);
                            Console.WriteLine(returndata);
                        }
                    }
                }).Start();
            }
            else if (!networkStream.CanRead)
            {
                Console.WriteLine("You can not write data to this stream");
                tcpClient.Close();
            }
            else if (!networkStream.CanWrite)
            {
                Console.WriteLine("You can not read data from this stream");
                tcpClient.Close();
            }
        }
        catch (SocketException)
        {
            Console.WriteLine("Sever not available!");
        }
        catch (System.IO.IOException)
        {
            Console.WriteLine("Sever not available!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }// Main() 
} // class TcpClientTest {
