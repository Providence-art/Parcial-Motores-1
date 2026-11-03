using HashGame.CubeWorld.Extensions;
using UnityEngine;
namespace HashGame.CubeWorld
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public AudioSource musicSource;
        public AudioSource sfxSource;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                BasicExtensions.DestroyObject(this);
                return;
            }
            DontDestroyOnLoad(gameObject);
            if (sfxSource == null)
            {
                sfxSource= gameObject.AddComponent<AudioSource>();
            }
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null || musicSource == null) return;
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null || sfxSource == null) return;
            sfxSource.PlayOneShot(clip);
        }
    }
}