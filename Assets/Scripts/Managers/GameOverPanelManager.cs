using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelManager : MonoBehaviour
{
    [SerializeField] Text firstMinute;
    [SerializeField] Text secondMinute;
    [SerializeField] Text firstSecond;
    [SerializeField] Text secondSecond;
    [SerializeField] Timer timer;
    [SerializeField] Transform clockHand;
    [SerializeField] float updateInterval = 2;
    float currentIntervalTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        clockHand.Rotate(0, 0, Time.deltaTime * -80);   
        UpdateTimerValue();
    }

    void UpdateTimerValue()
    {
        currentIntervalTime += Time.deltaTime;
        if (currentIntervalTime >= updateInterval)
        { 
            currentIntervalTime = 0;

            if (int.Parse(firstMinute.text) < int.Parse(timer.firstMinute.text))
            {
                firstMinute.text = (int.Parse(firstMinute.text) + 1).ToString();
            }

            if (int.Parse(secondMinute.text) < int.Parse(timer.secondMinute.text))
            {
                secondMinute.text = (int.Parse(secondMinute.text) + 1).ToString();
            }

            if (int.Parse(firstSecond.text) < int.Parse(timer.firstSecond.text))
            {
                firstSecond.text = (int.Parse(firstSecond.text) + 1).ToString();
            }

            if (int.Parse(secondSecond.text) < int.Parse(timer.secondSecond.text))
            {
                secondSecond.text = (int.Parse(secondSecond.text) + 1).ToString();
            }
        }

        
    }

    private void OnEnable() {
        //firstMinute.text = timer.firstMinute.text;
        //secondMinute.text = timer.secondMinute.text;
        //firstSecond.text = timer.firstSecond.text;
        //secondSecond.text = timer.secondSecond.text;
    }
}
