using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

namespace TcpComm
{
    public static class Ultility
    {
        public static byte FromHexToByte(string hex)
        {
            //if (hex.Contains('-'))
            //    hex = hex.Replace("-", "");
            //hex = hex.Replace(" ", "");

            hex = RefineHexText(hex);
            hex = Regex.Replace(hex, @"\s+", "");

            byte raw;// = new byte[hex.Length / 2];

            raw = Convert.ToByte(hex, 16);

            return raw;
        }
        public static string ByteToHex(byte bInt)
        {
            return BitConverter.ToString(new byte[] { bInt });
        }
        public static byte[] FromHex(string hex)
        {
            //if (hex.Contains('-'))
            //    hex = hex.Replace("-", "");
            //hex = hex.Replace(" ", "");

            hex = RefineHexText(hex);
            hex = Regex.Replace(hex, @"\s+", "");

            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        public static string ToHexText(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", " ");
        }

        const string HexPattern = @"([0-9a-fA-F]{2}\s*)+";
        public static bool IsHexString(string s)
        {
            return Regex.IsMatch(s, HexPattern);
        }

        public static string RefineHexText(string s)
        {
            s = Regex.Replace(s, @"[^0-9a-fA-F\s]+", " ");
            s = Regex.Replace(s, @"[\r\n]", " ");
            s = Regex.Replace(s, @"\s{2,}", " ");
            return s;
        }

        public static string GetAsciiString(byte[] data)
        {
            var ascii = System.Text.Encoding.ASCII.GetString(data);
            ascii = Regex.Replace(ascii, "\n", ".");
            return ascii;
        }

        public static IPAddress GetMyIP()
        {
            IPHostEntry host;
            IPAddress localIP = IPAddress.Loopback;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip;
                }
            }
            return localIP;
        }

        public static bool IsConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) 
            { 
                return false; 
            }
        }
        public static string StringToHex(string str)
        {
            //string input = "Hello World!";
            //char[] values = str.ToCharArray();
            //foreach (char letter in values)
            //{
            //    // Get the integral value of the character. 
            //    int value = Convert.ToInt32(letter);
            //    // Convert the decimal value to a hexadecimal value in string form. 
            //    string hexOutput = String.Format("{0:X}", value);
            //    //Console.WriteLine("Hexadecimal value of {0} is {1}", letter, hexOutput);
            //    String.Join(
            //}
            return BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes(str)).Replace("-", " ");
        }
    }
}
