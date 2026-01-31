using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{

    [SerializeField] private AudioSource _backgroundSource;
    [SerializeField] private AudioSource _oneShotSource;

    [SerializeField] private AudioClip _birdSound;
    [SerializeField] private AudioClip _karnevalBackground;
    [SerializeField] private AudioClip _nocVjesticaBackground;


    public void PlayBirdSound() => _oneShotSource.PlayOneShot(_birdSound);
    public void PlayKarnevalBackground()
    {
        _backgroundSource.clip = _karnevalBackground;
        _backgroundSource.Play();
    }

    internal void PlayNocVjesticaBackground()
    {
        _backgroundSource.clip = _nocVjesticaBackground;
        _backgroundSource.Play();
    }

    internal void PlayOnce(AudioClip sound) => _oneShotSource.PlayOneShot(sound);
}
