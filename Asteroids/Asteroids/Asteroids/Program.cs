//File:     Projectile.cs
//Name:     Samuel Lewis (5821103) & Thomas Kempt (5781000)
//Date:     2013-03-19
//Class:    Simulation and Game Development
//Ass:      3
//
//Desc:     Main 

namespace Asteroids
{
#if WINDOWS || XBOX
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}