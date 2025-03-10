using UnityEngine;

public class PhysicsHelper : MonoBehaviour
{
    Rigidbody2D rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    public void ApplyForceToReachVelocity(Vector2 targetvelocity, float force = 1, ForceMode2D mode = ForceMode2D.Force, bool moveX = false, bool moveY = false)
    {
        Vector2 delta_V = Vector2.zero;

        if (moveX)
        {
            delta_V.x = targetvelocity.x - rbody.linearVelocity.x;
        } 
        if (moveY)
        {
            delta_V.y = targetvelocity.y - rbody.linearVelocity.y;
        }

        Vector2 acceleration = delta_V / Time.fixedDeltaTime;
        acceleration = Vector2.ClampMagnitude(acceleration, force);
        rbody.AddForce(acceleration, mode);
    }

    public Vector2 ForceToReachVelocity(Vector2 targetvelocity, float force = 1, bool moveX = false, bool moveY = false)
    {
        Vector2 delta_V = Vector2.zero;

        if (moveX)
        {
            delta_V.x = targetvelocity.x - rbody.linearVelocity.x;
        }
        if (moveY)
        {
            delta_V.y = targetvelocity.y - rbody.linearVelocity.y;
        }

        Vector2 acceleration = delta_V / Time.fixedDeltaTime;
        acceleration = Vector2.ClampMagnitude(acceleration, force);

        return acceleration;
    }

}
