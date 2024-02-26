using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Mortality
{
    public float moveSpeed = 3f;

    public float gravity = -0.1f;

    CharacterController charController;

    public float verticalVelocity = -.5f;

    public float currentExtraHorizontalVelocity = 0;

    public float jumpforce = 3f;

    public bool beingBounced = false;




    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector3 movementVector = Vector3.zero;


        if (charController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) || beingBounced)
            {
                verticalVelocity += jumpforce;

                beingBounced = false;
            }
            else
            {
                verticalVelocity = -0.5f;
            }
        }
        else
        {
            //Not on the ground

            verticalVelocity += gravity;
        }

        movementVector.y = verticalVelocity;
        movementVector.x = Input.GetAxis("Horizontal") * moveSpeed + currentExtraHorizontalVelocity;


        charController.Move(movementVector * Time.deltaTime);

        currentExtraHorizontalVelocity *= .99f;
        
        


    }
}
