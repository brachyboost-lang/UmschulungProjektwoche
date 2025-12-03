using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Streiter_Motorsport_Software
{
    // User zum Anmelden in der Software
    internal class User
    {
        public string Username { get; set; }
        public byte AccessLevel { get; set; } // 0 = admin, 1 = verwaltung, 2 = benutzer
        public string Password { get; set; }

        public User(string username, string password, byte accessLevel)
        {
            Username = username;
            Password = password;
            AccessLevel = accessLevel;
        }

        public bool Authenticate(string inputPassword)
        {
            return Password == inputPassword;
        }

        public void ChangePassword(string newPassword)
        {
            Password = newPassword;
        }
    }

    internal class UserManager
    {
        private readonly List<User> users = new List<User>();

        // Vordefinierte Benutzer
        public UserManager()
        {
            AddUser(new User("admin", "admin123", 0));
            AddUser(new User("verwaltung", "verwaltung123", 1));
            AddUser(new User("benutzer", "benutzer123", 2));
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        // Gibt den angemeldeten User zurück oder null bei Fehlschlag
        public User? Authenticate(string username, string password)
        {
            foreach (var u in users)
            {
                if (string.Equals(u.Username, username) //wenn string A und B gleich sind
                    && u.Authenticate(password)) //und das Passwort stimmt überein
                {
                    return u;   //dann gib den User zurück
                }
            }

            return null; //wenn kein User gefunden wurde, gib null zurück
        }
    }


    // Hilfsklasse für Benutzereingaben
    public static class GetUserInput
    {
        
        public static string GetUserInputStr()
        {
            return Console.ReadLine() ?? string.Empty;
        }
        public static int GetUserInputInt()
        {
            while (true)
            {
                string input = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(input, out int result))
                {
                    return result;
                }
                Console.Write("Ungültige Eingabe. Bitte eine Zahl eingeben: ");
            }
        }

        public static byte GetUserInputByte()
        {
            while (true)
            {
                string input = Console.ReadLine() ?? string.Empty;
                if (byte.TryParse(input, out byte result))
                {
                    return result;
                }
                Console.Write("Ungültige Eingabe. Bitte eine Zahl eingeben: ");
            }
        }

        public static TimeSpan GetUserInputTimeSpan()
        {
            while (true)
            {
                string input = Console.ReadLine() ?? string.Empty;
                if (TimeSpan.TryParse(input, out TimeSpan result))
                {
                    return result;
                }
                Console.Write("Ungültige Eingabe. Bitte eine Zeitspanne im Format hh:mm:ss eingeben: ");
            }
        }

        public static DateTime GetUserInputDateTime()
        {
            while (true)
            {
                string input = Console.ReadLine() ?? string.Empty;
                if (DateTime.TryParse(input, out DateTime result))
                {
                    return result;
                }
                Console.Write("Ungültige Eingabe. Bitte ein Datum und eine Uhrzeit im Format TT.MM.JJJJ hh:mm eingeben: ");
            }
        }

        public static char GetUserInputChar()
        {
            while (true)
            {
                string input = Console.ReadLine() ?? string.Empty; // string statt ConsoleKeyInfo umwandeln
                                                                   // (dann kann user nachdenken vor dem abschicken,
                                                                   // ansonsten geht es weiter sobald ein key gedrückt wurde
                if (input.Length == 1)
                {
                    return input[0];
                }
                Console.Write("Ungültige Eingabe. Bitte ein einzelnes Zeichen eingeben: ");
            }
        }

        public static string GetPasswordInput(bool mask = true)
        {
            // KI Snippet: Passwort Eingabe mit Maskierung
            var sb = new StringBuilder();   // StringBuilder zum Speichern der Eingabe
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter) // solange kein enter gedrückt wurde = eingabe fortsetzen
            {
                if (keyInfo.Key == ConsoleKey.Backspace) // wenn backspace gedrückt wurde
                {
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                        if (mask)
                        {
                            // entferne ein '*'
                            Console.Write("\b \b");     // bewegt den cursor zurück, überschreibt mit leerzeichen, bewegt cursor zurück
                        }
                    }
                }
                else if (!char.IsControl(keyInfo.KeyChar)) // wenn kein steuerzeichen gedrückt wurde (z.B. shift, strg, alt)
                {
                    sb.Append(keyInfo.KeyChar);
                    if (mask)
                    {
                        Console.Write('*');
                    }
                    // wenn mask == false: kein Echo -> Eingabe bleibt unsichtbar
                }
            }

            Console.WriteLine();
            return sb.ToString();
        }
    
    }

}
