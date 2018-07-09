using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JsonInjector.Resources.ViewModels
{
    public class JsonDirectoryViewModel
    {
        public string Directory { get; set; }

        public delegate void Remove(UIElement uIElement);
        public Remove RemoveDirectory;
    }
}
