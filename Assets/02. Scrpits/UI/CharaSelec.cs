using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player.Skill;

namespace UI.CharaSelec
{
    public class CharaSelec : MonoBehaviour
    {
        
        public GameObject[] CharaModel;

        public void ChangeCharacter(int num)
        {
            DisactiveChara();
            CharaModel[num].SetActive(true);
        }
        void DisactiveChara()
        {
            for (int i = 0; i < CharaModel.Length; i++)
            {
                CharaModel[i].SetActive(false);
            }
        }

        public GameObject[] Skill1stSelected;
        public GameObject[] Skill2stSelected;
        public GameObject[] difficultyButtons;
        bool Skill1Sel = false;
        bool Skill2Sel = false;
        bool difficultySel = false;
        public void Select1stSkill(int selected)
        {
            for(int i = 0; i < Skill1stSelected.Length; i++)
            {
                Skill1stSelected[i].GetComponent<Image>().color = new Color(Skill1stSelected[i].GetComponent<Image>().color.r, Skill1stSelected[i].GetComponent<Image>().color.g, Skill1stSelected[i].GetComponent<Image>().color.b, 100 / 255f);
            }
            Skill1stSelected[selected].GetComponent<Image>().color = new Color(Skill1stSelected[selected].GetComponent<Image>().color.r, Skill1stSelected[selected].GetComponent<Image>().color.g, Skill1stSelected[selected].GetComponent<Image>().color.b, 255 / 255f);
            PlayerPrefs.SetInt("Skill0", selected);
            Skill1Sel = true;
        }
        
        public void Select2ndSkill(int selected)
        {
            for (int i = 0; i < Skill1stSelected.Length; i++)
            {
                Skill2stSelected[i].GetComponent<Image>().color = new Color(Skill2stSelected[i].GetComponent<Image>().color.r, Skill2stSelected[i].GetComponent<Image>().color.g, Skill2stSelected[i].GetComponent<Image>().color.b, 100 / 255f);
            }
            Skill2stSelected[selected].GetComponent<Image>().color = new Color(Skill2stSelected[selected].GetComponent<Image>().color.r, Skill2stSelected[selected].GetComponent<Image>().color.g, Skill2stSelected[selected].GetComponent<Image>().color.b, 255 / 255f);
            PlayerPrefs.SetInt("Skill1", selected+2);
            Skill2Sel = true;
        }
        public void Changedifficulty(int difficulty) // 이지 0, 하드 1
        {
            for (int i = 0; i < difficultyButtons.Length; i++)
            {
                difficultyButtons[i].GetComponent<Image>().color = new Color(difficultyButtons[i].GetComponent<Image>().color.r, difficultyButtons[i].GetComponent<Image>().color.g, difficultyButtons[i].GetComponent<Image>().color.b, 100 / 255f);
            }
            difficultyButtons[difficulty].GetComponent<Image>().color = new Color(difficultyButtons[difficulty].GetComponent<Image>().color.r, difficultyButtons[difficulty].GetComponent<Image>().color.g, difficultyButtons[difficulty].GetComponent<Image>().color.b, 255 / 255f);

            PlayerPrefs.SetInt("difficulty", difficulty);
            difficultySel = true;
        }

        public void CheckIfSkillSelected(GameObject obj)
        {
            if(!Skill1Sel || !Skill2Sel)
            {
                obj.GetComponent<Button>().enabled = false;
            }
            else
            {
                obj.GetComponent<Button>().enabled = true;
            }
        }
    }
}
