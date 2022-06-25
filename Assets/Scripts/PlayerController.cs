using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody rb;
    private CharacterController chController;
    public float speed = 5;


    private float movementX;
    private float movementY;
    private int count;
    private int countHoop;

    //jump variables
    public float jumpSpeed = 5;
    public float gravity = 9.8f;
    public float coeffL = 1;
    private bool jumpPress = false;
    
    
    private Vector3 movement;
    private bool isGround;

    public TextMeshProUGUI countText;
    public TextMeshProUGUI HoopText;
    public GameObject winTextObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        chController = GetComponent<CharacterController>();

        count = 0;
        countHoop = 0;
        SetCountText();
        SetCountHoop();
        winTextObject.SetActive(false);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    //Jump
    private void actionJump()
    {
        if (isGround)
        {
            movement.y = 0.0f;
        }

        if (jumpPress && isGround)
        {
            movement.y += (jumpSpeed * gravity * 2 * coeffL);
            jumpPress = false;
            Debug.Log("JUMP");
        }

        movement.y -= (gravity * Time.deltaTime);

        chController.Move(movement * Time.deltaTime);
    }

    private void OnJump()
    {
        Debug.Log("Jump");
        isGround = chController.isGrounded;
        if (isGround)
        {
            jumpPress = true;
            Debug.Log("jumpepress");
        }
    }

    private void Update()
    {
        movement = new Vector3(movementX, 0.0f, movementY);
        chController.SimpleMove(movement * speed);
    }

    private void FixedUpdate()
    {
        movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        actionJump();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            // Run the 'SetCountText()' function (see below)
            SetCountText();
        }

        if (other.gameObject.CompareTag("Hoop"))
        {
            countHoop++;
            SetCountHoop();
        }


    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            // Set the text value of your 'winText'
            winTextObject.SetActive(true);
        }

    }

    void SetCountHoop()
    {
        HoopText.text = "Puntos: " + countHoop.ToString();
    }
}