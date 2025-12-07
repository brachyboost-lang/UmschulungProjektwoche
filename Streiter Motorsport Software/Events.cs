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
            // ID zuweisen
            Id = naechsteId;
            naechsteId = naechsteId++;

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
                if (vc.Game == simulation)
                {
                    VorgeschlageneFahrzeugklassen.Add(vc);
                }
            }
        }

        public Event(string name)
        {
            Name = name;
        }

        public Event CreateEvent(string name)
        {
            Name = name;
            while (true)
            {
                Console.WriteLine("Simulation auswählen: ");
                Console.WriteLine("1. iRacing");
                Console.WriteLine("2. Assetto Corsa Competizione");
                Console.WriteLine("3. Le Mans Ultimate");
                int simulation = GetUserInput.GetUserInputInt();
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
                        throw new ArgumentOutOfRangeException("Ungültige Auswahl für die Simulation.");

                }
                break;
            }
            Console.WriteLine("Strecke eingeben: ");
            this.Strecke = GetUserInput.GetUserInputStr();
            Console.WriteLine("Dauer des Events in Minuten eingeben: ");
            this.Dauer = GetUserInput.GetUserInputInt();
            Console.WriteLine("Datum des Events eingeben: ");
            this.Datum = GetUserInput.GetUserInputDateTime();
            Console.WriteLine("Fahrzeugklassen auswählen: ");
            this.VorgeschlageneFahrzeugklassen = EventManager.SchlageFahrzeugklassenVor(this.Simulation);
            for (int i = 0; i < this.VorgeschlageneFahrzeugklassen.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {this.VorgeschlageneFahrzeugklassen[i].Fahrzeugklasse}");
            }
            Console.WriteLine("Geben Sie die Nummern der gewünschten Fahrzeugklassen ein, getrennt durch Kommas: (z.B. 1, 5, 6");
            int eingabe = GetUserInput.GetUserInputInt();
            string[] ausgewaehlteNummern = eingabe.ToString().Split(','); // trennt die eingabe in einzelne nummern auf
            for (int i = 0; i < ausgewaehlteNummern.Length; i++)
            {
                if (int.TryParse(ausgewaehlteNummern[i].Trim(), out int nummer)) // trimmt leerzeichen und prüft ob es eine gültige zahl ist
                {
                    if (nummer >= 1 && nummer <= this.VorgeschlageneFahrzeugklassen.Count)
                    {
                        VehicleClasses ausgewaehlteKlasse = this.VorgeschlageneFahrzeugklassen[nummer - 1];
                        this.AusgewählteFahrzeugklassen.Add(ausgewaehlteKlasse);
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
            Console.WriteLine("Event erstellt. Folgende Auswahl wurde getroffen: ");
            bool finalValid = false;
            while (!finalValid)
            {
                Console.WriteLine($"Simulation: {this.Simulation}, Dauer: {this.Dauer} Minuten, Strecke: {this.Strecke}, Datum: {this.Datum.ToShortDateString()}");
                Console.WriteLine($"Fahrzeugklassen: ");
                foreach (var klasse in this.AusgewählteFahrzeugklassen)
                {
                    Console.WriteLine($"- {klasse.Fahrzeugklasse}");
                }
                Console.WriteLine("----------");
                Console.WriteLine("Ist die Auswahl so korrekt? Y/N");
                char bestaetigung = GetUserInput.GetUserInputChar();
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
                                Console.WriteLine("In welche Simulation soll das Event geändert werden?");
                                Console.WriteLine("1. iRacing");
                                Console.WriteLine("2. Assetto Corsa Competizione");
                                Console.WriteLine("3. Le Mans Ultimate");
                                int neueSimulation = GetUserInput.GetUserInputInt();
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
                            Console.WriteLine("Geben Sie die neue Strecke ein: ");
                            this.Strecke = GetUserInput.GetUserInputStr();
                            break;
                        case 3:
                            Console.WriteLine($"Aktuelle Dauer: {this.Dauer}");
                            Console.WriteLine("Geben Sie die neue Dauer in Minuten ein: ");
                            this.Dauer = GetUserInput.GetUserInputInt();
                            break;
                        case 4:
                            Console.WriteLine($"Aktuelles Datum: {this.Datum}");
                            Console.WriteLine("Geben Sie das neue Datum ein: ");
                            this.Datum = GetUserInput.GetUserInputDateTime();
                            break;
                        case 5:
                            Console.WriteLine($"Aktuelle Fahrzeugklassen: ");
                            foreach (var klasse in this.AusgewählteFahrzeugklassen)
                            {
                                Console.WriteLine($"- {klasse.Fahrzeugklasse}");
                            }
                            Console.WriteLine("Wählen Sie Fahrzeugklassen aus zum hinzufügen: ");
                            this.VorgeschlageneFahrzeugklassen = EventManager.SchlageFahrzeugklassenVor(this.Simulation);
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
                            Console.WriteLine("Geben Sie die Nummern der gewünschten Fahrzeugklassen ein, getrennt durch Kommas: (z.B. 1, 5, 6");
                            int neueEingabe = GetUserInput.GetUserInputInt();
                            string[] neueAusgewaehlteNummern = neueEingabe.ToString().Split(',');
                            for (int i = 0; i < neueAusgewaehlteNummern.Length; i++)
                            {
                                if (int.TryParse(neueAusgewaehlteNummern[i].Trim(), out int nummer))
                                {
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
                            finalValid = true;
                            return this;
                        default:
                            Console.WriteLine("Ungültige eingabe");
                            break;

                    }
                }
            }
            return this; // hat rumgeweint dass nicht alle pfade einen value zurückgeben - muss nochmal angeschaut werden eventuell ob bug verursacht wurde
        }




        // Wählt eine Fahrzeugklasse aus der Vorschlagsliste aus.
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
                if (v.Game != this.Simulation)
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
            if (fahrzeug.Game != this.Simulation)
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
                if (vc.Game == simulation)
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
                if (v.Game == simulation && v.Fahrzeugklasse == fahrzeugklasse)
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
        internal Vehicles? GewaehltesFahrzeug { get; private set; } // Gewähltes Fahrzeug (kann null sein)
        internal static List<EventMember> Mitgliederliste { get; private set; } = eventMembers;

        public EventMember(string name)
        {
            // ID zuweisen und Zähler erhöhen
            Id = naechsteId;
            naechsteId = naechsteId++;

            Name = name;
            Mitgliederliste.Add(this); // fügt dieses mitglied direkt auf die mitgliederliste hinzu

        }

        // Setzt das gewählte Fahrzeug für dieses Mitglied.
        // Validierungen (ob das Fahrzeug erlaubt ist) erfolgen in der Event-Logik.
        internal void CarChoice(Vehicles fahrzeug)
        {
            GewaehltesFahrzeug = fahrzeug;
        }
    }

}


