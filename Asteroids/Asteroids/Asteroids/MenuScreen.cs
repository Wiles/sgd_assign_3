using System;
using System.Collections.Generic;

namespace Asteroids
{
    class MenuScreen
    {
        public MenuScreen(String title)
        {
            Title = title;
        }

        public String Title { get; set; }
        public Dictionary<String, Action> elements { get; set; }
        public int selectedIndex { get; set; }
    }
}
