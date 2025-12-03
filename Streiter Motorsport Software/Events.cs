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
}
