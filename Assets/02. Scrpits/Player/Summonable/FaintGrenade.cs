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
		private bool		isShotable = false;

		public void Awake()
		{
			rigid = GetComponent<Rigidbody>();
			return ;
		}

		private void Update()
		{
			if (isShotable)
			{
				rigid.AddForce(transform.forward * 10f, ForceMode.Impulse);
				isShotable = false;
			}
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
			isShotable = true;
			return ;
		}

		// TODO!
		public void Boom()
		{
			Collider[] hits = new Collider[32];
			int idx = Physics.OverlapSphereNonAlloc(transform.position, 5f, hits, LayerMask.GetMask("Enemy"));
			SInfoAttack info;

			for (int i = 0; i < idx; i++)
			{
				if (!hits[i].gameObject.TryGetComponent<EnemyBase>(out var enemy))
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
