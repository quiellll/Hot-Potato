using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovementTest : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI accelerometerText;
    [SerializeField] private float speedThreshold = 2.0f;

    private Vector3 lastAcceleration;
    private float currentSpeed;

    void Start()
    {
        lastAcceleration = Input.acceleration;

    }

    void Update()
    {
        // Calculate change in acceleration
        Vector3 deltaAccel = Input.acceleration - lastAcceleration;
        currentSpeed = deltaAccel.magnitude;
        lastAcceleration = Input.acceleration;

        // Update UI text with all acceleration data and current speed
        accelerometerText.text = $"Raw Accel: {Input.acceleration}\nSpeed: {currentSpeed:F3}";

    }
}