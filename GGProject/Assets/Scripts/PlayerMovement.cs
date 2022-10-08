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
    public float glideSpeed = 0.1f;
    public GameObject player;
    public GameObject glider;
    public GameObject testBullet;
    private bool grappleActive = false;
    private SpringJoint grappleJoint;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInputs = new PlayerControl();
        playerInputs.Enable();
        //playerRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
    }
    private void Update()
    {
        Vector2 moveVector = playerInputs.Player.Movement.ReadValue<Vector2>();
        //Button rotateVector = playerInputs.Player.Rotate.ReadValue<Button>();
        //print(rotateVector);
        transform.position += new Vector3(moveVector.x, 0, moveVector.y) * speed * Time.deltaTime; // makes character move with WASD input
        //transform.Rotate(moveVector.y * Time.deltaTime, moveVector.x, 0);
        //playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, Mathf.Sign(playerRigidbody.velocity.y) * glideSpeed);

        // Check if grappling state needs to be changed
        GrappleCheck();
    }

    public void Jump(InputAction.CallbackContext context) // makes character jump
    {
        if (context.performed)
        {
            playerRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

        }
        Debug.Log("Jumped" + context.phase);
    }

    IEnumerator GliderAction() // the commentaed out things are what i tried to use to do gliding :(
    {
        float forwardSpeed;
        float liftFactor = 2f;
        float dragValue = 0.05f;
        float liftValue = 0f;


        Vector3 currentVelocity = Vector3.zero;
        for (;;)
        {
            /*playerRigidbody.constraints = RigidbodyConstraints.None;
            forwardSpeed = transform.InverseTransformDirection(currentVelocity).z;
            currentVelocity += Physics.gravity * Time.deltaTime; //weight
            liftValue = Vector3.Dot(transform.forward, currentVelocity.normalized) * liftFactor * forwardSpeed; //lift
            currentVelocity = Vector3.Lerp(currentVelocity, transform.forward * forwardSpeed, liftValue * Time.deltaTime); // thrust
            currentVelocity *= dragValue * Time.deltaTime; //drag*/
            transform.Translate(Vector3.up * glideSpeed * Time.deltaTime); // super basic glide
            //Physics.gravity = new Vector3(0, -5f, 0);
            yield return null;
        }

    }

    public void GliderToggle(InputAction.CallbackContext context) // changes color of glider when active and changes it back when not active
    {
        var gliderRenderer = glider.GetComponent<MeshRenderer>();
        var glideCoroutine = GliderAction();
        if (context.performed)
        {
            glider.GetComponent<MeshRenderer>().material.color = Color.red;
            //player.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self); // rotates the character to make it look like they're gliding
            StartCoroutine(glideCoroutine);
            
        }
        if (context.canceled)
        {
            glider.GetComponent<MeshRenderer>().material.color = Color.gray;
            //player.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);
            StopAllCoroutines(); // stopping the coroutine didn't work so this is the failsafe :/
            // StopCoroutine(glideCoroutine);
        }
        Debug.Log("Color Change: " + context.phase);
    }

    private void GrappleCheck()
    {
        if (Input.GetMouseButtonDown(0) && !grappleActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                
                if (hitObject != null && GrappleTargetCheck(hitObject))
                {
                    grappleActive = true;
                    Debug.Log("Grapple has been activated.");

                    Grapple(hit, 0.2f, 1.0f, 10f, 0.2f, 0.025f);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && grappleActive)
        {
            grappleActive = false;
            Debug.Log("Grapple has been deactivated.");

            if (grappleJoint != null)
            {
                Destroy(grappleJoint);
            }
        }
    }

    // Returns true if target can be grappled, false if not
    private bool GrappleTargetCheck(GameObject target)
    {
        switch(target.tag)
        {
            case "Player":
                return false;
            case "Obstacle":
                return true;
            default:
                return true;
        }

    }

    private void Grapple(RaycastHit hit, float minDistance, float maxDistance, float spring, float damper, float tolerance)
    {
        grappleJoint = gameObject.AddComponent<SpringJoint>();

        if (hit.rigidbody != null)
        {
            grappleJoint.connectedBody = hit.rigidbody;
        }
        else
        {
            grappleJoint.connectedBody = hit.collider.gameObject.AddComponent<Rigidbody>();
        }
        
        Debug.Log("Connected Rigidbody is on " + grappleJoint.connectedBody.gameObject.name);

        grappleJoint.minDistance = minDistance;
        grappleJoint.maxDistance = maxDistance;

        grappleJoint.spring = spring;
        grappleJoint.damper = damper;
        grappleJoint.tolerance = tolerance;
    }

    /*public void TestShooter(InputAction.CallbackContext context)
    {
        var clone = testBullet.GetComponent<Rigidbody>();
        testBullet.transform.position += 5;
        if (context.performed)
        {
            Instantiate(testBullet, transform.position, transform.rotation);
            //testBullet.transform.position += transform.TransformDirection(Vector3.forward * 100);
            clone.velocity = transform.TransformDirection(Vector3.forward * 100);
        }
    }*/
}
