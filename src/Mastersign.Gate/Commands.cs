using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mastersign.Gate
{
    public static class GateCommands
    {
        public static readonly RoutedUICommand ServiceNew = new RoutedUICommand(
            "New Service", "ServiceNew",
            typeof(GateCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            });

        public static readonly RoutedUICommand ServiceMoveUp = new RoutedUICommand(
            "Move Service Up", "ServiceMoveUp",
            typeof(GateCommands));

        public static readonly RoutedUICommand ServiceMoveDown = new RoutedUICommand(
            "Move Service Down", "ServiceMoveDown",
            typeof(GateCommands));

        public static readonly RoutedUICommand ServiceDelete = new RoutedUICommand(
            "Delete Service", "ServiceDelete",
            typeof(GateCommands));

    }
}
