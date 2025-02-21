using System.Runtime.CompilerServices;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Thruster : Module
    {
        private ShipController controller;
        private Rigidbody2D shipRb;

        private Vector2 currentThrustDirection;

        public new ThrusterData data
        {
            get => (ThrusterData)base.data;
            set => base.data = value;
        }

        private void Start()
        {
            shipRb = ship.GetComponent<Rigidbody2D>();

            if (ship.TryGetComponent(out controller))
                ConnectControllerEvents();
            else
                ship.ControllerAttached += OnControllerAttached;
        }

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

        private void ConnectControllerEvents()
        {
            controller.ThrustDirectionUpdated += OnThrustDirectionUpdate;
        }

        private void OnThrustDirectionUpdate(Vector2 newThrustDirection) => currentThrustDirection = newThrustDirection;

        private void OnControllerAttached(ShipController controller)
        {
            this.controller = controller;
            ConnectControllerEvents();
        }
    }
}
