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

        Brush _inactiveColor;

        private IUnit _unit;

        public IUnit Unit
        {
            get { return this._unit; }
        }

        public UnitSelector(IUnit unit)
        {
            InitializeComponent();
            
            this._unit = unit;
            
            _inactiveColor = this.UnitBox.Background;

            UnitName.Text = unit.Name;

            CurrentHealth.Text = unit.Health.ToString();
            MaxHealth.Text = unit.MaxHealth.ToString();

            CurrentMove.Text = unit.MovePoint.ToString();
            MaxMove.Text = unit.MaxMovePoint.ToString();
        }

        public void Highlight(bool active)
        {
            if (active)
            {
                this.UnitBox.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#a27091"));
            }
            else
            {
                this.UnitBox.Background = _inactiveColor;
            }
        }

    }
}
