using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConsoleMenu
{
    public delegate void CommandCall(ConsoleMenu self);

    

    public class ConsoleMenu
    {
        internal class Command
        {
            CommandCall function;
            private ConsoleMenu menu;
            public void Execute()
            {
                Console.Clear();
                Console.CursorVisible = true;
                this.function(menu);
            }

            public string Label { get; set; }
            public string UnpaddedLabel { get; set; }

            public Command(string name, CommandCall fn, ConsoleMenu menu)
            {
                this.UnpaddedLabel = name;
                this.Label = name;
                this.function = fn;
                this.menu = menu;
            }
        }

        private int command = 0;
        private List<Command> commands = new List<Command>();
        private List<ConsoleMenu> subMenus = new List<ConsoleMenu>();
        private bool run;
       
        public void AddCommand(string label, CommandCall action)
        {
            commands.Add(new Command(label, action, this));
            applyPadding();
        }

        public void AddSubMenu(string label, ConsoleMenu subMenu, bool addBackAction = true, string backActionLabel = "Back")
        {
            if(addBackAction)
            {
                subMenu.AddCommand(backActionLabel, (m) =>
                {
                    m.Exit();
                });
            }
            commands.Add(new Command(label, (m) =>
            {
                m.Exit();
                subMenu.Show();
                m.Show();
            }, this));
        }

        private void applyPadding()
        {
            if (commands.Count < 1)
                return;

            Command longest = this.commands.OrderByDescending(x => x.UnpaddedLabel.Length).First();

            int static_left = commands.Count.ToString().Length + 2; // number + space
            int l = longest.UnpaddedLabel.Length + static_left; // how wide we go
            l = l % 2 == 0 ? l : l + 1; // make even if not

            foreach(Command c in this.commands)
            {
                c.Label = c.UnpaddedLabel;
                bool t = false;
                for(int i = 0; c.Label.Length < l; i++)
                {
                    if (i < static_left)
                    {
                        c.Label = c.Label + " ";
                        t = true;
                    }
                    else
                    {
                        c.Label = i % 2 != 0 || t ? " " + c.Label : c.Label + " ";
                        t = false;
                    }
                }
            }
        }
        
        public void Show()
        {
            this.run = true;
            while (this.run)
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
        }

        public void Exit()
        {
            run = false;
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
