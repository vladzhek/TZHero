using System.Reflection.Emit;
using DefaultNamespace;
using Player;
using Services;
using UI;
using UI.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Infastructure
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "PlayerInitialPoint";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        
        private UnitSpawnService _unitSpawnService;
        private StaticDataService _staticDataService;

        [Inject]
        private void Construct(UnitSpawnService unitSpawnService, StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _unitSpawnService = unitSpawnService;
        }


        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
        }

        public void Enter(string scene)
        {
            InjectService.Instance.Inject(this);
            
            _curtain.Show();
            _sceneLoader.Load(scene, OnLoaded);
        }

        private void OnLoaded()
        {
            var coverPoints = GameObject.FindWithTag("CoverPoints").GetComponent<LevelCoverPoints>();
            if (coverPoints != null)
            {
                _staticDataService.LevelData.LevelConfigs[0].CoverPoints = coverPoints.GetLevelPoints();
            }

            _unitSpawnService.Spawn();
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _curtain.Hide();
        }
    }
}