using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

    // Copilot Vibe coding

namespace Streiter_Motorsport_Software
{
    // Simple JSON persistence helper (LINQ-free).
    internal static class JsonPersistence
    {
        private static readonly string DataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StreiterMotorsport", "data"); // lokaler ordner festgelegt als AppData ordner
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        // DTOs
        private record UserDto(string Username, string Password, byte AccessLevel);
        private record VehicleClassDto(string Fahrzeugklasse, string Game);
        private record VehicleDto(string Fahrzeugname, string Fahrzeugklasse, string Game);
        private record EventMemberDto(int Id, string Name, string? GewaehltesFahrzeugName);
        private record EventDto(int Id, string Name, string? Simulation, int? Dauer, DateTime Datum, string? Strecke, List<string> AusgewaehlteFahrzeugklassen, List<int> AngemeldeteMitgliederIds);

        public static void SaveAll(UserManager userManager)
        {
            Directory.CreateDirectory(DataFolder);

            SaveUsers(userManager);
            SaveVehicleClasses();
            SaveVehicles();
            SaveEventMembers();
            SaveEvents();
        }

        public static UserManager LoadAll()
        {
            Directory.CreateDirectory(DataFolder);

            // Load vehicle classes and vehicles first because other objects reference them
            LoadVehicleClasses();
            LoadVehicles();

            // Users
            UserManager userManager = new UserManager();
            LoadUsers(userManager);

            // EventMembers
            LoadEventMembers();

            // Events (after members loaded)
            LoadEvents();

            return userManager;
        }

        private static string PathFor(string name) => Path.Combine(DataFolder, name);

