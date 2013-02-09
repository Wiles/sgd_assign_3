namespace Asteroids
{
    class Circle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }

        public Circle(double X, double Y, double Radius)
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
