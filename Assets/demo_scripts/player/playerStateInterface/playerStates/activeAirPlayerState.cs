using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeAirPlayerState : i_PlayerState
{
    public void onEnter(player p)
    {
        p.animController.Play("run");
        //register action

    }

    public void onExit(player p)
    {
        //do something here    

        //deregister the actions for this input
    }

    public void update(player p)
    {

        //I want directional movemnet based on controller stick direction
        Matrix4x4 camRot = Matrix4x4.Rotate(Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0));

        //do something here

        if (p.leftStick != Vector2.zero)
        {
            p.velocityFWD += p.accelerationH * Time.deltaTime;

            if (p.velocityFWD >= p.velocityHLmit * p.pressure)
            {
                p.velocityFWD = p.velocityHLmit * p.pressure;
            }

            if (p.leftStick.x != 0)
            {
                //DP adventures style camera turning
                // not good if the player is supposed to shoot on the move for 3rd person shooter game
                //cam.targetAngleH += -(leftStick.x*camSpeed) * Time.deltaTime;
            }

            //the current forward needs to be relative to the camera's orientation
            p.currentForward = new Vector3(p.leftStick.x, 0, p.leftStick.y);
            p.currentForward.Normalize();

            float c2pAngle = Vector3.Angle(new Vector3(0, 0, 1), new Vector3(p.transform.forward.x, 0, p.transform.forward.z));

            if (Vector3.Dot(new Vector3(1, 0, 0), new Vector3(p.transform.forward.x, 0, p.transform.forward.z)) < 0)
            {
                p.targetAngle = (c2pAngle);
            }
            else
            {
                p.targetAngle = (360 - c2pAngle);
            }

            //new Matrix4x4(new Vector4(), transform.localToWorldMatrix.GetColumn(1), new Vector4(),new Vector4());
            p.transform.forward = camRot * p.currentForward;
        }

        //slow player down to halt
        p.velocityFWD -= p.deccelerationH * Time.deltaTime;
        if (p.velocityFWD <= 0)
        {
            p.velocityFWD = 0;
        }

        //remember the last direction input by the stick to keep facing that direction
        Vector3 direction = camRot * p.currentForward * p.velocityFWD * Time.deltaTime;

        //direction = p.horizontalCollision(direction, camRot, p.currentForward, p.pLastDir, p.pPrevPosition, p.pTouchingWall);

        //move horizontally
        p.transform.Translate(direction, Space.World);


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

                if (p.leftStick != Vector2.zero && p.velocityHLmit == p.runningSpeedLimit)
                    p.switchStates(playerStates.running);
                else if (p.leftStick != Vector2.zero && p.velocityHLmit == p.walkingSpeedLimit)
                    p.switchStates(playerStates.walking);
                else if (p.leftStick == Vector2.zero)
                    p.switchStates(playerStates.idle);
            }
        }

        Vector3 directionUp = new Vector3(0, 1, 0) * p.velocityUP * Time.deltaTime;

        //move vertically
        p.transform.Translate(directionUp, Space.World);
    }

}
