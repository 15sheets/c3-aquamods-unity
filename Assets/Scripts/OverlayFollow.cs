using UnityEngine;

public class OverlayFollow : CameraFollow
{
    public float minSpeedPos;
    public float maxSpeedPos;

    //public float smoothTime = 0.5f

    public float currOffset;
    private float goalOffset;

    private float yvelocity = 0;

    public override void setPos(Vector3 target)
    {
        goalOffset = Mathf.Lerp(minSpeedPos, maxSpeedPos, StatMan.sm.submods.getVSpeedPercent());
        currOffset = Mathf.SmoothDamp(currOffset, goalOffset, ref yvelocity, smoothTime);

        float y = toFollow.position.y + currOffset;
        transform.position = new Vector3(target.x, y, transform.position.z);
    }
}
