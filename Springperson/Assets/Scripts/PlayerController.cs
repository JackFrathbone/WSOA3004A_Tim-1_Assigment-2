using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Constants")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveMultiplier = 10f;
    [SerializeField] public float mouseSensitivity = 5f;
    [SerializeField] float groundDrag = 6f;
    
    float horzMovement;
    float vertMovement;
    Vector3 moveDirection;



    [Header("Jump Constants")]
    [SerializeField] float airDrag = 2f;
    [SerializeField] float playerHeight = 2f;
    [SerializeField] float jumpForce = 10f;
    bool isGrounded;



    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation= true;
    }

    void ControlDrag(){
        if(isGrounded){
            rb.drag = groundDrag;
        }
        else{
            rb.drag = airDrag;
        }
    }
    // Update is called once per frame
    void Update()
    {
       isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight/2 + 0.1f);
       ControlDrag();
       if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
           Jump();
       }
       myInput();
    }
    void myInput(){
        horzMovement = Input.GetAxisRaw("Horizontal");
        vertMovement = Input.GetAxisRaw("Vertical");
        moveDirection = (transform.forward * vertMovement) + (transform.right * horzMovement);

    }

    public void Jump(){
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void FixedUpdate() {
        Move();
    }
    void Move(){
        rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
    }
    
}
