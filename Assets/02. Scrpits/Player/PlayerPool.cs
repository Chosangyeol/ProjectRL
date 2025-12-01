using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerPool
	{
		private Dictionary<string, Pool<PoolableMono>> _pools = new Dictionary<string, Pool<PoolableMono>>();

		private readonly Transform _parentTr;

		public PlayerPool(Transform playerTr)
		{
			_parentTr = playerTr;
			return ;
		}

		public void CreatePool(PoolableMono prefab, int count = 10)
		{
			Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, _parentTr, count);
			_pools.Add(prefab.gameObject.name, pool);
			return ;
		}

		public PoolableMono Pop(string prefabName)
		{
			if (!_pools.ContainsKey(prefabName))
			{
				Debug.LogError($"Prefab does no exist on pool : {prefabName}");
				return null;
			}

			PoolableMono item = _pools[prefabName].Pop();
			Debug.Log(item.name + " 꺼냄");
			item.Reset();
			return (item);
		}

		public void Push(PoolableMono obj)
		{
			_pools[obj.name].Push(obj);
			return ;
		}
	}
}