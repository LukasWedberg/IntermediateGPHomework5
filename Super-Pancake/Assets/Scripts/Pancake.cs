using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lukas.Utilities;

public class Pancake : MonoBehaviour
{
    public bool freefall = false;
    float gravityValue = -.1f;
    bool bouncy = false;


    public bool exploding = false;



    float verticalVelocity = 0;
    float horizontalVelocity = 0;
    float angularVelocity = 0;


    private Rigidbody myRB;
    private BoxCollider myBC;
    public SphereCollider myExplosionRadius;

    ParticleSystem explosionParticles;


    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myBC = GetComponent<BoxCollider>();
        myExplosionRadius = GetComponent<SphereCollider>();

        explosionParticles = GetComponent<ParticleSystem> ();
    }

    // Update is called once per frame
    void Update()
    {
        PancakePhysics();
    }

    public void Liftoff(Vector3 velocity)
    {
        //Debug.Log("We have liftoff!");

        verticalVelocity = velocity.y;

        horizontalVelocity = velocity.x;

        freefall = true;

        angularVelocity = transform.rotation.eulerAngles.z;

        Invoke("SelfDestruct", 8);

    }

    void PancakePhysics()
    {
        if (freefall)
        {
            
            Vector3 movementVector = new Vector3(horizontalVelocity, verticalVelocity, 0) * Time.deltaTime;

            transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z + angularVelocity * Time.deltaTime);

            verticalVelocity += gravityValue;

            horizontalVelocity *= .9f;

            transform.position += movementVector;

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (freefall)
        {
            if (other.CompareTag("Untagged"))
            {
                //This part of the if statement assumes we hit the ground or something
                //This means the pancake will turn into a trampoline
                //Debug.Log("And we've landed!");
                freefall = false;

                myRB.isKinematic = false;
                myBC.isTrigger = false;

                bouncy = true;



            }
            else if (other.CompareTag("Pan") && verticalVelocity < -2)
            {
                //The player caught the pan! Time to explode.
                explosionParticles.Play();

                butterExplosion();

            }
        }

        else if (other.gameObject.tag == "Player" && !CompareTag("Pan"))
        {
            //Debug.Log("2 Cowabunga!!!");
            EffectsManager.instance.PlayerKnockback(Vector3.up * 9f);
        }

    }

    void OnCollisionEnter(Collision other) {
        
        

    }


    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    public virtual void butterExplosion()
    {
        //Debug.Log("KABOOM");

        freefall = false;

        exploding = true;

        myExplosionRadius.enabled = true;

        gameObject.tag = "Explosion";

        EffectsManager.instance.ActivateScreenShake(.5f, 0.1f);

    }


}
