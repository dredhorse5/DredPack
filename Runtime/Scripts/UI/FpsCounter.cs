using UnityEngine;
using UnityEngine.UI;
 
///==========================================
///         Made in Pair of slippers
///              By dredhorse5
///         gmail: dima.titov18@gmail.com
///         yandex: dredhorse5@yandex.ru
///==========================================
namespace DredPack
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private Text _fpsText;
        [SerializeField] private float UpdateEachTime = 1f;

        private float _timer;

        private void Update()
        {
            if (_timer > 0f)
                _timer -= Time.deltaTime;
            else
            {
                float fps = (1f / Time.deltaTime);
                if (_fpsText)
                    _fpsText.text = "FPS: " + Mathf.Round(fps);
                _timer = UpdateEachTime;
            }
        }
    }
}