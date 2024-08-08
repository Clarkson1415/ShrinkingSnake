using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.InteropServices;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Snake playerSnake;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _highscoreText;
    [SerializeField] private float shrinkTimeInterval; // time between snake losing a segment, must be greater than 1
    float timeToWaitPast; // time to wait until after for next shrink()
    Timer gameTimer;
    float bestTime = 0f;
    // Snake playerSnake;
    void Awake()
    {
        gameTimer = gameObject.AddComponent<Timer>();
        timeToWaitPast = shrinkTimeInterval/2;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeText();
    }
    void FixedUpdate()
    {
        if(!playerSnake.isInvincible && playerSnake.IsDead)
        {
            playerSnake.ResetSnake(); // kill and reset snake
            if(gameTimer.CurrentTime > bestTime)
            {
                bestTime = gameTimer.CurrentTime;
                TimeSpan time = TimeSpan.FromSeconds(gameTimer.CurrentTime);
                _highscoreText.text = "Best Time: " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
            }
            gameTimer.ResetTimer();
        }

        // remove segment every shrinktimeinterval to go in coroutine shrink
        if(!playerSnake.isInvincible)
        {
            if (((Mathf.Round(gameTimer.CurrentTime) % shrinkTimeInterval) == 0) && (gameTimer.CurrentTime > timeToWaitPast))
            {
                timeToWaitPast = gameTimer.CurrentTime + (shrinkTimeInterval/2); // increment timeto wait past so next shrink not callled again till next time interval wanted.
                playerSnake.Shrink();
            }
        }
    }

    void UpdateTimeText()
    {
        TimeSpan time = TimeSpan.FromSeconds(gameTimer.CurrentTime);
        _timeText.text = "Time: " + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
    }
    
}


