using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ClientAnimation : NetworkBehaviour{

	Dictionary<SkillAnimation, float> anims;

	// Use this for initialization
	void Start(){
		anims = new Dictionary<SkillAnimation, float>();
	}
	
	// Update is called once per frame
	void Update(){
		//if(isLocalPlayer){
		HashSet<SkillAnimation> toBeRemoved = new HashSet<SkillAnimation>();
		foreach(KeyValuePair<SkillAnimation, float> anim in anims){
			anim.Key.Update(anim.Key.GetLength() - (anim.Value - Time.time));
			if(Time.time > anim.Value){
				anim.Key.Destroy();
				toBeRemoved.Add(anim.Key);
			}
		}
		foreach(SkillAnimation anim in toBeRemoved){
			anims.Remove(anim);
		}
		//}
	}

	public void RunAnimation(SkillAnimation animation){
		anims.Add(animation, Time.time + animation.GetLength());
		animation.Animate();
	}

	[ClientRpc]
	public void RpcAnimateSkill(int id, int x, int y, int direction, float length){
		Skill s = Skills.GetDefaultFromSkillInfo(Skills.GetSkillInfoFromID(id));
		RunAnimation(s.GetAnimation(x, y, direction, length));
	}
}
