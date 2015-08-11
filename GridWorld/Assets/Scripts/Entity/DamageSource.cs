using UnityEngine;
using System.Collections;

public interface DamageSource{
	void DamageDealt(EntityCombat combat, float amt);
	void EntityDestroyed(EntityCombat combat);
}
