using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BallBlast.UI
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textCounter;

        private float _deltaTime = 0;
        private int fps = 0;

        private void OnEnable()
        {
            StartCoroutine(FpsUpdater());
        }

        private IEnumerator FpsUpdater()
        {
            var ret = new WaitForSeconds(1f);
            while(true)
            {
                textCounter.text = string.Format("{0:0.} fps", fps);
                yield return ret;
            }
        }


        private void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            fps = Mathf.RoundToInt(1.0f / _deltaTime);
        }
    }
}