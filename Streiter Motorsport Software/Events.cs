using System;
using System.Collections.Generic;
using System.Text;

namespace Streiter_Motorsport_Software
{
    internal class Events
    {
        public string EventName { get; set; }
        public string Simulation { get; set; }
        public DateTime EventDate { get; set; } // Datum und Uhrzeit des events
        public TimeSpan Duration { get; set; } // time span für dauer des events
        public List<Vehicles> ParticipatingVehicles { get; set; } = new(); //lässt Fahrzeuge hinzufügen
        public Events(string eventName, string simulation, TimeSpan duration)
        {
            EventName = eventName;
            Simulation = simulation;
            Duration = duration;
        }

        public void ChangeDuration(TimeSpan newDuration) // ermöglicht nachträgliche änderung der event dauer
        {
            Duration = newDuration;
        }

        public void AddVehicle(Vehicles vehicle) // ermöglicht hinzufügen von fahrzeugen zum event
        {
            ParticipatingVehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicles vehicle)
        {
            // ermöglicht entfernen von fahrzeugen aus dem event
            ParticipatingVehicles.Remove(vehicle);
        }

        public void SetDateTime(DateTime eventDate) // ermöglicht setzen des datums und der uhrzeit des events
        {
            EventDate = eventDate;
        }
    }
}
