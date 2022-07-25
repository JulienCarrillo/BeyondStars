using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMouvement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -5f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public GameObject SpaceShip;
    public LineRenderer line;
    public Camera camera;
    public GameObject UIText;
    
    Vector3 velocity;
    bool isGrounded;
    bool CanGoInSpaceShip = false;


    void Update()
    {
        Move();
        actions();
    }

    private void Move()
    {
        Debug.Log(GameObject.FindGameObjectWithTag("MainCam"));
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = -Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void actions()
    {
        RayCastCheck();
        //Remonter dans l'avion
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CanGoInSpaceShip)
            {
                GoToSpaceShip();
            }
        }
    }

    private void RayCastCheck()
    {
        RaycastHit hitInfo;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 50f, 1 << 8))
        {
                Interact(hitInfo.transform.tag);
                UIText.SetActive(true);  
        }
        else
        {
            UIText.SetActive(false);
        }
    }

    private void Interact(string InterractObjectName)
    {
        switch (InterractObjectName)
        {
            case "Player":

                CanGoInSpaceShip = true;
                break;
            default:
                CanGoInSpaceShip = false;
                
                break;
        }
        
    }

    private void GoToSpaceShip()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Instantiate(SpaceShip, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
