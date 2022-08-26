using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class runPlayerState : i_PlayerState
{

    public void onEnter(player p)
    {
        p.animController.Play("run");
        //register action
        p.velocityHLmit = p.runningSpeedLimit;
        p.animController.speed = 1.5f;
        p.turnRate = 0.08f;
    }

    public void onExit(player p)
    {
        //do something here    
        p.animController.speed = 1f;

        p.model.transform.localRotation = Quaternion.Euler(0,p.transform.rotation.y, 0);
        //deregister the actions for this input
    }
    Vector3 oldForward;
    float angleDif;
    public void update(player p)
    {

        //I want directional movemnet based on controller stick direction
        p.camRot = Matrix4x4.Rotate(Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0));

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
                //p.cam.targetAngleH += -(p.leftStick.x*p.camSpeed) * Time.deltaTime;
            }

            //the current forward needs to be relative to the camera's orientation
            p.mMovementVector = new Vector3(p.leftStick.x, 0, p.leftStick.y);
            p.mMovementVector.Normalize();

            //Debug.Log("shortest angle between: " + (Mathf.DeltaAngle(p.cam.theta * Mathf.Rad2Deg, p.targetAngle)));

            float c2pAngle = Vector3.Angle(new Vector3(0, 0, 1), new Vector3(p.transform.forward.x, 0, p.transform.forward.z));

            if (Vector3.Dot(new Vector3(1, 0, 0), new Vector3(p.transform.forward.x, 0, p.transform.forward.z)) < 0)
            {
                p.prevTargetAngle = p.targetAngle;
                p.targetAngle = (c2pAngle);
            }
            else
            {
                p.targetAngle = (360 - c2pAngle);
            }

            //new Matrix4x4(new Vector4(), transform.localToWorldMatrix.GetColumn(1), new Vector4(),new Vector4());
            p.prevFWD = p.transform.forward;
            p.transform.forward = Vector3.Lerp(p.transform.forward, p.camRot * p.mMovementVector, p.turnRate);


            float LR_Verdict = Mathf.Clamp(Vector3.Dot(p.transform.right, p.prevFWD), -0.01f, 0.01f);
            float turnAngle = Vector3.Angle(p.prevFWD, p.transform.forward);
            float tiltAngle = turnAngle * LR_Verdict;

            //Debug.Log("Delta Angle: " + turnAngle);

            p.model.transform.eulerAngles = new Vector3(p.transform.eulerAngles.x, p.transform.eulerAngles.y, tiltAngle * 800);

        }
        else
        {

            p.model.transform.eulerAngles = new Vector3(p.transform.eulerAngles.x, p.transform.eulerAngles.y, 0);
            p.switchStates(playerStates.idle);
        }

        //remember the last direction input by the stick to keep facing that direction
        Vector3 direction = p.camRot * p.currentForward * p.velocityFWD * Time.deltaTime;

        //direction = p.horizontalCollision(direction, camRot, p.currentForward, p.pLastDir, p.pPrevPosition, p.pTouchingWall);

        // _rotation = Quaternion.Euler(direction);

        //_position = _position + direction ;

        //remember the last direction input by the stick to keep facing that direction
        p.direction = p.camRot * p.mMovementVector;

        p.direction = p.horizontalCollision(p.direction);

        // _rotation = Quaternion.Euler(direction);

        //_position = _position + direction ;

        p.mPrevPosition = p.transform.position;
        //move horizontally
        p.transform.position += p.velocityFWD * (p.direction) * Time.deltaTime;



        //falling
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
            p.switchStates(playerStates.activeAir);
        }

        Vector3 directionUp = new Vector3(0, 1, 0) * p.velocityUP * Time.deltaTime;

        //move vertically
        p.transform.Translate(directionUp, Space.World);
    }
}
