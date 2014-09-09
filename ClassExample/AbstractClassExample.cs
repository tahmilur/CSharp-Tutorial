using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Author: Tahmilur
// Creation Date : 9-9-2014

namespace ClassExample
{
    abstract class Motorcycle
    {
        // Anyone can call this. 
        public void StartEngine() {/* Method statements here */ }

        // Only derived classes can call this. 
        protected void AddGas(int gallons) { /* Method statements here */ }

        // Derived classes can override the base class implementation. 
        public virtual int Drive(int miles, int speed) { /* Method statements here */ return 1; }

        // Derived classes must implement this. 
        public abstract double GetTopSpeed();
    }

    class Abarth : Motorcycle
    {
        public override double GetTopSpeed()
        {
            return 108.4;
        }
    }

    class BMW : Motorcycle
    {
        public override double GetTopSpeed()
        {
            return 250;
        }
    }

    class Ferrari : Motorcycle
    {
        public override double GetTopSpeed()
        {
            return 250;
        }
    }

    class Mercedes : Motorcycle
    {
        public override double GetTopSpeed()
        {
            return 150;
        }
    }
}
