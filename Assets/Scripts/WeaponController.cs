using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Aim Settings")]
    private Transform originalTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public Transform aimDownSightsPosition;
    public Transform aimDownSightsRotation;
    public float aimTransitionSpeed = 10f;
    public float fireRate;

    [Header("Sound Settings")]
    [SerializeField] private ParticleSystem gunShotEffect;
    [SerializeField] private AudioClip gunShotSound;
    [SerializeField] private AudioClip gunReloadSound;
    private AudioSource gunAudio;

    // Start is called before the first frame update
    void Start()
    {
        originalTransform = transform;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        gunAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            AimDownSights(true);
        }
        else
        {
            AimDownSights(false);
        }
    }

    private void AimDownSights(bool aiming)
    {
        if (aiming)
        {
            // Smoothly move and rotate the weapon to the aim down sights position and rotation
            originalTransform.localPosition = Vector3.Lerp(originalTransform.localPosition, aimDownSightsPosition.localPosition, Time.deltaTime * aimTransitionSpeed);
            originalTransform.localRotation = Quaternion.Lerp(originalTransform.localRotation, aimDownSightsRotation.localRotation, Time.deltaTime * aimTransitionSpeed);
        }
        else
        {
            // Smoothly move and rotate the weapon back to its original position and rotation
            originalTransform.localPosition = Vector3.Lerp(originalTransform.localPosition, originalPosition, Time.deltaTime * aimTransitionSpeed);
            originalTransform.localRotation = Quaternion.Lerp(originalTransform.localRotation, originalRotation, Time.deltaTime * aimTransitionSpeed);
        }
    }

    public void ReloadSound() {
        gunAudio.PlayOneShot(gunReloadSound, 1.34f);
    }

    public void GunShotSound() {
        gunAudio.PlayOneShot(gunShotSound, 0.07f);
    }

    public void GunParticlePlay() {
        gunShotEffect.Play();
    }

    public void GunParticleStop() {
        gunShotEffect.Stop();
    }

    public bool GunParticlePlayingBool() {
        return gunShotEffect.isPlaying;
    }

    public bool GunParticleExists() {
        return gunShotEffect == null;
    }
}
