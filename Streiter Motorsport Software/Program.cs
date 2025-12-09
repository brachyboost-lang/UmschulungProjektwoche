using System;
using System.Collections.Generic;

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
                        switch (GetUserInput.GetUserInputInt())
                        {
                            case 1:
                                ShowVerwaltungsMenu();
                                program.VerwaltungsMenu(GetUserInput.GetUserInputInt());
                                break;
                            case 2:
                                // Show admin menu and let AdminMenu handle its own input loop
                                ShowAdminMenu();
                                program.AdminMenu();
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
                        program.VerwaltungsMenu(GetUserInput.GetUserInputInt());
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

            // später hinzufügen: automatisches anzeigen von nächsten Events

            Console.WriteLine("1. Alle Events anzeigen");
            Console.WriteLine("0. Abmelden");
        }


        // AdminMenu jetzt ohne Eingabe-Parameter; liest innerhalb der Schleife neu ein.
        public void AdminMenu()
        {
            while (true)
            {
                ShowAdminMenu();
                int input = GetUserInput.GetUserInputInt();

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
                                if (newMember == "0" || newMember.ToLower() == "exit")
                                {
                                    break;
                                }
                                if (newMember.Trim() == "")
                                {
                                    Console.WriteLine("Ungültiger Name. Bitte versuchen Sie es erneut.");
                                    continue;
                                }
                                try
                                {

                                    EventMember newMember1 = new EventMember(newMember);
                                    JsonPersistence.SaveAll(AppUserManager);
                                    Console.WriteLine($"Neues Mitglied: {EventMember.Mitgliederliste.Last()} wurde erstellt.");
                                    continue;
                                }
                                catch (Exception alreadyExists)
                                {
                                    foreach (var member in EventMember.Mitgliederliste)
                                    {
                                        if (member.Name.Equals(newMember, StringComparison.OrdinalIgnoreCase)) // ignorieren der Groß-/Kleinschreibung zum Vergleich
                                        {
                                            throw new Exception("Ein Mitglied mit diesem Namen existiert bereits.");
                                        }
                                    }
                                }
                            }
                            if (wahl == 2)
                            {
                                Console.WriteLine("Welches Mitglied soll entfernt werden?");
                                for (int i = 0; i < EventMember.Mitgliederliste.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {EventMember.Mitgliederliste[i].Name}");
                                }
                                int membernumber = GetUserInput.GetUserInputInt();
                                if (membernumber > 0 && membernumber <= EventMember.Mitgliederliste.Count)
                                {
                                    EventMember.Mitgliederliste.RemoveAt(membernumber - 1);
                                    JsonPersistence.SaveAll(AppUserManager);
                                    Console.WriteLine("Mitglied entfernt und Daten gespeichert.");
                                }
                            }
                            break;
                        }
                        break;
                    case 3:
                        Console.WriteLine("Event Verwaltung\n1. Event erstellen\n2. Event löschen\n0. Zurück");
                        int eventchoice = GetUserInput.GetUserInputInt();
                        if (eventchoice == 0)
                        {
                            break;
                        }
                        if (eventchoice == 1)
                        {
                            Console.WriteLine("Name des Events eingeben (0 zum Abbrechen): ");
                            string eventName = GetUserInput.GetUserInputStr();
                            if (eventName == "0" || eventName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Event-Erstellung abgebrochen.");
                                break;
                            }

                            // CreateEvent kann jetzt null zurückgeben, wenn der Benutzer abbricht.
                            var created = new Event(eventName).CreateEvent(eventName);
                            if (created == null)
                            {
                                Console.WriteLine("Event-Erstellung abgebrochen.");
                                break;
                            }

                            EventManager.AddEvent(created);
                            JsonPersistence.SaveAll(AppUserManager);
                            Console.WriteLine("Event erstellt und gespeichert.");

                            // Return to caller so user isn't "stuck" in admin menu after creating an event
                            Console.WriteLine("Drücken Sie eine Taste, um zum vorherigen Menü zurückzukehren.");
                            Console.ReadKey();
                            return;
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
                    case 0:
                        // Zurück ins vorherige Menü
                        return;
                    default:
                        Console.WriteLine("Ungültige Auswahl.");
                        break;
                }
            }
        }

        public void VerwaltungsMenu(int input)
        {
            switch (input)
            {
                case 1:
                    if (EventManager.Events.Count == 0)
                    {
                        Console.WriteLine("Keine Events vorhanden.");
                        break;
                    }

                    // Liste aller Events anzeigen
                    for (int i = 0; i < EventManager.Events.Count; i++)
                    {
                        var ev = EventManager.Events[i];
                        Console.WriteLine($"ID: {i + 1}, Event: {ev.Name}, Simulation: {ev.Simulation}, Dauer: {ev.Dauer}");
                    }

                    Console.WriteLine("Wählen Sie die ID des Events (0 zum Abbrechen):");
                    int eventChoice = GetUserInput.GetUserInputInt();
                    if (eventChoice == 0)
                    {
                        break;
                    }
                    if (eventChoice < 1 || eventChoice > EventManager.Events.Count)
                    {
                        Console.WriteLine("Ungültige Event-Auswahl.");
                        break;
                    }

                    var selectedEvent = EventManager.Events[eventChoice - 1];

                    // Interaktives Event-Menü
                    while (true)
                    {
                        Console.WriteLine("");
                        Console.WriteLine($"--- Event: {selectedEvent.Name} ---");
                        Console.WriteLine($"Simulation: {selectedEvent.Simulation}");
                        Console.WriteLine($"Dauer: {selectedEvent.Dauer} Minuten");
                        Console.WriteLine($"Datum: {selectedEvent.Datum.ToShortDateString()}");
                        Console.WriteLine($"Strecke: {selectedEvent.Strecke}");
                        Console.WriteLine("");
                        Console.WriteLine("Angemeldete Mitglieder:");
                        if (selectedEvent.AngemeldeteMitglieder.Count == 0)
                        {
                            Console.WriteLine(" (keine angemeldeten Mitglieder)");
                        }
                        else
                        {
                            for (int i = 0; i < selectedEvent.AngemeldeteMitglieder.Count; i++)
                            {
                                var m = selectedEvent.AngemeldeteMitglieder[i];
                                string carInfo = m.GewaehlteFahrzeuge.Count > 0 ? $" - {string.Join(", ", m.GewaehlteFahrzeuge.ConvertAll(v => v.Fahrzeugname))}" : "";
                                Console.WriteLine($"{i + 1}. {m.Name} (ID: {m.Id}){carInfo}");
                            }
                        }

                        Console.WriteLine("");
                        Console.WriteLine("1. Mitglied von der Mitgliederliste zum Event hinzufügen");
                        Console.WriteLine("2. Mitglied vom Event entfernen");
                        Console.WriteLine("0. Zurück zur vorherigen Ansicht");
                        int act = GetUserInput.GetUserInputInt();

                        if (act == 0)
                        {
                            break;
                        }
                        else if (act == 1)
                        {
                            if (EventMember.Mitgliederliste.Count == 0)
                            {
                                Console.WriteLine("Es sind keine Mitglieder in der Mitgliederliste vorhanden.");
                                continue;
                            }

                            bool continueAdding = true;
                            while (continueAdding)
                            {
                                Console.WriteLine("Mitgliederliste:");
                                for (int i = 0; i < EventMember.Mitgliederliste.Count; i++)
                                {
                                    var globalM = EventMember.Mitgliederliste[i];
                                    bool already = false;
                                    foreach (var am in selectedEvent.AngemeldeteMitglieder)
                                    {
                                        if (am.Id == globalM.Id) { already = true; break; }
                                    }
                                    Console.WriteLine($"{i + 1}. {globalM.Name} (ID: {globalM.Id}){(already ? " [bereits angemeldet]" : "")}");
                                }

                                Console.WriteLine("Wählen Sie die Nummer des hinzuzufügenden Mitglieds (0 zum Abbrechen):");
                                int memberChoice = GetUserInput.GetUserInputInt();
                                if (memberChoice == 0) break;
                                if (memberChoice < 1 || memberChoice > EventMember.Mitgliederliste.Count)
                                {
                                    Console.WriteLine("Ungültige Mitglieds-Auswahl.");
                                    continue;
                                }

                                var memberToAdd = EventMember.Mitgliederliste[memberChoice - 1];
                                bool exists = false;
                                foreach (var am in selectedEvent.AngemeldeteMitglieder)
                                {
                                    if (am.Id == memberToAdd.Id) { exists = true; break; }
                                }
                                if (exists)
                                {
                                    Console.WriteLine("Dieses Mitglied ist bereits für das Event angemeldet.");
                                }
                                else
                                {
                                    selectedEvent.AngemeldeteMitglieder.Add(memberToAdd);
                                    JsonPersistence.SaveAll(AppUserManager);
                                    Console.WriteLine($"Mitglied '{memberToAdd.Name}' wurde zum Event '{selectedEvent.Name}' hinzugefügt und Daten wurden gespeichert.");

                                    // Fahrzeugzuweisung anbieten basierend auf ausgewählten Fahrzeugklassen des Events
                                    Console.WriteLine("Möchten Sie diesem Mitglied jetzt Fahrzeuge zuweisen? Y/N");
                                    char assignCarAll = GetUserInput.GetUserInputChar();
                                    if (assignCarAll == 'y')
                                    {
                                        var availableCars = selectedEvent.ShowCars();
                                        if (availableCars.Count == 0)
                                        {
                                            Console.WriteLine("Es sind keine passenden Fahrzeuge verfügbar (keine Fahrzeugklassen ausgewählt oder keine Fahrzeuge zur Simulation).");
                                        }
                                        else
                                        {
                                            bool assigning = true;
                                            while (assigning)
                                            {
                                                Console.WriteLine("Verfügbare Fahrzeuge:");
                                                for (int i = 0; i < availableCars.Count; i++)
                                                {
                                                    var v = availableCars[i];
                                                    Console.WriteLine($"{i + 1}. {v.Fahrzeugname} ({v.Fahrzeugklasse}) - {v.Game}");
                                                }
                                                Console.WriteLine("Wählen Sie die Nummer des Fahrzeugs zum Hinzufügen (0 zum Beenden der Zuweisung):");
                                                int carChoice = GetUserInput.GetUserInputInt();
                                                if (carChoice == 0)
                                                {
                                                    assigning = false;
                                                    break;
                                                }
                                                if (carChoice < 1 || carChoice > availableCars.Count)
                                                {
                                                    Console.WriteLine("Ungültige Auswahl.");
                                                    continue;
                                                }

                                                try
                                                {
                                                    selectedEvent.ChooseCar(memberToAdd, availableCars[carChoice - 1]);
                                                    JsonPersistence.SaveAll(AppUserManager);
                                                    Console.WriteLine($"Fahrzeug '{availableCars[carChoice - 1].Fahrzeugname}' wurde '{memberToAdd.Name}' zugewiesen.");
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine($"Fehler bei der Fahrzeugzuweisung: {ex.Message}");
                                                }

                                                Console.WriteLine("Möchten Sie ein weiteres Fahrzeug für dieses Mitglied zuweisen? Y/N");
                                                char more = GetUserInput.GetUserInputChar();
                                                if (more != 'y') assigning = false;
                                            }
                                        }
                                    }

                                }

                                // Nachfragen: weiteres Mitglied hinzufügen oder Fahreraufstellung generieren
                                Console.WriteLine("");
                                Console.WriteLine("Was möchten Sie als nächstes tun?");
                                Console.WriteLine("1. Weiteres Mitglied hinzufügen");
                                Console.WriteLine("2. Fahreraufstellung generieren");
                                Console.WriteLine("0. Zurück zum Event-Menü");
                                int next = GetUserInput.GetUserInputInt();
                                if (next == 1)
                                {
                                    continueAdding = true;
                                }
                                else if (next == 2)
                                {
                                    // Neuer: Fahreraufstellung generieren mittels Fahrerzuteilung.GenerateTeams
                                    Console.WriteLine("");
                                    Console.WriteLine($"Fahreraufstellung für Event '{selectedEvent.Name}':");

                                    // 1) Build DriverSelection-Liste aus angemeldeten Mitgliedern mit zugewiesenen Fahrzeugen
                                    //    IMPORTANT: Ensure each driver is included exactly once. If a member selected multiple vehicles,
                                    //    pick a single vehicle (heuristic: choose the most popular vehicle among their choices).
                                    var selections = new List<DriverSelection>();
                                    var noVehicleMembers = new List<string>();

                                    // Build frequency map of vehicle choices to prefer common vehicles
                                    var vehicleCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                                    for (int i = 0; i < selectedEvent.AngemeldeteMitglieder.Count; i++)
                                    {
                                        var member = selectedEvent.AngemeldeteMitglieder[i];
                                        if (member.GewaehlteFahrzeuge == null) continue;
                                        for (int v = 0; v < member.GewaehlteFahrzeuge.Count; v++)
                                        {
                                            var veh = member.GewaehlteFahrzeuge[v];
                                            var vid = veh.Fahrzeugname ?? string.Empty;
                                            if (string.IsNullOrWhiteSpace(vid)) continue;
                                            if (!vehicleCounts.ContainsKey(vid)) vehicleCounts[vid] = 0;
                                            vehicleCounts[vid]++;
                                        }
                                    }

                                    for (int i = 0; i < selectedEvent.AngemeldeteMitglieder.Count; i++)
                                    {
                                        var member = selectedEvent.AngemeldeteMitglieder[i];
                                        if (member.GewaehlteFahrzeuge == null || member.GewaehlteFahrzeuge.Count == 0)
                                        {
                                            noVehicleMembers.Add(member.Name + " (ID: " + Convert.ToString(member.Id) + ")");
                                            continue;
                                        }

                                        // Choose single vehicle for this member:
                                        string chosenVehicleId = null;
                                        int bestCount = -1;
                                        for (int v = 0; v < member.GewaehlteFahrzeuge.Count; v++)
                                        {
                                            var veh = member.GewaehlteFahrzeuge[v];
                                            var vid = veh.Fahrzeugname ?? string.Empty;
                                            if (string.IsNullOrWhiteSpace(vid)) continue;
                                            vehicleCounts.TryGetValue(vid, out int cnt);
                                            if (cnt > bestCount)
                                            {
                                                bestCount = cnt;
                                                chosenVehicleId = vid;
                                            }
                                        }

                                        // Fallback to first choice if nothing selected
                                        if (string.IsNullOrWhiteSpace(chosenVehicleId))
                                        {
                                            chosenVehicleId = member.GewaehlteFahrzeuge[0].Fahrzeugname ?? string.Empty;
                                        }

                                        selections.Add(new DriverSelection(Convert.ToString(member.Id), member.Name, chosenVehicleId));
                                    }

                                    if (selections.Count == 0)
                                    {
                                        Console.WriteLine("Es sind keine Fahrer mit zugewiesenen Fahrzeugen vorhanden. Bitte weise zuerst Fahrzeuge zu.");
                                        if (noVehicleMembers.Count > 0)
                                        {
                                            Console.WriteLine("Mitglieder ohne Fahrzeug:");
                                            for (int i = 0; i < noVehicleMembers.Count; i++)
                                            {
                                                Console.WriteLine($" - {noVehicleMembers[i]}");
                                            }
                                        }
                                        Console.WriteLine("Drücken Sie eine Taste zum Fortfahren.");
                                        Console.ReadKey();
                                        continueAdding = false;
                                        break;
                                    }

                                    // 2) Rennlänge in Stunden (aufgerundet)
                                    int raceHours = GetRaceHoursFromEvent(selectedEvent);

                                    // 3) Simulationstyp bestimmen
                                    string simStr = (selectedEvent.Simulation ?? string.Empty).ToLowerInvariant();
                                    SimulationType simType = SimulationType.Default;
                                    if (simStr.IndexOf("lmu", StringComparison.OrdinalIgnoreCase) >= 0 || simStr.IndexOf("le mans", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        simType = SimulationType.LMU;
                                    }
                                    else if (simStr.IndexOf("acc", StringComparison.OrdinalIgnoreCase) >= 0 || simStr.IndexOf("assetto", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        simType = SimulationType.ACC;
                                    }
                                    else if (simStr.IndexOf("iracing", StringComparison.OrdinalIgnoreCase) >= 0 || simStr.IndexOf("i-racing", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        simType = SimulationType.IRacing;
                                    }

                                    // 4) Teams generieren
                                    List<Team> teams = null;
                                    try
                                    {
                                        teams = Fahrerzuteilung.GenerateTeams(selections, simType, raceHours);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Fehler beim Generieren der Teams: {ex.Message}");
                                        Console.WriteLine("Drücken Sie eine Taste zum Fortfahren.");
                                        Console.ReadKey();
                                        continueAdding = false;
                                        break;
                                    }

                                    // 5) Ausgabe der generierten Teams
                                    if (teams == null || teams.Count == 0)
                                    {
                                        Console.WriteLine("Es konnten keine Teams generiert werden.");
                                    }
                                    else
                                    {
                                        for (int ti = 0; ti < teams.Count; ti++)
                                        {
                                            var t = teams[ti];
                                            Console.WriteLine($"Team {t.TeamId} - Fahrzeug: {t.VehicleId} - Fahrer: {t.Drivers.Count}");
                                            for (int di = 0; di < t.Drivers.Count; di++)
                                            {
                                                var d = t.Drivers[di];
                                                Console.WriteLine($"  {di + 1}. {d.MemberName} (ID: {d.MemberId})");
                                            }
                                            if (t.IsUnderstaffed)
                                            {
                                                Console.WriteLine("  -> Hinweis: Team unterbesetzt (unter Mindestgröße).");
                                            }
                                            if (t.IsOverstaffed)
                                            {
                                                Console.WriteLine("  -> Hinweis: Team überbesetzt (über Maximum).");
                                            }
                                            Console.WriteLine("");
                                        }
                                    }

                                    // 6) Mitglieder ohne Fahrzeug anzeigen
                                    if (noVehicleMembers.Count > 0)
                                    {
                                        Console.WriteLine("Mitglieder ohne Fahrzeugzuweisung:");
                                        for (int i = 0; i < noVehicleMembers.Count; i++)
                                        {
                                            Console.WriteLine($" - {noVehicleMembers[i]}");
                                        }
                                    }

                                    Console.WriteLine("Fahreraufstellung fertig. (Drücken Sie eine Taste zum Fortfahren)");
                                    Console.ReadKey();
                                    continueAdding = false;
                                    break;
                                }
                                else
                                {
                                    continueAdding = false;
                                }
                            }
                        }
                        else if (act == 2)
                        {
                            if (selectedEvent.AngemeldeteMitglieder.Count == 0)
                            {
                                Console.WriteLine("Keine Mitglieder zum Entfernen vorhanden.");
                                continue;
                            }

                            Console.WriteLine("Angemeldete Mitglieder:");
                            for (int i = 0; i < selectedEvent.AngemeldeteMitglieder.Count; i++)
                            {
                                var m = selectedEvent.AngemeldeteMitglieder[i];
                                Console.WriteLine($"{i + 1}. {m.Name} (ID: {m.Id})");
                            }

                            Console.WriteLine("Wählen Sie die Nummer des zu entfernenden Mitglieds (0 zum Abbrechen):");
                            int rem = GetUserInput.GetUserInputInt();
                            if (rem == 0) continue;
                            if (rem < 1 || rem > selectedEvent.AngemeldeteMitglieder.Count)
                            {
                                Console.WriteLine("Ungültige Auswahl.");
                                continue;
                            }

                            var removed = selectedEvent.AngemeldeteMitglieder[rem - 1];
                            selectedEvent.AngemeldeteMitglieder.RemoveAt(rem - 1);
                            JsonPersistence.SaveAll(AppUserManager);
                            Console.WriteLine($"Mitglied '{removed.Name}' wurde vom Event entfernt und Daten wurden gespeichert.");
                        }
                        else
                        {
                            Console.WriteLine("Ungültige Auswahl.");
                        }
                    }

                    break;
                case 0:
                    // Keinen rekursiven Aufruf von MainLoop – stattdessen zurückgeben,
                    // damit der Aufrufer (MainLoop) die Kontrolle behält.
                    return;
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
                                if (newMember == "0" || newMember.ToLower() == "exit")
                                {
                                    break;
                                }
                                if (newMember.Trim() == "")
                                {
                                    Console.WriteLine("Ungültiger Name. Bitte versuchen Sie es erneut.");
                                    continue;
                                }
                                try
                                {

                                    EventMember newMember1 = new EventMember(newMember);
                                    JsonPersistence.SaveAll(AppUserManager);
                                    Console.WriteLine($"Neues Mitglied: {EventMember.Mitgliederliste.Last()} wurde erstellt.");
                                    continue;
                                }
                                catch (Exception alreadyExists)
                                {
                                    foreach (var member in EventMember.Mitgliederliste)
                                    {
                                        if (member.Name.Equals(newMember, StringComparison.OrdinalIgnoreCase)) // ignorieren der Groß-/Kleinschreibung zum Vergleich
                                        {
                                            throw new Exception("Ein Mitglied mit diesem Namen existiert bereits.");
                                        }
                                    }
                                }
                            }
                            if (wahl == 2)
                            {
                                Console.WriteLine("Welches Mitglied soll entfernt werden?");
                                for (int i = 0; i < EventMember.Mitgliederliste.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {EventMember.Mitgliederliste[i].Name}");
                                }
                                int membernumber = GetUserInput.GetUserInputInt();
                                if (membernumber > 0 && membernumber <= EventMember.Mitgliederliste.Count)
                                {
                                    EventMember.Mitgliederliste.RemoveAt(membernumber - 1);
                                    JsonPersistence.SaveAll(AppUserManager);
                                    Console.WriteLine("Mitglied entfernt und Daten gespeichert.");
                                }
                            }
                            break;
                        }
                        break;
                    case 3:
                        Console.WriteLine("Event Verwaltung\n1. Event erstellen\n2. Event löschen\n0. Zurück");
                        int eventchoice = GetUserInput.GetUserInputInt();
                        if (eventchoice == 0)
                        {
                            break;
                        }
                        if (eventchoice == 1)
                        {
                            Console.WriteLine("Name des Events eingeben (0 zum Abbrechen): ");
                            string eventName = GetUserInput.GetUserInputStr();
                            if (eventName == "0" || eventName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Event-Erstellung abgebrochen.");
                                break;
                            }

                            // CreateEvent kann jetzt null zurückgeben, wenn der Benutzer abbricht.
                            var created = new Event(eventName).CreateEvent(eventName);
                            if (created == null)
                            {
                                Console.WriteLine("Event-Erstellung abgebrochen.");
                                break;
                            }

                            EventManager.AddEvent(created);
                            JsonPersistence.SaveAll(AppUserManager);
                            Console.WriteLine("Event erstellt und gespeichert.");

                            // Return to caller so user isn't "stuck" in admin menu after creating an event
                            Console.WriteLine("Drücken Sie eine Taste, um zum vorherigen Menü zurückzukehren.");
                            Console.ReadKey();
                            return;
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
                    case 0:
                        return;
                }
            }
        }

        private static int GetRaceHoursFromEvent(Event ev)
        {
            if (ev == null) throw new ArgumentNullException(nameof(ev));

            // Wenn Dauer null ist: Default nutzen oder Benutzer nachfragen.
            // Hier: Default = 60 Minuten (1 Stunde). Alternativ könntest du den Nutzer auffordern, die Dauer zu setzen.
            int minutes = ev.Dauer ?? 60;

            int hours = (minutes + 59) / 60; // Aufrunden auf volle Stunden
            if (hours < 1) hours = 1;
            return hours;
        }

    }
}