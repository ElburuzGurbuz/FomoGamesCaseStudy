using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Core
{
    public class LoadingManager : Service
    {
        private const float _loadingAnimationTime = 0.1f;

        [SerializeField]
        private TextMeshProUGUI _loadingText;

        [SerializeField]
        private GameObject _canvas;

        private Coroutine _cor;
        private WaitForSeconds _waitForSecond = new WaitForSeconds(_loadingAnimationTime);

       
        public override IEnumerator Initialize()
        {
            yield return null;
            OpenLoadingUI();
        }

        public void OpenLoadingUI()
        {
            _canvas.SetActive(true);
            _cor = StartCoroutine(LoadingAnim());
        }

        private IEnumerator LoadingAnim()
        {
            var builder = new StringBuilder();

            while (true)
            {
                var animationResetCounter = 4;

                builder.Clear();
                builder.Append("Loading");

                for (int i = 0; i < animationResetCounter; i++)
                {
                    builder.Append(".");
                    _loadingText.text = builder.ToString();
                    yield return _waitForSecond;
                }
            }
        }
        public void CloseLoadingScene()
        {
            _canvas.SetActive(false);
            StopCoroutine(_cor);
        }
    }
}

