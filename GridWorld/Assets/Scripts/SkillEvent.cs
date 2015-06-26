using UnityEngine;
using System.Collections;

public interface SkillEvent{

	bool Initialize();
	bool Update ();
	void CleanUp ();
	float GetCoolDown();
}
