namespace Player.Skill
{
	public struct SPlayerSkillData
	{
		private APlayerSkillDataSO playerSkillDataSO;
		private int skillIndex;
		private bool isActive;

		public SPlayerSkillData(int index, APlayerSkillDataSO skillDataSO)
		{
			skillIndex = index;
			playerSkillDataSO = skillDataSO;
			isActive = false;
			return ;
		}

		public SPlayerSkillData(int index, APlayerSkillDataSO skillDataSO, bool active)
		{
			skillIndex = index;
			playerSkillDataSO = skillDataSO;
			isActive = active;
			return ;
		}

		public APlayerSkillDataSO GetData()
		{
			return (playerSkillDataSO);
		}

		public int GetSkillIndex()
		{
			return (skillIndex);
		}

		public bool GetActive()
		{
			return (isActive);
		}

		public bool SetActive(bool active)
		{
			bool result = (isActive != active);

			isActive = active;
			return (result);
		}
	}
}
