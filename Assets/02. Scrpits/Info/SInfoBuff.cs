using Player.Buff;
using UnityEngine;

// TODO!
namespace Info
{
	public struct SInfoBuff : IInfo
	{
		public GameObject Source { get; private set; }
		public GameObject Target { get; private set; }
		public BuffType type;
		public IBuffAct act;

		public SInfoBuff(GameObject source, GameObject target, BuffType type, IBuffAct act)
		{
			Source = source;
			Target = target;
			this.type = type;
			this.act = act;
			return ;
		}
	}
}