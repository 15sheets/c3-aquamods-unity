using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEffects : MonoBehaviour
{
    public Transform sub;

    public int resolution;

    public Vector3[] segmentPos;
    private Vector3[] segmentV;

    public float num_waves;
    public float length;

    public LineRenderer[] lr;

    public float wiggleSpeed;
    public float wiggleMagnitude;

    private float pointsep;
    private float degreesep;
    private float ptzero;

    private Vector3 subbasepos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        segmentPos = new Vector3[resolution];
        segmentV = new Vector3[resolution];

        pointsep = length / resolution;
        degreesep = (2 * Mathf.PI * num_waves) / resolution;

        ptzero = 0;

        for (int i = 0; i < lr.Length; i++)
        {
            lr[i].positionCount = resolution;
        }

        subbasepos = sub.position;
    }

    // Update is called once per frame
    void Update()
    {
        ptzero += ((wiggleSpeed * Mathf.PI) / 360) * Time.deltaTime;
        setPosStartingFrom(ptzero);
    }

    private void setPosStartingFrom(float pt)
    {
        Vector3 base_pos = transform.position;
        float offset;

        // delete later
        //pointsep = length / resolution;
        //degreesep = (2 * Mathf.PI * num_waves) / resolution;

        for (int i = 0; i < segmentPos.Length; i++)
        {
            offset = Mathf.Sin(pt + degreesep * i) * wiggleMagnitude;
            segmentPos[i] = new Vector3(pointsep * i, offset);
        }

        for (int i = 0; i < lr.Length; i++)
        {
            lr[i].SetPositions(segmentPos);
        }

        float subOffset = Mathf.Sin(pt + subbasepos.x) * wiggleMagnitude;
        sub.position = new Vector3(subbasepos.x, subbasepos.y + subOffset, subbasepos.z);
    }

    public void switchScene()
    {
        SceneManager.LoadScene(1);
    }
}
