using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using login_screen_test.Game;
using login_screen_test.Global;

namespace login_screen_test
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        List<Character> characters = new List<Character>();

        public GameWindow(Account account)
        {
            InitializeComponent();
            init_Characters();
        }

        private void init_Characters()
        {
            for (int i = 1; i < 3; i++)
            {
                characters.Add(new Character(10, $"Character{i}"));
            }
        }

        private void Display_Character_Info(object sender, RoutedEventArgs e)
        {
            string Name = ((MenuItem)sender).Name;
            int charID = findCharacter(Name);

            if (charID == -1) 
            {
                InfoSection.Text = $"Error ID = '-1'\nName = '{Name}'";
                return;
            }
            Character character = characters[charID];
            InfoSection.Text = $"Name: {character.name}\n\nHealth: {character.health}";
        }

        private int findCharacter(string Name)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].name == Name)
                {
                    return i;
                }
            }
            return -1;
        }

        private void Character_MouseDown(object sender, RoutedEventArgs e)
        {

        }
    }
}
