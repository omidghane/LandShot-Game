using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 StartingPosition;
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period;
    // Start is called before the first frame update
    void Start()
    {
        StartingPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon){ return; }
        calculateMovementFactor();

        Vector3 offset = movementVector * movementFactor;
        transform.position = StartingPosition + offset;
    }

    private void calculateMovementFactor()
    {
        float cycle = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSin = Mathf.Sin(tau * cycle);
        movementFactor = (rawSin + 1) / 2;
    }
}
