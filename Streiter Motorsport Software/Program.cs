using System.Threading.Channels;

namespace Streiter_Motorsport_Software
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Willkommen in der Streiter Motorsport Endurance Software!");
            Console.WriteLine("Sie können jederzeit mit 0 oder \"exit\" zum vorherigen Menü zurückkehren.");
            MainLoop();

        }
        public static void MainLoop()
        {
            Console.WriteLine("Bitte geben Sie Ihren Nutzernamen an: ");
            string username = GetUserInput.GetUserInputStr();

            Console.WriteLine("Passwort eingeben: ");
            string password = GetUserInput.GetPasswordInput(); //versteckt die Eingabe
            UserManager userManager = new UserManager();
            User? user = userManager.Authenticate(username, password);
            if (user != null)
            {
                Console.WriteLine($"Willkommen, {user.Username}!");

                while (true)
                {

                    if (user.AccessLevel == 0)
                    {
                        Console.WriteLine("1. Verwaltungsmenü");
                        Console.WriteLine("2. Adminmenü");
                        Console.WriteLine("0. Beenden");
                        switch
                            (GetUserInput.GetUserInputInt())
                        {
                            case 1:
                                ShowVerwaltungsMenu();
                                break;
                            case 2:
                                ShowAdminMenu();
                                break;
                            case 0:
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Ungültige Auswahl.");
                                break;
                        }
                    }
                    else if (user.AccessLevel == 1)
                    {
                        ShowVerwaltungsMenu();
                    }
                    else if (user.AccessLevel == 2)
                    {
                        Console.WriteLine("Sie haben eingeschränkte Benutzerrechte.");
                        Console.WriteLine("Ihr Konto ist im momentanen Entwicklungszustand nutzlos.");
                    }

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
            Console.WriteLine("0. Abmelden");
        }

        public static void ShowVerwaltungsMenu()
        {
            Console.WriteLine("Verwaltungsmenü:");
            Console.WriteLine("1. Alle Events anzeigen");
            Console.WriteLine("0. Abmelden");
        }

        public void VerwaltungsMenu(int input)
        {

            RaceEventManager raceEventManager = new RaceEventManager();
            switch (input)
            {
                case 1:

                    int id = 0;
                    foreach (var raceEvent in raceEventManager.raceEvents)
                    {
                        id++;
                        Console.WriteLine($"ID: {id}, Event: {raceEvent.EventName}, Simulation: {raceEvent.Simulation}, Datum: {raceEvent.EventDate}, Dauer: {raceEvent.Duration}");
                    }
                    int choice = GetUserInput.GetUserInputInt();

                    break;
                case 0:
                    MainLoop();
                    break;
            }
        }
        public void AdminMenu(int input)
        {
            switch (input)
            {
                case 1:
                    UserManager userManager = new UserManager();
                    // Benutzerverwaltungsfunktionen hier implementieren
                    break;
                case 2:
                    // Teammitgliedverwaltungsfunktionen hier implementieren
                    break;
                case 3:
                    Console.WriteLine("Simulation auswählen:");
                    Console.WriteLine("1. Assetto Corsa Competizione");
                    Console.WriteLine("2. iRacing");
                    Console.WriteLine("3. Le Mans Ultimate");
                    
                    // Eventverwaltungsfunktionen hier implementieren
                    break;
                case 0:
                    MainLoop();
                    break;
            }
        }
    }
}
