using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Config;
using DG.Tweening;
using Player;

namespace UI
{
    public class UIFunc : MonoBehaviour
    {
        public GameObject PausePanel;
        public GameObject BlurPanel;
        public GameObject InGameScreen;
        public GameObject askQuit;
        public PlayerController pc;

        public bool IsEscBanned = false;
        bool IsPaused = false;

        private void Update()
        {
            
            if (ConfigUserInput.Instance.GetKeyDown("keyPause") && !IsEscBanned)
            {
                if (IsPaused)
                {
                    ObjectHide(PausePanel);
                    ObjectHide(BlurPanel);
                    ObjectShow(InGameScreen);
                    pc.FixCursor(true);
                    IsPaused = false;
                }
                else
                {
                    ObjectShowWithMove(PausePanel);
                    ObjectShow(BlurPanel);
                    ObjectHide(InGameScreen);
                    ObjectHide(askQuit);
                    pc.FixCursor(false);
                    IsPaused = true;
                }
            }
        }

        public void OffPause()
        {
            pc.FixCursor(true);
            IsPaused = false;
        }

        public void ChangeScene(string sceneName)
        {
            UI.SceneManage.LoadSceneManagement.LoadScene(sceneName);
            ResetGame();
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

        public void ResetGame()
        {
            var player = FindFirstObjectByType<PlayerController>();
            Destroy(player.gameObject);

            //var pool = FindFirstObjectByType<PoolManager>();
            //Destroy(pool.gameObject);

            GameObject bulletpool = GameObject.Find("PlayerBulletParent");
            Destroy(bulletpool);

            var gm = FindFirstObjectByType<GameManager>();
            Destroy(gm.gameObject);
        }
    }
}
