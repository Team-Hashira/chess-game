using System;

[Flags]
public enum ECurse
{
	Destruction			= 1 << 1,
	Pride				= 1 << 2,
	Envy				= 1 << 3,
	Gluttony			= 1 << 4,
	Belonging			= 1 << 5,
	Regret				= 1 << 6,
}

[Flags]
public enum EBlessing
{
	Charity				= 1 << 1,
	Resection			= 1 << 2,
	Love				= 1 << 3,
	Penance				= 1 << 4,
}