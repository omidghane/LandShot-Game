using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    [SerializeField] float thrustRotate = 10f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem thrustParticle;
    [SerializeField] private float fullFuel = 200f;
    [SerializeField] private float fuelBurnRate = 20f;
    [SerializeField] private Slider fuelSlider;
    // private Slider newFuelSlider;

    public static float currentFuel;
    public static bool fuelIsOver = false;
    private bool haveFuel = true;
    AudioSource myAudio;
    Rigidbody rb;
    // Start is called before the first frame update

    private void Awake()
    {
        // fuelSlider.maxValue = fullFuel;
        if(gameObject.tag.Equals("Player1")){
            currentFuel = SavePlayerInfo.player1Fuel;
        }else if(gameObject.tag.Equals("Player2")){
            currentFuel = SavePlayerInfo.player2Fuel;
        }
        // Debug.Log(currentFuel + " currentFuel");
        // currentFuel = fuel;
    }

    void Start()
    {
        // newFuelSlider = Instantiate(fuelSlider,new Vector3(-302,-109.7f,0),new Quaternion(0,0,0,0));

        rb = GetComponent<Rigidbody>();
        myAudio = GetComponent<AudioSource>();

        // fuelSlider.value = currentFuel / fullFuel;
        // Debug.Log(fuelSlider.value + " fuelslider");

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag.Equals("Player1")){
            currentFuel = SavePlayerInfo.player1Fuel;
        }else if(gameObject.tag.Equals("Player2")){
            currentFuel = SavePlayerInfo.player2Fuel;
        }

        if (currentFuel <= 0)
        {
            haveFuel = false;
           if(gameObject.tag.Equals("Player1")){
                SavePlayerInfo.player1HaveFuel = false;
            }else if(gameObject.tag.Equals("Player2")){
                SavePlayerInfo.player2HaveFuel = false;
            }
        }

        thrusting();
        rotating();
    }

    void thrusting(){
        if(Input.GetKey(KeyCode.Space) && currentFuel > 0)
        {
            startThrusting();
        }
        else
        {
            stopThrusting();
        }
    }

    void rotating(){
        if(Input.GetKey(KeyCode.A))
        {
            rotateApplying(thrustRotate);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            rotateApplying(-thrustRotate);
        }
    }

      void startThrusting()
    {
        burningFuel();

        if (!myAudio.isPlaying)
        {
            myAudio.PlayOneShot(mainEngine);
        }
        if (!thrustParticle.isPlaying)
        {
            thrustParticle.Play();
        }
        rb.AddRelativeForce(force: Vector3.up * mainThrust * Time.deltaTime);
    }

    void stopThrusting()
    {
        thrustParticle.Stop();
        myAudio.Stop();
    }

    void burningFuel(){
        if(gameObject.tag.Equals("Player1")){
            SavePlayerInfo.player1Fuel -= fuelBurnRate * Time.deltaTime;
            currentFuel = SavePlayerInfo.player1Fuel;
        }else if(gameObject.tag.Equals("Player2")){
            SavePlayerInfo.player2Fuel -= fuelBurnRate * Time.deltaTime;
            currentFuel = SavePlayerInfo.player2Fuel;
        }
        
    }


    private void rotateApplying(float thrustThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * thrustThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }

}
