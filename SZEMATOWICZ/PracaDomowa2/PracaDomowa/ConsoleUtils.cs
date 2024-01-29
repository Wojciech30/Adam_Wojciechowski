using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowa
{
    internal class ConsoleUtils
    {
        public static void PrintCarTable(IEnumerable<Car> cars)
        {
            Console.WriteLine("Make | Model | Year | Color");
            foreach (Car car in cars)
            {
                Console.WriteLine($"{car.Make} | {car.Model} | {car.Year} | {car.Color}");
            }
            Console.WriteLine();
        }
        public static void PrintSongTable(IEnumerable<Song> songs)
        {
            Console.WriteLine("Title | Artist | Year | DurationInSeconds");
            foreach (Song song in songs)
            {
                Console.WriteLine($"{song.Title} | {song.Artist} | {song.Year} | {song.DurationInSeconds}");
            }
            Console.WriteLine();
        }



    }
}
