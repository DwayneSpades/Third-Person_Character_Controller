using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{

    //wall jump timer
    GameObject wallJumpTimer;

    public enum WEAPON_TYPE
    {
        PISTOL,
        MACHINE_GUN,
        SHOTGUN,
        NUM_TYPES
    }

    [Header("Movement")]
    [SerializeField] private Transform cameraTransform = null;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float jumpForce = 500f;
    [SerializeField] private float headRotationLimit = 90f;
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float healthbarSize = 1000f;
    public Text wallNormal1;
    public Text wallNormal2;
    public Text playerPos;

    [SerializeField] private Transform body;

    public float headRotation;


    private float mJumpEnabledDist;
    
    private PlayerInputActions mPlayerInputActions;
    private Vector3 mMovementVector = new Vector3(0f, 0f, 0f);
    private Vector2 rightStick;
    [SerializeField] private int maxJumps = 1;
    private int availableJumps = 2;
    private float startJumpTime;
    private bool onGround;

     //Preston's stuff - hello :^)
    public float verticalAcceleration = 2;
    public float horizontalAcceleration = 5;

    public float maxVerticalSpeed = -35;
    public float maxHorizontalSpeed = 15;
    public float speedFWD=0;
    public float speedUp=0;
    public float collisionForce = 0.5f;


    [Header("Combat")]
 

    public Text waveCounter;
    public Text tip;

    private bool touchingWall=false;
    public bool jumpingOffWall=false;
    void resetJump(){
        jumpingOffWall = false;
    }
    

    bool jumping = false;
    // Start is called before the first frame update
    void Start()
    { 
        //set up alarm for when wall jumps happen
        wallJumpTimer = new GameObject();
        wallJumpTimer.AddComponent<alarmTimer>();

        UnityAction a = () => 
        {
            resetJump();
            Debug.Log("wall jump timer TRIGGERED");
        };
        
        wallJumpTimer.GetComponent<alarmTimer>().addMethod(a);
        wallJumpTimer.GetComponent<alarmTimer>().setDurationSeconds(0.3f);

        // Lock and Hide the Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        mPlayerInputActions = new PlayerInputActions();
        mPlayerInputActions.Enable();
        mPlayerInputActions.Player.Jump.performed += Jump;

        mPlayerInputActions.Player.Jump.canceled += context => 
        { 
            jumping = false;
            if(speedUp > 0) 
            {
                
                speedUp = speedUp/2;
            }
        };

        mPlayerInputActions.Player.RotationX.performed += OnRotationX;
        mPlayerInputActions.Player.RotationY.performed += OnRotationY;

        mPlayerInputActions.Player.Move.performed += context => mMovementVector = context.ReadValue<Vector2>();
        mPlayerInputActions.Player.Move.canceled += context => mMovementVector = Vector2.zero;

        mPlayerInputActions.Player.controllerLook.performed += context => rightStick = context.ReadValue<Vector2>();
        mPlayerInputActions.Player.controllerLook.canceled += context => rightStick = Vector2.zero;

        mPlayerInputActions.Player.Shoot.performed += Shoot;

        mJumpEnabledDist = this.transform.position.y;
        
        

        mPlayerInputActions.Player.Quit.performed += Quit;
        availableJumps = maxJumps;

        mPrevPosition = transform.position;
        hurt(0f);

    }

    private void OnDestroy()
    {
        mPlayerInputActions.Player.Jump.performed -= Jump;
        mPlayerInputActions.Player.RotationX.performed -= OnRotationX;
        mPlayerInputActions.Player.RotationY.performed -= OnRotationY;

        mPlayerInputActions.Player.Move.performed -= Move;
        mPlayerInputActions.Player.Move.canceled -= Move;

        mPlayerInputActions.Player.controllerLook.performed -= context => rightStick = context.ReadValue<Vector2>();
        mPlayerInputActions.Player.controllerLook.canceled -= context => rightStick = Vector2.zero;

        mPlayerInputActions.Player.Shoot.performed -= Shoot;

        mPlayerInputActions.Disable();
    }

    // Update is called once per frame
    

    public Vector3 GetVelocity()
    {
        return rigidbody.velocity;
    }

    private Vector3 mPrevPosition;
    private Vector3 direction;
    private Vector3 lastDir;


    private Vector3 lastWallNormalDir;

    private Matrix4x4 camRot;

    private void FixedUpdate()
    {
        

        
        //move camera with controller
        if (rightStick != Vector2.zero)
        {
            this.transform.Rotate(new Vector3(0f, rightStick.x * sensitivity, 0f));

            headRotation -= rightStick.y * sensitivity;
            headRotation = Mathf.Lerp(headRotation, Mathf.Clamp(headRotation, -headRotationLimit, headRotationLimit), 0.05f);    // there's a limit
            cameraTransform.localEulerAngles = new Vector3(headRotation, 0.0f, 0.0f);
        }
        
        //get the orientation of the cam to get the right direction to move the player in
        camRot = Matrix4x4.Rotate(Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0));

    //don't change direction and speed when jumping off wall
        if(!jumpingOffWall){
            if(mMovementVector != Vector3.zero)
            {
                Debug.Log("moveing stick and wasd");
                speedFWD += horizontalAcceleration*Time.deltaTime;

                if(speedFWD>=maxHorizontalSpeed){
                    speedFWD = maxHorizontalSpeed;
                }
            }
            else{
                speedFWD = 0;
            }
        
            direction = camRot * new Vector3(mMovementVector.x,0,mMovementVector.y);
            
        }


        //check for collision to do sliding responce

        //direction = direction - N * dot(direction , N);

        //check for horizontal collision through walls
        
        direction = horizontalCollision(direction);
        
        

        mPrevPosition = transform.position;
        transform.position += speedFWD *  direction * Time.deltaTime;

        
        //horizontalCollision2(direction);

        

        falling();

        //rest wall touch confirmation
        slideOnWall = false;
        jumping = false;
    }

    public float ignoreSidesAngle=45;

   Vector3 horizontalCollision(Vector3 dirNorm){
        
        float distDelta = Vector3.Distance(mPrevPosition, transform.position);

        Ray ray = new Ray(transform.position, dirNorm);
        RaycastHit hit;
        RaycastHit hit2;

        Vector3 dirr = camRot * new Vector3(mMovementVector.x,0,mMovementVector.y);
        //RaycastHit[] hits = Physics.RaycastAll(mPrevPosition, dirNorm, distDelta);

        if(mMovementVector!= Vector3.zero)
            lastDir = dirNorm;

        bool contact1 = false;
        bool contact2 = false;

        Debug.DrawRay(transform.position,dirr,Color.cyan);

        float radius = 1.2f;

        Vector3 wall1 = new Vector3();
        Vector3 wall2 = new Vector3();


        Vector3 cornerVect = new Vector3();

        Vector3 colDir = new Vector3();
        touchingWall = false;

        
        //identify wall normals
        if (Physics.SphereCast(transform.position+transform.up*0.1f,1,dirr,out hit)){
            Debug.DrawRay(transform.position,-hit.normal,Color.green);
            
            wall1 = -hit.normal;
            
            //float collisionDistance = Vector3.Distance(hit.point,transform.position);

            colDir = dirr - hit.normal * Vector3.Dot(dirr,hit.normal);
        }
        
        if(Physics.SphereCast(transform.position+transform.up*0.1f,1f,new Vector3(colDir.x,0,colDir.z),out hit2)){
           
            wall2 = -hit2.normal;
            Debug.DrawRay(transform.position,wall2,Color.yellow);

            cornerVect = -(hit.normal+hit2.normal);
            cornerVect.Normalize();

            
        }
        
        //corner ray
        Debug.DrawRay(transform.position,cornerVect,Color.red);

        //divert player direction
        if(Physics.SphereCast(transform.position+transform.up*0.1f,1,dirr,out hit)){
            if(Vector3.Distance(transform.position,hit.point)<=radius)
            {

                float angle = Vector3.Angle(hit.normal, new Vector3(0, 1, 0));

                if (angle > 70)
                {
                    dirNorm = dirr - hit.normal * Vector3.Dot(dirr,hit.normal);
                    //direction = transform.position += -walllNormal * seperation;
                }
            }
        }

        Vector3 contactNormal = new Vector3();
        Vector3 contactNormal2 = new Vector3();

        //wallNormal1.text = "wall Normal: "+ (-wall1);
        //wallNormal2.text = "wall 2 Normal: " + (-wall2);

        if(wall1 != wall2)
        {
            //tier 2 corner test
            if(Physics.SphereCast(transform.position + transform.up * 0.1f,1,dirNorm,out hit2)){
                if(Vector3.Distance(transform.position,hit2.point) <= radius)
                    if(hit.normal != new Vector3(hit2.normal.x,0,hit2.normal.z)){

                        touchingWall = true;
                        float angle = Vector3.Angle(hit2.normal, new Vector3(0, 1, 0));

                        if (angle > 70)
                        {
                            dirNorm = new Vector3(0, 0, 0);
                        }
                    }
            }

            //identify wall normals
            if(Physics.SphereCast(transform.position+transform.up*0.1f,1,wall1,out hit)){
                //Debug.DrawRay(transform.position,wall1,Color.green);

                contactNormal = hit.normal;
                

                if(Vector3.Distance(transform.position,hit.point)<=radius)
                {
                    if(-contactNormal == wall1){
                        contact1 = true;
                        lastWallNormalDir = hit.normal;
                        touchingWall = true;
                    }
                }
                    
            }

            //identify wall normals
            if(Physics.SphereCast(transform.position+transform.up*0.1f,1,wall2,out hit2)){
                
                contactNormal2 = hit2.normal;
                

                if(Vector3.Distance(transform.position,hit2.point)<=radius)
                    if(-contactNormal2 == wall2){
                        contact2 =true;
                        lastWallNormalDir = hit2.normal;
                        touchingWall = true;
                    }
            }
        }
        
        if(contact1 && contact2){
            if(Vector3.Angle(dirr,cornerVect) < Vector3.Angle(wall1,wall2)){
                float angle = Vector3.Angle(hit.normal, new Vector3(0, 1, 0));
                float angle2 = Vector3.Angle(hit2.normal, new Vector3(0, 1, 0));

                if (angle > 70 && angle2 > 70)
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
        Debug.Log("intersecting at: " + angle + " angle");

        if (angle > 70)
        {
            direction = transform.position += -walllNormal * seperation;
        }

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            onGround = true;
            availableJumps = maxJumps;

            Debug.DrawRay(ray.origin, hit.point);
            transform.position = new Vector3(transform.position.x, hit.point.y + 1, transform.position.z);
            speedUp = 0;

        }


    }

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

        Debug.DrawLine(transform.position,transform.position - transform.up * 1.5f);
        
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

    private void OnRotationX(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            this.transform.Rotate(new Vector3(0f, _context.ReadValue<float>() * sensitivity * Time.deltaTime, 0f));
        }
    }

    private void OnRotationY(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            headRotation -= _context.ReadValue<float>() * sensitivity * Time.deltaTime;
            headRotation = Mathf.Clamp(headRotation, -headRotationLimit, headRotationLimit);    // there's a limit
            cameraTransform.localEulerAngles = new Vector3(headRotation, 0.0f, 0.0f);
        }
    }
    Vector3 lastWallJumpDir;
    Vector3 kickDir;
    private void Jump(InputAction.CallbackContext _context)
    {
        jumping = true;

        if (_context.performed & availableJumps>0)
        {
            availableJumps--;
            if(touchingWall && !onGround){

                if(!jumpingOffWall)
                    lastWallJumpDir = camRot * new Vector3(mMovementVector.x,0,mMovementVector.y);

                if(lastWallJumpDir!= Vector3.zero){
                    //lastWallJumpDir = new Vector3(Camera.main.transform.forward.x,0,Camera.main.transform.forward.z);
                    //reset alarm to cancel walljump direction if you have a double jump
                    wallJumpTimer.GetComponent<alarmTimer>().stopAlarm();
                    

                    Debug.Log("Jump off the wall!!!");
                    //as long as there are walls to jump off of you can jump on them
                    jumpingOffWall=true;
                    availableJumps = 1;
                    wallJumpTimer.GetComponent<alarmTimer>().startAlarm();
                    kickDir = Vector3.Reflect(lastWallJumpDir,lastWallNormalDir); 
                    lastWallJumpDir = kickDir;
                    speedFWD = maxHorizontalSpeed*2;
                    //touchingWall = false;
                }
            }

            
            speedUp = jumpForce;
            onGround = false;

            //soundManager.getInstance().playSound("Jump");
            Debug.Log("Jump");
        }
    }

    public bool slideOnWall=false;
    
    Vector3 newDir;
    Plane collidedPlane;

    public bool onWall=false;
    void OnCollisionEnter(Collision col){
       
    }

    bool wideAngle = false;

    float dist = 0;  
    

    public void hurt(float _opacity)
    {
        //healthBar.GetComponent<RectTransform>().localScale.Set(healthBar.GetComponent<RectTransform>().localScale.x * _opacity, healthBar.GetComponent<RectTransform>().localScale.y, healthBar.GetComponent<RectTransform>().localScale.z);
    }

    private void Move(InputAction.CallbackContext _context)
    {
        
            Vector2 input = _context.ReadValue<Vector2>();
            mMovementVector = new Vector3(input.x, 0f, input.y) * maxSpeed;
    }

    private void Shoot(InputAction.CallbackContext _context)
    {
       
    }

    void Quit(InputAction.CallbackContext _context)
    {
        Application.Quit();
    }
}