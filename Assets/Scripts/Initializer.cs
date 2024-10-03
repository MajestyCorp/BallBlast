using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
            var array = GetComponentsInChildren<IInitializer>();

            if(array != null)
            {
                for (var i = 0; i < array.Length; i++)
                    array[i].InitializeSelf();

                for (var i = 0; i < array.Length; i++)
                    array[i].InitializeAfter();
            }
        }
    }
}