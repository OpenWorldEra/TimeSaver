using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    bool move = false;
    Vector2 targetPos = Vector2.zero;
    Vector2 normalPos;
    [SerializeField] float handMoveSpeed = 2;
    [SerializeField] Transform capturedNumberContainer;
    [SerializeField] float nextCaptureInterval = 1f;
    public float currentCaptureIntervalTime = 0;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] AudioSource captureAudio;
    [SerializeField] AudioSource releaseAudio;

    // Start is called before the first frame update
    void Start()
    {
        currentCaptureIntervalTime = nextCaptureInterval;
        normalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (capturedNumberContainer.childCount <= 0)
        {
            currentCaptureIntervalTime += Time.deltaTime;
        } else
        {
            currentCaptureIntervalTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (capturedNumberContainer.childCount != 0)
            {
                Release();
            } else
            {
                Capture();
            }            
        }

        if (move)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * handMoveSpeed);
            if (Vector2.Distance(transform.localPosition, targetPos) <= 0.01f)
            {
                if (targetPos != normalPos)
                {
                    DisableTrailRenderer();
                    targetPos = normalPos;
                } else
                {
                    transform.localPosition = normalPos;
                    move = false;
                }                
            }
        }
    }

    void Release()
    {
        releaseAudio.Play();
        DisableTrailRenderer();
        GameObject capturedNumber = capturedNumberContainer.GetChild(0).gameObject;
        capturedNumber.transform.parent = null;
        capturedNumber.AddComponent<Rigidbody2D>();
        capturedNumber.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        capturedNumber.GetComponent<Rigidbody2D>().gravityScale = 0;
        capturedNumber.GetComponent<Number>().rb = capturedNumber.GetComponent<Rigidbody2D>();
        capturedNumber.GetComponent<Number>().enabled = true;
        capturedNumber.GetComponent<Rigidbody2D>().AddForce((capturedNumber.transform.position - transform.position) * 20, ForceMode2D.Impulse);
        capturedNumber.GetComponent<Number>().justReleased = true;
    }

    void Capture()
    {
        if (move == false)
        {
            captureAudio.Play();
            trailRenderer.enabled = true;
            targetPos = normalPos + Vector2.right * 1.5f;
            move = true;
        }        
    }

    void SetCapturedNumber(GameObject capturedNumber)
    {
        capturedNumber.transform.parent = capturedNumberContainer;
        capturedNumber.transform.localPosition = Vector3.zero;
        capturedNumber.GetComponent<Number>().DeactivateBackgroundEffect();
        capturedNumber.GetComponent<Number>().enabled = false;
        Destroy(capturedNumber.GetComponent<Rigidbody2D>());
        DisableTrailRenderer();
        targetPos = normalPos;
    }

    void DisableTrailRenderer()
    {
        trailRenderer.Clear();
        trailRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((Vector2)transform.localPosition != normalPos && capturedNumberContainer.childCount <= 0 && currentCaptureIntervalTime >= nextCaptureInterval)
        {
            
            Debug.Log("Trigger");
            if (other.gameObject.CompareTag("Number"))
            {
                Debug.Log("NumberCaptured");
                SetCapturedNumber(other.gameObject);
            }
        }
    }
}
