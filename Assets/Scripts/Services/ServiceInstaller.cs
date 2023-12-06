using DefaultNamespace;
using Infastructure;
using Player;
using Zenject;

namespace Services
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InjectService.Instance.SetContainer(Container);
            
            BindService();
            BindModel();
        }
    
        private void BindModel()
        {

        }
    
        private void BindService()
        {
            Container.Bind<StaticDataService>().AsSingle();
            Container.Bind<UnitSpawnService>().AsSingle();
            Container.Bind<UnitTrackerService>().AsSingle();
            Container.Bind<SeekCoverService>().AsSingle();
            Container.Bind<CurrencyService>().AsSingle();
        }
    }
}