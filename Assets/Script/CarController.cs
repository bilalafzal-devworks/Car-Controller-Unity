using UnityEngine;

public class CarController : MonoBehaviour
{
    // WheelColliders
    WheelCollider FrontLeftWheelCollider;
    WheelCollider FrontRightWheelCollider;
    WheelCollider RearLeftWheelCollider;
    WheelCollider RearRightWheelCollider;

    //Transform
    Transform FrontLeftWheelTransform;
    Transform FrontRightWheelTransform;
    Transform RearLeftWheelTransform;
    Transform RearRightWheelTransform;

    // Rigidbody
    Rigidbody rb;

    // Vehicle tuning values
    [Header("Car Settings")]
    public float motorForce = 2500f;
    public float brakeForce = 3000f;
    public float maxSteerAngle = 30f;
    public Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0);

    // Inputs
    float horizontalInput;
    float verticalInput;
    bool isBraking;

    void Awake()
    {
        // ✅ Correct hierarchy path based on your screenshot
        Transform colliderParent = transform.Find("Wheels/Colliders");

        if (colliderParent == null)
        {
            Debug.LogError("❌ 'Wheels/Colliders' not found! Check hierarchy path.");
            return;
        }

        // Get all colliders inside the folder
        WheelCollider[] colliders = colliderParent.GetComponentsInChildren<WheelCollider>();

        foreach (WheelCollider w in colliders)
        {
            switch (w.name)
            {
                case "FrontLeftWheel":
                    FrontLeftWheelCollider = w;
                    break;
                case "FrontRightWheel":
                    FrontRightWheelCollider = w;
                    break;
                case "RearLeftWheel":
                    RearLeftWheelCollider = w;
                    break;
                case "RearRightWheel":
                    RearRightWheelCollider = w;
                    break;
            }
        }
        Transform ParentTransform = transform.Find("Wheels/Meshes");
        Transform[] wheelTransforms = ParentTransform.GetComponentsInChildren<Transform>();
        foreach (Transform t in wheelTransforms)
        {
            switch (t.name)
            {
                case "FrontLeftWheel":
                    FrontLeftWheelTransform = t;
                    break;
                case "FrontRightWheel":
                    FrontRightWheelTransform = t;
                    break;
                case "RearLeftWheel":
                    RearLeftWheelTransform = t;
                    break;
                case "RearRightWheel":
                    RearRightWheelTransform = t;
                    break;
            }
        }

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            Debug.LogWarning("⚠️ Rigidbody was missing — added automatically.");
        }

        // Adjust center of mass for stability
        rb.centerOfMass = centerOfMassOffset;
    }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        HandleBraking();
        UpdateWheels();
    }

    // --- Input handling ---
    void GetInput()
    {
        verticalInput = Input.GetAxis("Vertical");     // W/S keys or controller trigger
        horizontalInput = Input.GetAxis("Horizontal"); // A/D keys or stick
        isBraking = Input.GetKey(KeyCode.Space);       // Space for brake
    }

    // --- Movement ---
    void HandleMotor()
    {
        float torque = motorForce * verticalInput;

        FrontLeftWheelCollider.motorTorque = torque;
        FrontRightWheelCollider.motorTorque = torque;
        RearLeftWheelCollider.motorTorque = torque;
        RearRightWheelCollider.motorTorque = torque;

        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            Debug.Log($"🚗 Applying torque: {torque}");
        }
    }

    // --- Steering (Front Wheels Only) ---
    void HandleSteering()
    {
        float steerAngle = maxSteerAngle * horizontalInput;

        FrontLeftWheelCollider.steerAngle = steerAngle;
        FrontRightWheelCollider.steerAngle = steerAngle;
    }

    // --- Braking ---
    void HandleBraking()
    {
        float brakeTorque = isBraking ? brakeForce : 0f;

        FrontLeftWheelCollider.brakeTorque = brakeTorque;
        FrontRightWheelCollider.brakeTorque = brakeTorque;
        RearLeftWheelCollider.brakeTorque = brakeTorque;
        RearRightWheelCollider.brakeTorque = brakeTorque;
    }
    void UpdateWheels()
    {
        UpdateSingleWheel(FrontLeftWheelCollider, FrontLeftWheelTransform);
        UpdateSingleWheel(FrontRightWheelCollider, FrontRightWheelTransform);
        UpdateSingleWheel(RearLeftWheelCollider, RearLeftWheelTransform);
        UpdateSingleWheel(RearRightWheelCollider, RearRightWheelTransform);


    }

    void UpdateSingleWheel(WheelCollider collider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }
}
