using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateGraphDemo2
{
    public class State
    {
        public string Name { get; set; } = "";

        public List<Transition> Transitions { get; set; }
            = new();


        public bool IsActive { get; set; }
    }



    public class Transition
    {
        public string Condition { get; set; } = "";

        public State Target { get; set; } = null!;
    }



    public class GraphNode
    {
        public State State { get; set; } = null!;


        public double X { get; set; }

        public double Y { get; set; }


        public int Level { get; set; }
    }
}
