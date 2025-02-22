using UnityEngine;

namespace Kocmoc
{
    public class Tab : MonoBehaviour
    {
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close() 
        {
            gameObject.SetActive(false);
        }
    }
}
