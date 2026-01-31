using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{

    [SerializeField] private AudioSource _backgroundSource;
    [SerializeField] private AudioSource _oneShotSource;

    [SerializeField] private AudioClip _birdSound;
    [SerializeField] private AudioClip _karnevalBackground;


    public void PlayBirdSound() => Debug.Log("BIRRRRRRRRRRD");// _oneShotSource.PlayOneShot(_birdSound);
    public void PlayKarnevalBackground()
    {
        Debug.Log("BIRRRRRRRRRRD");
        _backgroundSource.clip = _karnevalBackground;
        // _backgroundSource.Play();
    }

}
