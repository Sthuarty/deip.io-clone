using UnityEngine;

public class Gun : MonoBehaviour {
    private ParticleSystem bulletsPaticleSystem;


    private void Awake() => bulletsPaticleSystem = GetComponentInChildren<ParticleSystem>();

    public void Fire() => bulletsPaticleSystem.Play();
}
