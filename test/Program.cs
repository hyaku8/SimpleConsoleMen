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

        static void Main(string[] args)
        {
            ConsoleMenu menu = new ConsoleMenu();
            ConsoleMenu subMenu = new ConsoleMenu();
            subMenu.AddCommand("Do nothing", (sm) =>
            {
                Console.WriteLine("nothing done");
                Console.ReadKey();
            });
            subMenu.AddCommand("Do something else", (sm) =>
            {
                Console.WriteLine("still not doing jack shit");
                Console.ReadKey();
            });
            menu.AddSubMenu("Action 1: submenu", subMenu);
            menu.AddCommand("You may prefer action 2", (m) =>
            {
                Console.WriteLine("Action 2 Called");
                Console.ReadKey();
            });
            menu.AddCommand("Exit", (m) =>
            {
                m.Exit();
            });

            // navigate the menu with arrow keys and select with enter or use number as shortcut
            menu.Show();
        }
    }
}
