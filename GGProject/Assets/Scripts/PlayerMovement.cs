using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private PlayerControl playerInputs;
    private Scene currentScene;

    private PlayerUIManager playerUIManager;

    public float speed;
    private float rotateSpeed = 1500f;

    public float jumpHeight = 7f;
    public int currentJumps = 3;
    public int maxJumps = 3;

    public bool glideEnabled;
    public float glideSpeed = 0.1f;
    public GameObject glider;
    private bool gliderActive;
    public GameObject testBullet;

    public bool grappleEnabled;
    private bool grappleActive = false;
    private bool grapplingEnemy = false;
    private GameObject grappledEnemy;
    private SpringJoint grappleJoint;
    private Vector3 grapplePoint;
    public Transform cameraTransform, player;
    public float minGrappleDistance = 0.2f;
    public float maxGrappleDistance = 200f;
    private LineRenderer lineRenderer;
    public Image reticle;
    private Color grappleInactiveColor = Color.red;
    private Color grappleActiveColor = Color.blue;
    private Color cantGrappleColor = Color.grey;


    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "TutorialLevel")
        {
            glideEnabled = false;
            grappleEnabled = false;
        }
        else
        {
            glideEnabled = true;
            grappleEnabled = true;
        }
        
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.drag = 0.0f;
        playerRigidbody.mass = 1.0f;

        playerInputs = new PlayerControl();
        playerInputs.Enable();

        reticle.color = grappleInactiveColor;
    }

    // Start is used to make sure that any accessed singletons have been set during Awake
    private void Start()
    {
        playerUIManager = PlayerUIManager.local;
    }

    private void Update()
    {
        Vector2 moveVector = playerInputs.Player.Movement.ReadValue<Vector2>();
        Vector3 movementDirection = new Vector3(moveVector.x, 0, moveVector.y);
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        transform.position += movementDirection * speed * Time.deltaTime; // makes character move with WASD input

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }

        // Check if currentJumps needs to be reset
        JumpResetCheck();

        // Check if grappling state needs to be changed
        if (grappleEnabled)
        {
            GrappleCheck();

        }
        
        // Check if gliding state needs to be changed
        if (glideEnabled)
        {
            GliderCheck();
        }

        PauseCheck();
    }

    public void Jump(InputAction.CallbackContext context) // makes character jump
    {
        if (context.performed)
        {
            if (currentJumps > 0)
            {
                --currentJumps;

                playerRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
            
        }
        Debug.Log("Jumped" + context.phase);
    }

    public void JumpResetCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + (Vector3.down * 0.1f), Vector3.down, out hit))
        {
            if (hit.distance <= 1.0f)
            {
                currentJumps = maxJumps;
            }
        }
    }

    public void GliderCheck()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GliderToggle(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            GliderToggle(false);
        }
    }

    public void GliderToggle(bool state) 
    {
        // changes color of glider when active and changes it back when not active
        if (gliderActive != state)
        {
            gliderActive = state;
            
            if (state)
            {
                glider.GetComponent<MeshRenderer>().material.color = Color.red;
                playerRigidbody.drag = 2.5f;
                playerRigidbody.mass = 0.5f;

            }
            if (!state)
            {
                glider.GetComponent<MeshRenderer>().material.color = Color.gray;
                playerRigidbody.drag = 0.0f;
                playerRigidbody.mass = 1.0f;
            }
        }

        Debug.Log("Color Change: " + state);
    }

    private void GrappleCheck()
    {
        if (Input.GetMouseButtonDown(0) && !grappleActive)
        {
            RaycastHit hit;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxGrappleDistance))
            {
                if (GrappleTargetCheck(hit.transform.gameObject))
                {
                    grappleActive = true;

                    if (grapplingEnemy && grappledEnemy != null)
                    {
                        grapplePoint = grappledEnemy.transform.position;
                    }
                    else
                    {
                        grapplePoint = hit.point;
                    }
                    
                    grappleJoint = player.gameObject.AddComponent<SpringJoint>();
                    grappleJoint.autoConfigureConnectedAnchor = false;
                    grappleJoint.connectedAnchor = grapplePoint;

                    float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                    grappleJoint.maxDistance = distanceFromPoint * 0.5f;
                    grappleJoint.minDistance = distanceFromPoint * 0.25f;

                    grappleJoint.spring = 100f;
                    grappleJoint.damper = 50f;
                    grappleJoint.massScale = 4.5f;

                    lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
                    lineRenderer.startColor = Color.black;
                    lineRenderer.endColor = Color.black;
                    lineRenderer.startWidth = 0.01f;
                    lineRenderer.endWidth = 0.01f;
                    lineRenderer.positionCount = 2;
                    lineRenderer.useWorldSpace = true;

                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, grapplePoint);

                    reticle.color = grappleActiveColor;
                }
                
            }
        }

        if (Input.GetMouseButtonUp(0) && grappleActive)
        {
            grappleActive = false;
            Debug.Log("Grapple has been deactivated.");

            if (grapplingEnemy)
            {
                grapplingEnemy = false;

                Destroy(grappledEnemy);

                grappledEnemy = null;
            }

            if (grappleJoint != null)
            {
                Destroy(grappleJoint);
            }

            if (lineRenderer != null)
            {
                Destroy(lineRenderer.gameObject);
            }
        }

        if (grappleActive)
        {
            lineRenderer.SetPosition(0, transform.position);

            if (grapplingEnemy && grappledEnemy != null)
            {
                grappleJoint.connectedAnchor = grappledEnemy.transform.position;

                lineRenderer.SetPosition(1, grappleJoint.connectedAnchor);
            }
        }
        else
        {
            RaycastHit hit;
            
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxGrappleDistance))
            {
                if (GrappleTargetCheck(hit.transform.gameObject))
                {
                    reticle.color = grappleInactiveColor;
                }
                else
                {
                    reticle.color = cantGrappleColor;
                }
            }
            else
            {
                reticle.color = cantGrappleColor;
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
            case "Enemy":
                grapplingEnemy = true;
                grappledEnemy = target;
                return true;
            default:
                return true;
        }

    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void PauseCheck()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerUIManager.TogglePauseState();
        }
    }
}
