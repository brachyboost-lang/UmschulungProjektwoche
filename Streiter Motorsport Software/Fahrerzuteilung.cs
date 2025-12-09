using System;
using System.Collections.Generic;
using System.Text;

namespace Streiter_Motorsport_Software
{
    internal enum SimulationType // enum ist ein Datentyp, der eine Gruppe von Konstanten darstellt - er ist nützlich, um eine Variable zu erstellen, die nur einen von mehreren vordefinierten Werten annehmen kann.
    {
        Default,
        LMU,
        IRacing,
        ACC
    }

    internal sealed class DriverSelection
    {
        public string MemberId { get; init; }
        public string MemberName { get; init; }
        public string VehicleId { get; init; }

        public DriverSelection(string memberId, string memberName, string vehicleId)
        {
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            MemberName = memberName ?? string.Empty;
            VehicleId = vehicleId ?? throw new ArgumentNullException(nameof(vehicleId));
        }
    }

    internal sealed class Team
    {
        public string TeamId { get; init; }
        public string VehicleId { get; init; }
        public List<DriverSelection> Drivers { get; init; } = new List<DriverSelection>();
        public bool IsUnderstaffed { get; set; }
        public bool IsOverstaffed { get; set; }

        public Team(string teamId, string vehicleId)
        {
            TeamId = teamId;
            VehicleId = vehicleId;
        }
    }

    // Hilfsklasse zur Gruppierung
    internal sealed class VehicleGroup
    {
        public string VehicleId { get; init; }
        public List<DriverSelection> Drivers { get; init; } = new List<DriverSelection>();

        public VehicleGroup(string vehicleId)
        {
            VehicleId = vehicleId ?? string.Empty;
        }
    }

    internal static class Fahrerzuteilung
    {
        private const int MaxLmu = 6;
        private const int MaxAcc = 4;
        private const int MaxIRacing = 32;

