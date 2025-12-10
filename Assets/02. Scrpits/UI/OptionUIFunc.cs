using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Config;
using UnityEngine.UI;

namespace UI.Option
{
    public class OptionUIFunc : MonoBehaviour
    {
        SInputSetting InputSet;
        public GameObject CurrentActive;
        ConfigUserInput UserInput;

        public GameObject[] KeySetButtons;

        private void OnGUI()
        {
            Event KeyEvent = Event.current;
            if (KeyEvent.isKey)
            {
                if (KeyEvent.keyCode != KeyCode.None && keynum != -1)
                {
                    switch (keynum)
                    {
                        case 0:
                            InputSet.keyMoveFront = KeyEvent.keyCode;
                            break;
                        case 1:
                            InputSet.keyMoveBack = KeyEvent.keyCode;
                            break;
                        case 2:
                            InputSet.keyMoveLeft = KeyEvent.keyCode;
                            break;
                        case 3:
                            InputSet.keyMoveRight = KeyEvent.keyCode;
                            break;

                        case 4:
                            InputSet.keySprint = KeyEvent.keyCode;
                            break;
                        case 5:
                            InputSet.keyDash = KeyEvent.keyCode;
                            break;
                        case 6:
                            InputSet.keyJump = KeyEvent.keyCode;
                            break;

                        case 7:
                            InputSet.keyInventory = KeyEvent.keyCode;
                            break;

                        case 8:
                            InputSet.keySkill1 = KeyEvent.keyCode;
                            break;
                        case 9:
                            InputSet.keySkill2 = KeyEvent.keyCode;
                            break;

                        case 10:
                            InputSet.keyPause = KeyEvent.keyCode;
                            break;
                    }

                    KeySetButtons[keynum].GetComponentInChildren<Text>().text = KeyEvent.keyCode.ToString();
                    UserInput.SetKeyInputSetting(InputSet);
                    keynum = -1;
                }
            }

        }
        int keynum = -1;
        public void ChangeKeyButtonFunc(int key)
        {
            keynum = key;
        }


        public void ChangeCurrentActive(GameObject obj)
        {
            CurrentActive = obj;
        }
        public void SlideChange(GameObject obj)
        {
            if(obj != CurrentActive)
            {
                CurrentActive.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-2000, CurrentActive.GetComponent<RectTransform>().anchoredPosition.y), 0.5f);
                CurrentActive.SetActive(false);
                CurrentActive.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, CurrentActive.GetComponent<RectTransform>().anchoredPosition.y);
                StartCoroutine(Wait());
                obj.SetActive(true);
                obj.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, CurrentActive.GetComponent<RectTransform>().anchoredPosition.y), 0.5f);
                CurrentActive = obj;
            }   
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.4f);
        }
    }
}
