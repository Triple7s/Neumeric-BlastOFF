using UnityEngine;

namespace GameSettings
{
    public class S_RefreshrateSettings : MonoBehaviour
    {
        
        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}

