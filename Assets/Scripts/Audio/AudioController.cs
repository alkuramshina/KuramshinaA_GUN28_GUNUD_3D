using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioController: MonoBehaviour
    {
        private readonly Dictionary<AudioClip, AudioSource> _audioSources = new();
        [SerializeField] private SoundBoard soundBoard;

        public void PlayThrowingSound() => PlayAudioClip(soundBoard.throwingSound);
        public void PlayMenuSound() => PlayAudioClip(soundBoard.menuSound);
        public void PlayBallChoosingSound() => PlayAudioClip(soundBoard.ballChoosingSound);
        public void PlayPinFallingSound() => PlayAudioClip(soundBoard.pinFallingSound);
        public void PlayVictorySound() => PlayAudioClip(soundBoard.victorySound);

        private void PlayAudioClip(AudioClip clip)
        {
            if (!_audioSources.TryGetValue(clip, out AudioSource audioSource))
                audioSource = AddAudioSource(clip);

            PlayAudioSource(audioSource);
        }

        private static void PlayAudioSource(AudioSource audioSource)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.Play();
        }

        private AudioSource AddAudioSource(AudioClip clip)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();

            _audioSources.Add(clip, audioSource);

            audioSource.clip = clip;

            return audioSource;
        }
    }
}