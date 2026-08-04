using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateGraphDemo2
{
    public class GraphBuilder
    {

        public class GraphResult
        {
            public List<GraphNode> Nodes { get; set; } = new();

            public double Width { get; set; }

            public double Height { get; set; }
        }

        public GraphResult Build(State root)
        {

            var nodes = new List<GraphNode>();


            Scan(root, 0, nodes);


            Layout(nodes);



            return new GraphResult
            {
                Nodes = nodes,

                Width =
                    nodes.Max(x => x.X) + 200,

                Height =
                    nodes.Max(x => x.Y) + 150
            };

        }





        void Scan(
            State state,
            int level,
            List<GraphNode> list)
        {

            if (list.Any(
                x => x.State == state))
                return;



            list.Add(
                new GraphNode
                {
                    State = state,
                    Level = level
                });



            foreach (var t
                in state.Transitions)
            {

                Scan(
                    t.Target,
                    level + 1,
                    list);

            }

        }






        void Layout(
            List<GraphNode> nodes)
        {

            int width = 160;

            int height = 100;


            foreach (var group
                in nodes.GroupBy(
                    x => x.Level))
            {

                int count =
                    group.Count();


                double start =
                    400 -
                    ((count * width) / 2);



                int index = 0;


                foreach (var node in group)
                {

                    node.X =
                        start +
                        index * width;


                    node.Y =
                        50 +
                        node.Level * height;


                    index++;

                }

            }

        }

    }
}
