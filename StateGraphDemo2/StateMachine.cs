using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateGraphDemo2
{
    public class StateMachine
    {

        public State Current { get; private set; }


        public event Action? StateChanged;



        public StateMachine(State start)
        {
            Current = start;

            start.IsActive = true;
        }



        public void Trigger(string condition)
        {

            var next =
                Current.Transitions
                .FirstOrDefault(
                    x => x.Condition == condition);


            if (next == null)
                return;


            Current.IsActive = false;


            Current =
                next.Target;


            Current.IsActive = true;


            StateChanged?.Invoke();

        }


    }
}
