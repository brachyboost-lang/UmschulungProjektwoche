using System;
using System.Collections.Generic;

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
}
