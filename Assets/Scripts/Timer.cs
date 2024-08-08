using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {
    
    private bool _timerActive = true;
    private float _currentTime = 0;
    public float CurrentTime
    {
        get { return _currentTime; }
    }
    
    void Start()
    {
        
    }
    void Update()
    {
        if(_timerActive)
        {
            _currentTime = _currentTime + Time.deltaTime;
        }
        
    }
    public void StartTimer(){
        _timerActive = true;
    }
    public void StopTimer(){
        _timerActive = false;
    }
    public void ResetTimer()
    {
        _currentTime = 0;
        _timerActive = true;
    }
   

}