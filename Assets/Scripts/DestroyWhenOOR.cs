using UnityEngine;

public class DestroyWhenOOR : MonoBehaviour
{
    private BoxCollider2D coll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        StatMan.sm.setRangeCheck(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // destroy object if it leaves trigger area
    // this will deal with projectiles, enemies, some obstacles
    // if there is terrain / background stuff, deal with that elsewhere?
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }

    public bool isOutOfRange(Vector3 pos) 
    {
        Vector2 minBounds = new Vector2(transform.position.x, transform.position.y) - coll.size / 2;
        Vector2 maxBounds = new Vector2(transform.position.x, transform.position.y) + coll.size / 2;
        if (pos.x < minBounds.x || pos.y < minBounds.y || pos.x > maxBounds.x || pos.y > maxBounds.y)
        {
            return true;
        }
        return false;
    }
}
