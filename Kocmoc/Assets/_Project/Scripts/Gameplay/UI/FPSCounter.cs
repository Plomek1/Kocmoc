using System.Collections;
using TMPro;
using UnityEngine;

namespace Kocmoc
{
    public class FPSCounter : MonoBehaviour
    {
        private TMP_Text label;

        private void Start()
        {
            label = GetComponent<TMP_Text>();
            //InvokeRepeating("UpdateCounter", 0f, .5f);
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
