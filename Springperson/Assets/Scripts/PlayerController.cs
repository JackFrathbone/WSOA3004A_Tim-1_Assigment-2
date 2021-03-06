using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Constants")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveSpeedPowerUp = 8f;
    [SerializeField] float moveMultiplier = 10f;
    [SerializeField] float airMoveMultiplier = 5f;
    [SerializeField] public float mouseSensitivity = 5f;
    [SerializeField] float groundDrag = 6f;
    [SerializeField] Transform orientation;
    
    float horzMovement;
    float vertMovement;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;



    [Header("Jump Constants")]
    [SerializeField] float airDrag = 2f;
    [SerializeField] float playerHeight = 2f;
    [SerializeField] float jumpForce = 10f;
    Vector3 globalGravity;
    [Range(0, 1)] [SerializeField] float AirMoveNerf;
    [Header("Ground Detection")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.4f;
    bool isGrounded;
    RaycastHit slopeHit;
    Vector3 lastVelocity;

    Rigidbody rb;
    float basicMoveSpd;

    

    // Start is called before the first frame update
    void Start()
    {
        globalGravity = Physics.gravity;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation= true;
        //rb.useGravity = false;
        basicMoveSpd = moveSpeed;
    }

    
    // Update is called once per frame
    void Update()
    {
       //isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);
       Debug.Log(isGrounded);
       if(rb.velocity.magnitude > 0){
           lastVelocity = rb.velocity;
        }
       
       if(Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.Space) && OnSlope()){
           Jump();
        }

       myInput();
       ControlDrag();
       slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }
    void ApplyGravity(){
        if(isGrounded || OnSlope()){
            rb.AddForce(slopeHit.normal * Physics.gravity.magnitude, ForceMode.Acceleration);

        }
        else{
            rb.AddForce(Physics.gravity, ForceMode.Acceleration);
        }
    }
    public Vector3 getLastVelocity(){
        return lastVelocity;
    }
    void ControlDrag(){
        if(isGrounded){
            rb.drag = groundDrag;
        }
        else{
            rb.drag = airDrag;
        }
    }

    bool OnSlope(){
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f)){
            if(slopeHit.normal != Vector3.up){
                Debug.Log("on slope");
                return true;
            }
            else{
                return false;
            }
        }
        return false;
    }
    void myInput(){
        horzMovement = Input.GetAxisRaw("Horizontal");
        vertMovement = Input.GetAxisRaw("Vertical");
        moveDirection = (orientation.transform.forward * vertMovement) + (orientation.transform.right * horzMovement);
        
    }
    

    public void Jump(){
        SoundBoard.instance.JumpSound();
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Vector3 velocity = rb.velocity;
        velocity *= AirMoveNerf;
        rb.velocity = velocity;
    }
    private void FixedUpdate() {
        
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);
        Move();
        
        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && isGrounded){
            rb.velocity = Vector3.zero;
        }
        //ApplyGravity();
        
        
    }
    void Move(){

        if(GameManager.instance.IsPoweredUp){
            moveSpeed = moveSpeedPowerUp;
        }
        else{
            moveSpeed = basicMoveSpd;
        }

        if(isGrounded && !OnSlope()){
            rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope()){
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if(!isGrounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * airMoveMultiplier, ForceMode.Acceleration);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hole")
        {
            StopAllCoroutines();
            GameManager.instance.EndLevel();
        }
        else if(other.tag == "Projectile")
        {
            Projectile proj = other.gameObject.GetComponent<Projectile>();
            if(!proj.Reversed && !GameManager.instance.IsPoweredUp){
                Destroy(other.gameObject);
                GameManager.instance.PlayerLoseHealth();
            }
            
        }
    }
    

}
