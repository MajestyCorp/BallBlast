using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            CustomPlayerPrefs.Initialize(new WebGLPlayerPrefs());
#else
            CustomPlayerPrefs.Initialize(new DefaultPlayerPrefs());
#endif

#if UNITY_ANDROID
            Application.targetFrameRate = 60;
#endif
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