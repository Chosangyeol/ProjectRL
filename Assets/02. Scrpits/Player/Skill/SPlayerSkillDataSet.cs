using System.Collections.Generic;

namespace Player.Skill
{
	public struct SPlayerSkillDataSet
	{
		private SPlayerSkillData[] datas;
		private int targetIndex;

		public SPlayerSkillDataSet(int index, List<SPlayerSkillData> datas)
			: this(index, datas.ToArray())
		{
		}

		public SPlayerSkillDataSet(int index, SPlayerSkillData[] datas)
		{
			this.targetIndex = index;
			this.datas = datas;
			return ;
		}

		public SPlayerSkillDataSet(int index, SPlayerSkillData data)
		{
			this.targetIndex = index;
			this.datas = new SPlayerSkillData[1];
			this.datas[0] = data;
			return ;
		}

		public SPlayerSkillData[] GetDatas()
		{
			return (datas);
		}

		public ref SPlayerSkillData GetRefData(int index)
		{
			return ref (datas[index]);
		}

		public int GetTargetIndex()
		{
			return (targetIndex);
		}
	}
}
