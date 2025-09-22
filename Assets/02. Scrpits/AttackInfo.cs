using System.Collections.Generic;
using UnityEngine;

/*
 * TODO!
 * 
 * 나중에 몬스터랑 플레이어 이챠이챠해서
 * AttackInfo로 정보를 주고받게 하는게 목표
 * Interface 분리.. 해야겠지?
 */
public class AttackInfo
{
	public GameObject attacker;
	public GameObject target;
	public int damage;
	public ElementType type;
}
