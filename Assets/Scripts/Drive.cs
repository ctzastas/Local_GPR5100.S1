using UnityEngine;
using UnityEngine.UI;

public class  Drive : MonoBehaviour {
    
    [Header("Collider")]
    [Space]
    // Array for wheels collider
    [SerializeField] private WheelCollider [] wheelsCollider;
    
    private float moveX;                                  // Steering car
    private float moveY;                                  // Move car 
    private float braking;                                // Brake car

    [Header("Forces")]
    [Space]
    // Variables for forces
    [SerializeField] private float maxSpeed = 320f;       // The maximum speed of the car
    [SerializeField] private float torque = 800f;        // The torque power of the car
    [SerializeField] private float maxSteerAngle = 30;   // Steering Angle of the wheels
    [SerializeField] private float handBrake = 2500;     // The power of braking the car
    private float gearsLength = 4f;                      // This value change the engine sound of the car  
    [SerializeField] private float gears = 4f;
    private float RPM;
    private int currentGear = 1;
    private float currentGearPerc;
    private float thrustTorque;
    public Rigidbody carRigidbody;

    [Header("Meshes")]
    [Space]
    // Variables for wheel meshes
    [SerializeField] private GameObject [] wheels;
    Quaternion wheelQuaternion;
    Vector3 wheelPosition;
    
    [Header("Sounds")]
    [Space]
    // Variables for sounds
    [SerializeField] private AudioSource skidSound;
    [SerializeField] private AudioSource highAcceleration;
    [SerializeField] private float lowPitch = 1f;           // Transform low engine sound
    [SerializeField] private float highPitch = 7f;          // Transform high engine sound 

    [Header("Prefabs")] 
    [Space] 
    [SerializeField] private ParticleSystem wheelSmokePrefab;
    [SerializeField] private GameObject playerNamePrefab;
    ParticleSystem [] wheelSmoke = new ParticleSystem[4];
    
    [Header("Lights")]
    [Space]
    [SerializeField] private GameObject [] brakeLight = new GameObject[2];

    // Start is called before the first frame update
    void Start() {
        carRigidbody = GetComponent<Rigidbody>();
        GameObject playerName = Instantiate(playerNamePrefab);
        playerName.GetComponent<PlayerName>().target = carRigidbody.gameObject.transform;
        playerName.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName");
        // Set brake light off
        for (int i = 0; i < 2; i++) {
            brakeLight[i].SetActive(false);    
        }
        
        // Stop particle system for all wheels
        for (int i = 0; i < 4; i++) {
            wheelSmoke[i] = Instantiate(wheelSmokePrefab);
            wheelSmoke[i].Stop();
        }
    }

    // Update is called once per frame
    public void Update() {
        // Wait for countdown to end then player can move
        if (!UIManager.racing) {
            return;
        }
        moveY = Input.GetAxis("Vertical");
        moveX = Input.GetAxis("Horizontal");
        braking = Input.GetAxis("Jump");
        Go(moveY, moveX, braking);
        SpeedInKHM();
        Skidding();
        EngineSound();
    }
    
    // Calculates car speed in khm
    public float currentSpeed => carRigidbody.velocity.magnitude * gearsLength;

    public void Go(float acceleration, float steering, float braking) {
        // Set acceleration, steering and brake with min and max values of Mathf.Clamp 
        acceleration = Mathf.Clamp(acceleration, -1, 1);
        steering = Mathf.Clamp(steering, -1, 1) * maxSteerAngle;
        braking = Mathf.Clamp(braking, 0, 1) * handBrake;
        // Enable brake light when the value is higher than 0
        for (int j = 0; j < 2; j++) {
            if (braking > 0) {
                brakeLight[j].SetActive(true);
            }
            else {
                brakeLight[j].SetActive(false);
            }   
        }
        thrustTorque = 0;
        // Set a max speed of the car
        if (currentSpeed < maxSpeed) {
            thrustTorque = acceleration * torque;
        }
        for (int i = 0; i < 4; i++) {
            // Set a rear wheels torque
            if (i > 1) {
                wheelsCollider[i].motorTorque = thrustTorque;
            }
            // Set only the first 2 wheels to turn
            if (i < 2) {
                wheelsCollider[i].steerAngle = steering;
            }
            else {
                wheelsCollider[i].brakeTorque = braking;
            }
            // Rotating the mesh(wheels) 
            wheelsCollider[i].GetWorldPose(out wheelPosition, out wheelQuaternion);
            wheels[i].transform.position = wheelPosition;
            wheels[i].transform.rotation = wheelQuaternion;
        }
    }

    public void Skidding() {
        // Wheels skidding
        int numSkidding = 0;
        for (int i = 0; i < 4; i++) {
            WheelHit wheelHit;
            wheelsCollider[i].GetGroundHit(out wheelHit);
            // Check if slip happens
            if (Mathf.Abs(wheelHit.forwardSlip) >= 0.97f || Mathf.Abs(wheelHit.sidewaysSlip) >= 0.7f) {
                // Increase wheels skidding
                numSkidding++;
                // Check if sound is not playing
                if (!skidSound.isPlaying) {
                    skidSound.Play();
                }
                // Position smoke in the wheels
                wheelSmoke[i].transform.position = wheelsCollider[i].transform.position - wheelsCollider[i].transform.up * wheelsCollider[i].radius;
                wheelSmoke[i].Emit(1);
            }
        }
        // Check if is not slipping(for all wheels) and is playing sound 
        if (numSkidding == 0 && skidSound.isPlaying) {
            skidSound.Stop();
        }
    }
    
    // Show speed in KHM
    public void SpeedInKHM() {
        float showSpeed = Mathf.Round(currentSpeed);
        Debug.Log("Speed " + showSpeed + " Km/h");
    }

    public void EngineSound() {
        float gearPercentage = (1 / gears);
        float targetGear = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1), Mathf.Abs(currentSpeed / maxSpeed));
        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGear, Time.deltaTime * 5f);
        float gearsNumFactor = currentGear / (float) gears;
        RPM = Mathf.Lerp(gearsNumFactor, 1, currentGearPerc);
        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1 / gears) * (currentGear + 1);
        float downGearMax = (1 / gears) * currentGear;
        if (currentGear > 0 && speedPercentage < downGearMax) {
            currentGear--;
        }
        if (speedPercentage > upperGearMax && currentGear < (gears - 1)) {
            currentGear++;
        }
        float pitch = Mathf.Lerp(lowPitch, highPitch, RPM);
        highAcceleration.pitch = Mathf.Min(highPitch,pitch) * 0.3f;
    }
}