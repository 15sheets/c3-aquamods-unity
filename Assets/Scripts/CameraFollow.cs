using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform toFollow;
    public float smoothTime;

    //public float marginY = 0;
    //public float marginX;

    private Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        /*
        //Vector3 diff = toFollow.position - transform.position;
        float cameraX = Mathf.Clamp(transform.position.x, toFollow.position.x - marginX, toFollow.position.x + marginX);
        float cameraY = Mathf.Clamp(transform.position.y, toFollow.position.y - marginY, toFollow.position.y + marginX);
        transform.position = new Vector3(cameraX, cameraY, transform.position.z);
        */
        Vector3 target = Vector3.SmoothDamp(transform.position, toFollow.position, ref velocity, smoothTime);
        setPos(target);
    }

    public virtual void setPos(Vector3 target)
    {
        transform.position = new Vector3(target.x, toFollow.position.y, transform.position.z);
    }
}
