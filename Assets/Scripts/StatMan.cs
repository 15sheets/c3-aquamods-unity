using UnityEngine;

public class StatMan : MonoBehaviour
{
    public static StatMan sm { get; private set; }

    public SubmarineModules submods { get; private set; }
    public DestroyWhenOOR rangeCheck { get; private set; }

    private void Awake()
    {
        if (sm != null && sm != this)
        {
            Destroy(this);
        } else
        {
            sm = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSubmarine(SubmarineModules sub)
    {
        submods = sub;
    }

    public void setRangeCheck(DestroyWhenOOR rc)
    {
        rangeCheck = rc;
    }
}
