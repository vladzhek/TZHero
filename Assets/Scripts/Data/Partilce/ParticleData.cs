using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ParticleData", menuName = "Data/ParticleData")]
    public class ParticleData : ScriptableObject
    {
        public ParticleSystem Particle;
    }
}