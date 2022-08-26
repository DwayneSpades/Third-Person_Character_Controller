using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class meleePlayerState : i_PlayerState
{
    public void onEnter(player p)
    {
        p.animController.Play("water");
        //register action
        p.velocityFWD = p.velocityHLmit;

        if (p.pState == playerStates.midAir)
        {
            UnityAction a = () => { p.switchStates(playerStates.midAir); };
            p.activatetAlarmTimer(0.3f, a);
            p.startAlarmTimer();
        }
        else if (p.pState == playerStates.walking )
        {
            UnityAction a = () => { p.switchStates(playerStates.walking); };
            p.activatetAlarmTimer(0.3f, a);
            p.startAlarmTimer();
        }
        else if (p.pState == playerStates.running)
        {
            UnityAction a = () => { p.switchStates(playerStates.running); };
            p.activatetAlarmTimer(0.3f, a);
            p.startAlarmTimer();
        }
        else
        {
            UnityAction a = () => { p.switchStates(playerStates.idle); };
            p.activatetAlarmTimer(0.3f, a);
            p.startAlarmTimer();
        }

        p.lightHitbox.SetActive(true);

    }

    public void onExit(player p)
    {
        Debug.Log("hit over");
        //turn off hit box
        p.lightHitbox.SetActive(false);
        //do something here    
        p.stopAlarmTimer();
        p.deactivateAlarm();
        p.hitSuccess = false;
        //deregister the actions for this input
    }

    public void update(player p)
    {
        if (p.hitSuccess && p.pState==playerStates.midAir || p.hitSuccess && p.pState == playerStates.dash)
        {
            p.velocityUP = 8.5f;
            p.switchStates(playerStates.midAir);
        }

        //I want directional movemnet based on controller stick direction
        p.camRot = Matrix4x4.Rotate(Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0));

        
        if (p.leftStick.x != 0)
        {
            //DP adventures style camera turning
            // not good if the player is supposed to shoot on the move for 3rd person shooter game
            //cam.targetAngleH += -(leftStick.x*camSpeed) * Time.deltaTime;
        }

        //the current forward needs to be relative to the camera's orientation
        

        float c2pAngle = Vector3.Angle(new Vector3(0, 0, 1), new Vector3(p.transform.forward.x, 0, p.transform.forward.z));

        if (Vector3.Dot(new Vector3(1, 0, 0), new Vector3(p.transform.forward.x, 0, p.transform.forward.z)) < 0)
        {
            p.targetAngle = (c2pAngle);
        }
        else
        {
            p.targetAngle = (360 - c2pAngle);
        }

        //new Matrix4x4(new Vector4(), transform.localToWorldMatrix.GetColumn(1), new Vector4(),new Vector4())
        //remember the last direction input by the stick to keep facing that direction

        p.direction = p.transform.forward;
        p.direction = p.horizontalCollision(p.direction);

        // _rotation = Quaternion.Euler(direction);

        //_position = _position + direction ;

        p.mPrevPosition = p.transform.position;
        //move horizontally
        p.transform.position += p.velocityFWD * (p.direction) * Time.deltaTime;


        falling(p);
    }

    public void falling(player p)
    {
        p.ray = new Ray(p.transform.position, -p.transform.up);
        Debug.DrawRay(p.transform.position, -p.transform.up, Color.blue);


        p.velocityUP -= p.deccelerationV * Time.deltaTime;
        if (p.velocityUP <= p.fallSpeed)
        {
            p.velocityUP = p.fallSpeed;
        }


        p.ray = new Ray(p.transform.position, -p.transform.up);



        if (Physics.Raycast(p.ray, out p.hit, p.footSensor))
        {
            Debug.DrawLine(p.ray.origin, p.hit.point);
            //Debug.Log(hit.collider.name);

            if (p.hit.collider.tag == "ground" && p.velocityUP <= 0)
            {
                p.transform.position = new Vector3(p.transform.position.x, p.hit.point.y + p.footResponce, p.transform.position.z);
                //p.transform.position = Vector3.Lerp(p.transform.position, new Vector3(p.transform.position.x, p.hit.point.y + p.footResponce, p.transform.position.z), p.footResponceRate * Time.deltaTime);
                p.velocityUP = 0;
                p.onGround = true;


            }
        }
        else
        {
            Debug.Log("not touching ground");
            //p.switchStates(playerStates.activeAir);
        }

        Vector3 directionUp = new Vector3(0, 1, 0) * p.velocityUP * Time.deltaTime;

        //move vertically
        p.transform.Translate(directionUp, Space.World);
    }


}
