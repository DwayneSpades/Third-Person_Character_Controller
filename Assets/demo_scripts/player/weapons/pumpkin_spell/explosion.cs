using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class explosion : MonoBehaviour
{

    private alarmTimer _alarmTimer;

    // Start is called before the first frame update
    void Start()
    {
        _alarmTimer = gameObject.AddComponent<alarmTimer>();

        _alarmTimer.setDurationSeconds(0.6f);

        //create a unity action to give to the 
        UnityAction a = () => { Destroy(gameObject); };
        _alarmTimer.addMethod(a);
        _alarmTimer.startAlarm();

    }
    public void setScale(float s)
    {
        transform.localScale = new Vector3(s, s, s);
    }
    void OnDestroy()
    {

        Destroy(_alarmTimer);
    }

}
