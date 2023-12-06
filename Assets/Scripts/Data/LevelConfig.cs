using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class LevelConfig
    {
        public List<EnemyStruct> EnemyConfig;
        [NonSerialized] public List<Transform> CoverPoints;
    }
}