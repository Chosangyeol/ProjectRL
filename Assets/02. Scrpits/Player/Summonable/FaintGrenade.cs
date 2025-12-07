using Info;
using Player.Component;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class FaintGrenade : PlayerSummonableMono
	{
		private Rigidbody	rigid;
		private float		time = 0f;
		private float		activeTime = 3.0f;

		public void Awake()
		{
			rigid = GetComponent<Rigidbody>();
			return ;
		}

		private void Update()
		{
			time += Time.deltaTime;

			if (time >= activeTime)
			{
				Boom();
				player.Pool.Push(this);
			}
			return ;
		}

		public override void Reset()
		{
			time = 0f;
			return ;
		}

		// TODO!
		public void Boom()
		{
			Collider[] hits = Physics.OverlapSphere(transform.position, 5f);
			SInfoAttack info;

			for (int i = 0; i < hits.Length; i++)
			{

				if (!hits[i].TryGetComponent<EnemyBase>(out var enemy))
				{
					continue ;
				}
				info = new SInfoAttack(player.gameObject, hits[i].gameObject, 80);
				enemy.TakeDamage(info.damage);
				player.AfterAttackEnemy(info);
				Debug.Log($"{gameObject.name} - boom -> {enemy.name}");
			}
			return ;
		}
	}
}
