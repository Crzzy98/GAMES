using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    private float scaleSize = 1f;
    private float damageValue = .20f; 

    private Transform bar;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
        bar.localScale = new Vector3(scaleSize, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        if (scaleSize == 0)
            Debug.Log("Scale reached zero.....");

        bar.localScale = new Vector3(scaleSize, 1f);

        if (scaleSize <= 0)
            Debug.Log("Game Over!");

    }

    public void lowerHealth()
    {
        if (scaleSize > 0)
            scaleSize -= damageValue;
        if (scaleSize == damageValue)
            scaleSize = 0;
    }

    public void raiseHealth()
    {
        if (scaleSize > 0)
            scaleSize += damageValue;
    }

    public void SetColor(Color color)
    {
        bar.Find("Health_Bar_Sprite").GetComponent<SpriteRenderer>().color = color;

    }

    public Color getColor()
    {

        return bar.Find("Health_Bar_Sprite").GetComponent<SpriteRenderer>().color;
    }
}
