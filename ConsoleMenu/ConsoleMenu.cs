using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConsoleMenu
{
    public delegate void CommandCall();

    

    public class ConsoleMenu
    {
        public class Command
        {
            CommandCall function;
            private ConsoleMenu menu;
            public void Execute()
            {
                Console.Clear();
                Console.CursorVisible = true;
                this.function();
            }

            internal string label;
            public string Label
            {
                get
                {
                    return label;
                }
                internal set
                {
                    unpaddedLabel = value;
                    if(menu != null)
                    {
                        menu.applyPadding();
                    }
                    else
                    {
                        label = value;
                    }
                }
            }
            internal string unpaddedLabel;

            public Command(string name, CommandCall fn)
            {
                this.unpaddedLabel = name;
                this.Label = name;
                this.function = fn;
            }
        }

        private static ConsoleMenu _menu = new ConsoleMenu();

        private int command = 0;
        private List<Command> commands = new List<Command>();
        private ConsoleMenu() {
            this.commands = new List<Command>();
        }

        public static ConsoleMenu Menu
        {
            get
            {
                return _menu;
            }
        }

        public static void AddCommand(Command command)
        {
            _menu.commands.Add(command);
            _menu.applyPadding();
        }

        public static void AddCommand(Command[] commands) {
            _menu.commands.AddRange(commands);
            _menu.applyPadding();
        }

        private void applyPadding()
        {
            if (commands.Count < 1)
                return;

            Command longest = this.commands.OrderByDescending(x => x.unpaddedLabel.Length).First();

            int static_left = commands.Count.ToString().Length + 2; // number + space
            int l = longest.unpaddedLabel.Length + static_left; // how wide we go
            l = l % 2 == 0 ? l : l + 1; // make even if not

            foreach(Command c in this.commands)
            {
                c.label = c.unpaddedLabel;
                bool t = false;
                for(int i = 0; c.label.Length < l; i++)
                {
                    if (i < static_left)
                    {
                        c.label = c.label + " ";
                        t = true;
                    }
                    else
                    {
                        c.label = i % 2 != 0 || t ? " " + c.label : c.label + " ";
                        t = false;
                    }
                }
            }
        }
        
        public void Show()
        {
            PrintMenu(commands, command);
            Console.CursorVisible = false;
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    command--;
                    if (command < 0)
                        command = commands.Count - 1;
                    break;

                case ConsoleKey.DownArrow:
                    command++;
                    if (command == commands.Count)
                        command = 0;
                    break;

                case ConsoleKey.Enter:
                    commands[command].Execute();
                    break;
                default:
                    int input = 0;
                    Int32.TryParse(key.KeyChar.ToString(), out input);
                    if (input > 0 && input < commands.Count + 1)
                    {
                        command = input - 1;
                        commands[command].Execute();
                    }

                    break;
            }
        }

        private void PrintMenu(List<Command> commands, int highlight)
        {
            Console.Clear();
            Console.SetCursorPosition(Console.CursorLeft, (int)Math.Ceiling(Console.WindowHeight / 2.0) - commands.Count);
            for (int i = 0; i < commands.Count; i++)
            {
                int left = commands[i].Label.Length + commands[i].Label.Length % 2;
                left = (int)Math.Ceiling((Console.WindowWidth - left) / 2.0);
                Console.SetCursorPosition(left, Console.CursorTop);
                if (i == highlight)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.Write((i + 1) + " " + commands[i].Label);
                Console.Write("\n");
                Console.ResetColor();
            }
        }
    }
}
