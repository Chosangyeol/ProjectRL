using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.CharaSelec
{
    public class CharaSelec : MonoBehaviour
    {
        public GameObject[] CharaModel;

        public void ChangeCharacter(int num)
        {
            DisactiveChara();
            CharaModel[num].SetActive(true);
            // 스킬셋 설명 변경 함수 추가
        }
        void DisactiveChara()
        {
            for (int i = 0; i < CharaModel.Length; i++)
            {
                CharaModel[i].SetActive(false);
            }
        }
        /* 스킬셋 설명 변경 함수
            데이터 파일에서 끌고와서 알아서 쏙쏙 넣어보자.
            당 데이터는 이쪽이 만들어서 관리하는게 좋을 것 같긴 한데 스킬 쪽 어느정도 나오고 딜 계수 나오면 설명문 맞춰서 재작성하면 될 듯.
            << 돌겠네 JSON 안 써봤는데 ㅋㅋ
        

            난이도 변경 함수도 여기.
            그냥 뭐... 시스템에서 뭔가 빼와서 바꾸면 되지 않을까...
        */
    }
}
