using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 8f;
    public float jumpPower = 5f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    private float startPositionX = -12.69f;
    private float startPositionY = 0.736f;
    private float startPositionZ = 2.24f;

    public Camera jumpCamera;
    private Camera MainCamera;
    public Animator mouseAnimator;
    public AudioListener JumpAudioListen;
    private AudioListener MainListen;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MainCamera = GetComponentInChildren<Camera>();
        JumpAudioListen = jumpCamera.GetComponent<AudioListener>();
        MainListen = GetComponentInChildren<AudioListener>();
        JumpAudioListen.enabled = false;
        jumpCamera.enabled = false;
        mouseAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
            mouseAnimator.SetBool("IsWalking", false);
            mouseAnimator.SetBool("IsJumping", true);
        }
        else
        {
            moveDirection.y = movementDirectionY;

        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            mouseAnimator.SetBool("IsJumping", false);
        }


        characterController.Move(moveDirection * Time.deltaTime);
        if (canMove)
        {
            
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        if(characterController.velocity.x > 0.001f || characterController.velocity.z > 0.001f) { mouseAnimator.SetBool("IsWalking", true); } else { mouseAnimator.SetBool("IsWalking", false); }
    }
    public Animator catsAnimator;
    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.tag == "Cheese")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("WinScreen");
        }

        if(other.gameObject.tag == "Guard")
        {
            MainListen.enabled = false;
            MainCamera.enabled = false;
            jumpCamera.enabled = true;
            JumpAudioListen.enabled = true;
            catsAnimator.SetBool("IsAttacking", true);
            transform.LookAt(other.gameObject.transform);
            //Invoke("Reload", 1f);
        }
    }
    public void Reload()
    {
        SceneManager.LoadScene("MainLevel");
    }
}
