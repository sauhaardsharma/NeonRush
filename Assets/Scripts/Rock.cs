using Unity.Cinemachine;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] ParticleSystem collisionParticleSystem;
    [SerializeField] AudioSource rockAudioSource;
    [SerializeField] float shakeModifier = 10f;
    [SerializeField] float cooldownTime = 1f;
    CinemachineImpulseSource cinemachineImpulseSource;
    float elapsedTime = 1f;

    void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (elapsedTime < cooldownTime) return;
        
        FireImpulse();
        CollisionFX(other);
        elapsedTime = 0f;
    }

    private void FireImpulse()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float shakeIntensity = (1f / distance) * shakeModifier;
        shakeIntensity = Mathf.Min(shakeIntensity, 1f);
        cinemachineImpulseSource.GenerateImpulse(shakeIntensity);
    }

    void CollisionFX(Collision other)
    {
        ContactPoint contactPoint = other.contacts[0];
        collisionParticleSystem.transform.position = contactPoint.point;
        collisionParticleSystem.Play();
        rockAudioSource.Play();
    }
}
