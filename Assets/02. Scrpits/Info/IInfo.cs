namespace Info
{
	public interface IInfo<GType1, GType2>
	{
		public GType1 Source { get; }
		public GType2 Target { get; }
	}
}
