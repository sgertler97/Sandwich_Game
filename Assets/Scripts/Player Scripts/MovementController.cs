using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Movement Implementation from: https://medium.com/ironequal/unity-character-controller-vs-rigidbody-a1e243591483
namespace Player
{
    public class MovementController : MonoBehaviour
    {
        public float JumpHeight = 2f;
        public float RunSpeed = 5f;
        public float JumpSpeed = 2.5f;
        public float GroundDistance = 0.002f;
        public LayerMask Ground;

        private Rigidbody2D rb2D;
        private BoxCollider2D boxCollider;
        private Vector3 inputs = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
            rb2D = GetComponent < Rigidbody2D >();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            GetPlayerInput();
            
            TryJump();
        }

        private void GetPlayerInput()
        {
            inputs = Vector3.zero;
            inputs.x = Input.GetAxis("Horizontal");
        }

        public bool isGrounded()
        {
            return Physics2D.OverlapCircle(new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y), GroundDistance,Ground);
        }

        private void TryJump()
        {
            if (Input.GetAxisRaw("Vertical") > 0 && isGrounded())
            {
                rb2D.AddForce(Vector2.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode2D.Force);
            }
        }

        private void FixedUpdate()
        {
            MoveHorizontal();
        }

        private void  MoveHorizontal()
        {
            if (isGrounded())
            {
                rb2D.MovePosition(rb2D.position + (Vector2)(inputs * RunSpeed * Time.deltaTime));
            }
            else
            {
                rb2D.MovePosition(rb2D.position + (Vector2)(inputs * JumpSpeed * Time.deltaTime));
            }
        }
    }
}
