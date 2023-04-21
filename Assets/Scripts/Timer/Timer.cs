using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text firstMinute;
    public Text secondMinute;
    public Text firstSecond;
    public Text secondSecond;
    [SerializeField] float speed =  1.5f;
    [SerializeField] Clock clock;
    float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clock.numberExists == false || GameStateManager.Instance.CurrentGameState == GameState.Paused)
        {
            return;
        }

        currentTime += Time.deltaTime * speed;
        SetTimerText(currentTime);
        //SetTimerText();
    }

    void SetTimerText(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string timeText = string.Format("{00:00}{1:00}", minutes, seconds);
        firstMinute.text = timeText[0].ToString();
        secondMinute.text = timeText[1].ToString();
        firstSecond.text = timeText[2].ToString();
        secondSecond.text = timeText[3].ToString();
    }
}
