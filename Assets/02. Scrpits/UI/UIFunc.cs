using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIFunc : MonoBehaviour
    {
        
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
