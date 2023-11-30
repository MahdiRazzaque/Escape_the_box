using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;

    private Vector3 moveDirection;
    public float gravityScale;

    //Animation
    public Animator animator;

    public float rotateSpeed;
    public Transform pivot;

    public GameObject playerModel;

    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;

    void Start() {
        controller = GetComponent<CharacterController>();
    }


    void Update() {

        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);

        if(knockBackCounter <= 0/* && controller.isGrounded*/) {
            float yStore = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxis("Vertical") + (transform.right * Input.GetAxis("Horizontal")));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            if (controller.isGrounded) {
                moveDirection.y = 0f;

                if (Input.GetButtonDown("Jump")) {
                    moveDirection.y = jumpForce;
                }
            }
        } else {
            knockBackCounter -= Time.deltaTime;
        }
    
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);

        controller.Move(moveDirection * Time.deltaTime);

        //Move the player in different directions based on camera look direction
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        animator.SetBool("isGrounded", controller.isGrounded);
        animator.SetFloat("speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));    
    }

    public void knockback(Vector3 direction) {
        knockBackCounter = knockBackTime;

        moveDirection = direction * knockBackForce;
        moveDirection.y = knockBackForce;
    }
}
