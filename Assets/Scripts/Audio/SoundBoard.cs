using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "Sound board", menuName = "ScriptableObjects/Sound board", order = 2)]
    public class SoundBoard : ScriptableObject
    {
        public AudioClip throwingSound;
        public AudioClip ballChoosingSound;
        public AudioClip menuSound;
        public AudioClip pinFallingSound;
        public AudioClip victorySound;
    }
}