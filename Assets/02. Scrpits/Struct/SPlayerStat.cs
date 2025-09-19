namespace Player
{
	public struct SPlayerStat : IObjectStat
	{
		public int HealthPoint { get; }
		public int CurrentHP { get; }
		public float SpeedWalk { get; }
		public float SpeedRush { get; }
		public int Damage { get; }
	}
}
