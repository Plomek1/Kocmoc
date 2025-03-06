using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kocmoc
{
    public static class Utility
    {
        public static bool IsMouseOverUI()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> objectsUnderMouse = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, objectsUnderMouse);
            int ignoreCounter = 0;
            foreach (var obj in objectsUnderMouse)
            {
                if (obj.gameObject.TryGetComponent(out MouseUIClickthrough m))
                    ignoreCounter++;
            }
            return objectsUnderMouse.Count - ignoreCounter > 0;
        }

        public static string ArrayToString<T>(T[] array)
        {
            string arrString = "(";
            foreach (T item in array)
                arrString += item.ToString() + ", ";
            arrString = arrString.Substring(0, arrString.Length - 2);
            arrString += ')';
            return arrString;
        }

        public static int TrailingZeroCount(int n)
        {
            int count = 0;
            while ((n & 1) == 0)
            {
                count++;
                n >>= 1;
            }
            return count;
        }

    }
}
