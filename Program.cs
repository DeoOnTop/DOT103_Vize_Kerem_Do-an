using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Channels;
class HelloWorld
{
    static void Main()
    {
        int Width = 30, Height = 15;

        int Horizontal = 15;
        int Vertical = 7;

        string Direction = "I";

        int PlayerHP = 100;
        int PlayerLevel = 1;
        int HealthDrink = 1;

        int MonsterHP = 100;

        bool GameStart = false;
        bool InCombat = false;
        bool CombatEnabled = true;

        Random Random = new Random();

        Console.WriteLine("Oyuna hoş geldin!!!!! Kahramanınıa bir isim ver:::::");
        string? PlayerName = Console.ReadLine();
        Console.WriteLine($"Hey {PlayerName}!! Oyuna başlamak için herhangi bir tuşa bas!");
        Console.ReadKey(true);
        Console.WriteLine();
        GameStart = true;


        while (GameStart == true)
        {

            while (GameStart == true && InCombat == false)
            {
                int W = Width + 2, H = Height + 2;

                for (int y = 0; y < H; y++)
                {
                    for (int x = 0; x < W; x++)
                    {
                        if (y == 0 || y == H - 1 || x == 0 || x == W - 1)
                            Console.Write("# ");

                        else if (x == Horizontal && y == Vertical)
                            Console.Write($"{Direction} ");

                        //else if (Random.NextDouble() < 0.15) // ~20% grass density
                        //	Console.Write(", ");

                        else
                            Console.Write("  ");
                    }
                    Console.WriteLine();
                }


                //Console.WriteLine($"X: {Vertical}, Y: {Horizontal}");

                int NewHorizontal = Horizontal;
                int NewVertical = Vertical;

                ConsoleKey key = Console.ReadKey(true).Key;

                if (Vertical == 1 && key == ConsoleKey.W) { NewVertical = Height; Direction = "^"; }
                else if (key == ConsoleKey.W) { NewVertical--; Direction = "^"; }
                else if (Vertical == Height && key == ConsoleKey.S) { NewVertical = 1; Direction = "v"; }
                else if (key == ConsoleKey.S) { NewVertical++; Direction = "v"; }
                if (Horizontal == 1 && key == ConsoleKey.A) { NewHorizontal = Width; Direction = "<"; }
                else if (key == ConsoleKey.A) { NewHorizontal--; Direction = "<"; }
                if (Horizontal == Width && key == ConsoleKey.D) { NewHorizontal = 1; Direction = ">"; }
                else if (key == ConsoleKey.D) { NewHorizontal++; Direction = ">"; }

                if (NewHorizontal > 0 && NewHorizontal < W - 1 && NewVertical > 0 && NewVertical < H - 1)
                {
                    Horizontal = NewHorizontal;
                    Vertical = NewVertical;
                    Console.WriteLine();
                    Console.Clear();
                }

                int MonsterChance = Random.Next(1, 101);
                if (CombatEnabled == true && InCombat == false && MonsterChance > 95)
                    InCombat = true;
            }

            if (CombatEnabled == true && InCombat == true)
            {
                string Monster = Random.Next(1, 4) switch
                {
                    1 => "Frog",
                    2 => "Slime",
                    3 => "Ghost",
                    4 => "Phoenix",
                    5 => "Kitsune",
                    _ => ""
                };

                Console.WriteLine($"                   Karşına bir {Monster} çıktı!!!!!!!");
                Console.WriteLine();

                string Option;
                bool Escape = false;
                do
                {
                    do
                    {
                        Console.WriteLine($"                           {Monster} HP: {MonsterHP}");
                        Console.WriteLine($"                Level: {PlayerLevel} | HP: {PlayerHP} / 100 | İksir: {HealthDrink}");
                        Console.WriteLine($"              |1. Saldır|     |2. İksir|     |3. Kaç|");
                        Console.WriteLine();
                        int.TryParse(Console.ReadLine(), out int Sayı);
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();

                        Option = Sayı switch
                        {
                            1 => "Saldır",
                            2 => "İksir",
                            3 => "Kaç",
                            _ => "Default"
                        };
                    }
                    while (Option == "Default");

                    if (Option == "Saldır")
                    {
                        int LevelAttack = 2 * PlayerLevel;
                        int LevelAttackCeiling = PlayerLevel * 25;
                        int Critic;
                        int MonsterCritic;
                        int PlayerAttackChance = Random.Next(LevelAttack, LevelAttackCeiling);
                        int MonsterAttackChance = Random.Next(3, 19);
                        if (PlayerAttackChance >= 18)
                        {
                            Critic = Random.Next(25, 31);
                            Console.WriteLine($"                      Kritik hasarrr {Critic} vurdun");
                            MonsterHP -= Critic;
                        }
                        else if (PlayerAttackChance < 18)
                        {
                            Console.WriteLine($"                     Normal hasar {PlayerAttackChance} vurdun");
                            MonsterHP -= PlayerAttackChance;
                        }

                        if (MonsterAttackChance >= 16)
                        {
                            MonsterCritic = Random.Next(20, 26);
                            PlayerHP -= MonsterCritic;
                            Console.WriteLine($"                  Obaaa {Monster} sana kritik {MonsterCritic} vurdu");
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                        else if (MonsterAttackChance <= 15)
                        {
                            PlayerHP -= MonsterAttackChance;
                            Console.WriteLine($"                          {Monster} {MonsterAttackChance} vurdu");
                            Console.WriteLine();
                            Console.WriteLine();
                        }

                        if (MonsterHP <= 0)
                        {
                            MonsterHP = 0;
                            Console.WriteLine($"           {Monster} canavarını yendin!!!!!! Seviyen 1 arttı!!!");
                            Console.WriteLine();
                            Console.WriteLine();
                            PlayerLevel++;

                            int HealthDrinkDropAmount = Random.Next(1, 4);
                            Console.WriteLine(HealthDrinkDropAmount);
                            int HealthDrinkDropChance = Random.Next(1, 11);
                            if (HealthDrinkDropChance < 7) { Console.WriteLine($"                     {HealthDrinkDropAmount} can iksiri kazandın!!!"); HealthDrink += HealthDrinkDropAmount;}
                            else if (HealthDrinkDropChance > 7) { Console.WriteLine("                    Hiçbir şey droplamadı!!!!"); }

                            InCombat = false;
                            Escape = true;
                            MonsterHP = 100;
                        }

                        if (PlayerHP <= 0)
                        {
                            PlayerHP = 0;
                            GameStart = false;
                            InCombat = false;
                            Escape = true;
                            Console.WriteLine("                         Looooser öldün");
                            Console.WriteLine($"            Son statların: Level: {PlayerLevel} | Can: {PlayerHP} | İksir: {HealthDrink}");
                        }
                    }
                    else if (Option == "İksir")
                    {
                        if (HealthDrink > 0 && PlayerHP <= 75)
                        {
                            PlayerHP += 25;
                            HealthDrink--;
                        }
                        else if (PlayerHP != 100 && PlayerHP >= 75)
                        {
                            PlayerHP = 100;
                            HealthDrink--;
                        }

                        else if (PlayerHP == 100 && HealthDrink > 0)
                            Console.WriteLine("Can zaten dolu");

                        else if (HealthDrink < 0)
                            Console.WriteLine("İksir Yok");
                    }

                    else if (Option == "Kaç")
                    {
                        int EscapeChances = Random.Next(1, 11);
                        if (EscapeChances > 9)
                        {
                            Escape = true; InCombat = false;
                            Console.WriteLine("                            Kaçtın!!!!");
                            Console.WriteLine();
                        }
                        else if (EscapeChances < 8)
                        {
                            Escape = false;
                            Console.WriteLine("                          Kaçamadın!!!!");
                            Console.WriteLine();
                        }
                    }
                }
                while (InCombat == true || Escape == false);
            }
        }
    }
}