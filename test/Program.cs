using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleConsoleMenu;

namespace test
{
    class Program
    {
        static void Item1Action()
        {
            Console.WriteLine("Item 1 Action");
            Console.ReadKey();
        }

        static void Item2Action()
        {
            Console.WriteLine("Item 2 Action");
            Console.ReadKey();
        }

        static void Exit()
        {
            run = false;
        }
        static bool run = true;

        static void Main(string[] args)
        {

            ConsoleMenu.AddCommand(new ConsoleMenu.Command("Action 1", () =>
            {
                Console.WriteLine("Action 1 Called");
                Console.ReadKey();
            }));
            ConsoleMenu.AddCommand(new ConsoleMenu.Command("You may prefer action 2", () =>
            {
                Console.WriteLine("Action 2 Called");
                Console.ReadKey();
            }));
            ConsoleMenu.AddCommand(new ConsoleMenu.Command("Exit", () =>
            {
                run = false;
            }));

            while (run)
            {
                // navigate the menu with arrow keys and select with enter or use number as shortcut
                ConsoleMenu.Menu.Show();
            }
        }
    }
}
