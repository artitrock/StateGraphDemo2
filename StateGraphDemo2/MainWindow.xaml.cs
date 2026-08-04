using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StateGraphDemo2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StateMachine machine;
        List<GraphNode> nodes;
        public MainWindow()
        {
            InitializeComponent();

            machine =
            CreateMachine();



            var builder =
                new GraphBuilder();


            var graph =
                builder.Build(
                    machine.Current);


            nodes =
                graph.Nodes;



            GraphCanvas.Width =
                graph.Width;


            GraphCanvas.Height =
                graph.Height;



            machine.StateChanged += Render;


            Render();
        }

        StateMachine CreateMachine()
        {


            var idle =
                new State()
                {
                    Name = "Idle"
                };


            var check =
                new State()
                {
                    Name = "Check"
                };


            var run =
                new State()
                {
                    Name = "Run"
                };


            var error =
                new State()
                {
                    Name = "Error"
                };

            var error2 =
                new State()
                {
                    Name = "Error2"
                };

            var errorAction2 =
                new State()
                {
                    Name = "ErrorAction2"
                };


            var finish =
                new State()
                {
                    Name = "Finish"
                };



            idle.Transitions.Add(
                new Transition
                {
                    Condition = "Start",
                    Target = check
                });



            check.Transitions.Add(
                new Transition
                {
                    Condition = "OK",
                    Target = run
                });



            check.Transitions.Add(
                new Transition
                {
                    Condition = "OK",
                    Target = error
                });

            check.Transitions.Add(
                new Transition
                {
                    Condition = "NG2",
                    Target = error2
                });

            error.Transitions.Add(
                new Transition
                {
                    Condition = "Error Done",
                    Target = finish
                });

            error2.Transitions.Add(
                new Transition
                {
                    Condition = "Error2 Done",
                    Target = errorAction2
                });

            errorAction2.Transitions.Add(
                new Transition
                {
                    Condition = "Error Action2 Done",
                    Target = finish
                });


            run.Transitions.Add(
                new Transition
                {
                    Condition = "Done",
                    Target = finish
                });

            finish.Transitions.Add(
                new Transition
                {
                    Condition = "Reset",
                    Target = idle
                });



            return new StateMachine(idle);

        }








        void Render()
        {

            GraphCanvas.Children.Clear();



            foreach (var n in nodes)
            {

                foreach (var t
                    in n.State.Transitions)
                {

                    var target =
                        nodes.First(
                        x => x.State == t.Target);



                    //Line line = new();


                    //line.X1 =
                    //    n.X + 60;


                    //line.Y1 =
                    //    n.Y + 50;


                    //line.X2 =
                    //    target.X + 60;


                    //line.Y2 =
                    //    target.Y;


                    //line.Stroke =
                    //    Brushes.Gray;


                    //line.StrokeThickness = 2;


                    //GraphCanvas.Children.Add(line);

                    ////add condition text in the middle of the line
                    //TextBlock condition = new();

                    //condition.Text = t.Condition;

                    //condition.Foreground =
                    //    Brushes.Blue;


                    //Canvas.SetLeft(
                    //    condition,
                    //    (line.X1 + line.X2) / 2);


                    //Canvas.SetTop(
                    //    condition,
                    //    (line.Y1 + line.Y2) / 2);


                    //GraphCanvas.Children.Add(condition);
                    ////
                    DrawSmoothConnection(n, target, t.Condition);
                }

            }




            foreach (var n in nodes)
            {

                Border box = new();


                box.Width = 120;

                box.Height = 50;


                box.Background =
                    n.State.IsActive
                    ? Brushes.LimeGreen
                    : Brushes.LightGray;



                box.BorderBrush =
                    Brushes.Black;


                box.BorderThickness =
                    new Thickness(2);



                TextBlock text = new();


                text.Text =
                    n.State.Name;


                text.HorizontalAlignment =
                    HorizontalAlignment.Center;


                text.VerticalAlignment =
                    VerticalAlignment.Center;



                box.Child = text;



                Canvas.SetLeft(
                    box, n.X);


                Canvas.SetTop(
                    box, n.Y);



                GraphCanvas.Children.Add(box);

            }


        }

        private void DrawSmoothConnection(
    GraphNode from,
    GraphNode to,
    string condition)
        {
            double x1 = from.X + 60;
            double y1 = from.Y + 50;

            double x2 = to.X + 60;
            double y2 = to.Y;

            // จุดควบคุม Bezier
            double midY = (y1 + y2) / 2;

            PathFigure figure = new();
            figure.StartPoint = new Point(x1, y1);

            figure.Segments.Add(
                new BezierSegment(
                    new Point(x1, midY),
                    new Point(x2, midY),
                    new Point(x2, y2),
                    true));

            PathGeometry geometry = new();
            geometry.Figures.Add(figure);

            Path path = new();
            path.Data = geometry;
            path.Stroke = Brushes.Gray;
            path.StrokeThickness = 2;

            GraphCanvas.Children.Add(path);

            // condition label
            TextBlock txt = new();
            txt.Text = condition;
            txt.Foreground = Brushes.Blue;
            txt.Background = Brushes.White;

            Canvas.SetLeft(txt, (x1 + x2) / 2 + 5);
            Canvas.SetTop(txt, midY - 10);

            GraphCanvas.Children.Add(txt);

            //Polygon arrow = new();
            //arrow.Points = new PointCollection
            //{
            //    new Point(x2 - 5, y2 - 10),
            //    new Point(x2 + 5, y2 - 10),
            //    new Point(x2, y2)
            //};

            //arrow.Fill = Brushes.Gray;
            //GraphCanvas.Children.Add(arrow);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = DemoAsync();
        }

        private async Task DemoAsync()
        {
            while (true)
            {
                await Task.Delay(100);
                machine.Trigger("Start");

                await Task.Delay(100);
                machine.Trigger("OK");

                await Task.Delay(100);
                machine.Trigger("Done");

                await Task.Delay(100);
                machine.Trigger("Error Action2 Done");

                await Task.Delay(100);
                machine.Trigger("Reset");
            }
        }
    }
}