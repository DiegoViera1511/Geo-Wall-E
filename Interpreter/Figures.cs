using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Interpreter
{
    public abstract class Figure {}
    
    public class Point : Figure
    {      
        private double x ;
        public double X{get => x;}
        private double y ;
        public double Y{get => y;}
        public Point( double x , double y)
        {
            this.x = x ;
            this.y = y ;
        }
    }

    public class Line : Figure
    {
        private Point p1 ;
        public Point P1{get => p1;}
        private Point p2 ;
        public Point P2{get => p2;}

        public Line(Point p1 , Point p2 )
        {
            this.p1 = p1 ;
            this.p2 = p2 ;
        }

    }

    public class Segment : Figure 
    {

        private Point p1 ;
        public Point P1{get => p1;}
        private Point p2 ;
        public Point P2{get => p2;}
        public Segment(Point p1 , Point p2)
        {
            this.p1 = p1 ;
            this.p2 = p2 ;
        }

    }

    public class Ray : Figure
    {
        private Point p1 ;
        public Point P1{get => p1;}
        private Point p2 ;
        public Point P2{get => p2;}
       
        public Ray(Point p1 , Point p2)
        {
            this.p1 = p1 ;
            this.p2 = p2 ;
        }
        
    }

    public class Arc : Figure
    {
        private Point p1 ;
        public Point P1 {get => p1;}
        private Point p2 ;
        public Point P2 {get => p2;}
        private Point p3 ;
        public Point P3 {get => p3;}
        private double measure ;
        public double Measure {get => measure;}

        public Arc(Point p1 , Point p2 , Point p3 , double measure)
        {
            this.p1 = p1 ;
            this.p2 = p2 ;
            this.p3 = p3 ;
            this.measure = measure ;
        }

    }

    public class Circle : Figure
    {
        private Point center ;
        public Point Center {get => center;}
        private double radio ;
        public double Radio {get => radio;}

        public Circle(Point center , double radio)
        {
            this.center = center ;
            this.radio = radio ;
        }
    }

}