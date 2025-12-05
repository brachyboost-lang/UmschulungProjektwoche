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

        internal int Id { get; private set; }                // Einfache int-ID
        internal string Name { get; set; }                   // Name des Events
        internal string Simulation { get; set; }             // Zugehörige Simulation
        internal int Dauer { get; set; }                     // Dauer in Minuten

        // Vorschläge: Fahrzeugklassen, die zur Simulation passen.
        internal List<VehicleClasses> VorgeschlageneFahrzeugklassen { get; private set; }

        // Vom Veranstalter ausgewählte Fahrzeugklassen für dieses Event.
        internal List<VehicleClasses> AusgewählteFahrzeugklassen { get; private set; }

        // Angemeldete Mitglieder für das Event.
        internal List<EventMember> Mitglieder { get; private set; }

        public Event(string name, string simulation, int dauer)
        {
            // ID zuweisen
            Id = naechsteId;
            naechsteId = naechsteId ++;

            Name = name;
            Simulation = simulation;
            Dauer = dauer;


            VorgeschlageneFahrzeugklassen = new List<VehicleClasses>();
            AusgewählteFahrzeugklassen = new List<VehicleClasses>();
            Mitglieder = new List<EventMember>();

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

        // Wählt eine Fahrzeugklasse aus der Vorschlagsliste aus.
        // Hier prüfen wir einfach per Vergleich auf Übereinstimmung der Eigenschaften.
        internal void WähleFahrzeugklasse(VehicleClasses klasse)
        {
            if (klasse == null)
            {
                throw new ArgumentNullException("klasse", "Die Fahrzeugklasse darf nicht null sein.");
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
        internal List<Vehicles> VerfügbareFahrzeugeFürAusgewählteKlassen()
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
                if (v.Simulation != this.Simulation)
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
        internal EventMember FuegeMitgliedHinzu(string name)
        {
            EventMember m = new(name); // macht das gleiche wie m = new EventMember(name);- auch mit parameterübergabe abkürzbar
            Mitglieder.Add(m);
            return m;
        }

        // Weist einem Mitglied ein Fahrzeug zu.
        internal void WeiseFahrzeugZu(EventMember mitglied, Vehicles fahrzeug)
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
            foreach (EventMember m in Mitglieder)
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
            if (fahrzeug.Simulation != this.Simulation)
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
            mitglied.WähleFahrzeug(fahrzeug);
        }
    }

    // Einfacher Manager zur zentralen Verwaltung von Events.
    internal static class EventManager
    {
        internal static List<Event> Events { get; private set; } = new();

        // Erstellt ein Event und speichert es in der Liste.
        internal static Event ErzeugeEvent(string name, string game, int dauer)
        {
            Event ev = new Event(name, game, dauer);
            Events.Add(ev);
            return ev;
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
                if (v.Simulation == simulation && v.Fahrzeugklasse == fahrzeugklasse)
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

        internal int Id { get; private set; }           // Eindeutige einfache ID
        internal string Name { get; set; }              // Anzeigename des Mitglieds
        internal Vehicles GewaehltesFahrzeug { get; private set; } // Gewähltes Fahrzeug (kann null sein)

        public EventMember(string name)
        {
            // ID zuweisen und Zähler erhöhen
            Id = naechsteId;
            naechsteId = naechsteId ++;

            Name = name;
            
        }

        // Setzt das gewählte Fahrzeug für dieses Mitglied.
        // Validierungen (ob das Fahrzeug erlaubt ist) erfolgen in der Event-Logik.
        internal void WähleFahrzeug(Vehicles fahrzeug)
        {
            GewaehltesFahrzeug = fahrzeug;
        }
    }

}


