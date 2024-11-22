using UnityEngine;

namespace Objects
{
    [CreateAssetMenu(fileName = "Ball Type", menuName = "ScriptableObjects/Ball Types", order = 1)]
    public class BallTypeSO : ScriptableObject
    {
        public Ball prefab;
        public BallType type;
    }

    public enum BallType
    {
        S,
        M,
        L
    }
}