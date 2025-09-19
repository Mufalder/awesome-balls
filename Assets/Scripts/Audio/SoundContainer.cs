using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundContainer : MonoBehaviour
    {
        [System.Serializable]
        public class Sound
        {

            [SerializeField]
            private string id = "";
            public string ID => id;
            [SerializeField, Range(0, 128)]
            private int priority = 0;
            public int Priority => priority;
            [SerializeField, Range(0, 1)]
            private float volume = 1;
            public float Volume => volume;
            [SerializeField, Range(-2, 2)]
            private float pitch = 1;
            [SerializeField, Range(0, 1)]
            private float pitchRange = 0;
            public float Pitch => pitch + Random.Range(-pitchRange, pitchRange);
            [SerializeField]
            private float delay = 0;
            [SerializeField]
            private AudioClip[] clips;
            public AudioClip[] Clips => clips;

            private float cooldown = 0;

            public AudioClip Play()
            {
                if (cooldown > 0)
                    return null;

                cooldown = delay;
                return clips.RandomElement();
            }

            public void Update()
            {
                if (cooldown > 0)
                    cooldown -= Time.deltaTime;
            }

        }

        [SerializeField]
        private List<Sound> sounds = new List<Sound>();
        public List<Sound> Sounds => sounds;
        [SerializeField]
        private AudioSource source = null;

        private int lastPriority;

        private void Update()
        {
            foreach(Sound sound in sounds)
            {
                sound.Update();
            }
        }

        private bool Check(string id, out int index)
        {
            index = Sounds.FindIndex(x => x.ID == id);

            if (index < 0)
                return false;

            if (source.isPlaying && Sounds[index].Priority < lastPriority)
                return false;

            if (Sounds[index].Clips.Length <= 0)
                return false;

            return true;
        }

        private void Play(int index, float volume, float pitch)
        {
            AudioClip clip = Sounds[index].Play();

            if (clip == null)
                return;

            lastPriority = Sounds[index].Priority;
            source.Stop();
            source.volume = volume;
            source.pitch = pitch;
            source.clip = clip;
            source.Play();
        }

        public bool IsPlaying()
        {
            return source.isPlaying;
        }

        public void StopSound()
        {
            source.Stop();
        }

        public void PlaySound(string id)
        {
            if (!Check(id, out int index))
                return;

            Play(index, Sounds[index].Volume, Sounds[index].Pitch);
        }

        public void PlaySound(string id, float pitch)
        {
            if (!Check(id, out int index))
                return;

            Play(index, Sounds[index].Volume, pitch);
        }

        public void PlaySound(string id, float pitch, float volume)
        {
            if (!Check(id, out int index))
                return;

            Play(index, volume, pitch);
        }
    }
}