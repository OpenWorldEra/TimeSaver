using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField] Transform hand;
    [SerializeField] float rotationInterval = 2f;
    [SerializeField] float rotationVal = 10;
    [SerializeField] Transform numbersContainer;
    float currentTime = 0;
    int angleChangeCount = 0;
    public int currentNumber = 12;
    [SerializeField] float numberBreakInterval = 4;
    float currentNumberBreakTime;
    [SerializeField] GameObject numberPrefab;
    public bool numberExists = true;
    [SerializeField] GameObject rippleEffect;
    float noNumberIntervalTime = 0;
    [SerializeField] float health = 3;
    float maxHealth;
    [SerializeField] Transform structureContainer;
    [SerializeField] NumberInstantiator numberInstantiator;
    //[SerializeField] Text timerText;
    //[SerializeField] float timerUpdateInterval;
    float currentTimerIntervalVal = 0;
    float timerVal = 0;
    [SerializeField] GameSceneManager gameSceneManager;
    [SerializeField] AudioSource tickAudio;
    [SerializeField] AudioSource rippleAudio;
    [SerializeField] AudioSource clockBreakAudio;

    private void Awake() {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        
    }

    public void Enable()
    {
        enabled = true;
        numberInstantiator.InstantiateNumbers();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 || GameStateManager.Instance.CurrentGameState == GameState.Paused)
        {
            return;
        }

        RotateHand();
        BreakNumber();
        //UpdateTimerValue();
    }

    void RotateHand()
    {
        if (!numberExists)
        {
            noNumberIntervalTime -= Time.deltaTime;
            if (noNumberIntervalTime <= 0)
            {
                health -= 1;
                rippleAudio.Play();
                Instantiate(rippleEffect, transform.position, rippleEffect.transform.rotation);
                noNumberIntervalTime = numberBreakInterval;

                if (health <= 0)
                {
                    Debug.Log("Game Over");
                    GameStateManager.Instance.SetState(GameState.Paused);
                    DeathEffect();
                }
            }
        }

        currentTime += Time.deltaTime;
        if (currentTime >= rotationInterval || !numberExists)
        {
            currentTime = 0;
            if (numberExists)
            {
                hand.Rotate(new Vector3(0, 0, -rotationVal)); 
                tickAudio.Play();           
                angleChangeCount += 1;
            }            

            int currentNumberBeforeChange = currentNumber;

            if (angleChangeCount / 3 == 12)
            {
                angleChangeCount = 0;
            }

            if ((int)(angleChangeCount / 3) == 0)
            {
                currentNumber = 12;
            } else
            {
                currentNumber = angleChangeCount / 3;
            }

            if (currentNumberBeforeChange != currentNumber || !numberExists)
            {
                CheckIfNumberExists();
            }
        }

        
    }

    void CheckIfNumberExists()
    {
        if (numbersContainer.GetChild(currentNumber - 1).GetComponent<TextMesh>().color.a == 0)
        {            
            numberExists = false;
        } else
        {
            health = maxHealth;
            numberExists = true;
        }
    }

    void BreakNumber()
    {
        currentNumberBreakTime += Time.deltaTime;
        if (currentNumberBreakTime >= numberBreakInterval)
        {
            if (numberExists == false)
            {
                return;
            }

            int numberToBreak = Random.Range(currentNumber + 3, currentNumber + 9);
            if (numberToBreak > 12)
            {
                numberToBreak = numberToBreak - 12;
            }
            Color color = numbersContainer.GetChild(numberToBreak - 1).gameObject.GetComponent<TextMesh>().color;
            if (color.a != 0)
            {
                color.a = 0;
                numbersContainer.GetChild(numberToBreak - 1).gameObject.GetComponent<TextMesh>().color = color;
                InstantiateBreakedNumber(numberToBreak);
                currentNumberBreakTime = 0; 
            }            
        }
    }

    void InstantiateBreakedNumber(int numberToBreak)
    {
        GameObject obj = Instantiate(numberPrefab, numbersContainer.GetChild(numberToBreak - 1).transform.position, Quaternion.identity);
        obj.GetComponent<TextMesh>().text = numbersContainer.GetChild(numberToBreak - 1).gameObject.GetComponent<TextMesh>().text;
        obj.GetComponent<Number>().PlayBreakAnim(hand);
        //obj.GetComponent<Animator>().Play("break");
        //obj.GetComponent<Number>().rb.AddForce(obj.transform.position - hand.transform.position, ForceMode2D.Impulse);
    }

    void DeathEffect()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        foreach (Transform item in structureContainer)
        {
            //item.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            item.gameObject.AddComponent<BoxCollider2D>();
            item.gameObject.AddComponent<Rigidbody2D>();
            Rigidbody2D itemRB = item.gameObject.GetComponent<Rigidbody2D>();
            itemRB.gravityScale = 0;
            itemRB.AddForce((item.position - hand.position) * Random.Range(0.2f, 1f), ForceMode2D.Impulse);
            item.gameObject.AddComponent<BrokenPiece>();
        }

        foreach (Transform number in numbersContainer)
        {
            if (number.gameObject.GetComponent<TextMesh>().color.a != 0)
            {
                int numberToBreak = number.GetSiblingIndex() + 1;
                GameObject obj = Instantiate(numberPrefab, numbersContainer.GetChild(numberToBreak - 1).transform.position, Quaternion.identity);
                obj.GetComponent<TextMesh>().text = numbersContainer.GetChild(numberToBreak - 1).gameObject.GetComponent<TextMesh>().text;
                obj.GetComponent<Number>().rb.AddForce((obj.transform.position - hand.transform.position) * Random.Range(0.2f, 1f), ForceMode2D.Impulse);
            }

            number.gameObject.SetActive(false);
        }

        clockBreakAudio.Play();

        Invoke(nameof(ActivateGameOver), 2);
    }

    void ActivateGameOver()
    {
        gameSceneManager.ActivateGameOver();
    }

    //void UpdateTimerValue()
    //{
    //    if (numberExists)
    //    {
    //        currentTimerIntervalVal += Time.deltaTime;
    //        if (currentTimerIntervalVal >= timerUpdateInterval)
    //        {
    //            //timerText.text = timerText.text
    //        }
    //    }
    //}

    void OnGameStateChanged(GameState newState) 
    {
        if (newState == GameState.GamePlay)
        {
            enabled = true;
        } else if (newState == GameState.Paused)
        {
            enabled = false;
        }
    }
}
