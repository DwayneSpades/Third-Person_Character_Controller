using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : myTransform
{
    public GameObject model;
    public GameObject weapon;
    public GameObject lightHitbox;
    public GameObject mediumHitbox;
    public GameObject heavyHitbox;

    public camera cam;
    public Transform arm;

    public Animator animController;
    public PlayerInput playerInput;
    public Vector3 currentForward;

    public float camSpeed = 2.5f;
    public float velocityFWD;
    public float velocityUP;

    public float accelerationH;
    public float deccelerationH;

    public float accelerationV;
    public float deccelerationV;

    public float velocityHLmit;
    public float walkingSpeedLimit = 8;
    public float runningSpeedLimit = 12;

    public float jumpHeight = 5;
    public float fallSpeed = -10;
    public float footSensor = 1;
    public float footResponce = 1;
    public float footResponceRate = 0.8f;

    public Vector2 leftStick;
    public Vector2 rightStick;
    public float pressure;

    public float targetAngle = 0;

    public i_PlayerState currentState;

    public Dictionary<playerStates,i_PlayerState> states;

    public Transform camFocusPoint;

    public float rotRate=0.5f;
    public float turnRate = 0;

    public playerStates cState;
    public playerStates pState;

    public alarmTimer getAlarmTimer() { return _alarmtimer.GetComponent<alarmTimer>(); }

    //start the alarm
    // 
    //this should be in a different class by itself because of how generic it is
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
    public Vector2 prevStickDir;
    public void Start()
    {
        lightHitbox.SetActive(false);

        //cap frame rate at 60FPS
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //set camera for player
        cam = FindObjectOfType<camera>();

        _alarmtimer = new GameObject();
        _alarmtimer.AddComponent<alarmTimer>();

        //initialize input
        playerInput = new PlayerInput();
        playerInput.playerController.Enable();

        //I'll be using a lambda expression
        // I have to define a lambda expresstion by a context
        //defined lambda for reading left stick movement values
        playerInput.playerController.left_stick.performed += context => 
        {
            prevStickDir = leftStick;
            leftStick = context.ReadValue<Vector2>();
        };

        playerInput.playerController.left_stick.canceled += context => 
        {
            prevStickDir = leftStick;
            leftStick = Vector2.zero;
        };

        playerInput.playerController.right_stick.performed += context => {
            rightStick = context.ReadValue<Vector2>();
        };

        playerInput.playerController.mouse_look.performed += context => {
            rightStick = Mouse.current.delta.ReadValue() * Time.smoothDeltaTime;
        };

        playerInput.playerController.mouse_look.canceled += context => rightStick = Vector2.zero;

        playerInput.playerController.right_stick.canceled += context => rightStick = Vector2.zero;
        
        playerInput.playerController.snapCamFWD.performed += context => setCamAngle();
        playerInput.playerController.jump.performed += context => jump();
        playerInput.playerController.sprint.performed += context => run();
        playerInput.playerController.zTarget.performed += context => { zTarget();};

        playerInput.playerController.dash.performed += context => dash();

        playerInput.playerController.attack.performed += context => attack();
        playerInput.playerController.attack.canceled += context => release();

        velocityFWD = 0;
        velocityUP = 0;
        targetHeight = cam.setCamHeight;

        //initialize states
        states = new Dictionary<playerStates, i_PlayerState>();
        states.Add(playerStates.idle, new idlePlayerState());
        states.Add(playerStates.walking, new walkPlayerState());
        states.Add(playerStates.running, new runPlayerState());
        states.Add(playerStates.midAir, new midAirPlayerState());
        states.Add(playerStates.activeAir, new activeAirPlayerState());
        states.Add(playerStates.melee, new meleePlayerState());
        states.Add(playerStates.dash, new strafeDashPlayerState());

        cState = playerStates.idle;
        pState = playerStates.idle;

        currentState = states[playerStates.idle];
        currentState.onEnter(this);

        //initialize transform
        _position = transform.position;
        _rotation = transform.rotation;
        _scale = transform.localScale;
        computeTransform();
    }
    float velSideDelta;
    GameObject temp;

    public void attack()
    {
        if (cState != playerStates.melee)
            switchStates(playerStates.melee);
        //temp = Instantiate(weapon,arm.position,arm.rotation);
        //temp.GetComponent<pumpkinBlast>().arm = arm;
        //temp.GetComponent<pumpkinBlast>().initialize();


    }

    public void dash()
    {
        if (cState != playerStates.dash && cam.zTargeting == true)
            switchStates(playerStates.dash);
        //temp = Instantiate(weapon,arm.position,arm.rotation);
        //temp.GetComponent<pumpkinBlast>().arm = arm;
        //temp.GetComponent<pumpkinBlast>().initialize();

        
    }
    
    public void release()
    {
        Debug.Log("released");
        //temp.GetComponent<pumpkinBlast>().shoot();
        //temp.transform.forward = arm.forward;
    }
    public void undoLock()
    {
        if (strafeTarget)
        {
            cam.zTargeting = false;
            cam.maxDistance = 4.3f;
            cam.setCamHeight = 1.72f;
            cam.camTarget = gameObject;
            strafeTarget = null;
        }
    }

    public void zTarget()
    {
        if (viewTarget.name.Contains("ghast") & !strafeTarget)
        {
            strafeTarget = viewTarget;
            cam.zTargeting = true;
            targetHeight = 0 + (transform.position.y -strafeTarget.transform.position.y);
            cam.setCamHeight = 0;
            cam.camTarget = viewTarget;
        }
        else if(strafeTarget)
        {
            cam.zTargeting = false;
            cam.maxDistance = 4.3f;
            cam.setCamHeight = 1.72f;
            cam.camTarget = gameObject;
            strafeTarget = null;
        }
            
    }

    //switch states and execute on enter and exit
    public void switchStates(playerStates state)
    {
        pState = cState;
        cState = state;

        currentState.onExit(this);
        currentState = states[state];
        currentState.onEnter(this);
    }

    public RaycastHit hit = new RaycastHit();
    public Ray ray;
    
    public bool onGround = true;
    public float force = 5;

    Vector3 oldPosition;
    public Vector3 oldFWD;

    public float verticleAngleMax = 2.0f;
    public float verticleAngleMin = 0.9f;

    public GameObject viewTarget = null;
    public GameObject strafeTarget=null;
    public float LR_Verdict;
    public float turnAngle;
    public void FixedUpdate()
    {
        

        velSideDelta =  transform.position.x - oldPosition.x;
        oldPosition = transform.position;

        if (rightStick.x != 0)
        {
            cam.targetAngleH += -rightStick.x * camSpeed * Time.deltaTime;
        }

        if (rightStick.y != 0)
        {
            cam.targetAngleV += -rightStick.y * camSpeed * Time.deltaTime;
            if (cam.targetAngleV > verticleAngleMax)
            {
                cam.targetAngleV = verticleAngleMax;
            }
            else if (cam.targetAngleV < verticleAngleMin)
            {
                cam.targetAngleV = verticleAngleMin;
            }
        }

        pressure = (Mathf.Abs(leftStick.x) + Mathf.Abs(leftStick.y));
        pressure = Mathf.Clamp(pressure,0,1);

        //shooting
        Ray ray = new Ray(cam.transform.position,cam.transform.forward);

        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit,100, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
        {
            Debug.DrawRay(arm.transform.position, hit.point - arm.position);

            arm.forward = hit.point - arm.position;
            viewTarget = hit.transform.gameObject;

            //Debug.Log("Camera Collision");
        }


        
        //update curernt state
        currentState.update(this);
        if (strafeTarget)
            camForceAngle();
        
        //raycastCollision();

        //resolve collisions

        //cam.positionCamera(this);
        //compute current transform
        //computeTransform();

        //collision variables Delta
    }

    public bool hitSuccess = false;
    void jump()
    {
        if (currentState != states[playerStates.midAir] && cState != playerStates.melee)
        {
            Debug.Log("Jump");
            onGround = false;
            velocityUP = jumpHeight;
            switchStates(playerStates.midAir);
        }
    }

    void run()
    {
        switchStates(playerStates.running);
    }

    public float targetAngleV = 0;
    float targetHeight = 0;
    public float zTargetOffsetX = 0;
    public void camForceAngle()
    {
        float c2pAngle = Vector3.Angle(new Vector3(0, 0, 1), -((transform.position + transform.right * zTargetOffsetX)-strafeTarget.transform.position).normalized);
        float c2pAngleV = Vector3.Angle(new Vector3(0, -1, 0), -((transform.position + transform.right * zTargetOffsetX) - strafeTarget.transform.position).normalized);

        if (Vector3.Dot(new Vector3(1, 0, 0), -((transform.position + transform.right * zTargetOffsetX) - strafeTarget.transform.position).normalized) < 0)
        {
           prevTargetAngle = targetAngle;
            targetAngle = (c2pAngle);
        }
        else
        {
            targetAngle = (360 - c2pAngle);
        }

        if (Vector3.Dot(new Vector3(0, -1, 0), -((transform.position + transform.right * zTargetOffsetX) - strafeTarget.transform.position).normalized) < 0)
        {
            //prevTargetAngle = targetAngle;
            targetAngleV = (c2pAngleV);
        }
        else
        {
            targetAngleV = (360 - c2pAngleV);
        }

        cam.targetAngleH = ((cam.theta * Mathf.Rad2Deg) + (Mathf.DeltaAngle(cam.theta * Mathf.Rad2Deg, targetAngle))) * Mathf.Deg2Rad;
        //cam.targetAngleV = ((cam.phi * Mathf.Rad2Deg) + (Mathf.DeltaAngle(cam.phi * Mathf.Rad2Deg, targetAngleV))) * Mathf.Deg2Rad;

        //targetHeight = targetHeight + (transform.position.y - oldPosition.y);
        //cam.setCamHeight = Mathf.Lerp(cam.setCamHeight,targetHeight ,1);
        
    }

    void setCamAngle()
    {
        //to solve the problem of interpolating across shortest possible angle between to angles
        //I just let the theta of the spherical coordinates loop over or under 360 by simply
        // adding the angle difference to the current theta and interpolating from there.
        // equation for shortest angle path
        //Debug.Log("FROM: " + (cam.theta * Mathf.Rad2Deg) + " TO: " + targetAngle);
        cam.targetAngleH = ((cam.theta * Mathf.Rad2Deg) + (Mathf.DeltaAngle(cam.theta * Mathf.Rad2Deg, targetAngle))) * Mathf.Deg2Rad;

        Debug.Log("shortest angle between: " + (Mathf.DeltaAngle(cam.theta * Mathf.Rad2Deg, targetAngle)));
    }

    void enable()
    {
        playerInput.playerController.Enable();
    }

    void disable()
    {
        playerInput.playerController.Disable();
    }

    public Vector3 direction;

    public Vector3 plastWallNormalDir;
    //public Vector3 wallNormall2;

    public bool touchingWall = false;
    public Vector3 mPrevPosition;
    public Matrix4x4 camRot;
    public Vector3 mMovementVector;
    public Vector3 lastDir;

    public Vector3 prevFWD;
    public float deltaAngle = 0;
    public float prevTargetAngle = 0;


    public float originalwallEscapeAngle = 70;
    public float wallEscapeAngle = 10;

    public Vector3 horizontalCollision(Vector3 dirNorm)
    {

        float distDelta = Vector3.Distance(mPrevPosition, transform.position);

        Ray ray = new Ray(transform.position, dirNorm);
        RaycastHit hit;
        RaycastHit hit2;

        Vector3 dirr = camRot * new Vector3(mMovementVector.x, 0, mMovementVector.z);
        //RaycastHit[] hits = Physics.RaycastAll(mPrevPosition, dirNorm, distDelta);

        if (mMovementVector != Vector3.zero)
            lastDir = dirNorm;

        bool contact1 = false;
        bool contact2 = false;

        Debug.DrawRay(transform.position, dirr, Color.cyan);

        float radius = 1.05f;

        Vector3 wall1 = new Vector3();
        Vector3 wall2 = new Vector3();


        Vector3 cornerVect = new Vector3();

        Vector3 colDir = new Vector3();
        touchingWall = false;


        //identify wall normals
        if (Physics.SphereCast(transform.position + transform.up * 0.1f, 1, dirr, out hit))
        {
            Debug.DrawRay(transform.position, -hit.normal, Color.green);

            wall1 = -hit.normal;

            //float collisionDistance = Vector3.Distance(hit.point,transform.position);

            colDir = dirr - hit.normal * Vector3.Dot(dirr, hit.normal);
        }

        if (Physics.SphereCast(transform.position + transform.up * 0.1f, 1f, new Vector3(colDir.x, 0, colDir.z), out hit2))
        {

            wall2 = -hit2.normal;
            Debug.DrawRay(transform.position, wall2, Color.yellow);

            cornerVect = -(hit.normal + hit2.normal);
            cornerVect.Normalize();


        }

        //corner ray
        Debug.DrawRay(transform.position, cornerVect, Color.red);

        //divert player direction
        if (Physics.SphereCast(transform.position + transform.up * 0.1f, 1, dirr, out hit))
        {
            if (Vector3.Distance(transform.position, hit.point) <= radius)
            {

                float angle = Vector3.Angle(hit.normal, new Vector3(0, 1, 0));

                if (angle > 70 && hit.transform.name!="hitBox")
                {
                    dirNorm = dirr - hit.normal * Vector3.Dot(dirr, hit.normal);
                    //direction = transform.position += -walllNormal * seperation;
                }
            }
        }

        Vector3 contactNormal = new Vector3();
        Vector3 contactNormal2 = new Vector3();

        //wallNormal1.text = "wall Normal: "+ (-wall1);
        //wallNormal2.text = "wall 2 Normal: " + (-wall2);

        if (wall1 != wall2)
        {
            //tier 2 corner test
            if (Physics.SphereCast(transform.position + transform.up * 0.1f, 1, dirNorm, out hit2))
            {
                if (Vector3.Distance(transform.position, hit2.point) <= radius)
                    if (hit.normal != new Vector3(hit2.normal.x, 0, hit2.normal.z))
                    {

                        touchingWall = true;
                        float angle = Vector3.Angle(hit2.normal, new Vector3(0, 1, 0));

                        if (angle > 70 && hit.transform.name != "hitBox")
                        {
                            dirNorm = new Vector3(0, 0, 0);
                        }
                    }
            }

            //identify wall normals
            if (Physics.SphereCast(transform.position + transform.up * 0.1f, 1, wall1, out hit))
            {
                //Debug.DrawRay(transform.position,wall1,Color.green);

                contactNormal = hit.normal;


                if (Vector3.Distance(transform.position, hit.point) <= radius)
                {
                    if (-contactNormal == wall1)
                    {
                        contact1 = true;
                        //lastWallNormalDir = hit.normal;
                        touchingWall = true;
                    }
                }

            }

            //identify wall normals
            if (Physics.SphereCast(transform.position + transform.up * 0.1f, 1, wall2, out hit2))
            {

                contactNormal2 = hit2.normal;


                if (Vector3.Distance(transform.position, hit2.point) <= radius)
                    if (-contactNormal2 == wall2)
                    {
                        contact2 = true;
                        //lastWallNormalDir = hit2.normal;
                        touchingWall = true;
                    }
            }
        }

        if (contact1 && contact2)
        {
            if (Vector3.Angle(dirr, cornerVect) < Vector3.Angle(wall1, wall2))
            {
                float angle = Vector3.Angle(hit.normal, new Vector3(0, 1, 0));
                float angle2 = Vector3.Angle(hit2.normal, new Vector3(0, 1, 0));

                if (angle > 70 && angle2 > 70 && hit.transform.name != "hitBox")
                {
                    dirNorm = new Vector3(0, 0, 0);
                }
            }
        }

        return dirNorm;
    }
    void OnCollisionStay(Collision col)
    {

        ContactPoint point = col.GetContact(0);
        Vector3 walllNormal = point.normal;
        walllNormal.Normalize();

        float seperation = point.separation;
        

        float angle = Vector3.Angle(walllNormal, new Vector3(0, 1, 0));
        //Debug.Log("intersecting at: " + angle + " angle");

        if (angle > 70 && col.transform.name != "hitBox")
        {
            transform.position += -walllNormal * seperation;
        }
    }

    /*
    void falling()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        Debug.DrawRay(transform.position, -transform.up, Color.blue);

        speedUp -= verticalAcceleration * Time.deltaTime;
        if (speedUp <= maxVerticalSpeed)
        {
            speedUp = maxVerticalSpeed;
        }

        Debug.DrawLine(transform.position, transform.position - transform.up * 1.5f);

        if (Physics.Raycast(ray, out hit, 1.8f))
        {
            if (speedUp <= 0)
            {
                onGround = true;
                availableJumps = maxJumps;

                Debug.DrawRay(ray.origin, hit.point);
                transform.position = new Vector3(transform.position.x, hit.point.y + 1, transform.position.z);
                speedUp = 0;
            }
        }

        Vector3 directionUp = new Vector3(0, 1, 0) * speedUp * Time.deltaTime;

        //move vertically
        transform.Translate(directionUp, Space.World);
    }
    */
    /*
     void raycastCollision()
     {
         float rH = -0.5f;
         //collision resolution
         ray = new Ray(transform.position + new Vector3(0, 1, 0) * rH, transform.forward);

         float legnts = 0.8f;
         Debug.DrawRay(transform.position + new Vector3(0, 1, 0) * rH, transform.forward, Color.cyan, legnts);
         Debug.DrawRay(transform.position + new Vector3(0, 1, 0) * rH, transform.right, Color.cyan, legnts);
         Debug.DrawRay(transform.position + new Vector3(0, 1, 0) * rH, -transform.right, Color.cyan, legnts);
         Debug.DrawRay(transform.position + new Vector3(0, 1, 0) * rH, -transform.right + transform.forward, Color.cyan, legnts);
         Debug.DrawRay(transform.position + new Vector3(0, 1, 0) * rH, transform.right + transform.forward, Color.cyan, legnts);
         Debug.DrawRay(transform.position + new Vector3(0, 1, 0) * rH, -transform.right - transform.forward, Color.cyan, legnts);
         Debug.DrawRay(transform.position + new Vector3(0, 1, 0) * rH, transform.right - transform.forward, Color.cyan, legnts);

         //collision with walls
         if (Physics.Raycast(ray, out hit, legnts))
         {
             Vector3 forceDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
             transform.position -= forceDir * force * Time.deltaTime;
             //Debug.Log("Camera Collision");
         }
         ray = new Ray(transform.position + new Vector3(0, 1, 0) * rH, transform.right);

         //collision with walls
         if (Physics.Raycast(ray, out hit, legnts))
         {
             Vector3 forceDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
             transform.position -= forceDir * force * Time.deltaTime;
             //Debug.Log("Camera Collision");
         }
         ray = new Ray(transform.position + new Vector3(0, 1, 0) * rH, -transform.right);

         //collision with walls
         if (Physics.Raycast(ray, out hit, legnts))
         {
             Vector3 forceDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
             transform.position -= forceDir * force * Time.deltaTime;
             //Debug.Log("Camera Collision");
         }

         ray = new Ray(transform.position + new Vector3(0, 1, 0) * rH, transform.right + transform.forward);

         //collision with walls
         if (Physics.Raycast(ray, out hit, legnts))
         {
             Vector3 forceDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
             transform.position -= forceDir * force * Time.deltaTime;
             //Debug.Log("Camera Collision");
         }
         ray = new Ray(transform.position + new Vector3(0, 1, 0) * rH, -transform.right + transform.forward);

         //collision with walls
         if (Physics.Raycast(ray, out hit, legnts))
         {
             Vector3 forceDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
             transform.position -= forceDir * force * Time.deltaTime;
             //Debug.Log("Camera Collision");
         }
     }
     */

    void falling()
    {
        ray = new Ray(transform.position, -transform.up);
        Debug.DrawRay(transform.position, -transform.up, Color.blue);


        velocityUP -= deccelerationV * Time.deltaTime;
        if (velocityUP <= fallSpeed)
        {
            velocityUP = fallSpeed;
        }


        ray = new Ray(transform.position, -transform.up);



        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            Debug.DrawLine(ray.origin, hit.point);
            //Debug.Log(hit.collider.name);

            if (hit.collider.tag == "ground" && velocityUP < 0)
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + 1, transform.position.z);
                velocityUP = 0;
                onGround = true;

                if (leftStick != Vector2.zero)
                    switchStates(playerStates.running);
                else if (leftStick == Vector2.zero)
                    switchStates(playerStates.idle);
            }
        }

        Vector3 directionUp = new Vector3(0, 1, 0) * velocityUP * Time.deltaTime;

        //move vertically
        transform.Translate(directionUp, Space.World);
    }

}
