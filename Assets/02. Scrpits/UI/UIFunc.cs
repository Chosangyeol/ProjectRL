using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Config;
using DG.Tweening;

namespace UI
{
    public class UIFunc : MonoBehaviour
    {
        public GameObject PausePanel;
        public GameObject BlurPanel;
        public GameObject InGameScreen;

        public bool IsPaused = false;

        private void Update()
        {
            if (ConfigUserInput.Instance.GetKeyDown("keyPause")) // Esc
            {
                if (IsPaused)
                {
                    ObjectHide(PausePanel);
                    ObjectHide(BlurPanel);
                    ObjectShow(InGameScreen);
                    IsPaused = false;
                }
                else
                {
                    ObjectShowWithMove(PausePanel);
                    ObjectShow(BlurPanel);
                    ObjectHide(InGameScreen);
                    IsPaused = true;
                }

                // 나중에 특정 씬에서는 안 펼쳐지게 하면 될 듯
            }
        }

        public void OffPause()
        {
            IsPaused = false;
        }

        public void ChangeScene(string sceneName)
        {
            UI.SceneManage.LoadSceneManagement.LoadScene(sceneName);
        }
        public void QuitGame()
        {
            Application.Quit();
        }

        public void ObjectShow(GameObject obj)
        {
            obj.SetActive(true);
        }

        public void ObjectHide(GameObject obj)
        {
            obj.SetActive(false);
        }

        public void ObjectShowWithMove(GameObject obj)
        {
            Vector2 SetPos = obj.GetComponent<RectTransform>().anchoredPosition;
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);
            obj.SetActive(true);

            obj.GetComponent<RectTransform>().DOAnchorPos(SetPos, 0.5f);
        }
    }
}
