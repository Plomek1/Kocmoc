using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Thruster : Module
    {
        private Rigidbody2D shipRb;

        private Vector2 currentThrustDirection;

        public new ThrusterData data
        {
            get => base.data as ThrusterData;
            protected set => base.data = value;
        }

        protected override void OnStart()
        {
            base.OnStart();
            shipRb = ship.GetComponent<Rigidbody2D>();
        }

        protected override void SetController(ShipController controller)
        {
            base.SetController(controller);
            controller.ThrustDirectionUpdated += OnThrustDirectionUpdate;
        }
        
        private void OnThrustDirectionUpdate(Vector2 newThrustDirection) => currentThrustDirection = newThrustDirection;

        private void FixedUpdate()
        {
            if (currentThrustDirection == Vector2.zero)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                return;
            }

            float thrustDirectionDot = Vector2.Dot(transform.up, currentThrustDirection);
            
            if (thrustDirectionDot > .001f)
            {
                shipRb.AddForce(transform.up * data.thrustForce * thrustDirectionDot);
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
                GetComponent<SpriteRenderer>().color = Color.white;
        }

        public override void Init()
        {
            cell = GetComponent<ShipCell>();
            
            foreach (ModuleData data in cell.data.modules)
            {
                this.data = data as ThrusterData;
                if (this.data != null) break;
            }
        }
    }
}
