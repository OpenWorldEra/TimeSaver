using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberInstantiator : MonoBehaviour
{
    [SerializeField] GameObject number;
    [SerializeField] int count = 10;
    [SerializeField] BoxCollider2D clockBox;
    Vector3 left, right, top, bottom;
    List<Transform> usedPositions = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        //InstantiateNumbers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateNumbers()
    {
        GetEdgePoints();

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = GetInstantiatePoint();
            GameObject obj = Instantiate(number, pos, Quaternion.identity);
            obj.GetComponent<Number>().enabled = false;
            obj.GetComponent<TextMesh>().text = ((int)Random.Range(1, 13)).ToString();
            usedPositions.Add(obj.transform);
        }
    }

    void GetEdgePoints()
    {
        left = new Vector3(0, Screen.height / 2, 0);
        right = new Vector3(Screen.width, Screen.height / 2, 0);
        top = new Vector3(Screen.width / 2, Screen.height, 0);
        bottom = new Vector3(Screen.width / 2, 0, 0);

        left = Camera.main.ScreenToWorldPoint(left);
        right = Camera.main.ScreenToWorldPoint(right);
        top = Camera.main.ScreenToWorldPoint(top);
        bottom = Camera.main.ScreenToWorldPoint(bottom);

        left.z = right.z = top.z = bottom.z = 0;
    }

    Vector3 GetInstantiatePoint()
    {
        while (true)
        {
            float x = Random.Range(left.x + .5f, right.x - .5f);
            float y = Random.Range(bottom.y + .5f, top.y - .5f);

            if ((x <= clockBox.bounds.max.x && x >= clockBox.bounds.min.x) /*||
                (y <= clockBox.bounds.max.y && y >= clockBox.bounds.min.y)*/)
            {
                continue;
            }

            Vector3 pos = new Vector3(x, y, 0);

            bool used = false;
            foreach (Transform item in usedPositions)
            {
                if (Vector2.Distance(item.position, pos) < 1)
                {
                    used = true;
                    break;
                }
            }
            if (used)
            {
                continue;
            }

            return pos;
        }
        
    }
}
