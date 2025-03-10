/// <summary>
/// interface for fsm states
/// </summary>
public interface IState
{
    public void Enter()
    {
        // code that runs upon entering the state
    }

    public void Update()
    {
        // code that runs in update. include transitions to new states
    }

    public void FixedUpdate()
    {
        // code that runs in fixedupdate. physics stuff.
    }

    public void Exit()
    {
        // code that runs on exiting the state
    }
}
