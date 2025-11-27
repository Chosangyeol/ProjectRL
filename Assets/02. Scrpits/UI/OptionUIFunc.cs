using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace UI.Option
{
    public class OptionUIFunc : MonoBehaviour
    {
        /* 만들것
            키 세팅 변경 - 저쪽 키코드 구조체 빼다 쓰면 될 것 같기도 하고 고민좀.
            -> 데이터 저장할거면 저장된 데이터 읽어서 시작할 때 그거로 끼워두면 될 듯
            -> 버튼 클릭시 해당 버튼 내용물 [  ] 로 변경, 키 입력 시 해당 키로 변경. 겹치는 키가 있다면 null처리.

            뭐 더 만들 설정 있나...

        */
        public GameObject CurrentActive;

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
