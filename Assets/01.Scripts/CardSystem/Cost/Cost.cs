public class Cost
{
	private static int _cost = 10;
	public static int Get()
	{
		return _cost;
	}
	public static void Set(int cost)
	{
		_cost = cost;
	}
	public static bool TryUse(int amount)
	{
		if (_cost < amount) return false;

		_cost -= amount;
		return true;
	}
}