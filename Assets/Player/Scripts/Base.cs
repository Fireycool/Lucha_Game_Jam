using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] Rigidbody2D body;
    [SerializeField] BoxCollider2D groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] CircleCollider2D parryHitbox;
    [SerializeField] LayerMask enemyMask;

    [SerializeField] Animator animator;

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
    
    public float parry_count;
    public bool grounded = false;
    public bool walking = false;
    public bool jumping = false;
    public bool falling = false;
    public bool walled = false;
    public bool parry = false;
    public bool parriable = false;
    public bool parried = false;
    public bool damaged = false;
    
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

        ParryPlayer();
        
        if(parry){
            ParryRead();
            animator.Play("Parry");
        }
        else{
            parryHitbox.enabled = false;
            parry_count = PARRY_TIME;
            parryHitbox.enabled = false;
        }


        if (grounded){
            parry = false;
            parried = false;
            parriable = true;

            body.rotation = 0;
            
        }

        if (parried){
            animator.Play("Parried");
            parriable = true;
        }
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
            animator.Play("Idle");
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
            if(grounded)
               animator.Play("Walk");
                    
            float increment = xInput * ACCELERATION;
            float newSpeed = Mathf.Clamp(body.velocity.x + increment, -SPEED, SPEED);
            body.velocity = new Vector2(newSpeed, body.velocity.y);

            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1 , 1);
        }     
    }

    void JumpPlayer() {
        if(Input.GetButtonDown("Jump") && grounded) {
            body.velocity = new Vector2(body.velocity.x, JUMP_VELOCITY);      
        }       
    }

    void ParryPlayer()
    {
        if (!grounded && Input.GetButtonDown("Fire1") && parriable)
        {
            parry = true;
            parriable = false;
            parryHitbox.enabled = true;
            parry_count = PARRY_TIME;
        }
    }

    void ParryRead()
    {
        if (parry && Physics2D.OverlapAreaAll(parryHitbox.bounds.min, parryHitbox.bounds.max, enemyMask).Length > 0)
        {
            body.velocity = new Vector2(body.velocity.x, PARRY_BOUNCE);           
            parried = true;
            parry = false;
        }
        else{
            parry_count -= 0.1F;
            if (parry_count <= 0){
                parry = false;
            }
        }
    }

    void CheckGround(){
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void CheckJump(){
        if(body.velocity.y > 0 && !grounded){
            jumping = true;
            if (!parried && !parry) 
                animator.Play("Up");
        }
        else{
            jumping = false;
        }
    }

    void CheckFall(){
        if(body.velocity.y < 0 && !grounded){
            falling = true;
            if (!parried && !parry) 
                animator.Play("Down");
        }
        else{
            falling = false;
        }    
    }
}
