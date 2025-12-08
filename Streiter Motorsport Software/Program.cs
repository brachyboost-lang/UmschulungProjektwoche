using System;

namespace Streiter_Motorsport_Software
{
    internal class Program
    {
        // Application-wide UserManager (set at startup by loader)
        internal static UserManager AppUserManager { get; private set; } = new UserManager();

        static void Main(string[] args)
        {
            // Load persisted data (vehicles, classes, users, members, events)
            try
            {
                var loaded = JsonPersistence.LoadAll();
                if (loaded != null)
                {
                    AppUserManager = loaded;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Daten: {ex.Message}");
                AppUserManager = new UserManager();
            }

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
            UserManager userManager = AppUserManager;
            User? user = userManager.Authenticate(username, password);
            if (user != null)
            {
                Console.WriteLine($"Willkommen, {user.Username}!");

                while (true)
                {
                    Program program = new Program();

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
                                program.VerwaltungsMenu(GetUserInput.GetUserInputInt());
                                break;
                            case 2:
                                ShowAdminMenu();
                                program.AdminMenu(GetUserInput.GetUserInputInt());
                                break;
                            case 0:
                                Console.WriteLine("Möchten Sie alle Daten speichern bevor Sie beenden? Y/N");
                                char confirm = GetUserInput.GetUserInputChar();
                                if (confirm == 'y')
                                {
                                    JsonPersistence.SaveAll(AppUserManager);
                                    Console.WriteLine("Daten gespeichert.");
                                }
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
                        Console.WriteLine("In Zukunft soll sich dieses Konto an Events selber anmelden können.");
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
            Console.WriteLine("4. Daten speichern");
            Console.WriteLine("0. Abmelden");
        }

        public static void ShowVerwaltungsMenu()
        {
            Console.WriteLine("Verwaltungsmenü:");

            // später hinzufügen: automatisches anzeigen von nächsten Events

            Console.WriteLine("1. Alle Events anzeigen");
            Console.WriteLine("0. Abmelden");
        }

        public void VerwaltungsMenu(int input)
        {
            switch (input)
            {
                case 1:
                    int id = 0;
                    foreach (var ev in EventManager.Events)
                    {
                        id++;
                        Console.WriteLine($"ID: {id}, Event: {ev.Name}, Simulation: {ev.Simulation}, Dauer: {ev.Dauer}");
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
            while (true)
            {
                switch (input)
                {
                    case 1:
                        UserManager userManager = AppUserManager;
                        Console.WriteLine("Software Benutzer verwalten");
                        Console.WriteLine("---------------------------");
                        Console.WriteLine("1. Benutzer anlegen");
                        Console.WriteLine("2. Benutzer entfernen");
                        Console.WriteLine("0. Zurück");
                        int choice = GetUserInput.GetUserInputInt();
                        if (choice == 0)
                        {
                            break;
                        }
                        if (choice == 1)
                        {
                            while (true)
                            {
                                Console.WriteLine("Rolle zuweisen: ");
                                Console.WriteLine("1. Administrator");
                                Console.WriteLine("2. Teamverwaltung");
                                Console.WriteLine("3. Mitgliedskonto (WiP)");
                                int choice1 = GetUserInput.GetUserInputInt();

                                if (choice1 == 1)
                                {
                                    Console.WriteLine("Neuen Administrator Benutzernamen eingeben: ");
                                    string newAdminUsername = GetUserInput.GetUserInputStr();
                                    Console.WriteLine("Neues Administrator Passwort eingeben: ");
                                    string newAdminPassword = GetUserInput.GetPasswordInput();
                                    User newAdminUser = new User(newAdminUsername, newAdminPassword, 0);
                                    userManager.AddUser(newAdminUser);
                                    JsonPersistence.SaveAll(AppUserManager);
                                    Console.WriteLine($"Administrator Benutzer '{newAdminUsername}' wurde erfolgreich erstellt und gespeichert.");
                                }
                                else if (choice1 == 2)
                                {
                                    Console.WriteLine("Neuen Teamverwaltungs Benutzernamen eingeben: ");
                                    string newTeamManagerUsername = GetUserInput.GetUserInputStr();
                                    Console.WriteLine("Neues Teamverwaltungs Passwort eingeben: ");
                                    string newTeamManagerPassword = GetUserInput.GetPasswordInput();
                                    User newTeamManagerUser = new User(newTeamManagerUsername, newTeamManagerPassword, 1);
                                    userManager.AddUser(newTeamManagerUser);
                                    JsonPersistence.SaveAll(AppUserManager);
                                    Console.WriteLine($"Teamverwaltungs Benutzer '{newTeamManagerUsername}' wurde erfolgreich erstellt und gespeichert.");
                                }
                                else if (choice1 == 3)
                                {
                                    Console.WriteLine("Mitgliedskonto Funktion ist noch in Arbeit.");
                                }
                                else if (choice1 == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Ungültige Auswahl.");

                                }
                            }
                            break;
                        }

                        else if (choice == 2)
                        {
                            while (true)
                            {
                                Console.WriteLine("Zu entfernenden Benutzernamen eingeben: ");
                                string removeUsername = GetUserInput.GetUserInputStr();
                                bool removed = userManager.RemoveUser(removeUsername);
                                if (removed)
                                {
                                    JsonPersistence.SaveAll(AppUserManager);
                                    Console.WriteLine($"Benutzer '{removeUsername}' wurde erfolgreich entfernt und Daten gespeichert.");
                                }
                                else
                                {
                                    Console.WriteLine($"Benutzer '{removeUsername}' wurde nicht gefunden.");
                                }
                                break;
                            }
                        }
                        break;
                    case 2:
                        while (true)
                        {
                            Console.WriteLine("Teammitglieder verwalten\n----------------");
                            Console.WriteLine("1. Mitglied hinzufügen");
                            Console.WriteLine("2. Mitglied entfernen");
                            Console.WriteLine("0. Zurück");
                            int wahl = GetUserInput.GetUserInputInt();
                            if (wahl == 0)
                            {
                                break;
                            }
                            if (wahl == 1)
                            {
                                Console.WriteLine("Neues Mitglied erstellen\n--------------");
                                Console.WriteLine("Vornamen und Nachnamen eingeben: (z.B. \"Max Mustermann\"");
                                string newMember = GetUserInput.GetUserInputStr();
                                EventMember newMember1 = new EventMember(newMember);

                            }
                            if (wahl == 2)
                            {

                            }
                            break;
                        }
                        break;
                    case 3:
                        Console.WriteLine("Event Verwaltung\n1. Event erstellen\n2. Event löschen\n0. Zurück");
                        int eventchoice = GetUserInput.GetUserInputInt();
                        if (eventchoice == 1)
                        {
                            Console.WriteLine("Name des Events eingeben: ");
                            string eventName = GetUserInput.GetUserInputStr();
                            Event newEvent = new Event(eventName);
                            newEvent.CreateEvent(eventName);
                            EventManager.AddEvent(newEvent);
                            JsonPersistence.SaveAll(AppUserManager);
                            Console.WriteLine("Event erstellt und gespeichert.");
                        }
                        if (eventchoice == 2)
                        {
                            Console.WriteLine("Welches Event soll entfernt werden?");
                            for (int i = 0; i < EventManager.Events.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {EventManager.Events[i].Name}");
                            }
                            int eventnumber = GetUserInput.GetUserInputInt();
                            if (eventnumber > 0 && eventnumber <= EventManager.Events.Count)
                            {
                                EventManager.Events.RemoveAt(eventnumber - 1);
                                JsonPersistence.SaveAll(AppUserManager);
                                Console.WriteLine("Event entfernt und Daten gespeichert.");
                            }
                        }
                        break;
                    case 4:
                        Console.WriteLine("Möchten Sie alle persistierbaren Daten jetzt speichern? Y/N");
                        char saveConfirm = GetUserInput.GetUserInputChar();
                        if (saveConfirm == 'y')
                        {
                            JsonPersistence.SaveAll(AppUserManager);
                            Console.WriteLine("Daten wurden gespeichert.");
                        }
                        else
                        {
                            Console.WriteLine("Speichern abgebrochen.");
                        }
                        break;
                    case 0:
                        return;
                }
            }
        }
    }
}