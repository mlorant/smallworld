using SmallWorld;
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
    /// Logique d'interaction pour GameLoadFile.xaml
    /// </summary>
    public partial class GameLoadFile : UserControl
    {
        public GameLoadFile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load a game file, previously saved
        /// </summary>
        private void loadGame(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.DefaultExt = ".sw";
            dlg.Filter = "SmallWorld save file (.sw)|*.sw";

            bool ok = false;
            while (!ok)
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        Game.Instance.restoreGame(dlg.FileName);
                    }
                    catch (InvalidSaveFileException exc)
                    {
                        MessageBox.Show("Incorrect file. Are you sure that " + dlg.FileName + " is a SmallWorld game file?");
                        continue; // Ask a new file
                    }

                    ok = true;

                    // Reload the whole window
                    GameBoard main = new GameBoard(Game.Instance);
                    App.Current.MainWindow = main;
                    Window.GetWindow(this).Close();
                    main.Show();
                }
            }

        }
    }
}
