using System.Threading.Channels;

namespace Streiter_Motorsport_Software
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EventMember aLang = new("alexander lang");
            EventMember eAkay = new("edo akay");
            EventMember jMayer = new("johannes mayer");
            EventMember mSwatko = new("meik swatko");
            EventMember mBauduihn = new("marco bauduihn");
            EventMember aScheidel = new("alex scheidel");
            EventMember bJung = new("basti jung");
            EventMember vMissler = new("vincent missler");
            EventMember bKaragoez = new("bedran karagöz");
            EventMember bWolf = new("ben wolf");
            EventMember bWagner = new("benjamin wagner");
            EventMember cBalter = new("christian balter");
            EventMember cLehnert = new("christian lehnert");
            EventMember dStraube = new("danny straube");
            EventMember dMilesan = new("darius milesan");
            EventMember dMioska = new("david mioska");
            EventMember dWegel = new("dustin wegel");
            EventMember eBaumgartner = new("elia baumgartner");
            EventMember eKoenig = new("elijas könig");
            EventMember eDalgic = new("erkan dalgic");
            EventMember fWagner = new("fabian-maurice wagner");
            EventMember hRenk = new("heiko renk");
            EventMember jSchulze = new("jeremy schulze");
            EventMember lDeymann = new("leon deymann");
            EventMember lBlaschke = new("luca blaschke");
            EventMember lTrapani = new("luca trapani");
            EventMember lBorchmann = new("lucas borchmann");
            EventMember mNaujok = new("maikel naujok");
            EventMember mLaufenburg = new("marc von laufenburg");
            EventMember mDaiberl = new("matthäus daiberl");
            EventMember mDobbelstein = new("max dobbelstein");
            EventMember nDenzel = new("niklas denzel");
            EventMember nBunsh = new("nils bunsh");
            EventMember nKadur = new("noah kadur");
            EventMember pHans = new("paul hans");
            EventMember pSolinke = new("paul solinke");
            EventMember pSchneider = new("phillip schneider");
            EventMember rXanatos = new("ray xanatos");
            EventMember roman = new("roman");
            EventMember sStansen = new("sven stansen");
            EventMember tBock = new("tobias bock");
            EventMember tWillgalis = new("tobias willgalis");
            EventMember tKempin = new("tom kempin");
            EventMember tHam = new("totti ham");
            EventMember cWillhalm = new("christoph willhalm");
            EventMember dBartodziej = new("dennis bartodziej");
            EventMember fBissani = new("frederic bissani");
            EventMember sBidian = new("s. Bidian");
            EventMember aSpang = new("aaron spang");
            EventMember aStedler = new("aaron stedler");
            EventMember aHeinz = new("anton heinz");
            EventMember aMihulka = new("arkadius mihulka");
            EventMember bMiksa = new("benjamin miska");
            EventMember cSchulz = new("christian schulz");
            EventMember cDimcescu = new("cristian dimcescu");
            EventMember dVisciglia = new("dario visciglia");
            EventMember dIsgro = new("devin isgro");
            EventMember eErdei = new("erhard erdei");
            EventMember gEggerl = new("german eggerl");
            EventMember gScheelen = new("gerrit scheelen");
            EventMember hWolters = new("h. wolters");
            EventMember hSüßmann = new("holger süßmann");
            EventMember jSlow = new("j. slow");
            EventMember jKampke = new("jan kampke");
            EventMember jPaul = new("jannik paul");
            EventMember jStevens = new("jason stevens");
            EventMember kGorelik = new("k. Gorelik");
            EventMember lSnowman = new("l. snowman");
            EventMember lJaumann = new("leon jaumann");
            EventMember lErsing = new("leopold ersing");
            EventMember lTrost = new("lisa trost");
            EventMember lWagner = new("lorenz wagner");
            EventMember lKern = new("lukas kern");
            EventMember mAydin = new("mesut aydin");
            EventMember mVersteppen = new("max versteppen");
            EventMember mAllemann = new("marc allemann");
            EventMember mSommerlad = new("marcel sommerlad");
            EventMember mKoch = new("mathias koch");
            EventMember mFunck = new("mike funck");
            EventMember nMei = new("n. mei");
            EventMember nRethfeldt = new("nico rethfeldt");
            EventMember nGoettert = new("nils göttert");
            EventMember oPlavac = new("oliver plavac");
            EventMember pHoffmann = new("pascal hoffmann");
            EventMember pSchwob = new("phillip schwob");
            EventMember sSchroedel = new("simon schrödel");
            EventMember sDelalut = new("stefan delalut");
            EventMember tMeier = new("thomas meier");
            EventMember tWolff = new("thomas wolff");
            EventMember tKeirsbulck = new("timo keirsbulck");



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
            switch (input)
            {
                case 1:
                    UserManager userManager = new UserManager();
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
                                Console.WriteLine($"Administrator Benutzer '{newAdminUsername}' wurde erfolgreich erstellt.");
                            }
                            else if (choice1 == 2)
                            {
                                Console.WriteLine("Neuen Teamverwaltungs Benutzernamen eingeben: ");
                                string newTeamManagerUsername = GetUserInput.GetUserInputStr();
                                Console.WriteLine("Neues Teamverwaltungs Passwort eingeben: ");
                                string newTeamManagerPassword = GetUserInput.GetPasswordInput();
                                User newTeamManagerUser = new User(newTeamManagerUsername, newTeamManagerPassword, 1);
                                userManager.AddUser(newTeamManagerUser);
                                Console.WriteLine($"Teamverwaltungs Benutzer '{newTeamManagerUsername}' wurde erfolgreich erstellt.");
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
                                Console.WriteLine($"Benutzer '{removeUsername}' wurde erfolgreich entfernt.");
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
