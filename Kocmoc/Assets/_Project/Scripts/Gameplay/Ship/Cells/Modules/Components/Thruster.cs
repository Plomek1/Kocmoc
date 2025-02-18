using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Thruster : Module
    {
        public new ThrusterData data
        {
            get => (ThrusterData)base.data;
            set => base.data = value;
        }

        private void Start()
        {

        }
    }
}
