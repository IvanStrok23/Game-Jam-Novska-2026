using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{

    [SerializeField] private AudioSource _source;

    [SerializeField] private AudioClip _birdSound;

    public void PlayBirdSound() => Debug.Log("BIRRRRRRRRRRD");// _source.PlayOneShot(_birdSound);

}
