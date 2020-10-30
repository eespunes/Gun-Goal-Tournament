using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    
    [SerializeField] private AudioClip noAmmo, reload,jumpInit;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void HoldAiming()
    {
        _animator.SetBool("holdAiming", true);
    }
    
    public void ReloadSound()
    {
        _audioSource.clip = reload;
        _audioSource.Play();
    }
    public void ShootNoAmmoSound()
    {
        _audioSource.clip = noAmmo;
        _audioSource.Play();
    }
    public void JumpInitSound()
    {
        _audioSource.clip = jumpInit;
        _audioSource.Play();
    }
}