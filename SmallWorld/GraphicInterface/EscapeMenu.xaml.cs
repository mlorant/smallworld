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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicInterface
{
    /// <summary>
    /// Logique d'interaction pour EscapeMenu.xaml
    /// </summary>
    public partial class EscapeMenu : UserControl
    {

        public Action hideEscapeMenu;

        public EscapeMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Remove the menu and resume the game
        /// </summary>
        private void clickResume(object sender, RoutedEventArgs e)
        {
            this.hideEscapeMenu();
            // Get parent and remove itself
            ((Panel)this.Parent).Children.Remove(this);
        }


        /// <summary>
        /// Close the game
        /// </summary>
        private void clickExit(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
