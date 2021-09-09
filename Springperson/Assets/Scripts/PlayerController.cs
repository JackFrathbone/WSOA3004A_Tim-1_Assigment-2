using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Constants")]
    [SerializeField] float moveSpeed = 5f;
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
    [Range(0, 1)] [SerializeField] float AirMoveNerf;
    bool isGrounded;
    RaycastHit slopeHit;
    Vector3 lastVelocity;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation= true;
    }

    
    // Update is called once per frame
    void Update()
    {


       isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight/2 + 0.1f);
       if(rb.velocity.magnitude > 0){
           lastVelocity = rb.velocity;
       }
       ControlDrag();
       if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
           Jump();
       }
       myInput();
       slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
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
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Vector3 velocity = rb.velocity;
        velocity *= AirMoveNerf;
        rb.velocity = velocity;
    }
    private void FixedUpdate() {
        Move();
    }
    void Move(){
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
            GameManager.instance.EndLevel();
        }
        else if(other.tag == "Projectile")
        {
            Destroy(other.gameObject);
            GameManager.instance.PlayerLoseHealth();
        }
    }

}
