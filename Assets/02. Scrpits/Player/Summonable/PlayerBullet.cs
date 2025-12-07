using Info;
using Player.Component;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerBullet : PoolableMono
	{
		private PlayerModel	player;
		private Rigidbody	rigid;
		private float		time = 0f;
		private float		destroyTime = 10.0f;
		private SInfoAttack info;

		public void Awake()
		{
			rigid = GetComponent<Rigidbody>();
			return ;
		}

		private void Update()
		{
			time += Time.deltaTime;

			if (time >= destroyTime)
			{
				player.Pool.Push(this);
			}
			return ;
		}

		public override void Reset()
		{
			time = 0f;
			return ;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Enemy"))
			{
				DealDamageToEnemy(other.GetComponent<EnemyBase>());
				player.Pool.Push(this);
			}
			return ;
		}

		public virtual void DealDamageToEnemy(EnemyBase enemy)
		{
			SPlayerStat stat = player.Stat.Stat;

			if (enemy == null || !enemy.gameObject.activeInHierarchy)
				return ;
			info.SetTarget(enemy.gameObject);
			enemy.TakeDamage(info.damage);
			player.AfterAttackEnemy(info);
			Debug.Log($"Player attack {enemy.gameObject.name}, Damage {info.damage}");
			return ;
		}

		public void SetPlayer(PlayerModel player, float destroyTime = 10.0f)
		{
			this.player = player;
			this.destroyTime = destroyTime;
			return ;
		}

		public void SetInfo(SInfoAttack info)
		{
			this.info = info;
			return ;
		}

		public void SetSpeed(float speed)
		{
			rigid.velocity = transform.forward * speed;
			return ;
		}
	}
}
