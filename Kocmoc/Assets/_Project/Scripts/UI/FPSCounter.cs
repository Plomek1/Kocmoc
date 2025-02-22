using UnityEngine;
using TMPro;

namespace Kocmoc.UI
{
    public class FPSCounter : MonoBehaviour
    {
        private TMP_Text label;

        private void Start()
        {
            label = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            int fps = (int)(1 / Time.unscaledDeltaTime);
            label.text = fps.ToString();
        }
    }
}
