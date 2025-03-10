using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{

    public UnityEvent[] animationEvents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < animationEvents.Length; i++)
        {
            if (animationEvents[i] == null)
            {
                animationEvents[i] = new UnityEvent();
            }
        }
    }

    public void invokeEvent(int idx)
    {
        animationEvents[idx].Invoke();
    }
}
