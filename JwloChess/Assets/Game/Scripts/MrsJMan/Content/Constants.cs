namespace MrsJMan
{
	public class Constants : Singleton<Constants>
	{
		public float GhostSpeed = 0.5f,
					 MrsJManSpeed = 0.5f;
		public float GhostHitRadius = 0.2f;

		public float HatUseTime = 10.0f,
					 HatNearEndTime = 3.0f;

		public float ChocolateSpawnInterval = 20.0f,
					 ChocolateLife = 8.5f,
					 ChocolateBlinkTimeLeft = 3.0f;
		public float GhostFreezeTime = 5.0f;
	}
}