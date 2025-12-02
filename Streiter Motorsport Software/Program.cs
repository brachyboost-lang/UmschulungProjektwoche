using System.Threading.Channels;

namespace Streiter_Motorsport_Software
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Willkommen in der Streiter Motorsport Endurance Software!");
            MainLoop();

        }
        public static void MainLoop()
        {
            Console.WriteLine("Bitte geben Sie Ihren Nutzernamen an: ");
            string username = Console.ReadLine();
            
            Console.WriteLine("Passwort eingeben: ");
            string password = Console.ReadLine();
            UserManager userManager = new UserManager();
            User? user = userManager.Authenticate(username, password);
            if (user != null)
            {
                Console.WriteLine($"Willkommen, {user.Username}!");
                
                if (user.AccessLevel == 0)
                {
                    ShowAdminMenu();
                }
                else if (user.AccessLevel == 1)
                {
                    Console.WriteLine("Sie haben Verwaltungsrechte.");
                }
                else if (user.AccessLevel == 2)
                {
                    Console.WriteLine("Sie haben eingeschränkte Benutzerrechte.");
                }

            }
            else
            {
                Console.WriteLine("Authentifizierung fehlgeschlagen. Bitte überprüfen Sie Ihren Nutzernamen und Ihr Passwort.");
            }
        }

        public static void ShowAdminMenu()
        {
            Console.WriteLine("Admin Menü:");
            Console.WriteLine("1. Software Benutzer verwalten");
            Console.WriteLine("2. Teammitglieder verwalten");
            Console.WriteLine("3. Events verwalten");
            Console.WriteLine("4. Systemeinstellungen");
            Console.WriteLine("5. Abmelden");
        }
    }

    
}
