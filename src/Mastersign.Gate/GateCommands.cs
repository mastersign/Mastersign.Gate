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
        public static readonly RoutedUICommand ProjectFileNew = new RoutedUICommand(
            "New Project File", "ProjectFileNew",
            typeof(GateCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            });

        public static readonly RoutedUICommand ProjectFileOpen = new RoutedUICommand(
            "Open Project File", "ProjectFileOpen",
            typeof(GateCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control)
            });

        public static readonly RoutedUICommand ProjectFileSave = new RoutedUICommand(
            "Save Project File", "ProjectFileSave",
            typeof(GateCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            });

        public static readonly RoutedUICommand ServiceNew = new RoutedUICommand(
            "New Service", "ServiceNew",
            typeof(GateCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control | ModifierKeys.Shift)
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

        public static readonly RoutedUICommand NginxFindDownload = new RoutedUICommand(
            "Find Nginx Download", "NginxFindDownload",
            typeof(GateCommands));

        public static readonly RoutedUICommand NginxDownload = new RoutedUICommand(
            "Download Nginx", "NginxDownload",
            typeof(GateCommands));
    }
}
