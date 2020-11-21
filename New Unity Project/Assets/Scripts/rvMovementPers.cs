using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rvMovementPers : MonoBehaviour
{
    //movement
    [Header("movement")]
    public Rigidbody myRb;
    public float speed=2;
    public float speedGTX = 0.2f;
    public float maxSpeed=5;
    public float jumpForce = 20;
    public float airControl=1;
    Vector3 desiredVelocity;
    float horizontal;
    float vertical;
    private bool _isGrounded = true;
    public Transform _groundChecker;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    public cameraScript myCam;
    private GameObject myPlayer;
    public Slider mySlider;
    // Update is called once per frame
    //Gun
    [Header("GUN")]
    public float damage = 10f;
    public float range = 100f;
    public float bulletSpeed = 100;
    public float CrystalSpeed = 80;
    public Rigidbody bullet;
    public Rigidbody crystal;
    public Transform rightPistol;
    public Transform leftPistol;
    bool canShoot = true;
    bool canShoot2 = true;
    bool isDashing=false;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float fireRate2 = 0.8f;
    float timerToShoot, timerToShoot2;

    [Header("DASH")]
    float dashTimer = 0f;
    public float dashvelocity = 30;
    private Vector3 dashV;
    bool doubleJumped = false;
    public float dashTime = 0.15f;

    //other
    [Header("other")]
    float currentHealth = 100;
    float maxHealth = 100;

    private void Start()
    {
        currentHealth = maxHealth;
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        timerToShoot = fireRate;
        timerToShoot2 = fireRate2;
    }

    void Update()
    {
        mySlider.value = currentHealth / maxHealth;
        var targetRotation = Quaternion.LookRotation(new Vector3(myCam.myMouse.x, transform.position.y, myCam.myMouse.z)  - transform.position);


        myPlayer.transform.LookAt(new Vector3(myCam.myMouse.x, this.transform.position.y, myCam.myMouse.z));

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        if (_isGrounded && myRb.drag != 7)
            myRb.drag =7;
        else if(!_isGrounded)
            myRb.drag = 0;

        

        desiredVelocity= new Vector3(horizontal,0f,vertical);
        desiredVelocity.Normalize();
        //move respect camera

        float angle = Vector3.Angle(desiredVelocity, myRb.velocity.normalized);

        if (!_isGrounded)
        {
            desiredVelocity *= airControl;
            Vector3 aux = new Vector3(myRb.velocity.x, 0, myRb.velocity.z);
            if ((angle < 80) && (aux.magnitude > maxSpeed))
            {
                desiredVelocity = new Vector3(0, 0, 0);
            }
        }
        else if (doubleJumped)
            doubleJumped = false;

        if (Input.GetButtonDown("Jump"))
        {
            if (!isDashing)
            {
                if (!_isGrounded)
                {
                    if (!doubleJumped)
                    {
                        doubleJumped = true;
                        myRb.velocity = new Vector3(myRb.velocity.x,0,myRb.velocity.z);
                        myRb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
                    }
                }
                else
                    myRb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
            }
        }

        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }
        if (Input.GetButtonDown("Fire2") && canShoot2)
        {
            ShootCrystal();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
            myRb.AddForce((desiredVelocity * speed ), ForceMode.Acceleration);

        if ((myRb.velocity.magnitude > maxSpeed) && _isGrounded)
        {
            myRb.velocity = myRb.velocity.normalized * maxSpeed;
        }
        if (isDashing)
        {
            myRb.MovePosition(myRb.position + dashV * dashvelocity * 2 * Time.fixedDeltaTime);
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }

        if (!canShoot)
        {
            timerToShoot -= Time.fixedDeltaTime;
            if (timerToShoot <= 0)
            {
                canShoot = true;
                timerToShoot = fireRate;
            }
        }

        if (!canShoot2)
        {
            timerToShoot2 -= Time.fixedDeltaTime;
            if (timerToShoot2 <= 0)
            {
                canShoot2 = true;
                timerToShoot2 = fireRate2;
            }
        }
    }

    void Shoot()
    {
        canShoot = false;
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, rightPistol.transform.position, rightPistol.transform.rotation);
        bulletClone.velocity = transform.forward * bulletSpeed;
    }

    void ShootCrystal()
    {
        canShoot2 = false;
        Rigidbody bulletClone2 = (Rigidbody)Instantiate(crystal, leftPistol.transform.position, leftPistol.transform.rotation);
        bulletClone2.velocity = transform.forward * CrystalSpeed;
    }

    void Dash()
    {
        dashV = desiredVelocity.normalized;
        dashTimer = dashTime;
        isDashing = true;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Curren Health : " + currentHealth);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //pause game with score and gfgo main menu
        
    }
}
