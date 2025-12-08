using System;
using System.Collections.Generic;
using System.Text;

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
        internal static List<VehicleClasses> fahrzeugklassenliste = new()
        {
            new VehicleClasses("TCR", "iRacing"),
            new VehicleClasses("GT3", "iRacing"),
            new VehicleClasses("LMP2", "iRacing"),
            new VehicleClasses("GTP", "iRacing"),
            new VehicleClasses("LMP1", "iRacing"),
            new VehicleClasses("GT4", "iRacing"),
            new VehicleClasses("Cup", "iRacing"),
            new VehicleClasses("LMP2", "LMU"),
            new VehicleClasses("Hypercar", "LMU"),
            new VehicleClasses("GTE", "LMU"),
            new VehicleClasses("GT3", "LMU"),
            new VehicleClasses("LMP3", "LMU"),
            new VehicleClasses("GT3", "ACC"),
            new VehicleClasses("GT4", "ACC"),
            new VehicleClasses("M2", "ACC"),
            new VehicleClasses("ST/Challenge", "ACC"),
            new VehicleClasses("Cup", "ACC"),
            new VehicleClasses("GT2", "ACC"),
        };

        public VehicleClasses(string fahrzeugklasse, string game) : base(game)
        {
            Fahrzeugklasse = fahrzeugklasse;
            Game = game;
        }
    }

    internal class Vehicles : VehicleClasses
    {

        internal string Fahrzeugname { get; set; }   // Name des Fahrzeugs

        // Zentrale Fahrzeugliste.
        internal static List<Vehicles> fahrzeugeliste = new()
        {
                // ===========================
                //        iRACING
                // ===========================

                // --- NEC / TCR 2025 ---
                new Vehicles("Audi RS3 LMS TCR", "TCR", "iRacing"),
                new Vehicles("Hyundai Elantra N TCR", "TCR", "iRacing"),
                new Vehicles("Honda Civic Type R TCR", "TCR", "iRacing"),

            
                // --- GT3 ---
                new Vehicles("Acura NSX GT3 Evo 22", "GT3", "iRacing"),
                new Vehicles("Audi R8 LMS GT3", "GT3", "iRacing"),
                new Vehicles("Audi R8 LMS Evo II GT3", "GT3", "iRacing"),
                new Vehicles("BMW M4 GT3 Evo", "GT3", "iRacing"),
                new Vehicles("BMW Z4 GT3", "GT3", "iRacing"),
                new Vehicles("Mercedes AMG GT3", "GT3", "iRacing"),
                new Vehicles("Mercedes AMG GT3 2020", "GT3", "iRacing"),
                new Vehicles("Ferrari 296 GT3", "GT3", "iRacing"),
                new Vehicles("Ferrari 488 GT3", "GT3", "iRacing"),
                new Vehicles("Ferrari 488 GT3 Evo 2020", "GT3", "iRacing"),
                new Vehicles("Lamborghini Huracán GT3 EVO", "GT3", "iRacing"),
                new Vehicles("Chevrolet Corvette Z06 GT3.R", "GT3", "iRacing"),
                new Vehicles("McLaren 720S GT3", "GT3", "iRacing"),
                new Vehicles("McLaren MP4-12C GT3", "GT3", "iRacing"),
                new Vehicles("Ford Mustang GT3", "GT3", "iRacing"),
                new Vehicles("Porsche 911 GT3 R", "GT3", "iRacing"),
                new Vehicles("Porsche 911 GT3 R (992)", "GT3", "iRacing"),
                new Vehicles("Aston Martin Vantage GT3", "GT3", "iRacing"),

            
                // --- GT4 ---
                new Vehicles("Aston Martin Vantage GT4", "GT4", "iRacing"),
                new Vehicles("BMW M4 F82 GT4", "GT4", "iRacing"),
                new Vehicles("BMW M4 G82 GT4", "GT4", "iRacing"),
                new Vehicles("Porsche 718 Cayman GT4 Clubsport", "GT4", "iRacing"),
                new Vehicles("McLaren 570S GT4", "GT4", "iRacing"),
                new Vehicles("Mercedes-AMG GT4", "GT4", "iRacing"),
                new Vehicles("Ford Mustang GT4", "GT4", "iRacing"),
                
                // --- M2 ---
                new Vehicles("BMW M2 CS Racing", "M2", "iRacing"),
            
                // --- LMP2 ---
                new Vehicles("Dallara P217 LMP2", "LMP2", "iRacing"),

                // --- GTP ---
                new Vehicles("Porsche 963 GTP", "GTP", "iRacing"),
                new Vehicles("BMW M Hybrid V8", "GTP", "iRacing"),
                new Vehicles("Cadillac V-Series.R GTP", "GTP", "iRacing"),
                new Vehicles("Acura ARX-06 GTP", "GTP", "iRacing"),
                new Vehicles("Ferrari 499P", "GTP", "iRacing"),


                // --- Cup ---
                new Vehicles("Porsche 911 GT3 Cup", "Cup", "iRacing"),
                new Vehicles("Porsche 911 GT3 Cup (992)", "Cup", "iRacing"),
            
            
                // ===========================
                //          LMU
                // ===========================
            
                // --- LMGT3 (GT3) ---
                new Vehicles("Ford Mustang LMGT3", "GT3", "LMU"),
                new Vehicles("McLaren 720S LMGT3 Evo", "GT3", "LMU"),
                new Vehicles("Mercedes AMG LMGT3 Evo", "GT3", "LMU"),
                new Vehicles("BMW M4 LMGT3", "GT3", "LMU"),
                new Vehicles("Aston Martin Vantage LMGT3", "GT3", "LMU"),
                new Vehicles("Chevrolet Corvette Z06 GT3.R", "GT3", "LMU"),
                new Vehicles("Ferrari 296 LMGT3", "GT3", "LMU"),
                new Vehicles("Lexus RC F LMGT3", "GT3", "LMU"),
                new Vehicles("Porsche 911 GT3 R LMGT3", "GT3", "LMU"),
                new Vehicles("Lamborghini Huracán LMGT3 Evo2", "GT3", "LMU"),
            
                // --- LMP2 ---
                new Vehicles("Oreca 07 Gibson", "LMP2", "LMU"),
            
                // --- LMP3 ---
                new Vehicles("Ligier JS P325", "LMP3", "LMU"),

                // --- Hypercar ---
                new Vehicles("Alpine A424", "Hypercar", "LMU"),
                new Vehicles("Aston Martin Valkyrie AMR-LMH", "Hypercar", "LMU"),
                new Vehicles("BMW M Hybrid V8", "Hypercar", "LMU"),
                new Vehicles("Cadillac V-Series.R", "Hypercar", "LMU"),
                new Vehicles("Ferrari 499P", "Hypercar", "LMU"),
                new Vehicles("Glickenhaus SCG 007", "Hypercar", "LMU"),
                new Vehicles("Isotta Fraschini Tipo 6", "Hypercar", "LMU"),
                new Vehicles("Lamborghini SC63", "Hypercar", "LMU"),
                new Vehicles("Peugeot 9X8", "Hypercar", "LMU"),
                new Vehicles("Peugeot 9X8 2024", "Hypercar", "LMU"),
                new Vehicles("Porsche 963", "Hypercar", "LMU"),
                new Vehicles("Toyota GR010-Hybrid", "Hypercar", "LMU"),
                new Vehicles("Vanwall Vandervell 680", "Hypercar", "LMU"),


                
                // --- GTE ---
                new Vehicles("Aston Martin Vantage AMR", "GTE", "LMU"),
                new Vehicles("Chevrolet Corvette C8.R", "GTE", "LMU"),
                new Vehicles("Ferrari 488 GTE Evo", "GTE", "LMU"),
                new Vehicles("Porsche 911 RSR-19", "GTE", "LMU"),
            
            
                // ===========================
                //           ACC
                // ===========================
            
                // --- GT3 ---
                new Vehicles("Aston Martin V8 Vantage GT3 2019", "GT3", "ACC"),
                new Vehicles("Aston Martin V12 Vantage GT3 2013", "GT3", "ACC"),
                new Vehicles("Audi R8 LMS GT3 2015", "GT3", "ACC"),
                new Vehicles("Audi R8 LMS Evo GT3 2019", "GT3", "ACC"),
                new Vehicles("Audi R8 LMS Evo II GT3 2022", "GT3", "ACC"),
                new Vehicles("Bentley Continental GT3 2015", "GT3", "ACC"),
                new Vehicles("Bentley Continental GT3 2018", "GT3", "ACC"),
                new Vehicles("BMW M4 GT3", "GT3", "ACC"),
                new Vehicles("BMW M6 GT3", "GT3", "ACC"),
                new Vehicles("Emil Frey Jaguar GT3", "GT3", "ACC"),
                new Vehicles("Ferrari 488 GT3 2018", "GT3", "ACC"),
                new Vehicles("Ferrari 488 GT3 Evo 2020", "GT3", "ACC"),
                new Vehicles("Ferrari 296 GT3 2023", "GT3", "ACC"),
                new Vehicles("Honda NSX GT3 2017", "GT3", "ACC"),
                new Vehicles("Honda NSX GT3 Evo 2019", "GT3", "ACC"),
                new Vehicles("Lamborghini Huracán GT3 2015", "GT3", "ACC"),
                new Vehicles("Lamborghini Huracán GT3 Evo 2019", "GT3", "ACC"),
                new Vehicles("Lamborghini Huracán GT3 Evo II 2023", "GT3", "ACC"),
                new Vehicles("Lexus RC F GT3", "GT3", "ACC"),
                new Vehicles("McLaren 650S GT3 2015", "GT3", "ACC"),
                new Vehicles("McLaren 720S GT3 2019", "GT3", "ACC"),
                new Vehicles("McLaren 720S GT3 Evo 2023", "GT3", "ACC"),
                new Vehicles("Mercedes AMG GT3 2015", "GT3", "ACC"),
                new Vehicles("Mercedes AMG GT3 Evo 2020", "GT3", "ACC"),
                new Vehicles("Nissan GT-R Nismo GT3 2015", "GT3", "ACC"),
                new Vehicles("Nissan GT-R Nismo GT3 2018", "GT3", "ACC"),
                new Vehicles("Porsche 911 GT3 R 2018", "GT3", "ACC"),
                new Vehicles("Porsche 911 II GT3 R 2019", "GT3", "ACC"),
                new Vehicles("Porsche 992 GT3 R 2023", "GT3", "ACC"),
                new Vehicles("Reiter EngineeringR-EX GT3", "GT3", "ACC"),
            
                // --- GT4 ---
                new Vehicles("Alpine A110 GT4", "GT4", "ACC"),
                new Vehicles("Aston Martin Vantage GT4", "GT4", "ACC"),
                new Vehicles("Audi R8 LMS GT4", "GT4", "ACC"),
                new Vehicles("BMW M4 GT4", "GT4", "ACC"),
                new Vehicles("Chevrolet Camaro GT4.R", "GT4", "ACC"),
                new Vehicles("Ginetta G55 GT4", "GT4", "ACC"),
                new Vehicles("KTM X-Bow GT4", "GT4", "ACC"),
                new Vehicles("Maserati GranTurismo MC GT4", "GT4", "ACC"),
                new Vehicles("McLaren 570S GT4", "GT4", "ACC"),
                new Vehicles("Mercedes AMG GT4", "GT4", "ACC"),
                new Vehicles("Porsche 718 Cayman GT4 Clubsport", "GT4", "ACC"),

                // --- GT2 ---
                new Vehicles("Audi R8 LMS GT2", "GT2", "ACC"),
                new Vehicles("KTM X-Bow GT2", "GT2", "ACC"),
                new Vehicles("Maserati MC20 GT2", "GT2", "ACC"),
                new Vehicles("Mercedes AMG GT2", "GT2", "ACC"),
                new Vehicles("Porsche 991 II GT2 RS CS Evo", "GT2", "ACC"),
                new Vehicles("Porsche 935 GT2", "GT2", "ACC"),
                

                // --- BMW M2 ---
                new Vehicles("BMW M2 CS Racing", "M2", "ACC"),
            
                // --- ST (Super Trofeo) ---
                new Vehicles("Lamborghini Huracán Super Trofeo 2015", "ST/Challenge", "ACC"),
                new Vehicles("Lamborghini Huracán Super Trofeo EVO2 2021", "ST/Challenge", "ACC"),
            
                // --- Ferrari Challenge ---
                new Vehicles("Ferrari 488 Challenge Evo", "ST/Challenge", "ACC"),
            
                // --- Porsche Cup ---
                new Vehicles("Porsche 911 II GT3 Cup 2017", "Cup", "ACC"),
                new Vehicles("Porsche 911 GT3 Cup (992)", "Cup", "ACC"),
        };

        public Vehicles(string fahrzeugname, string fahrzeugklasse, string game) : base(fahrzeugklasse, game)
        {
            Fahrzeugname = fahrzeugname;
        }
    }
}


