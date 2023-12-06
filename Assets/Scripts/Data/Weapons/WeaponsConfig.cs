using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Data/WeaponsConfig")]
    public class WeaponsConfig : ScriptableObject
    {
        public List<WeaponData> Weapons;
    }
}