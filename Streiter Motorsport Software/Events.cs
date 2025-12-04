using System;
using System.Collections.Generic;
using System.Text;

namespace Streiter_Motorsport_Software
{

    internal interface IRaceEvents
    {
        public void ChangeDuration(TimeSpan duration);
        public void AddVehicle(VehicleClasses classes);
        public void RemoveVehicle(VehicleClasses classes);
        public void SetDateTime(DateTime time);

    }
    internal class RaceEvents : IRaceEvents
    {
        public string EventName { get; set; }
        public string Simulation { get; set; }
        public DateTime EventDate { get; set; } // Datum und Uhrzeit des events
        public TimeSpan Duration { get; set; } // time span für dauer des events
        public List<VehicleClasses> ParticipatingVehicles { get; set; } = new(); //lässt Fahrzeugklasse hinzufügen
        public RaceEvents(string eventName, string simulation, TimeSpan duration)
        {
            EventName = eventName;
            Simulation = simulation;
            Duration = duration;
        }

        public void ChangeDuration(TimeSpan newDuration) // ermöglicht nachträgliche änderung der event dauer
        {
            Duration = newDuration;
        }

        public void AddVehicle(VehicleClasses vehicle) // ermöglicht hinzufügen von fahrzeugen zum event
        {
            ParticipatingVehicles.Add(vehicle);
        }

        public void RemoveVehicle(VehicleClasses vehicle)
        {
            // ermöglicht entfernen von fahrzeugen aus dem event
            ParticipatingVehicles.Remove(vehicle);
        }

        public void SetDateTime(DateTime eventDate) // ermöglicht setzen des datums und der uhrzeit des events
        {
            EventDate = eventDate;
        }
    }
    internal class RaceEventManager
    {
        internal readonly List<RaceEvents> raceEvents = new();
        public void AddRaceEvent(RaceEvents raceEvent)
        {
            raceEvents.Add(raceEvent);
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
            naechsteId = naechsteId + 1;

            Name = name;
        }

        // Setzt das gewählte Fahrzeug für dieses Mitglied.
        // Validierungen (ob das Fahrzeug erlaubt ist) erfolgen in der Event-Logik.
        internal void WähleFahrzeug(Vehicles fahrzeug)
        {
            GewaehltesFahrzeug = fahrzeug;
        }
    }

    // Event: einfache Darstellung einer Veranstaltung.
    // Verwendet nur einfache Konstrukte: int-IDs, List<T>, foreach-Schleifen.
    internal class Event
    {
        private static int naechsteId = 1; // Zähler für Event-IDs

        internal int Id { get; private set; }                // Einfache int-ID
        internal string Name { get; set; }                   // Name des Events
        internal string Simulation { get; set; }             // Zugehörige Simulation

        // Vorschläge: Fahrzeugklassen, die zur Simulation passen.
        internal List<VehicleClasses> VorgeschlageneFahrzeugklassen { get; private set; }

        // Vom Veranstalter ausgewählte Fahrzeugklassen für dieses Event.
        internal List<VehicleClasses> AusgewählteFahrzeugklassen { get; private set; }

        // Angemeldete Mitglieder für das Event.
        internal List<EventMember> Mitglieder { get; private set; }

        public Event(string name, string simulation)
        {
            // ID zuweisen
            Id = naechsteId;
            naechsteId = naechsteId + 1;

            Name = name;
            Simulation = simulation;

            VorgeschlageneFahrzeugklassen = new List<VehicleClasses>();
            AusgewählteFahrzeugklassen = new List<VehicleClasses>();
            Mitglieder = new List<EventMember>();

            // Fülle die Vorschlagsliste ohne LINQ, mit einfacher Schleife.
            // Wir durchsuchen die zentrale fahrzeugklassenliste und fügen passende Einträge hinzu.
            foreach (VehicleClasses vc in VehicleClasses.fahrzeugklassenliste)
            {
                // String-Vergleich: in C# kann man == verwenden, es vergleicht den Inhalt von strings.
                if (vc.Simulation == simulation)
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
                if (v.Simulation == klasse.Simulation && v.Fahrzeugklasse == klasse.Fahrzeugklasse)
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
                if (v.Simulation == klasse.Simulation && v.Fahrzeugklasse == klasse.Fahrzeugklasse)
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
        // Rückgabe als List<Vehicles>.
        internal List<Vehicles> VerfügbareFahrzeugeFürAusgewählteKlassen()
        {
            List<Vehicles> ergebnis = new List<Vehicles>();

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
            EventMember m = new EventMember(name);
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
        internal static List<Event> Events { get; private set; } = new List<Event>();

        // Erstellt ein Event und speichert es in der Liste.
        internal static Event ErzeugeEvent(string name, string simulation)
        {
            Event ev = new Event(name, simulation);
            Events.Add(ev);
            return ev;
        }

        // Liefert eine Liste von Fahrzeugklassen für die angegebene Simulation.
        // Keine LINQ; wir verwenden einfache Schleifen.
        internal static List<VehicleClasses> SchlageFahrzeugklassenVor(string simulation)
        {
            List<VehicleClasses> ergebnis = new List<VehicleClasses>();
            foreach (VehicleClasses vc in VehicleClasses.fahrzeugklassenliste)
            {
                if (vc.Simulation == simulation)
                {
                    ergebnis.Add(vc);
                }
            }
            return ergebnis;
        }

        // Liefert Fahrzeuge für eine Simulation und Fahrzeugklasse.
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
}


