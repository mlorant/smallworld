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
    /// Button for manage the load of a game save
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
            // Configure a file dialog to select the file to load
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.DefaultExt = ".sw";
            dlg.Filter = "SmallWorld save file (.sw)|*.sw";

            bool ok = false;
            while (!ok)
            {
                System.Windows.Forms.DialogResult res = dlg.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        Game.Instance.restoreGame(dlg.FileName);
                    }
                    catch (InvalidSaveFileException)
                    {
                        MessageBox.Show("Incorrect file. Are you sure that " + dlg.FileName + " is a SmallWorld game file?");
                        continue; // Ask a new file
                    }

                    // Stop asking file and reload the whole window
                    ok = true;

                    GameBoard main = new GameBoard(Game.Instance);
                    App.Current.MainWindow = main;
                    Window.GetWindow(this).Close();
                    main.Show();
                }
                // Stop asking if the user cancel the loading
                else if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    ok = true;
                }
            }

        }
    }
}
