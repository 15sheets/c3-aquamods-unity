using UnityEngine;

public class WaveScroller : MonoBehaviour
{
    public SpriteRenderer leftSprite;
    public SpriteRenderer midSprite;
    public SpriteRenderer rightSprite;

    [SerializeField] private Sprite[] waveTypes;
    private int waveIdx=0;

    [SerializeField] Transform waveSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        

    }

    // Update is called once per frame
    void Update()
    {
        bool leftVis = leftSprite.isVisible;
        bool midVis = midSprite.isVisible;
        bool rightVis = rightSprite.isVisible;

        Vector3 subpos = StatMan.sm.submods.transform.position;

        float leftDist = Vector3.Distance(subpos, leftSprite.transform.position);
        float midDist = Vector3.Distance(subpos, midSprite.transform.position);
        float rightDist = Vector3.Distance(subpos, rightSprite.transform.position);

        if (!leftVis && !midVis && !rightVis && ((transform.position - subpos).y > 0))
        {
            transform.position = waveSpawn.position;
            pickRandomWaveSprite();
        } 
        else if (leftDist < midDist && !rightVis)
        {
            // update names
            SpriteRenderer tmp = midSprite;
            midSprite = leftSprite;
            leftSprite = rightSprite;
            rightSprite = tmp;

            // teleport right sprite left 
            leftSprite.transform.position = midSprite.transform.position - new Vector3(waveTypes[waveIdx].bounds.size.x * transform.localScale.x, 0);
        }
        else if (rightDist < midDist && !leftVis)
        {
            // update names
            SpriteRenderer tmp = midSprite;
            midSprite = rightSprite;
            rightSprite = leftSprite;
            leftSprite = tmp;

            // teleport left sprite right 
            rightSprite.transform.position = midSprite.transform.position + new Vector3(waveTypes[waveIdx].bounds.size.x * transform.localScale.x, 0);

        }

    }

    private void pickRandomWaveSprite()
    {
        waveIdx = Random.Range(0, waveTypes.Length);
        
        // update sprites
        leftSprite.sprite = waveTypes[waveIdx];
        midSprite.sprite = waveTypes[waveIdx];
        rightSprite.sprite = waveTypes[waveIdx];

        // update positions
        leftSprite.transform.position = midSprite.transform.position - new Vector3(waveTypes[waveIdx].bounds.size.x * transform.localScale.x, 0);
        rightSprite.transform.position = midSprite.transform.position + new Vector3(waveTypes[waveIdx].bounds.size.x * transform.localScale.x, 0);

    }
}
