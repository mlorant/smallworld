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
    /// Logique d'interaction pour UnitSelector.xaml
    /// </summary>
    public partial class UnitSelector : UserControl
    {
        /// <summary>Get a reference to the previous color to reset it
        /// when the user unselect the unit</summary>
        Brush _inactiveColor;

        /// <summary>Unit selected</summary>
        private IUnit _unit;

        /// <summary>Accessor to the unit selected</summary>
        public IUnit Unit
        {
            get { return this._unit; }
        }

        /// <summary>Create a unit selector panel according to the unit given</summary>
        public UnitSelector(IUnit unit)
        {
            InitializeComponent();

            this._unit = unit;

            // Set inactive color to the current background (set in the XAML)
            _inactiveColor = this.UnitBox.Background;

            UnitName.Text = unit.Name;
            // Set Health data
            CurrentHealth.Text = unit.Health.ToString();
            MaxHealth.Text = unit.MaxHealth.ToString();
            // Set Move data
            CurrentMove.Text = unit.MovePoint.ToString();
            MaxMove.Text = unit.MaxMovePoint.ToString();
        }

        /// <summary>
        /// Change the background of the panel to highlight it 
        /// (and shows that the unit is selected)
        /// </summary>
        /// <param name="active">true to highlight it, false to disabled it</param>
        public void Highlight(bool active)
        {
            if (active)
                this.UnitBox.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#a27091"));
            else
                this.UnitBox.Background = _inactiveColor;
        }
    }
}
