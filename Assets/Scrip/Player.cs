using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    Vector2 inputMov;
    Vector2 inputRot;
    public float sensity = 1f;
    Transform cam;
    float camRotX;

    public float speedWalk = 10f;
    public float run = 20f;
    public float trueSpeed;

    public float timetest = 8f;
    float time;

    public float ForceJump = 300f;

    Vector3 normalScale;
    public Vector3 scaleDown;
    bool scaleD;

    public bool fixReady;

    //animation
    public bool atack;
    public bool runing;
    public bool walk;
    public bool fix;
    public bool jump;
    public bool takeKey;
    public bool idl;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cam = transform.GetChild(0);

        jump = false;

        normalScale = transform.localScale;
        scaleDown = normalScale;
        scaleDown.y = normalScale.y * .75f;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CamMove();
        Jump();
        DownState();


        Animations();
       
    }
    private void FixedUpdate()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? run : speedWalk;
        rb.velocity = transform.forward * speed * inputMov.y +
                      transform.right * speed * inputMov.x +
                      new Vector3(0, rb.velocity.y, 0);
        trueSpeed = speed;

        transform.rotation *= Quaternion.Euler(0, inputRot.x, 0);
        camRotX -= inputRot.y;
        camRotX = Mathf.Clamp(camRotX, -50, 50);
        cam.localRotation = Quaternion.Euler(camRotX, 0, 0);

        transform.localScale = Vector3.Lerp(transform.localScale, scaleD ? scaleDown : normalScale, 1f);
        StateMoment();
        Fix();
    }

    private void Move()
    {
        inputMov.x = Input.GetAxis("Horizontal");
        inputMov.y = Input.GetAxis("Vertical");

    }

    private void CamMove()
    {
        inputRot.x = Input.GetAxis("Mouse X") * sensity;
        inputRot.y = Input.GetAxis("Mouse Y") * sensity;
    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !scaleD) rb.AddForce(0, ForceJump, 0);
        anim.SetTrigger("jump");

    }
    private void DownState()
    {
        scaleD = Input.GetKey(KeyCode.C);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag ==("Boxfix"))
        {
            fix = true;
        }
        else
        {
            fix = false;
        }
    }
    private void Fix()
    {
        
        time += Time.deltaTime;
        if (time >= timetest && fix)
        {
            fixReady = true;
            time = 0;
        }

    }
    private void StateMoment()
    {
        if (trueSpeed == run)
        {
            runing = true;
            idl = false;
            walk = false;

        }
        if (trueSpeed == speedWalk)
        {
            walk = true;
            runing = false;
            idl = false;
        }
        if (trueSpeed == 0)
        {
            idl = true;
            runing = false;
            walk = false;
        }
    }

    private void Atack()
    {
        if (Input.GetMouseButton(0))
        {
            atack = true;

        }
    }

    private void Animations()
    {
        if (atack)
        {
            anim.SetTrigger("atack");
        }
        if (runing)
        {
            anim.SetTrigger("run");
        }
        if (walk)
        {
            anim.SetTrigger("walk");
        }

        if (fix)
        {
            anim.SetTrigger("fix");
        }
        if (jump)
        {
            anim.SetTrigger("jump");
        }

        if (takeKey)
        {
            anim.SetTrigger("take");
        }
        if (idl)
        {
            anim.SetTrigger("idl1");
        }
    }
}
