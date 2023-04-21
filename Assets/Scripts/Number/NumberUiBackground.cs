using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberUiBackground : MonoBehaviour
{
    Vector2 normalSize;
    [SerializeField] Vector2 enlargedSize;
    Vector2 targetSize;
    Color targetColor;
    Color normalColor;

    // Start is called before the first frame update
    void Start()
    {
        normalSize = transform.localScale;
        targetSize = enlargedSize;
        targetColor = normalColor = GetComponent<Text>().color;
        targetColor.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, targetSize, Time.deltaTime);
        GetComponent<Text>().color = Color.Lerp(GetComponent<Text>().color, targetColor, Time.deltaTime * 2);
        if (Vector2.Distance(transform.localScale, targetSize) <= 0.1f)
        {
            transform.localScale = normalSize;
            GetComponent<Text>().color = normalColor;
        }
    }
}
