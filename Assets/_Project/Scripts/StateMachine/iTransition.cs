namespace Platformer
{
    public interface iTransition
    {
        iState To { get;  }
        iPredicate Condition { get; }
    }
}