using Services;
using Zenject;

namespace Infastructure
{
    public class InjectService
    {
        private static InjectService _instance;
        public static InjectService Instance => _instance ??= new InjectService();

        private DiContainer _diContainer;
        
        public void Inject(object obj)
        {
            if(!_diContainer.IsInstalling)
                _diContainer.Inject(obj);
        }

        public void SetContainer(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
    }
}