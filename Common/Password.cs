using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace FA_Accounting.Common
{
    public static class Password
    {
        public static string filepath = "password.txt";
        public static string passwordstr = "";  // command password
        public static string mainpasswordstr = ""; // login password
        public static string secret = "Goodman99";
        // make sure getpassword is called first when this class is used.
        public static void getpassword()
        {
            StreamReader sr = new StreamReader(filepath);
            mainpasswordstr = sr.ReadLine();
            passwordstr = sr.ReadLine();

            sr.Close();
        }
        /// <summary>
        /// Validate command password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
 
        public static bool ValidatePassword(string password)
        {
            if (String.IsNullOrEmpty(passwordstr))
            {
                getpassword();
            }
            if (Encrypt.EncryptString(password, secret) == passwordstr)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// validate log in password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
 
        public static bool ValidateMainPassword(string password)
        {
            if (Encrypt.EncryptString(password, secret) == mainpasswordstr)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void SaveNewCommandPassword(string newpasswordstr)
        {
            string newencryptedpass = Encrypt.EncryptString(newpasswordstr, secret);
            StreamWriter sw = new StreamWriter(filepath);
            sw.WriteLine(mainpasswordstr);
            sw.WriteLine(newencryptedpass);
            sw.Close();
        }
        public static void SaveNewMainPassword(string newpasswordstr)
        {
            string newencryptedpass = Encrypt.EncryptString(newpasswordstr, secret);
            StreamWriter sw = new StreamWriter(filepath);
            sw.WriteLine(newencryptedpass);
            sw.WriteLine(passwordstr);
            sw.Close();
        }
    }
}
