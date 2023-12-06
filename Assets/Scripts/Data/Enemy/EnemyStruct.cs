using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct EnemyStruct
    {
        public Vector3 SpawnPos;
        public UnitType unitType;
        public EnemyData EnemyData;
    }
}