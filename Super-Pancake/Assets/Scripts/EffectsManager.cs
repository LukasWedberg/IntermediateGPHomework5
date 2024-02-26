using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lukas.Utilities
{

    public class EffectsManager : MonoBehaviour
    {
        public static EffectsManager instance;

        public PlayerController player;

        Camera mainCam;

        Vector3 mainCamOffset;
        

        bool shakeScreen = false;
        float shakeEffectIntensity = 0f;
        float shakeEffectTimer = 0f;


        private void Awake()
        {
            //First time?
            if (instance == null)
            {
                //Assign instance
                instance = this;
                //Don't destroy this gameObject when cleaning up to load a new scene
                DontDestroyOnLoad(this);

            }
            else
            {
                //Destroy self if there is a duplicate
                Destroy(gameObject);
            }


            player = GameObject.Find("MouseChef").GetComponent<PlayerController>();

            mainCam = Camera.main;

            mainCamOffset = mainCam.transform.localPosition;

        }


        public void PlayerKnockback(Vector3 direction)
        {
            player.currentExtraHorizontalVelocity = direction.x;

            player.verticalVelocity += direction.y;

            player.beingBounced = true;
        }

        public void ActivateScreenShake(float duration, float intensity)
        {
            shakeScreen = true;

            shakeEffectTimer = duration;
            shakeEffectIntensity = intensity;


        }

        public void Update() {

            if (shakeScreen)
            {
                if (shakeEffectTimer > 0)
                {
                    mainCam.transform.localPosition = mainCamOffset + new Vector3(Random.Range(-shakeEffectIntensity, shakeEffectIntensity), Random.Range(-shakeEffectIntensity, shakeEffectIntensity),0);

                    shakeEffectTimer -= Time.deltaTime;

                }
                else
                {
                    shakeScreen = false;

                    mainCam.transform.localPosition = mainCamOffset;
                }


            }
        
        }


    }


}