        public static List<Team> GenerateTeams(IEnumerable<DriverSelection> selections, SimulationType simulation, int raceHours) // IEnumerable<DriverSelection> ist eine Schnittstelle, die es ermöglicht,
                                                                                                                                  // über eine Sammlung von DriverSelection-Objekten zu iterieren.
        {
            if (selections == null) throw new ArgumentNullException(nameof(selections));
            if (raceHours <= 0) throw new ArgumentOutOfRangeException(nameof(raceHours), "raceHours muss > 0 sein.");

            var limits = GetTeamSizeLimitsForRace(raceHours);
            int preferred = limits.preferred;
            int min = limits.min;
            int max = ApplySimulationLimit(limits.max, simulation);

            // VehicleGroup-Liste aufbauen
            var groups = new List<VehicleGroup>();
            foreach (var s in selections)
            {
                if (s == null) continue;
                string key = s.VehicleId ?? string.Empty;

                // Suche vorhandene Gruppe (case-insensitive)
                VehicleGroup found = null;
                for (int i = 0; i < groups.Count; i++)
                {
                    if (string.Equals(groups[i].VehicleId, key, StringComparison.OrdinalIgnoreCase))
                    {
                        found = groups[i];
                        break;
                    }
                }

                if (found == null)
                {
                    var g = new VehicleGroup(key);
                    g.Drivers.Add(s);
                    groups.Add(g);
                }
                else
                {
                    found.Drivers.Add(s);
                }
            }

            var result = new List<Team>();
            int globalTeamCounter = 1;

            // Für jede Gruppe Teams erzeugen
            for (int gi = 0; gi < groups.Count; gi++)
            {
                var group = groups[gi];
                var drivers = group.Drivers;
                if (drivers.Count == 0) continue;

                if (drivers.Count <= max)
                {
                    var team = new Team($"T{globalTeamCounter++}", group.VehicleId)
                    {
                        Drivers = new List<DriverSelection>(drivers),
                        IsUnderstaffed = drivers.Count < min,
                        IsOverstaffed = drivers.Count > max
                    };
                    result.Add(team);
                    continue;
                }

                // Zu viele Fahrer: in mehrere Teams aufteilen
                var remaining = new List<DriverSelection>(drivers);

                // Zuerst Teams mit preferred Größe erzeugen (aber nicht größer als max)
                while (remaining.Count >= preferred)
                {
                    int take = preferred;
                    if (take > max) take = max;

                    var teamDrivers = new List<DriverSelection>();
                    for (int t = 0; t < take && remaining.Count > 0; t++)
                    {
                        teamDrivers.Add(remaining[0]);
                        remaining.RemoveAt(0);
                    }

                    var team = new Team($"T{globalTeamCounter++}", group.VehicleId)
                    {
                        Drivers = teamDrivers,
                        IsUnderstaffed = teamDrivers.Count < min,
                        IsOverstaffed = teamDrivers.Count > max
                    };
                    result.Add(team);
                }

                if (remaining.Count > 0)
                {
                    // Sammle existierende Teams für dieses Fahrzeug 
                    var teamsForVehicle = new List<Team>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (string.Equals(result[i].VehicleId, group.VehicleId, StringComparison.OrdinalIgnoreCase))
                        {
                            teamsForVehicle.Add(result[i]);
                        }
                    }

                    if (remaining.Count < min && teamsForVehicle.Count > 0)
                    {
                        int idx = 0;
                        while (remaining.Count > 0)
                        {
                            // Prüfe, ob irgendein bestehendes Team noch Platz hat
                            bool anyHasSpace = false;
                            for (int i = 0; i < teamsForVehicle.Count; i++)
                            {
                                if (teamsForVehicle[i].Drivers.Count < max)
                                {
                                    anyHasSpace = true;
                                    break;
                                }
                            }

                            if (!anyHasSpace)
                            {
                                // Alle Teams voll -> neues unterbesetztes Team
                                var forced = new Team($"T{globalTeamCounter++}", group.VehicleId)
                                {
                                    Drivers = new List<DriverSelection> { remaining[0] },
                                    IsUnderstaffed = true,
                                    IsOverstaffed = false
                                };
                                remaining.RemoveAt(0);
                                result.Add(forced);
                                teamsForVehicle.Add(forced);
                                continue;
                            }

                            // Verteile einen Fahrer auf das nächste Team mit Platz
                            Team current = teamsForVehicle[idx % teamsForVehicle.Count];
                            if (current.Drivers.Count < max)
                            {
                                current.Drivers.Add(remaining[0]);
                                remaining.RemoveAt(0);
                            }
                            idx++;
                        }
                    }
                    else
                    {
                        // Erzeuge neue Team(s) aus verbleibenden Fahrern (jeweils bis max)
                        while (remaining.Count > 0)
                        {
                            int take = remaining.Count;
                            if (take > max) take = max;

                            var teamDrivers = new List<DriverSelection>();
                            for (int t = 0; t < take && remaining.Count > 0; t++)
                            {
                                teamDrivers.Add(remaining[0]);
                                remaining.RemoveAt(0);
                            }

                            var team = new Team($"T{globalTeamCounter++}", group.VehicleId)
                            {
                                Drivers = teamDrivers,
                                IsUnderstaffed = teamDrivers.Count < min,
                                IsOverstaffed = teamDrivers.Count > max
                            };
                            result.Add(team);
                        }
                    }
                }
            }

            return result;
        }

        private static (int preferred, int min, int max) GetTeamSizeLimitsForRace(int raceHours)
        {
            if (raceHours == 3) return (preferred: 2, min: 2, max: 4);
            if (raceHours == 6) return (preferred: 3, min: 2, max: 4);
            if (raceHours == 12) return (preferred: 4, min: 3, max: 5);
            if (raceHours == 24) return (preferred: 5, min: 4, max: 6);

            // Fallback
            int pref = Math.Min(3, Math.Max(1, raceHours / 4));
            int minimum = 1;
            int maximum = Math.Max(4, raceHours / 2);
            return (pref, minimum, maximum);
        }

        private static int ApplySimulationLimit(int baseMax, SimulationType simulation)
        {
            if (simulation == SimulationType.LMU) return Math.Min(baseMax, MaxLmu);
            if (simulation == SimulationType.ACC) return Math.Min(baseMax, MaxAcc);
            if (simulation == SimulationType.IRacing) return Math.Max(baseMax, MaxIRacing);
            return baseMax;
        }
    }
}
