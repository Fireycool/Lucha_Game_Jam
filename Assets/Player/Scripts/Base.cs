using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    [SerializeField] Rigidbody2D body;
    [SerializeField] Transform sprite;
    [SerializeField] BoxCollider2D groundCheck;
    [SerializeField] BoxCollider2D wallCheck;

    [SerializeField] LayerMask groundMask;
    [SerializeField] CircleCollider2D parryHitbox;
    [SerializeField] BoxCollider2D slamHitbox;
    [SerializeField] LayerMask enemyMask;

    [SerializeField] Animator animator;

    [SerializeField] float SPEED;
    [SerializeField] float SLAM_SPEED;
    [SerializeField] float ACCELERATION;
    [Range(0f,1f)]
    [SerializeField] float FRICTION;
    [Range(0f,1f)]
    [SerializeField] float AIR_FRICTION;
    [Range(0f,2f)]
    [SerializeField] float GRAVITY;

        [Range(0f,1f)]
    [SerializeField] float WALL_GRAVITY;

    [SerializeField] float JUMP_VELOCITY;
    [SerializeField] float WALL_JUMP_PUSHBACK;

    [SerializeField] float INPUT_BUFFER_PATIENCE;
    [SerializeField] float COYOTE_TIME;

    [SerializeField] float PARRY_TIME;
    [SerializeField] float PARRY_BOUNCE;

    SFX_Manager sfxManager;
    
    public float parry_count;
    public bool grounded = false;
    public bool walking = false;
    public bool jumping = false;
    public bool falling = false;
    public bool slamming = false;
    public bool walled = false;
    public bool parry = false;
    public bool parriable = false;
    public bool parried = false;
    public bool damaged = false;
    
    float xInput; 
    float yInput;

    public float score;

    public int combo;
    public int basecomboTime;
    public Combo_Bar_script combobar;
    public Scoreboard scorebord;
    public Combo_Board combom;

    public int comboTime;

    public int maxHealth = 3;
    public int health;

    public float KBForce = 2;
    public float KBCounter;
    public float KBTime = 1;
    public bool KnockFromRight;


    // Start is called before the first frame update
    void Start()
    {
        sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFX_Manager>();
        health = maxHealth;
        score = 0;
        combo = 0;
        combobar.SetMaxTimer(basecomboTime);
        scorebord.SetScore(score);
        combom.SetCombo(combo);
    }

    // Update is called once per frame
    void Update()
    {
        // Controles del Jugador
        if (Time.timeScale > 0){
            GetInput();

            JumpPlayer();

            WallJumpPlayer();

            ParryPlayer();

            SlamPlayer();
        }

        
        if(parry){
            ParryRead();
            animator.Play("Parry");
        }
        else{
            parryHitbox.enabled = false;
            parry_count = PARRY_TIME;
            parryHitbox.enabled = false;
        }


        if(slamming){
            SlamRead();
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
        DecreaseCombo();
        IncreaseScore();
        CheckGround();
        CheckJump();
        CheckWall();
        
        if (!walled){
            CheckFall();
        }

        if (!grounded){
            
            if (body.velocity.y > 1){
                body.velocity -= new Vector2(0,get_grav());
            }
            else if (body.velocity.y <= 1 && body.velocity.y > 0){
                body.velocity -= new Vector2(0,get_grav()/2);
            }
            else if (body.velocity.y <= 0){
                body.velocity -= new Vector2(0,get_grav()*2);
            }
        }

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
            
            if(KBCounter <= 0)
            {
                body.velocity = new Vector2(newSpeed, body.velocity.y);
            }
            else
            {
                if(KnockFromRight)
                {
                    body.velocity = new Vector2(-KBForce, KBForce);
                }
                else
                {
                    body.velocity = new Vector2(KBForce, KBForce);
                }

                KBCounter -= Time.deltaTime;
            }

            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1 , 1);
        }     
    }

    void JumpPlayer() {
        if(Input.GetButtonDown("Jump") && grounded) {
            body.velocity = new Vector2(body.velocity.x, JUMP_VELOCITY);
            sfxManager.PlaySFX(sfxManager.jump);
        }       
    }

    
    void WallJumpPlayer() {
        float direction = -Mathf.Sign(xInput);
        if(Input.GetButtonDown("Jump") && walled) {
            body.velocity = new Vector2((body.velocity.x + WALL_JUMP_PUSHBACK)* direction, JUMP_VELOCITY);
            sfxManager.PlaySFX(sfxManager.jump);     
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

    void SlamPlayer()
    {
        if (!grounded && Input.GetButtonDown("Fire2"))
        {
            if (body.velocity.y < SLAM_SPEED){
                body.velocity -= new Vector2(0,SLAM_SPEED);
            }
            else{
                body.velocity = new Vector2(body.velocity.x,SLAM_SPEED);
            }
            slamming = true;
            animator.Play("Slam");

        }
    }

    void ParryRead()
    {
        if (parry && Physics2D.OverlapAreaAll(parryHitbox.bounds.min, parryHitbox.bounds.max, enemyMask).Length > 0)
        {
            sfxManager.PlaySFX(sfxManager.parry);
            FindObjectOfType<HitSTop>().Stop(0.1F);
            body.velocity = new Vector2(body.velocity.x, PARRY_BOUNCE);           
            parried = true;
            parry = false;
            IncreaseCombo();
            
        }
        else{
            parry_count -= 0.1F;
            if (parry_count <= 0){
                parry = false;
            }
        }
    }

        void SlamRead()
    {
        if (slamming && Physics2D.OverlapAreaAll(slamHitbox.bounds.min, slamHitbox.bounds.max, groundMask).Length > 0)
        {
            sfxManager.PlaySFX(sfxManager.parry);
            FindObjectOfType<HitSTop>().Stop(0.1F);
            body.velocity = new Vector2(body.velocity.x, PARRY_BOUNCE);           
            parried = true;
            parry = false;            
            slamming = false;
        }
    }

    void CheckGround(){
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void CheckWall(){
        if (grounded)
            walled = false;
        else
            walled = Physics2D.OverlapAreaAll(wallCheck.bounds.min, wallCheck.bounds.max, groundMask).Length > 0;
            
        if(walled && !jumping && !grounded && !parry){
            animator.Play("Slide");

        }
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
        if(body.velocity.y < 0 && !grounded ){
            falling = true;
            if (!parried && !parry && !walled && !slamming) 
                animator.Play("Down");
        }
        else{
            falling = false;
            slamming = false;
        }    
    }

    float get_grav(){
        if (walled && body.velocity.y <= 0){
            return WALL_GRAVITY;
        }
        else
        {
            return GRAVITY;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            SceneManager.LoadSceneAsync("Infinite");
            Destroy(gameObject);
        }
    }

    void DecreaseCombo(){
        if(comboTime > 0){
            --comboTime;
        }
        else{
            combo = 0;
        }
        combobar.SetTimeLeft(comboTime);
        combom.SetCombo(combo);
    }

    void IncreaseCombo(){
        if(combo < 60){
            if(combo <= 0){
            combo = 2;
        }
        else{
            combo *= 2;
        }
        }
        
        comboTime = basecomboTime;
    }

    void IncreaseScore(){
        if (combo > 0){
            score += 0.01f*combo;
        }
        else{
            score += 0.01f;
        }
        scorebord.SetScore(score);
    }
}