        private static void SaveUsers(UserManager userManager)
        {
            try
            {
                var users = new List<UserDto>();
                var all = userManager.GetAllUsers();
                for (int i = 0; i < all.Count; i++)
                {
                    var u = all[i];
                    users.Add(new UserDto(u.Username, u.Password, u.AccessLevel));
                }

                var json = JsonSerializer.Serialize(users, JsonOptions);
                File.WriteAllText(PathFor("users.json"), json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Benutzer: {ex.Message}");
            }
        }

        private static void LoadUsers(UserManager userManager)
        {
            string path = PathFor("users.json");
            if (!File.Exists(path)) return;

            try
            {
                var raw = File.ReadAllText(path);
                var dtos = JsonSerializer.Deserialize<List<UserDto>>(raw);
                if (dtos != null)
                {
                    var users = new List<User>();
                    foreach (var d in dtos)
                    {
                        users.Add(new User(d.Username, d.Password, d.AccessLevel));
                    }
                    userManager.ReplaceUsers(users);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Benutzer: {ex.Message}");
            }
        }

        private static void SaveVehicleClasses()
        {
            try
            {
                var dtos = new List<VehicleClassDto>();
                var list = VehicleClasses.fahrzeugklassenliste;
                for (int i = 0; i < list.Count; i++)
                {
                    var vc = list[i];
                    dtos.Add(new VehicleClassDto(vc.Fahrzeugklasse, vc.Game));
                }

                var json = JsonSerializer.Serialize(dtos, JsonOptions);
                File.WriteAllText(PathFor("vehicleclasses.json"), json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Fahrzeugklassen: {ex.Message}");
            }
        }

        private static void LoadVehicleClasses()
        {
            string path = PathFor("vehicleclasses.json");
            if (!File.Exists(path)) return;

            try
            {
                var raw = File.ReadAllText(path);
                var dtos = JsonSerializer.Deserialize<List<VehicleClassDto>>(raw);
                if (dtos != null)
                {
                    VehicleClasses.fahrzeugklassenliste.Clear();
                    foreach (var d in dtos)
                    {
                        VehicleClasses.fahrzeugklassenliste.Add(new VehicleClasses(d.Fahrzeugklasse, d.Game));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Fahrzeugklassen: {ex.Message}");
            }
        }

        private static void SaveVehicles()
        {
            try
            {
                var dtos = new List<VehicleDto>();
                var list = Vehicles.fahrzeugeliste;
                for (int i = 0; i < list.Count; i++)
                {
                    var v = list[i];
                    dtos.Add(new VehicleDto(v.Fahrzeugname, v.Fahrzeugklasse, v.Game));
                }

                var json = JsonSerializer.Serialize(dtos, JsonOptions);
                File.WriteAllText(PathFor("vehicles.json"), json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Fahrzeuge: {ex.Message}");
            }
        }

        private static void LoadVehicles()
        {
            string path = PathFor("vehicles.json");
            if (!File.Exists(path)) return;

            try
            {
                var raw = File.ReadAllText(path);
                var dtos = JsonSerializer.Deserialize<List<VehicleDto>>(raw);
                if (dtos != null)
                {
                    Vehicles.fahrzeugeliste.Clear();
                    foreach (var d in dtos)
                    {
                        Vehicles.fahrzeugeliste.Add(new Vehicles(d.Fahrzeugname, d.Fahrzeugklasse, d.Game));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Fahrzeuge: {ex.Message}");
            }
        }

        private static void SaveEventMembers()
        {
            try
            {
                var dtos = new List<EventMemberDto>();
                var list = EventMember.Mitgliederliste;
                for (int i = 0; i < list.Count; i++)
                {
                    var m = list[i];
                    dtos.Add(new EventMemberDto(m.Id, m.Name, m.GewaehltesFahrzeug?.Fahrzeugname));
                }

                var json = JsonSerializer.Serialize(dtos, JsonOptions);
                File.WriteAllText(PathFor("eventmembers.json"), json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Event-Mitglieder: {ex.Message}");
            }
        }

        private static void LoadEventMembers()
        {
            string path = PathFor("eventmembers.json");
            if (!File.Exists(path)) return;

            try
            {
                var raw = File.ReadAllText(path);
                var dtos = JsonSerializer.Deserialize<List<EventMemberDto>>(raw);
                if (dtos != null)
                {
                    EventMember.Mitgliederliste.Clear();

                    foreach (var d in dtos)
                    {
                        var member = new EventMember(d.Id, d.Name);

                        if (!string.IsNullOrEmpty(d.GewaehltesFahrzeugName))
                        {
                            Vehicles? vehicle = null;
                            var vehicles = Vehicles.fahrzeugeliste;
                            for (int i = 0; i < vehicles.Count; i++)
                            {
                                if (vehicles[i].Fahrzeugname == d.GewaehltesFahrzeugName)
                                {
                                    vehicle = vehicles[i];
                                    break;
                                }
                            }

                            if (vehicle != null)
                            {
                                member.CarChoice(vehicle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Event-Mitglieder: {ex.Message}");
            }
        }

        private static void SaveEvents()
        {
            try
            {
                var dtos = new List<EventDto>();
                var events = EventManager.Events;
                for (int i = 0; i < events.Count; i++)
                {
                    var e = events[i];

                    var selectedClasses = new List<string>();
                    for (int j = 0; j < e.AusgewählteFahrzeugklassen.Count; j++)
                    {
                        selectedClasses.Add(e.AusgewählteFahrzeugklassen[j].Fahrzeugklasse);
                    }

                    var memberIds = new List<int>();
                    for (int j = 0; j < e.AngemeldeteMitglieder.Count; j++)
                    {
                        memberIds.Add(e.AngemeldeteMitglieder[j].Id);
                    }

                    dtos.Add(new EventDto(e.Id ?? 0, e.Name, e.Simulation, e.Dauer, e.Datum, e.Strecke, selectedClasses, memberIds));
                }

                var json = JsonSerializer.Serialize(dtos, JsonOptions);
                File.WriteAllText(PathFor("events.json"), json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Events: {ex.Message}");
            }
        }

        private static void LoadEvents()
        {
            string path = PathFor("events.json");
            if (!File.Exists(path)) return;

            try
            {
                var raw = File.ReadAllText(path);
                var dtos = JsonSerializer.Deserialize<List<EventDto>>(raw);
                if (dtos != null)
                {
                    EventManager.Events.Clear();

                    foreach (var d in dtos)
                    {
                        var ev = new Event(d.Id, d.Name, d.Simulation, d.Dauer, d.Datum, d.Strecke);

                        // restore selected vehicle classes by matching name & game if possible
                        foreach (var className in d.AusgewaehlteFahrzeugklassen)
                        {
                            VehicleClasses? vc = null;
                            var classes = VehicleClasses.fahrzeugklassenliste;
                            for (int i = 0; i < classes.Count; i++)
                            {
                                if (classes[i].Fahrzeugklasse == className && classes[i].Game == d.Simulation)
                                {
                                    vc = classes[i];
                                    break;
                                }
                            }

                            if (vc == null)
                            {
                                for (int i = 0; i < classes.Count; i++)
                                {
                                    if (classes[i].Fahrzeugklasse == className)
                                    {
                                        vc = classes[i];
                                        break;
                                    }
                                }
                            }

                            if (vc != null)
                            {
                                ev.AusgewählteFahrzeugklassen.Add(vc);
                            }
                        }

                        // restore member references by id
                        foreach (var memberId in d.AngemeldeteMitgliederIds)
                        {
                            EventMember? found = null;
                            var members = EventMember.Mitgliederliste;
                            for (int i = 0; i < members.Count; i++)
                            {
                                if (members[i].Id == memberId)
                                {
                                    found = members[i];
                                    break;
                                }
                            }

                            if (found != null)
                            {
                                ev.AngemeldeteMitglieder.Add(found);
                            }
                        }

                        EventManager.AddEvent(ev);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Events: {ex.Message}");
            }
        }
    }
}