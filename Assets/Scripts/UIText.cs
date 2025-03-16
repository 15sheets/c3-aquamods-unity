using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public TextMeshProUGUI ui;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ui.text = Mathf.Abs((int)StatMan.sm.submods.transform.position.y).ToString() + "m";
    }
}
