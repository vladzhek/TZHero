using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Services
{
    public class StaticDataService
    {
        public PlayerData PlayerData;
        public Dictionary<WeaponType, WeaponData> Weapons = new ();
        public LevelData LevelData;
        public Dictionary<ParticleType, ParticleData> Paricles = new ();

        public void Load()
        {
            LoadData();
            LoadWeapons();
            LoadParticles();
        }

        private void LoadData()
        {
            PlayerData = Resources.Load<PlayerData>("Configs/PlayerData");
            LevelData = Resources.Load<LevelData>("Configs/LevelData");
            LevelData = Resources.Load<LevelData>("Configs/LevelData");
        }

        private void LoadWeapons()
        {
            var weaponsConfig = Resources.Load<WeaponsConfig>("Configs/WeaponsConfig");
            foreach (var weapon in weaponsConfig.Weapons)
            {
                Weapons.Add(weapon.Type, weapon);
            }
        }
        
        private void LoadParticles()
        {
            var config = Resources.Load<ParticleConfig>("Configs/ParticleConfig");
            foreach (var particle in config.Particles)
            {
                Paricles.Add(particle.Type, particle.Data);
            }
        }
    }
}