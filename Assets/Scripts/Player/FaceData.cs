using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FaceData", order = 1)]
    public class FaceData : ScriptableObject
    {
        public Material faceMaterial;
        public Material skinMaterial;
        public Material hairMaterial;
    }
}
