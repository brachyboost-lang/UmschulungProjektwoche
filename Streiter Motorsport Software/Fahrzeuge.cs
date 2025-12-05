using System;
using System.Collections.Generic;
using System.Text;
//iRacing Fahrzeugkategorien: GT3 

//iRacing GT3 Fahrzeuge: Audi R8 LMS Evo II GT3, BMW M4 GT3, Chevrolet Corvette Z06 GT3.R, Ferrari 296 GT3, Ford Mustang GT3, Lamborghini Huracán GT3 EVO, McLaren 720S GT3 EVO, Mercedes-AMG GT3 2020, Porsche 911 GT3 R (992), Acura NSX GT3 EVO 22 



//LMU Fahrzeugkategorien: Hypercar, LMP2, GT3, GTE 

//LMU GT3 Fahrzeuge: Aston Martin Vantage AMR LMGT3 Evo, BMW M4 LMGT3, BMW M4 LMGT3 Evo, Chevrolet Corvette Z06 LMGT3.R, Ferrari 296 LMGT3, Ford Mustang LMGT3, Lamborghini Huracán LMGT3 Evo2, Lexus RC F LMGT3, McLaren 720S LMGT3 Evo, Porsche 911 LMGT3 R (992) 

//LMU LMP2 Fahrzeuge: Oreca 07 Gibson (WEC LMP2), Oreca 07 Gibson (ELMS LMP2) 

//LMU Hypercar Fahrzeuge: Alpine A424, Aston Martin Valkyrie AMR LMH, BMW M Hybrid V8, Cadillac V-Series.R, Ferrari 499P, Glickenhaus SCG 007 LMH, Isotta Fraschini Tipo 6-C, Lamborghini SC63, Peugeot 9X8 (2023), Peugeot 9X8 2024, Porsche 963, Toyota GR010-Hybrid, Vanwall Vandervell 680 

//LMU GTE Fahrzeuge: Aston Martin Vantage GTE, Chevrolet Corvette C8.R, Ferrari 488 GTE Evo, Porsche 911 RSR-19 



//ACC Fahrzeugkategorien: GT3, GT4, M2 

//ACC GT3 Fahrzeuge: Aston Martin V12 Vantage GT3, Aston Martin V8 Vantage GT3, Audi R8 LMS GT3, Audi R8 LMS Evo GT3, Audi R8 LMS Evo II GT3, Bentley Continental GT3 (diverse Varianten), BMW M6 GT3, BMW M4 GT3, Emil Frey Jaguar GT3, Ferrari 488 GT3, Ferrari 488 EVO GT3, Ferrari 296 GT3, Honda NSX GT3, Honda NSX GT3 Evo, Lamborghini Huracán GT3, Lamborghini Huracán Evo GT3, Lamborghini Huracán Evo2 GT3, Lexus RC F GT3, McLaren 650S GT3, McLaren 720S GT3, McLaren 720S GT3 Evo, Mercedes-AMG GT3, Mercedes-AMG GT3 Evo, Nissan GT-R Nismo GT3 (diverse Baujahre), Porsche 911 GT3 R (2018 / 2019), Porsche 911 GT3 R (992), Reiter Engineering R-EX GT3 



namespace Streiter_Motorsport_Software
{
    internal class Simulation
    {
        internal string Game { get; set; }    // z.B. "iRacing", "LMU", "ACC"

        public Simulation(string game)
        {
            Game = game;
        }
    }

    internal class VehicleClasses : Simulation
    {
        // Sichtbarkeit internal, damit andere Klassen im Projekt darauf zugreifen können.
        internal string Fahrzeugklasse { get; set; } // z.B. "GT3", "LMP2"

        // Zentrale Liste aller Fahrzeugklassen.
        // List<T> ist eine Standard-Sammlung aus dem Framework, die Elemente speichert.
        // Methoden: Add fügt ein Element hinzu, Count liefert die Anzahl, foreach zum Durchlaufen.
        internal static List<VehicleClasses> fahrzeugklassenliste = new List<VehicleClasses>()
        {
            new VehicleClasses("iRacing", "GT3"),
            new VehicleClasses("LMU", "LMP2"),
            new VehicleClasses("ACC", "GT3"),
        };

        public VehicleClasses(string fahrzeugklasse, string game) : base(game)
        {
            Fahrzeugklasse = fahrzeugklasse;
            Game = game;
        }
    }

    internal class Vehicles
    {
        internal string Simulation { get; set; }      // Zugehörige Simulation
        internal string Fahrzeugklasse { get; set; }  // Zugehörige Fahrzeugklasse
        internal string Fahrzeugname { get; set; }   // Name des Fahrzeugs

        // Zentrale Fahrzeugliste.
        internal static List<Vehicles> fahrzeugeliste = new List<Vehicles>()
        {
            new Vehicles("iRacing", "GT3", "Audi R8 LMS Evo II GT3"),
            new Vehicles("LMU", "LMP2", "Oreca 07 Gibson (WEC LMP2)"),
            new Vehicles("ACC", "GT3", "Aston Martin V12 Vantage GT3"),
        };

        public Vehicles(string simulation, string fahrzeugklasse, string fahrzeugname)
        {
            Fahrzeugklasse = fahrzeugklasse;
            Fahrzeugname = fahrzeugname;
            Simulation = simulation;
        }
    }
}
    

