using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HeadbobSystem : MonoBehaviour
{
    private static float Amount = 0.004f;

    private static float Frequency = 10.0f;

    private float Smooth = 80.0f;

    Vector3 StartPos;

    void Start()
    {
        StartPos = transform.localPosition;
    }

    void Update()
    {
        CheckForHeadbobTrigger();
        StopHeadbob();
    }



    private void CheckForHeadbobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }



    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;

        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);

        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);

        transform.localPosition += pos;

        return pos;
    }



    private void StopHeadbob()
    {
        if (transform.localPosition == StartPos) return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
    }

    public static void ChangeData(float amount, float frequency)
    {
        Amount = amount;
        Frequency = frequency;
    }

}
