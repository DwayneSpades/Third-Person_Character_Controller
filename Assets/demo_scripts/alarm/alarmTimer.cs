using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Universal Timer Object to use on anything in the game
// customize it with whatever methods you want
public class alarmTimer : MonoBehaviour
{
    //alarm data
    [SerializeField] private float duration = 1f;
    [SerializeField] private UnityEvent triggerAlarm = new UnityEvent();
    private bool alarmRunning = false;
    private UnityAction actionAlarm;


    public void setAlarmAction(UnityAction a) { actionAlarm = a; }
    public void setDurationSeconds(float d)
    {
        duration = d;
    }
    public void updateListener() { triggerAlarm.AddListener(actionAlarm); }

    //allows the alarm execution to be customized with functions 
    public void addMethod(UnityAction a) { 
       triggerAlarm.AddListener(a);
    }

    //used when I need to rest an alarm
    public void removeAllMethods()
    {
        //Debug.Log("Removing all methods from timer");
        triggerAlarm.RemoveAllListeners();
    }

    public bool getRunningStatus() { return alarmRunning; }

    public void stopAlarm()
    {
        alarmRunning = false;
        StopAllCoroutines();
    }

    //regular alarm call - runs alarm once
    public void startAlarm()
    {
        alarmRunning = true;
        StartCoroutine(updateTimer());
    }

    //loop alarm call - runs alarm on loop
    public void startAlarmLoop()
    {
        alarmRunning = true;
        StartCoroutine(updateTimerLoop());
    }

    private IEnumerator updateTimer()
    {
        //I'm using this to make sure My time is calculated in real time according to the engine
        yield return new WaitForSeconds(duration);

        triggerAlarm.Invoke();
        alarmRunning = false;
        
    }

    private IEnumerator updateTimerLoop()
    {
        //I'm using this to make sure My time is calculated in real time according to the engine
        yield return new WaitForSeconds(duration);

        
        triggerAlarm.Invoke();
        alarmRunning = false;
        startAlarmLoop();
    }

}
