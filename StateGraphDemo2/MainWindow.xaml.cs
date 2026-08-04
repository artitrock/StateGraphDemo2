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
                    Condition = "NG",
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



                    Line line = new();


                    line.X1 =
                        n.X + 60;


                    line.Y1 =
                        n.Y + 50;


                    line.X2 =
                        target.X + 60;


                    line.Y2 =
                        target.Y;


                    //add condition text in the middle of the line
                    TextBlock condition = new();

                    condition.Text = t.Condition;

                    condition.Foreground =
                        Brushes.Blue;


                    Canvas.SetLeft(
                        condition,
                        (line.X1 + line.X2) / 2);


                    Canvas.SetTop(
                        condition,
                        (line.Y1 + line.Y2) / 2);


                    GraphCanvas.Children.Add(condition);
                    //



                    line.Stroke =
                        Brushes.Gray;


                    line.StrokeThickness = 2;


                    GraphCanvas.Children.Add(line);

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
    }
}