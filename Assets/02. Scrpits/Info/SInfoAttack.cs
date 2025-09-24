using System.Collections.Generic;
using UnityEngine;

/*
 * TODO!
 * 
 * 나중에 몬스터랑 플레이어 이챠이챠해서
 * AttackInfo로 정보를 주고받게 하는게 목표
 * Interface 분리.. 해야겠지?
 */
namespace Info
{
	public struct SInfoAttack : IInfo
	{
		public GameObject Source { get; private set; }
		public GameObject Target { get; private set; }
		public ElementType type;
		public int damage;

		public SInfoAttack(GameObject source, GameObject target, int damage = 0, ElementType type = null)
		{
			Source = source;
			Target = target;
			this.damage = damage;
			this.type = type;
			return ;
		}
	}
}
