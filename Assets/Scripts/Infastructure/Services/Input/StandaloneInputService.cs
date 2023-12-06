using UnityEngine;

namespace Infastructure.Services
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                Vector2 axis = new (SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
                
                if (axis == Vector2.zero) 
                    axis = new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Horizontal));
                return axis;
            }
        }
    }
}