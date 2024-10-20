using BallBlast.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast.UI
{
    
    public class Popup : Frame
    {
        [SerializeField]
        private AudioClip clipOnPopup;

        protected override void AfterShow()
        {
            base.AfterShow();

            if(clipOnPopup != null)
                SoundManager.Instance.Play2D(clipOnPopup);
        }
    }
}