using System;
using System.Collections.Generic;
using System.Text;

namespace Streiter_Motorsport_Software
{

    // Event: einfache Darstellung einer Veranstaltung.
    // Verwendet nur einfache Konstrukte: int-IDs, List<T>, foreach-Schleifen.
    internal class Event
    {
        private static int naechsteId = 1; // Zähler für Event-IDs

        internal int? Id { get; private set; }                // Einfache int-ID
        internal string Name { get; set; }                   // Name des Events
        internal string? Simulation { get; set; }             // Zugehörige Simulation
        internal int? Dauer { get; set; }                     // Dauer in Minuten
        internal DateTime Datum { get; set; }                  // Datum des Events
        internal string? Strecke { get; set; }                 // Strecke des Events

        // Vorschläge: Fahrzeugklassen, die zur Simulation passen.
        internal List<VehicleClasses> VorgeschlageneFahrzeugklassen { get; private set; }

        // Vom Veranstalter ausgewählte Fahrzeugklassen für dieses Event.
        internal List<VehicleClasses> AusgewählteFahrzeugklassen { get; private set; }

        // Angemeldete Mitglieder für das Event.
        internal List<EventMember> AngemeldeteMitglieder { get; private set; }

        public Event(string name, string simulation, int dauer, DateTime datum, string strecke)
        {
            // ID zuweisen (korrekte Inkrementierung)
            Id = naechsteId++;
            Name = name;
            Simulation = simulation;
            Dauer = dauer;
            Datum = datum;
            Strecke = strecke;

            VorgeschlageneFahrzeugklassen = new List<VehicleClasses>();
            AusgewählteFahrzeugklassen = new List<VehicleClasses>();
            AngemeldeteMitglieder = new List<EventMember>();

            // Fülle die Vorschlagsliste mit foreach Schleife.
            // durchsucht zentrale fahrzeugklassenliste und fügt passenden Eintrag hinzu.
            foreach (VehicleClasses vc in VehicleClasses.fahrzeugklassenliste)
            {
                // Vergleich über normalisierte Simulationen (akzeptiert Kurzformen / unterschiedliche Schreibweisen)
                if (EventManager.NormalizeSimulationName(vc.Game) == EventManager.NormalizeSimulationName(simulation))
                {
                    VorgeschlageneFahrzeugklassen.Add(vc);
                }
            }
        }

        // Konstruktor für minimale Erstellung (z.B. aus Admin-Menü)
        public Event(string name)
        {
            Id = naechsteId++;
            Name = name;
            VorgeschlageneFahrzeugklassen = new List<VehicleClasses>();
            AusgewählteFahrzeugklassen = new List<VehicleClasses>();
            AngemeldeteMitglieder = new List<EventMember>();
        }

        // Spezieller Konstruktor zur Rekonstruktion beim Laden aus Persistenz (setzt Id direkt)
        internal Event(int id, string name, string? simulation, int? dauer, DateTime datum, string? strecke)
        {
            Id = id;
            // stelle sicher, dass nächstes Id mindestens id+1 ist
            if (id >= naechsteId) naechsteId = id + 1;

            Name = name;
            Simulation = simulation;
            Dauer = dauer;
            Datum = datum;
            Strecke = strecke;

            VorgeschlageneFahrzeugklassen = new List<VehicleClasses>();
            AusgewählteFahrzeugklassen = new List<VehicleClasses>();
            AngemeldeteMitglieder = new List<EventMember>();

            // Vorschläge werden ggf. später befüllt beim Re-Laden der Referenzen
        }

