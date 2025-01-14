using UnityEngine;
using UnityEngine.InputSystem;

public class BombMechanic : MonoBehaviour
{
    public bool IsBombLive { get; private set; }
    public float shakeThreshold = 2.5f;
    public float explosionCooldown = 1f;

    private Vector3 lastAcceleration;
    private float lastExplosionTime = 0;

    public void Start()
    {
        // Check if device has accelerometer
        if (Accelerometer.current == null)
        {
            Debug.LogWarning("No accelerometer found on device!");
            return;
        }

        // Enable the accelerometer
        InputSystem.EnableDevice(Accelerometer.current);

        lastAcceleration = Accelerometer.current.acceleration.ReadValue();
        ResetAccelerometer();
        SetBombLive();
    }

    public void OnDisable()
    {
        // Clean up when the component is disabled
        if (Accelerometer.current != null)
        {
            InputSystem.DisableDevice(Accelerometer.current);
        }
    }

    public void Update()
    {
        if (IsBombLive && Accelerometer.current != null)
        {
            DetectShake();
        }
    }

    public void SetBombLive()
    {
        IsBombLive = true;
        ResetAccelerometer();
    }

    public void DefuseBomb()
    {
        IsBombLive = false;
    }

    private void ResetAccelerometer()
    {
        if (Accelerometer.current != null)
        {
            lastAcceleration = Accelerometer.current.acceleration.ReadValue();
        }
        lastExplosionTime = Time.time;
    }

    private void DetectShake()
    {
        // Get current acceleration
        Vector3 currentAcceleration = Accelerometer.current.acceleration.ReadValue();

        // Calculate acceleration delta
        Vector3 deltaAcceleration = currentAcceleration - lastAcceleration;

        // Check if shake exceeds threshold
        if (deltaAcceleration.sqrMagnitude > shakeThreshold * shakeThreshold &&
            Time.time - lastExplosionTime > explosionCooldown)
        {
            Explode();
            lastExplosionTime = Time.time;
        }

        // Update last acceleration
        lastAcceleration = currentAcceleration;
    }

    public void Explode()
    {
        IsBombLive = false;
        GameManager.Instance.HandleExplosion();
    }
}