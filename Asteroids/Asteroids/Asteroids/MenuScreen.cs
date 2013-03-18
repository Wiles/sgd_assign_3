using System;
using System.Collections.Generic;

namespace Asteroids
{
    class MenuScreen
    {
        public MenuScreen(String title, MenuScreen parent)
        {
            Title = title;
            Parent = parent;
        }

        public String Title { get; set; }
        public MenuScreen Parent {get; set;}
        public Dictionary<String, Action> elements { get; set; }
        public int selectedIndex { get; set; }
    }
}
