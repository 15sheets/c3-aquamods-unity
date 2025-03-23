using System.Drawing;
using UnityEngine;

public class SailfishHelper : MonoBehaviour
{
    [SerializeField] private SailfishBehavior sfb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12) // shield
        {
            sfb.trigHitShield = true;
        } 
        else if (collision.gameObject.layer == 6) // player
        {
            sfb.trigHitPlayer = true;
        } 
        // right now sailfish immortal while attacking. change that
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
