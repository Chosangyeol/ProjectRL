using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Config;

namespace UI
{
    public class UIFunc : MonoBehaviour
    {
        ConfigUserInput CUI;
        public GameObject PausePanel;
        public GameObject BlurPanel;

        private void Update()
        {
            if (CUI.GetKeyDown("keyPause")) // Esc
            {
                // 나중에 특정 씬에서는 안 펼쳐지게 하면 될 듯
                ObjectShow(PausePanel);
                ObjectShow(BlurPanel);
            }
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
    }
}
