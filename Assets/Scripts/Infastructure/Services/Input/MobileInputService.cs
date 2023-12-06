using UnityEngine;

namespace Infastructure.Services
{
    public class MobileInputService : InputService
    {
        public override Vector2 Axis =>
            new (SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}