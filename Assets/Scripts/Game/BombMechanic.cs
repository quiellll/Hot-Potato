using UnityEngine;

public class BombMechanic : MonoBehaviour
{
    public bool IsBombLive { get; private set; }

    // ajustar valores a gusto
    public float shakeThreshold = 2.5f;
    public float explosionCooldown = 1f; // esto es un poco de debug para que no me explote muchas veces seguidas

    private Vector3 lastAcceleration;
    private float lastExplosionTime = 0;

    public void Start()
    {
        lastAcceleration = Input.acceleration;
        ResetAccelerometer();
        SetBombLive();
    }

    public void Update()
    {
        if (IsBombLive)
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
        lastAcceleration = Input.acceleration;
        lastExplosionTime = Time.time;
    }

    private void DetectShake()
    {
        // aceleracion actual
        Vector3 currentAcceleration = Input.acceleration;

        // diferencia de aceleracion
        Vector3 deltaAcceleration = currentAcceleration - lastAcceleration;

        // comprobacion
        if (deltaAcceleration.sqrMagnitude > shakeThreshold * shakeThreshold &&
            Time.time - lastExplosionTime > explosionCooldown)
        {
            Explode();
            lastExplosionTime = Time.time; // reinicia el cooldown esto luego se puede borrar
        }

        Debug.Log($"acelerometro: {deltaAcceleration}");


        // actualiza el valor
        lastAcceleration = currentAcceleration;
    }

    public void Explode()
    {
        Debug.Log("bomba");
        IsBombLive = false;
        GameManager.Instance.HandleExplosion();
    }
}
