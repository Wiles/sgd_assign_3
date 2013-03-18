using System;
using System.Collections.Generic;

namespace Asteroids
{
    internal class MenuScreen
    {
        public MenuScreen(String title, MenuScreen parent)
        {
            Title = title;
            Parent = parent;
        }

        public String Title { get; private set; }
        public MenuScreen Parent { get; private set; }
        public Dictionary<String, Action> Elements { get; set; }
        public int SelectedIndex { get; set; }
    }
}