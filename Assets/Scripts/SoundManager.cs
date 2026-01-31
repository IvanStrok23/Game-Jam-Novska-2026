using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{

    [SerializeField] private AudioSource _backgroundSource;
    [SerializeField] private AudioSource _oneShotSource;

    [SerializeField] private AudioClip _birdSound;
    [SerializeField] private AudioClip _karnevalBackground;
    [SerializeField] private AudioClip _nocVjesticaBackground;
    [SerializeField] private AudioClip _firstBuildingHitSound;
    [SerializeField] private AudioClip _firstPeopleHitSound;


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

    public void PlayBirdSound() => _oneShotSource.PlayOneShot(_birdSound);
    internal void PlayOnce(AudioClip sound) => _oneShotSource.PlayOneShot(sound);
    internal void PlayOnFirstBuildingHit() => _oneShotSource.PlayOneShot(_firstBuildingHitSound);
    internal void PlayOnFirstPeopleHit() => _oneShotSource.PlayOneShot(_firstPeopleHitSound);
}
