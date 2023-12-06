using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ParticleConfig", menuName = "Data/ParticleConfig")]
    public class ParticleConfig : ScriptableObject
    {
        public List<ParticleStruct> Particles;
    }
}