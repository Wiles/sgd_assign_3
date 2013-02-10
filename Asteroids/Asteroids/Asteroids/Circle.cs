namespace Asteroids
{
    internal class Circle
    {
        public Circle(double X, double Y, double Radius)
        {
            this.X = X;
            this.Y = Y;
            this.Radius = Radius;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }

        public bool Intersects(Circle that)
        {
            double dx = X - that.X;
            double dy = Y - that.Y;
            double radii = Radius + that.Radius;
            return (dx*dx) + (dy*dy) < radii*radii;
        }
    }
}