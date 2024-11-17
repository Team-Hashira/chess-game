public abstract class Effect
{
	protected CardManager _cardManager;
	protected EffectManager _effectManager;

	public virtual void ApplyEffect(Card owner)
	{
		_cardManager ??= CardManager.Instance;
		_effectManager ??= EffectManager.Instance;
	}

	public abstract void OnCardUse(Card owner);
}