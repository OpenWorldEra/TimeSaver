using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    [SerializeField] Vector2 targetSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetSize, Time.deltaTime);
        if (Vector2.Distance(transform.localScale, targetSize) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