        // CreateEvent now returns nullable Event: returns null when user cancels (presses 0 or "exit")
        public Event? CreateEvent(string name)
        {
            Name = name;

            // Simulation auswählen
            while (true)
            {
                Console.WriteLine("Simulation auswählen (0 zum Abbrechen): ");
                Console.WriteLine("1. iRacing");
                Console.WriteLine("2. Assetto Corsa Competizione");
                Console.WriteLine("3. Le Mans Ultimate");
                int simulation = GetUserInput.GetUserInputInt();
                if (simulation == 0) return null; // allow cancel
                switch (simulation)
                {
                    case 1:
                        this.Simulation = "iRacing";
                        break;
                    case 2:
                        this.Simulation = "Assetto Corsa Competizione";
                        break;
                    case 3:
                        this.Simulation = "Le Mans Ultimate";
                        break;
                    default:
                        Console.WriteLine("Ungültige Auswahl für die Simulation. Bitte erneut.");
                        continue;
                }
                break;
            }

            // Strecke eingeben (0 zum Abbrechen)
            Console.WriteLine("Strecke eingeben (0 zum Abbrechen): ");
            string streckeInput = GetUserInput.GetUserInputStr();
            if (streckeInput == "0" || streckeInput.Equals("exit", StringComparison.OrdinalIgnoreCase)) return null;
            this.Strecke = streckeInput;

            // Dauer (in Minuten) eingeben (0 zum Abbrechen)
            Console.WriteLine("Dauer des Events in Minuten eingeben (0 zum Abbrechen): ");
            int dauerInput = GetUserInput.GetUserInputInt();
            if (dauerInput == 0) return null;
            this.Dauer = dauerInput;

            // Datum eingeben (0 zum Abbrechen)
            Console.WriteLine("Datum des Events eingeben im Format TT.MM.JJJJ hh:mm (0 zum Abbrechen): ");
            while (true)
            {
                string dateInput = GetUserInput.GetUserInputStr();
                if (dateInput == "0" || dateInput.Equals("exit", StringComparison.OrdinalIgnoreCase)) return null;
                if (DateTime.TryParse(dateInput, out DateTime parsed))
                {
                    this.Datum = parsed;
                    break;
                }
                Console.Write("Ungültiges Datum. Bitte im Format TT.MM.JJJJ hh:mm eingeben oder 0 zum Abbrechen: ");
            }

            Console.WriteLine("Fahrzeugklassen auswählen: ");
            this.VorgeschlageneFahrzeugklassen = EventManager.SchlageFahrzeugklassenVor(this.Simulation);

            // Wenn keine Klassen verfügbar sind, dem Benutzer eine klare Meldung geben
            if (this.VorgeschlageneFahrzeugklassen.Count == 0)
            {
                Console.WriteLine("Für die gewählte Simulation sind keine Fahrzeugklassen hinterlegt.");
            }
            else
            {
                for (int i = 0; i < this.VorgeschlageneFahrzeugklassen.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {this.VorgeschlageneFahrzeugklassen[i].Fahrzeugklasse}");
                }

                Console.WriteLine("Geben Sie die Nummern der gewünschten Fahrzeugklassen ein, getrennt durch Kommas (z.B. 1,5,6) oder 0 für keine Auswahl:");
                string eingabe = GetUserInput.GetUserInputStr();
                if (eingabe == "0" || eingabe.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    // no selection, continue
                }
                else
                {
                    string[] ausgewaehlteNummern = eingabe.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); // trennt die eingabe in einzelne nummern auf
                    for (int i = 0; i < ausgewaehlteNummern.Length; i++)
                    {
                        if (int.TryParse(ausgewaehlteNummern[i].Trim(), out int nummer)) // trimmt leerzeichen und prüft ob es eine gültige zahl ist
                        {
                            if (nummer == 0) break;
                            if (nummer >= 1 && nummer <= this.VorgeschlageneFahrzeugklassen.Count)
                            {
                                VehicleClasses ausgewaehlteKlasse = this.VorgeschlageneFahrzeugklassen[nummer - 1];
                                // vermeide doppelte Einträge
                                if (!this.AusgewählteFahrzeugklassen.Contains(ausgewaehlteKlasse))
                                {
                                    this.AusgewählteFahrzeugklassen.Add(ausgewaehlteKlasse);
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Ungültige Nummer: {nummer}. Diese wird übersprungen.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Ungültige Eingabe: {ausgewaehlteNummern[i]}. Diese wird übersprungen.");
                        }
                    }
                }
            }

            Console.WriteLine("Event erstellt. Folgende Auswahl wurde getroffen: ");
            
            while (true)
            {
                Console.WriteLine($"Simulation: {this.Simulation}, Dauer: {this.Dauer} Minuten, Strecke: {this.Strecke}, Datum: {this.Datum.ToShortDateString()}");
                Console.WriteLine($"Fahrzeugklassen: ");
                foreach (var klasse in this.AusgewählteFahrzeugklassen)
                {
                    Console.WriteLine($"- {klasse.Fahrzeugklasse}");
                }
                Console.WriteLine("----------");
                Console.WriteLine("Ist die Auswahl so korrekt? Y/N (oder 0 zum Abbrechen)");
                char bestaetigung = GetUserInput.GetUserInputChar();

                if (bestaetigung == '0') // allow abort
                {
                    return null;
                }

                if (bestaetigung == 'y')
                {
                    Console.WriteLine("Event wurde erfolgreich erstellt.");
                    return this;
                }
                else
                {
                    Console.WriteLine("Was soll verändert werden?");
                    Console.WriteLine("1. Simulation");
                    Console.WriteLine("2. Strecke");
                    Console.WriteLine("3. Dauer");
                    Console.WriteLine("4. Datum");
                    Console.WriteLine("5. Fahrzeugklassen");
                    Console.WriteLine("0. Bestätigen, alles ist korrekt");

                    int auswahl = GetUserInput.GetUserInputInt();
                    bool valid = false;
                    switch (auswahl)
                    {
                        case 1:
                            while (!valid)
                            {
                                Console.WriteLine($"Aktuelle Simulation: {this.Simulation}");
                                Console.WriteLine("In welche Simulation soll das Event geändert werden? (0 zum Abbrechen)");
                                Console.WriteLine("1. iRacing");
                                Console.WriteLine("2. Assetto Corsa Competizione");
                                Console.WriteLine("3. Le Mans Ultimate");
                                int neueSimulation = GetUserInput.GetUserInputInt();
                                if (neueSimulation == 0) break;
                                switch (neueSimulation)
                                {
                                    case 1:
                                        this.Simulation = "iRacing";
                                        valid = true;
                                        break;

                                    case 2:
                                        this.Simulation = "Assetto Corsa Competizione";
                                        valid = true;
                                        break;
                                    case 3:
                                        this.Simulation = "Le Mans Ultimate";
                                        valid = true;
                                        break;
                                    default:
                                        Console.WriteLine("Ungültige Auswahl für die Simulation.");
                                        break;

                                }
                            }
                            break;
                        case 2:
                            Console.WriteLine($"Aktuelle Strecke: {this.Strecke}");
                            Console.WriteLine("Geben Sie die neue Strecke ein (0 zum Abbrechen): ");
                            string newStrecke = GetUserInput.GetUserInputStr();
                            if (!(newStrecke == "0" || newStrecke.Equals("exit", StringComparison.OrdinalIgnoreCase)))
                            {
                                this.Strecke = newStrecke;
                            }
                            break;
                        case 3:
                            Console.WriteLine($"Aktuelle Dauer: {this.Dauer}");
                            Console.WriteLine("Geben Sie die neue Dauer in Minuten ein (0 zum Abbrechen): ");
                            int newDauer = GetUserInput.GetUserInputInt();
                            if (newDauer != 0) this.Dauer = newDauer;
                            break;
                        case 4:
                            Console.WriteLine($"Aktuelles Datum: {this.Datum}");
                            Console.WriteLine("Geben Sie das neue Datum ein im Format TT.MM.JJJJ hh:mm (0 zum Abbrechen): ");
                            while (true)
                            {
                                string input = GetUserInput.GetUserInputStr();
                                if (input == "0" || input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                                if (DateTime.TryParse(input, out DateTime parsed)) { this.Datum = parsed; break; }
                                Console.Write("Ungültiges Datum. Bitte erneut oder 0 zum Abbrechen: ");
                            }
                            break;
                        case 5:
                            Console.WriteLine($"Aktuelle Fahrzeugklassen: ");
                            foreach (var klasse in this.AusgewählteFahrzeugklassen)
                            {
                                Console.WriteLine($"- {klasse.Fahrzeugklasse}");
                            }
                            Console.WriteLine("Wählen Sie Fahrzeugklassen aus zum hinzufügen (0 zum Abbrechen): ");
                            this.VorgeschlageneFahrzeugklassen = EventManager.SchlageFahrzeugklassenVor(this.Simulation);

                            if (this.VorgeschlageneFahrzeugklassen.Count == 0)
                            {
                                Console.WriteLine("Für die gewählte Simulation sind keine Fahrzeugklassen hinterlegt.");
                                break;
                            }

                            for (int i = 0; i < this.VorgeschlageneFahrzeugklassen.Count; i++)
                            {
                                if (this.AusgewählteFahrzeugklassen.Contains(this.VorgeschlageneFahrzeugklassen[i]))
                                {
                                    continue; // überspringt bereits ausgewählte klassen
                                }
                                else
                                {
                                    Console.WriteLine($"{i + 1}. {this.VorgeschlageneFahrzeugklassen[i].Fahrzeugklasse}");
                                }
                            }
                            Console.WriteLine("Geben Sie die Nummern der gewünschten Fahrzeugklassen ein, getrennt durch Kommas (z.B. 1,5,6) oder 0 zum Abbrechen:");
                            string neueEingabe = GetUserInput.GetUserInputStr();
                            if (neueEingabe == "0" || neueEingabe.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                            string[] neueAusgewaehlteNummern = neueEingabe.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < neueAusgewaehlteNummern.Length; i++)
                            {
                                if (int.TryParse(neueAusgewaehlteNummern[i].Trim(), out int nummer))
                                {
                                    if (nummer == 0) break;
                                    if (nummer >= 1 && nummer <= this.VorgeschlageneFahrzeugklassen.Count)
                                    {
                                        VehicleClasses ausgewaehlteKlasse = this.VorgeschlageneFahrzeugklassen[nummer - 1];
                                        if (this.AusgewählteFahrzeugklassen.Contains(ausgewaehlteKlasse))
                                        {
                                            Console.WriteLine($"Fahrzeugklasse {ausgewaehlteKlasse.Fahrzeugklasse} ist bereits ausgewählt. Diese wird übersprungen.");
                                        }
                                        else
                                        {
                                            this.AusgewählteFahrzeugklassen.Add(ausgewaehlteKlasse);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Ungültige Nummer: {nummer}. Diese wird übersprungen.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Ungültige Eingabe: {neueAusgewaehlteNummern[i]}. Diese wird übersprungen.");
                                }
                            }
                            break;
                        case 0:
                            
                            return this;
                        default:
                            Console.WriteLine("Ungültige eingabe");
                            break;

                    }
                }
            }
            
        }

        // Wählt eine Fahrzeugklasse aus der Vorschagsliste aus.
        // Hier prüfen wir einfach per Vergleich auf Übereinstimmung der Eigenschaften.
        internal void ChooseClass(VehicleClasses klasse)
        {
            if (klasse == null)
            {
                throw new ArgumentNullException("klasse", "Die Fahrzeugklasse darf nicht null sein."); // erster string ist der name des parameters, zweiter die fehlermeldung
            }

            // Prüfen, ob die Klasse in den Vorschlägen enthalten ist
            bool gefunden = false;
            foreach (VehicleClasses v in VorgeschlageneFahrzeugklassen)
            {
                if (v.Game == klasse.Game && v.Fahrzeugklasse == klasse.Fahrzeugklasse)
                {
                    gefunden = true;
                    break;
                }
            }

            if (!gefunden)
            {
                throw new InvalidOperationException("Diese Fahrzeugklasse gehört nicht zu den vorgeschlagenen Klassen für das Event.");
            }

            // Nur hinzufügen, wenn noch nicht vorhanden
            bool bereitsVorhanden = false;
            foreach (VehicleClasses v in AusgewählteFahrzeugklassen)
            {
                if (v.Game == klasse.Game && v.Fahrzeugklasse == klasse.Fahrzeugklasse)
                {
                    bereitsVorhanden = true;
                    break;
                }
            }

            if (!bereitsVorhanden)
            {
                AusgewählteFahrzeugklassen.Add(klasse);
            }
        }

        // Liefert alle Fahrzeuge, die zur Event-Simulation passen und zu den ausgewählten Klassen gehören.
        // Rückgabe als List<Vehicles>. <- Brainfuck
        internal List<Vehicles> ShowCars()
        {
            List<Vehicles> ergebnis = new();

            // Wenn keine Klassen ausgewählt sind, ist die Ergebnisliste leer.
            if (AusgewählteFahrzeugklassen.Count == 0)
            {
                return ergebnis;
            }

            // Für jeden Eintrag in der zentralen Fahrzeugliste prüfen wir Simulation + Klasse.
            foreach (Vehicles v in Vehicles.fahrzeugeliste)
            {
                // Vergleich über normalisierten Simulationen (akzeptiert Kurzformen / unterschiedliche Schreibweisen)
                if (EventManager.NormalizeSimulationName(v.Game) != EventManager.NormalizeSimulationName(this.Simulation))
                {
                    // Fahrzeug gehört nicht zur Simulation des Events
                    continue;
                }

                // Prüfe, ob die Fahrzeugklasse in AusgewählteFahrzeugklassen enthalten ist.
                bool klasseGefunden = false;
                foreach (VehicleClasses kc in AusgewählteFahrzeugklassen)
                {
                    if (kc.Fahrzeugklasse == v.Fahrzeugklasse)
                    {
                        klasseGefunden = true;
                        break;
                    }
                }

                if (klasseGefunden)
                {
                    ergebnis.Add(v);
                }
            }

            return ergebnis;
        }

        // Fügt ein Mitglied hinzu und gibt das Objekt zurück.
        internal EventMember AddMember(string name)
        {
            EventMember m = new(name); // macht das gleiche wie m = new EventMember(name);- auch mit parameterübergabe abkürzbar
            AngemeldeteMitglieder.Add(m);
            return m;
        }

        // Weist einem Mitglied ein Fahrzeug zu.
        internal void ChooseCar(EventMember mitglied, Vehicles fahrzeug)
        {
            if (mitglied == null)
            {
                throw new ArgumentNullException("mitglied", "Mitglied darf nicht null sein.");
            }
            if (fahrzeug == null)
            {
                throw new ArgumentNullException("fahrzeug", "Fahrzeug darf nicht null sein.");
            }

            // Prüfen, ob das Mitglied zu diesem Event gehört (Vergleich über ID)
            bool mitgliedGefunden = false;
            foreach (EventMember m in AngemeldeteMitglieder)
            {
                if (m.Id == mitglied.Id)
                {
                    mitgliedGefunden = true;
                    break;
                }
            }
            if (!mitgliedGefunden)
            {
                throw new InvalidOperationException("Das Mitglied gehört nicht zu diesem Event.");
            }

            // Fahrzeug muss zur Simulation passen
            if (EventManager.NormalizeSimulationName(fahrzeug.Game) != EventManager.NormalizeSimulationName(this.Simulation))
            {
                throw new InvalidOperationException("Das Fahrzeug gehört nicht zur Simulation des Events.");
            }

            // Falls Klassen ausgewählt sind: Fahrzeugklasse muss erlaubt sein
            if (AusgewählteFahrzeugklassen.Count > 0)
            {
                bool erlaubt = false;
                foreach (VehicleClasses kc in AusgewählteFahrzeugklassen)
                {
                    if (kc.Fahrzeugklasse == fahrzeug.Fahrzeugklasse)
                    {
                        erlaubt = true;
                        break;
                    }
                }
                if (!erlaubt)
                {
                    throw new InvalidOperationException("Das Fahrzeug gehört nicht zu einer für dieses Event ausgewählten Fahrzeugklasse.");
                }
            }

            // Zuweisung
            mitglied.CarChoice(fahrzeug);
        }
    }

    // Einfacher Manager zur zentralen Verwaltung von Events.
    internal static class EventManager
    {
        internal static List<Event> Events { get; private set; } = new();

        // Helfer: Normalisiert verschiedene Schreibweisen / Kurzformen einer Simulation auf eine kanonische Form.
        // So müssen Fahrzeug-/Klassenlisten nicht überall geändert werden.
        internal static string NormalizeSimulationName(string? sim)
        {
            if (string.IsNullOrWhiteSpace(sim)) return string.Empty;
            var s = sim.Trim().ToLowerInvariant();

            if (s == "acc" || s.Contains("assetto") || s.Contains("competizione"))
            {
                return "Assetto Corsa Competizione";
            }

            if (s == "lmu" || s.Contains("le mans") || s.Contains("ultimate"))
            {
                return "Le Mans Ultimate";
            }

            if (s.Contains("iracing") || s == "i-racing")
            {
                return "iRacing";
            }

            // Standard: Rückgabe in Original-Form, aber mit normalisiertem Whitespace
            // (das reicht für string-equality-Vergleich nach Normalisierung)
            return sim.Trim();
        }

        // Erstellt ein Event und speichert es in der Liste.
        public static void AddEvent(Event newEvent)
        // wird benötigt um ein event in die liste hinzuzufügen da Events private set ist
        {
            Events.Add(newEvent);

        }

        // Liefert eine Liste von Fahrzeugklassen für die angegebene Simulation.
        internal static List<VehicleClasses> SchlageFahrzeugklassenVor(string simulation)
        {
            List<VehicleClasses> ergebnis = new List<VehicleClasses>();
            foreach (VehicleClasses vc in VehicleClasses.fahrzeugklassenliste)
            {
                if (NormalizeSimulationName(vc.Game) == NormalizeSimulationName(simulation))
                {
                    ergebnis.Add(vc);
                }
            }
            return ergebnis;
        }

        // das gleiche für Fahrzeuge
        internal static List<Vehicles> SchlageFahrzeugeVor(string simulation, string fahrzeugklasse)
        {
            List<Vehicles> ergebnis = new List<Vehicles>();
            foreach (Vehicles v in Vehicles.fahrzeugeliste)
            {
                if (NormalizeSimulationName(v.Game) == NormalizeSimulationName(simulation) && v.Fahrzeugklasse == fahrzeugklasse)
                {
                    ergebnis.Add(v);
                }
            }
            return ergebnis;
        }
    }
    // EventMember: einfaches Mitglieds-Objekt.
    // IDs werden hier als int verwaltet (keine GUIDs), einfacher und überschaubar.
    internal class EventMember
    {
        // Statischer Zähler zur Vergabe einfacher, inkrementeller IDs.
        private static int naechsteId = 1;
        private static readonly List<EventMember> eventMembers = new();

        internal int Id { get; private set; }           // Eindeutige einfache ID
        internal string Name { get; set; }              // Anzeigename des Mitglieds
        // Liste der gewählten Fahrzeuge (jetzt mehrere möglich)
        internal List<Vehicles> GewaehlteFahrzeuge { get; private set; } = new List<Vehicles>();
        internal static List<EventMember> Mitgliederliste { get; private set; } = eventMembers;

        public EventMember(string name)
        {
            // ID zuweisen und Zähler erhöhen (korrekte Inkrementierung)
            Id = naechsteId++;
            Name = name;
            Mitgliederliste.Add(this); // fügt dieses mitglied direkt auf die mitgliederliste hinzu
            GewaehlteFahrzeuge = new List<Vehicles>();
        }

        // Spezieller Konstruktor zur Rekonstruktion beim Laden aus Persistenz (setzt Id direkt)
        internal EventMember(int id, string name)
        {
            Id = id;
            if (id >= naechsteId) naechsteId = id + 1;
            Name = name;
            Mitgliederliste.Add(this);
            GewaehlteFahrzeuge = new List<Vehicles>();
        }

        // Fügt ein Fahrzeug zu den gewählten Fahrzeugen dieses Mitglieds hinzu.
        // Validierungen (ob das Fahrzeug erlaubt ist) erfolgen in der Event-Logik.
        internal void CarChoice(Vehicles fahrzeug)
        {
            if (fahrzeug == null)
            {
                throw new ArgumentNullException(nameof(fahrzeug));
            }

            // Duplikate vermeiden (vergleich über Fahrzeugname & Game)
            foreach (var v in GewaehlteFahrzeuge)
            {
                if (v.Fahrzeugname == fahrzeug.Fahrzeugname && v.Game == fahrzeug.Game)
                {
                    // bereits vorhanden -> nichts tun
                    return;
                }
            }

            GewaehlteFahrzeuge.Add(fahrzeug);
        }
    }

}