using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    public TextMesh textMesh;
    public Color targetColor = Color.white;
    public Vector3 targetPos;
    public Rigidbody2D rb;
    public bool justReleased = false;
    Transform clockHand;
    [SerializeField] GameObject numberBackToPosAudio;
    [SerializeField] AudioSource breakAudio;
    [SerializeField] GameObject breakFreeAudio;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<TextMesh>().text = GetComponent<TextMesh>().text;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null)
        {
            if (transform.parent == null)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            } else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
            
            return;
        }

        if (rb.velocity.magnitude <= 0.2f && transform.parent == null && GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "break")
        {
            transform.GetChild(0).gameObject.SetActive(true);
        } else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (rb.velocity.magnitude != 0 && justReleased)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, .3f);
            foreach (Collider2D item in colliders)
            {
                if (item.CompareTag("ClockNumber"))
                {
                    if (item.gameObject.GetComponent<TextMesh>().text == GetComponent<TextMesh>().text)
                    {
                        Color color = item.gameObject.GetComponent<TextMesh>().color;
                        if (color.a == 0)
                        {
                            Destroy(Instantiate(numberBackToPosAudio, transform.position, Quaternion.identity), 2);
                            color.a = 1;
                            item.gameObject.GetComponent<TextMesh>().color = color;
                            Destroy(gameObject);
                        }
                    }
                }
            }
        } 
        if (rb.velocity.magnitude <= 0.2f)
        {
            bool insideClock = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, .5f);
            foreach (Collider2D item in colliders)
            {
                if (item.CompareTag("Clock"))
                {
                    Debug.Log("Found clock");
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.AddForce((transform.position - item.transform.position) * 2, ForceMode2D.Impulse);
                    insideClock = true;
                    break;
                }
            }
            if (!insideClock)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            justReleased = false;
        }
        //ChangeColor();
    }

    private void FixedUpdate() {
        if (rb == null)
        {
            return;
        }

        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime);
    }

    void ChangeColor()
    {
        textMesh.color = Color.Lerp(textMesh.color, targetColor, Time.deltaTime * 2f);
        if (Mathf.Abs(textMesh.color.a - targetColor.a) <= 0.2f)
        {
            PickTargetColor();
        }
    }

    public void PlayBreakAudio()
    {
        breakAudio.Play();
    }

    public void PlayBreakAnim(Transform clockHand)
    {
        Destroy(GetComponent<Rigidbody2D>());
        this.clockHand = clockHand;
        GetComponent<Animator>().Play("break");
    }

    public void AddBreakForce()
    {
        Destroy(Instantiate(breakFreeAudio, transform.position, Quaternion.identity), 2);
        gameObject.AddComponent<Rigidbody2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.AddForce(transform.position - clockHand.position, ForceMode2D.Impulse);
    }

    public void DeactivateBackgroundEffect()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Enable()
    {
        enabled = true;
    }

    void PickTargetColor()
    {
        targetColor = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, Random.Range(0.05f, 0.5f));
    }
}
