namespace Asteroids
{
    class Circle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Radius { get; set; }

        public Circle(int X, int Y, double Radius)
        {
            this.X = X;
            this.Y = Y;
            this.Radius = Radius;
        }

        public bool Intersects(Circle that)
        {
            var dx = X - that.X;
            var dy = Y - that.Y;
            var radii = Radius + that.Radius;
            return (dx * dx) + (dy * dy) < radii * radii;
        }
    }
}
