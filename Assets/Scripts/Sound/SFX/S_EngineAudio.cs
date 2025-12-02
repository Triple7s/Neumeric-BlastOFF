using UnityEngine;

public class S_EngineAudio : MonoBehaviour
{
    // --- Audio Sources and Max Values ---
    public AudioSource runningsound;
    public float runningMaxVolume = 1f; // Default values added for clarity
    public float runningMaxPitch = 2f;

    public AudioSource idlesound;
    public float idleMaxVolume = 0.6f;
    public float idleMaxPitch = 1f;

    // --- Rigidbody Reference ---
    public Rigidbody rb;

    // --- Tuning Parameters ---
    [Tooltip("The maximum speed (magnitude) the vehicle can reach.")]
    public float maxSpeed = 30f;

    // --- Start Method (Good Practice) ---
    void Start()
    {
        // Ensure the Rigidbody is assigned
        if (rb == null)
        {
            rb = GetComponentInParent<Rigidbody>(); // Try to get it from the parent
            if (rb == null)
            {
                Debug.LogError("Rigidbody is not assigned or found in the parent. Engine audio won't function.");
                enabled = false; // Disable the script if we can't find it
                return;
            }
        }

        // Ensure both sounds are playing and loop
        if (runningsound != null) runningsound.loop = true;
        if (idlesound != null) idlesound.loop = true;

        if (runningsound != null && !runningsound.isPlaying) runningsound.Play();
        if (idlesound != null && !idlesound.isPlaying) idlesound.Play();
    }

    // --- Update Method (Core Logic) ---
    void Update()
    {
        // Get the current speed magnitude from the Rigidbody
        float currentSpeed = rb.linearVelocity.magnitude;

        // Calculate a normalized value (0 to 1) representing the speed relative to maxSpeed
        // Mathf.Clamp01 ensures the value stays between 0 and 1
        float speedRatio = Mathf.Clamp01(currentSpeed / maxSpeed);

        // --- Apply Logic to Running Sound (Engine Revving) ---
        if (runningsound != null)
        {
            // The running sound's volume and pitch increase with speedRatio
            runningsound.volume = speedRatio * runningMaxVolume;
            runningsound.pitch = 1f + speedRatio * (runningMaxPitch - 1f); // 1f is base pitch
        }

        // --- Apply Logic to Idle Sound (Engine Hum) ---
        if (idlesound != null)
        {
            // The idle sound's volume decreases as the speedRatio increases
            // We use (1f - speedRatio) to inverse the volume curve
            idlesound.volume = (1f - speedRatio) * idleMaxVolume;

            // Pitch can stay constant or slightly decrease, or be tied to 1f - speedRatio
            idlesound.pitch = 1f + (1f - speedRatio) * (idleMaxPitch - 1f);
        }
    }
}