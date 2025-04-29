namespace Platformer
{
    public class Transition : iTransition
    {
        public iState To { get; }
        
        public iPredicate Condition { get; }
        
        public Transition(iState to, iPredicate condition)
        {
            To = to;
            Condition = condition;
        }
    }
}