using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [Range(0f, 2f)][SerializeField] private float _masterVolume;
    [SerializeField] private SoundsCollectionSO _soundsCollectionSO;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;

    private AudioSource _currentMusic;

    void Start()
    {
        Music();
    }

    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        Health.OnDeath += Health_OnDeath;
    }

    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        Health.OnDeath -= Health_OnDeath;
    }

    void PlayRandomSound(SoundSO[] sounds)
    {
        if (sounds != null && sounds.Length > 0)
        {
            SoundSO soundSO = sounds[Random.Range(0, sounds.Length)];
            SoundToPlay(soundSO);
        }
    }

    void SoundToPlay(SoundSO soundSO)
    {
        AudioClip clip = soundSO.Clip;
        float pitch = soundSO.Pitch;
        float volume = soundSO.Volume * _masterVolume;
        bool loop = soundSO.Loop;
        AudioMixerGroup audioMixerGroup;

        if (soundSO.RandomizePitch)
        {
            float randomPitchModifier =
                Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);
            pitch = randomPitchModifier;
        }

        switch (soundSO.AudioType)
        {
            case SoundSO.AudioTypes.SFX:
                audioMixerGroup = _sfxMixerGroup;
                break;
            case SoundSO.AudioTypes.Music:
                audioMixerGroup = _musicMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }
        
        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }

    void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();

        if (!loop) Destroy(soundObject, clip.length);

        if (audioMixerGroup == _musicMixerGroup)
        {
            if (_currentMusic != null)
            {
                _currentMusic.Stop();
            }

            _currentMusic = audioSource;
        }
    }

    void Gun_OnShoot()
    {
        PlayRandomSound(_soundsCollectionSO.GunShoot);
    }

    void PlayerController_OnJump()
    {
        PlayRandomSound(_soundsCollectionSO.Jump);
    }

    void Health_OnDeath(Health health)
    {
        PlayRandomSound(_soundsCollectionSO.Splat);
    }

    void Music()
    {
        PlayRandomSound(_soundsCollectionSO.FightMusic);
    }
}
