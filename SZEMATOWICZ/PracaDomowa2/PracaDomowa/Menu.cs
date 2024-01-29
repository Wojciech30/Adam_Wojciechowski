using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowa
{
    internal class Menu
    {
        public static void Run()
        {
            MenuController menuController = new MenuController();
            menuController.AddOption("1. Dodaj do bazy", DodajDoBazy);
            menuController.AddOption("2. Wyswietl baze danych", WyswietlBaze);
            menuController.AddOption("3. Usun po id", UsunPoID);
            menuController.AddOption("4. Zakończ program", () => Environment.Exit(0));

            while (true)
            {
                menuController.DisplayMenu();
                int choice = menuController.GetChoice();

                if (menuController.IsValidChoice(choice))
                {
                    menuController.ExecuteOption(choice);
                    break;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                }
            }
        }

        public static void UsunPoID()
        {
            MenuController menuController = new MenuController();
            menuController.AddOption("Wybierz baze danych\n1. MySQL", PobierzIDMySQL);
            menuController.AddOption("2. SQLite", PobierzIDSQLITE);
            menuController.AddOption("3. Zakończ program", () => Environment.Exit(0));

            while (true)
            {
                menuController.DisplayMenu();
                int choice = menuController.GetChoice();

                if (menuController.IsValidChoice(choice))
                {
                    menuController.ExecuteOption(choice);
                    break;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                }
            }
        }

        public static void PobierzIDMySQL()
        {
            Console.WriteLine("Podaj ID:");
            string str_id = Console.ReadLine();
            if (int.TryParse(str_id, out int id))
            {
                string wybor = "";
                while (wybor != "car" && wybor != "song")
                {
                    Console.WriteLine("Z jakiej tabeli chcesz usunac? [Car lub Song]:");
                    wybor = Console.ReadLine().ToLower();
                }
                
                
                if (wybor == "car")
                {
                    RepositoryController.GetMySQLCarRepository().DeleteCar(id);
                    Console.WriteLine($"Pomyslnie usunieto dane MYSQL z tabeli Car z id {id}");
                }
                else
                {
                    RepositoryController.GetMySQLSongRepository().DeleteSong(id);
                    Console.WriteLine($"Pomyslnie usunieto dane MYSQL z tabeli Song z id {id}");
                }
            }
            else
            {
                Console.WriteLine("To nie jest liczba");
            }
        }

        public static void PobierzIDSQLITE()
        {
            Console.WriteLine("Podaj ID:");
            string str_id = Console.ReadLine().Substring(11);
            if (int.TryParse(str_id, out int id))
            {
                string wybor = "";
                while (wybor != "car" && wybor != "song")
                {
                    Console.WriteLine("Z jakiej tabeli chcesz usunac? [Car lub Song]:");
                    wybor = Console.ReadLine().ToLower();
                }


                if (wybor == "car")
                {
                    RepositoryController.GetSQLITECarRepository().DeleteCar(id);
                    Console.WriteLine($"Pomyslnie usunieto dane SQLITE z tabeli Car z id {id}");
                }
                else
                {
                    RepositoryController.GetSQLITESongRepository().DeleteSong(id);
                    Console.WriteLine($"Pomyslnie usunieto dane SQLITE z tabeli Song z id {id}");
                }
            }
            else
            {
                Console.WriteLine("To nie jest liczba");
            }
        }


        public static void WyswietlBaze()
        {
            MySQL.CarRepository mysqlCar = RepositoryController.GetMySQLCarRepository();
            MySQL.SongRepository mysqlSong = RepositoryController.GetMySQLSongRepository();
            SQLITE.CarRepository sqliteCar = RepositoryController.GetSQLITECarRepository();
            SQLITE.SongRepository sqliteSong = RepositoryController.GetSQLITESongRepository();

            Console.WriteLine("Baza danych MySQL:");
            ConsoleUtils.PrintCarTable(mysqlCar.ReadCars());
            ConsoleUtils.PrintSongTable(mysqlSong.ReadSongs());

            Console.WriteLine("Baza danych SQLITE:");
            ConsoleUtils.PrintCarTable(sqliteCar.ReadCars());
            ConsoleUtils.PrintSongTable(sqliteSong.ReadSongs());
        }

        public static void DodajDoBazy()
        {
            MenuController menuController = new MenuController();
            menuController.AddOption("Wybierz baze danych\n1. MySQL", DodajMYSQL);
            menuController.AddOption("2. SQLite", DodajSQLITE);
            menuController.AddOption("3. Zakończ program", () => Environment.Exit(0));

            while (true)
            {
                menuController.DisplayMenu();
                int choice = menuController.GetChoice();

                if (menuController.IsValidChoice(choice))
                {
                    menuController.ExecuteOption(choice);
                    break;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                }
            }
        }

        public static void DodajMYSQL()
        {
            Console.WriteLine("Jaki obiekt chcesz dodac? [Car lub Song]:");
            string wybor = Console.ReadLine().ToLower();
            if (wybor == "car")
            {
                Car car = GetCar();
                RepositoryController.GetMySQLCarRepository().CreateCar(car);
                Console.WriteLine("Pomyslnie dodano do bazy MYSQL nowy obiekt Car");
            }
            else if (wybor == "song")
            {
                Song song = GetSong();
                RepositoryController.GetMySQLSongRepository().CreateSong(song);
                Console.WriteLine("Pomyslnie dodano do bazy MYSQL nowy obiekt Song");
            }
            else
            {
                DodajMYSQL();
            }
        }

        public static void DodajSQLITE()
        {
            Console.WriteLine("Jaki obiekt chcesz dodac? [Car lub Song]:");
            string wybor = Console.ReadLine().ToLower();
            if (wybor == "car")
            {
                Car car = GetCar();
                RepositoryController.GetSQLITECarRepository().CreateCar(car);
                Console.WriteLine("Pomyslnie dodano do bazy SQLITE nowy obiekt Car");
            }
            else if (wybor == "song")
            {
                Song song = GetSong();
                RepositoryController.GetSQLITESongRepository().CreateSong(song);
                Console.WriteLine("Pomyslnie dodano do bazy SQLITE nowy obiekt Song");
            }
            else
            {
                DodajSQLITE();
            }
        }

        public static Car GetCar()
        {
            Console.WriteLine("Podaj marke:");
            string make = Console.ReadLine();

            Console.WriteLine("Podaj model:");
            string model = Console.ReadLine();

            Console.WriteLine("Podaj rok:");
            int year = int.Parse(Console.ReadLine());

            Console.WriteLine("Podaj kolor:");
            string color = Console.ReadLine();

            return new Car { Make = make, Model = model, Year = year, Color = color };
        }

        public static Song GetSong()
        {
            Console.WriteLine("Podaj tytul:");
            string title = Console.ReadLine();

            Console.WriteLine("Podaj artyste:");
            string artist = Console.ReadLine();

            Console.WriteLine("Podaj rok (liczba):");
            int year = int.Parse(Console.ReadLine());

            Console.WriteLine("Podaj czas w sekundach (liczba):");
            int durationInSec = int.Parse(Console.ReadLine());

            return new Song { Title = title, Artist = artist, Year = year, DurationInSeconds = durationInSec };
        }

    }
}
