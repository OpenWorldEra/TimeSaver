using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberUI : MonoBehaviour
{
    Vector3 targetPosition;
    Vector3 normalPosition;
    Quaternion targetRotation;
    float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        PickNumber();
        normalPosition = transform.localPosition;
        PickTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);  
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime); 
        if (Vector2.Distance(transform.localPosition, targetPosition) <= 0.2f)
        {
            PickTargetPosition();
        }

        if (Mathf.Abs(Mathf.Abs(transform.localRotation.z) - Mathf.Abs(targetRotation.z)) <= .1f)
        {
            PickTargetRotation();
        }

        //Debug.Log(transform.localRotation.z + " -------- " + targetRotation.z);
    }

    void PickNumber()
    {
        int n = Random.Range(1, 13);
        GetComponent<Text>().text = n.ToString();
        transform.GetChild(0).GetComponent<Text>().text = n.ToString();
    }

    void PickTargetRotation()
    {
        float angle = Random.Range(-360, 360);
        targetRotation = Quaternion.Euler(0, 0, angle);
    }

    void PickTargetPosition()
    {
        float x = Random.Range(normalPosition.x - 25, normalPosition.x + 25);
        float y = Random.Range(normalPosition.y - 25, normalPosition.y + 25);

        targetPosition.x = x;
        targetPosition.y = y;
    }
}
