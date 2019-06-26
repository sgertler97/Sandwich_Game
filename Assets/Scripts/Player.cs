using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 10f;
    Animator Guy;
    bool facingRight = true;
    bool isJump = false;
    public float initJumpSpeed = 5f;
    public float JumpSpeed;
    public float accTime = 0.5f;
    public float decTime = 1f;
    private float inverseJumpSpeed;
    private BoxCollider2D boxCollider;         //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;                //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;
    private float counter;
    public LayerMask blockingLayer;
    // Start is called before the first frame update
    void Start()
    {
        Guy = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        /*if(Input.GetAxisRaw("Vertical") > Mathf.Epsilon && isJump == false)
        {
            isJump = true;
            StartCoroutine("Jump");
        }*/
    }

    //collisions above head check for
    

    private void Move()
    {
        //variables
        float dir = Input.GetAxisRaw("Horizontal");
        float xDir = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float yDir = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        if(dir > 0)
        {
            Guy.SetBool("Run", true);
            if (!facingRight)
                Flip();
        }
        else if(dir < 0)
        {
            Guy.SetBool("Run", true);
            transform.localScale.Set(-1f, 1f, 1f);
            if (facingRight)
                Flip();
        }
        else
        {
            Guy.SetBool("Run", false);
        }
        RaycastHit2D hit;
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        boxCollider.enabled = false;
        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, blockingLayer);
        //Re-enable boxCollider after linecast
        boxCollider.enabled = true;
        //Check if anything was hit
        if (hit.transform == null)
        {
            transform.position = end;
        }
    }

    private void Flip()
    {
      facingRight = !facingRight;
      Vector3 theScale = transform.localScale;
      theScale.x *= -1;
      transform.localScale = theScale;
    }

    IEnumerator Jump()
    {
        Vector2 start = transform.position;
        Vector2 end;
        inverseJumpSpeed = 1 / initJumpSpeed;
        JumpSpeed = initJumpSpeed;
        counter = 0f;
        
        while (counter < accTime)
        {
            //end = start + new Vector2(0, counter / (counter * counter + inverseJumpSpeed));
            end = start + new Vector2(0, counter * (-(6 * counter - 2) * (6 * counter - 2)+JumpSpeed));
            transform.position = end;
            counter += Time.deltaTime;
            yield return null;
        }
        isJump = false;
    }
}
