using System.Security.Cryptography;
using UnityEngine;

public class CarFollow : MonoBehaviour
{
    Transform carTransform;
    Transform cameraPointTransform;
    Vector3 velocity = Vector3.zero;

    void Awake()
    {


        //car transform
        carTransform = GameObject.Find("Free Racing Car Blue Variant").transform;
        if (!carTransform)
        {
            Debug.Log("Car dont get\n");
            return;
        }

        cameraPointTransform = carTransform.Find("CarPoint");
        if (!cameraPointTransform)
        {
            Debug.Log("Car Point dont get\n");
            return;
        }




    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(carTransform);
        transform.position = Vector3.SmoothDamp(transform.position, cameraPointTransform.position, ref velocity, 5f * Time.deltaTime);

    }
}
