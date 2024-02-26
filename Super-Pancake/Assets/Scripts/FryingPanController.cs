using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FryingPanController : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    GameObject player;

    [SerializeField]
    float currentPancakeVelocity = 0;


    //This game is going to be about launching pancakes off pans, so we're going to need to check the velocity!
    [SerializeField]
    float velocityForPancakeLiftoff = 10;


    [SerializeField]
    GameObject pancakePrefab;

    [SerializeField]
    GameObject criticalPancakePrefab;



    [SerializeField]
    Transform placeToInstantiatePancakePrefab;

    public Transform currentPancake;

    [SerializeField]
    Vector2 pancakeInstantiationBounds = new Vector2 (0, 0);

    [SerializeField]
    float pancakeInstantiationTime = 1;

    [SerializeField]
    float pancakeInstantiationTimer = 0;


    Vector3 previousPancakePos;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPanScreenPos = cam.WorldToScreenPoint(transform.position);

        Vector3 currentMousePos = Input.mousePosition;

        float requiredPanAngle = Mathf.Atan2(currentPanScreenPos.y -  currentMousePos.y, currentPanScreenPos.x - currentMousePos.x);

        transform.rotation = Quaternion.Euler(0,0, requiredPanAngle * 180/Mathf.PI);

        
        InstantiatePancake();
        LaunchPancake();
        GetPancakeVelocity();

    }

    void InstantiatePancake()
    {
        //Debug.Log(transform.rotation.eulerAngles.z);
        if (currentPancake == null &&  ( Mathf.Clamp(transform.rotation.eulerAngles.z, 0, pancakeInstantiationBounds.y)  == transform.rotation.eulerAngles.z || Mathf.Clamp(transform.rotation.eulerAngles.z, 360 + pancakeInstantiationBounds.x, 360) == transform.rotation.eulerAngles.z) )
        {


            if (pancakeInstantiationTimer > pancakeInstantiationTime)
            {
                //Instantiate Pancake on pan! 
                //Also randomize whether player gets a critical pancake or not. 1-in-5 chance seemed to work pretty good!
                currentPancake = Instantiate( Random.Range(0,6) == 0 ? criticalPancakePrefab : pancakePrefab, placeToInstantiatePancakePrefab.position, transform.rotation, transform).transform;

                //Debug.Log("Here's another one!");

                pancakeInstantiationTimer = 0;

                previousPancakePos = placeToInstantiatePancakePrefab.position;

                currentPancakeVelocity = 0;
            }
            else
            {
                pancakeInstantiationTimer += Time.deltaTime;

            }

        }
        else 
        {
            pancakeInstantiationTimer = 0;
        
        }


    }

    void GetPancakeVelocity()
    {
        if (currentPancake != null)
        {
            currentPancakeVelocity = (currentPancake.position - previousPancakePos).magnitude / Time.deltaTime;

            previousPancakePos = currentPancake.position;

            //Debug.Log(currentPancakeVelocity);

            

        }

    }

    void LaunchPancake()
    {
        //Here, we're going to check if --and how-- we should launch the pancake, according to things like velocity and angle

        if (currentPancake != null)
        {
            if (currentPancakeVelocity > velocityForPancakeLiftoff && transform.rotation.eulerAngles.z >0)
            {
                //Debug.Log(currentPancakeVelocity + "is more than" + velocityForPancakeLiftoff);

                //The pancake will need to act on its own from here. Time to set it up for launch!


                Pancake pancakeController = currentPancake.GetComponent<Pancake>();

                //We also have to check which direction the pan is being flung in--up or down?
                if (previousPancakePos.y > currentPancake.position.y)
                {
                    //if the pan is being flung downward, then we need to make it slide off the pan a little;
                    pancakeController.Liftoff(Vector3.right * currentPancakeVelocity);
                }
                else {

                    //If the pan is being flung upward, then we launch it!
                    pancakeController.Liftoff(Vector3.up * currentPancakeVelocity * 0.5f);
                }


                


                currentPancake.parent = null;

                currentPancake = null;



                currentPancakeVelocity = 0;


            }
        }
    }



}
