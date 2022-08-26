using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class ghast : MonoBehaviour
{

    public GameObject _alarmtimer;

    public void stopAlarmTimer()
    {
        _alarmtimer.GetComponent<alarmTimer>().stopAlarm();
    }

    public void activatetAlarmTimer(float s, UnityAction a)
    {
        Debug.Log("adding methods ot timer");
        _alarmtimer.GetComponent<alarmTimer>().setDurationSeconds(s);
        _alarmtimer.GetComponent<alarmTimer>().addMethod(a);
    }

    public void deactivateAlarm()
    {
        _alarmtimer.GetComponent<alarmTimer>().removeAllMethods();
    }

    public void startAlarmTimer()
    {
        _alarmtimer.GetComponent<alarmTimer>().startAlarm();
    }
    public void startAlarmTimerLoop()
    {
        _alarmtimer.GetComponent<alarmTimer>().startAlarmLoop();
    }

    private void Start()
    {
        _alarmtimer = new GameObject();
        _alarmtimer.AddComponent<alarmTimer>();


    }

    public float health = 20;


    void OnCollisionEnter(Collision collision)
    {
        
        
        if (collision.gameObject.tag == "pumpkin")
        {
            FindObjectOfType<player>().hitSuccess = true;

            stopAlarmTimer();
            UnityAction a = () => { GetComponent<SpriteRenderer>().color = Color.white; };
            activatetAlarmTimer(0.2f, a);
            startAlarmTimer();

            Debug.Log("HIT");
            health -= 5;
            GetComponent<SpriteRenderer>().color = Color.red;
            if (health <= 0)
            {

                FindObjectOfType<player>().undoLock();
                Destroy(gameObject);
            }
        }
    }
}
