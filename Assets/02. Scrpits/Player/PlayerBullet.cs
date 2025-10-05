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
				EnemyBase enemy = other.GetComponent<EnemyBase>();
				SPlayerStat stat = player.Stat.Stat;
				SInfoAttack info;

				if (enemy == null || !enemy.gameObject.activeInHierarchy)
					return ;
				if (Random.Range(0f, 1f) < stat.critPercent)
					stat.attackDamage = (int)(stat.attackDamage * stat.critDamagePercent);
				info = new SInfoAttack(player.gameObject, enemy.gameObject, stat.attackDamage);
				enemy.TakeDamage(info.damage);
				Debug.Log($"Player attack {enemy.gameObject.name}, Damage {info.damage}");
				player.Pool.Push(this);
			}
			return ;
		}

		public void SetInfo(PlayerModel player)
		{
			this.player = player;
			return ;
		}

		public void SetSpeed(float speed)
		{
			rigid.velocity = transform.forward * speed;
			return ;
		}
	}
}
