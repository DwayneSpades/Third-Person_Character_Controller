using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class pumpkinBlast : MonoBehaviour
{

    //charging the pumpkin blast
    //small shots aim straight but do little damage
    //a charged shot makes the pumpkin hit box bigger but the projectile will be thrown in an arc meaning that goo aim must be employed
    //
    //  later on with the fire upgrade the pumpkins can turn into jacko '0 laterns and explode on contact
    //  charged jack 'o latern will do AOE damage from a big explosion 
    //  this will drain more mana then the regular charged shot
    //    

    public GameObject explosion;

    public alarmTimer _alarmTimer;

    public float lifeDuration=2;

    float throwDirectionX;
 
    float velocitySide = 0;
    float velocityUp = 0;
    float velocityFWD = 40;
    public float playerVel = 0;
    public float playerForce = 0;

    public float fallSpeed = -10;
    float deccelerationRate = 0;

    float scale = 0.045f;
    public float growthRate = 0.0005f;

    public Transform arm;
    bool released = false;

    float explosionScales=0.2f;


    public float damageAmaount = 5;

    // Start is called before the first frame update
    public void initialize()
    {
        _alarmTimer = gameObject.AddComponent<alarmTimer>();

        _alarmTimer.setDurationSeconds(lifeDuration);

        //create a unity action to give to the 
        UnityAction a = () => { Destroy(gameObject); };
        _alarmTimer.addMethod(a);

        released = false;
        transform.localScale = new Vector3(scale, scale, scale);
    }



    void OnDestroy()
    {
        GameObject exp =  Instantiate(explosion,transform.position,transform.rotation);
        exp.GetComponent<explosion>().setScale(explosionScales);
        Destroy(_alarmTimer);
    }
    void grow()
    {
        transform.position = arm.position;
        transform.localScale = new Vector3(scale,scale,scale);


        damageAmaount = Mathf.Lerp(damageAmaount, 40f, 0.5f * Time.deltaTime);
        explosionScales = Mathf.Lerp(explosionScales, 1.5f, 0.5f * Time.deltaTime);
        playerForce = Mathf.Lerp(playerForce, playerVel, 0.5f * Time.deltaTime);

        velocityFWD = Mathf.Lerp(velocityFWD,6,0.5f*Time.deltaTime);
        velocityUp = Mathf.Lerp(velocityUp, 15, 0.5f * Time.deltaTime);
        deccelerationRate = Mathf.Lerp(deccelerationRate, 40, 0.5f * Time.deltaTime);
        scale = Mathf.Lerp(scale,0.1f, 0.5f* Time.deltaTime); 
    }

    public void shoot()
    {
        released = true;
        _alarmTimer.startAlarm();
    }

    // Update is called once per frame
    void Update()
    {
        //decrease all tradjectories
        //slow player down to halt
        if(!released)
        {
            grow();
        } 
        else
        {
            velocityUp -= deccelerationRate * Time.deltaTime;
            
            if (velocityUp < fallSpeed)
                velocityUp = fallSpeed;

            transform.position += transform.forward * (velocityFWD) * Time.deltaTime;
            transform.position += transform.up * velocityUp * Time.deltaTime;
        }

        
        //transform.position += new Vector3(throwDirectionX,0,0) * velocitySide * Time.deltaTime;


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" && released)
        {
            _alarmTimer.stopAlarm();
            Destroy(gameObject);
        }
    }
}
