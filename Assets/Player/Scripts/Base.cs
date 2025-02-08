using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] Rigidbody2D body;
    [SerializeField] BoxCollider2D groundCheck;
    [SerializeField] LayerMask groundMask;


    [SerializeField] float SPEED;
    [SerializeField] float ACCELERATION;
    [Range(0f,1f)]
    [SerializeField] float FRICTION;
    [Range(0f,1f)]
    [SerializeField] float AIR_FRICTION;
    [SerializeField] float GRAVITY;
    [SerializeField] float FALL_GRAVITY;
    [SerializeField] float FAST_FALL_GRAVITY;
    [SerializeField] float WALL_GRAVITY;

    [SerializeField] float JUMP_VELOCITY;
    [SerializeField] float WALL_JUMP_PUSHBACK;

    [SerializeField] float INPUT_BUFFER_PATIENCE;
    [SerializeField] float COYOTE_TIME;

    [SerializeField] float PARRY_TIME;
    [SerializeField] float PARRY_BOUNCE;
    
    
    public bool grounded;
    public bool walking;
    public bool jumping;
    public bool falling;
    public bool walled;
    public bool parry;
    public bool parried;
    public bool damaged;


    
    float xInput; 
    float yInput;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Recibe el Input del jugador
        GetInput();


        //Salto del Jugador
        JumpPlayer();

    }

    void FixedUpdate()
    {
        CheckGround();
        CheckJump();
        CheckFall();


        //Movimiento Izquierda y Derecha del Jugador
        MovePlayer();

        //Control de Friccion del jugador    
        if (grounded && xInput == 0 && yInput == 0) {
            body.velocity *= new Vector2(FRICTION, 1);
        }
        else if(!grounded && xInput == 0 && yInput == 0){
            body.velocity *= new Vector2(AIR_FRICTION, 1);
        }
    }

    void GetInput() {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        
    }

    void MovePlayer() {
        if(Mathf.Abs(xInput) > 0) {
            
            float increment = xInput * ACCELERATION;
            float newSpeed = Mathf.Clamp(body.velocity.x + increment, -SPEED, SPEED);
            body.velocity = new Vector2(newSpeed, body.velocity.y);

            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1 , 1);
        }     
    }

    void JumpPlayer() {
        if(Mathf.Abs(yInput) > 0 && grounded) {
            body.velocity = new Vector2(body.velocity.x, yInput * JUMP_VELOCITY);            
        }       
    }     

    void CheckGround(){
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void CheckJump(){
        if(body.velocity.y > 0 && !grounded){
            jumping = true;
        }
        else{
            jumping = false;
        }
    }

    void CheckFall(){
        if(body.velocity.y < 0 && !grounded){
            falling = true;
        }
        else{
            falling = false;
        }    
    }
}
