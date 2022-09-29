using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private PlayerControl playerInputs;
    public float speed = 10f;
    public float jumpHeight = 7f;
    public float glideSpeed = 2f;
    public GameObject player;
    public GameObject glider;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInputs = new PlayerControl();
        playerInputs.Enable();
    }
    private void Update()
    {
        Vector2 moveVector = playerInputs.Player.Movement.ReadValue<Vector2>();
        transform.position += new Vector3(moveVector.x, 0, moveVector.y) * speed * Time.deltaTime; // makes character move with WASD input
    }
    public void Jump(InputAction.CallbackContext context) // makes character jump
    {
        if (context.performed)
        {
            playerRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

        }
        Debug.Log("Jumped" + context.phase);
    }

    public void GliderToggle(InputAction.CallbackContext context) // changes color of glider when active and changes it back when not active
    {
        var gliderRenderer = glider.GetComponent<MeshRenderer>();
        if (context.performed)
        {
            glider.GetComponent<MeshRenderer>().material.color = Color.red;
            player.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);

            //playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, Mathf.Sign(playerRigidbody.velocity.y) * glideSpeed);
        }
        if (context.canceled)
        {
            glider.GetComponent<MeshRenderer>().material.color = Color.gray;
            player.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);
        }
        Debug.Log("Color Change: " + context.phase);
    }
}
