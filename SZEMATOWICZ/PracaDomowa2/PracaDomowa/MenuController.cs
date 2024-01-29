using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowa
{
    class MenuController
    {
        private Action[] _actions;
        private string[] _options;

        public MenuController()
        {
            _actions = new Action[0];
            _options = new string[0];
        }

        public void AddOption(string option, Action action)
        {
            Array.Resize(ref _actions, _actions.Length + 1);
            Array.Resize(ref _options, _options.Length + 1);

            int index = _actions.Length - 1;
            _options[index] = option;
            _actions[index] = action;
        }

        public void DisplayMenu()
        {
            Console.WriteLine("Wybierz opcję:");

            for (int i = 0; i < _options.Length; i++)
            {
                Console.WriteLine(_options[i]);
            }
        }

        public int GetChoice()
        {
            Console.Write("Twój wybór: ");
            return Convert.ToInt32(Console.ReadLine());
        }

        public bool IsValidChoice(int choice)
        {
            return choice >= 1 && choice <= _actions.Length;
        }

        public void ExecuteOption(int choice)
        {
            _actions[choice - 1].Invoke();
        }
    }
}
