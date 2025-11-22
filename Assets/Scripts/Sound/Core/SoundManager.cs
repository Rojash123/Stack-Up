using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource bgAudioSource, sfxAudioSource;

    [SerializeField] SoundSO soundSO;

    private bool isSoundMuted = false;

    public bool IsSoundMuted
    {
        get { return isSoundMuted; }
        set
        {
            isSoundMuted=value;
            PlayerPrefs.SetInt("sound", value ? 0 : 1);
        }
    }

    private void Awake()
    {
        base.Awake();
        if (!PlayerPrefs.HasKey("sound"))
        {
            IsSoundMuted = false;
            return;
        }
        if (PlayerPrefs.GetInt("sound") == 0)
        {
            IsSoundMuted = true;
        }
        else
        {
            IsSoundMuted = false;
        }
    }

    private void Start()
    {
        EnableBGmusic();
    }

    public void EnableBGmusic()
    {
        if (IsSoundMuted) return;
        Debug.Log("enabled");
        bgAudioSource.Play();
    }
    public void DisableBGmusic()
    {
        bgAudioSource.Stop();
    }

    public void PlayCubeCut()
    {
        if (IsSoundMuted) return;
        sfxAudioSource.pitch = 1;
        sfxAudioSource.PlayOneShot(soundSO.GetSound(SoundType.platFormCut));
    }
    public void PlayUIClick()
    {
        if (IsSoundMuted) return;
        sfxAudioSource.PlayOneShot(soundSO.GetSound(SoundType.UIClick));
    }
    public void PlayEndOfMovementHit()
    {
        if (IsSoundMuted) return;
        sfxAudioSource.PlayOneShot(soundSO.GetSound(SoundType.endOfPlatform));
    }

    public void PlayPerfectLand(int count)
    {
        if (IsSoundMuted) return;
        if (count == 1) sfxAudioSource.pitch = 0.30f;
        sfxAudioSource.pitch += 0.03f;
        sfxAudioSource.PlayOneShot(soundSO.GetSound(SoundType.platFormPerfectLanded));
    }
    public void PlayGameOver()
    {
        sfxAudioSource.PlayOneShot(soundSO.GetSound(SoundType.gameOver));
    }

}
