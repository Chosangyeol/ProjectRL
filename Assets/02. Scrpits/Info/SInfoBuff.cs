using Player.Buff;
using UnityEngine;

// TODO!
namespace Info
{
	public struct SInfoBuff : IInfo
	{
		public GameObject Source { get; private set; }
		public GameObject Target { get; private set; }
		public IBuffAct act;

		public SInfoBuff(GameObject source, GameObject target, IBuffAct act)
		{
			Source = source;
			Target = target;
			this.act = act;
			return ;
		}
	}
}