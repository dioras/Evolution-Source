using UnityEngine;

public class AmmoLP : Creature
{
	public Transform Root,Body,Tentacles,Right0,Right1,Right2,Right3,Right4,Right5,Right6,Right7,Right8,
	Left0,Left1,Left2,Left3,Left4,Left5,Left6,Left7,Left8;
  public AudioClip Waterflush,Hit_jaw,Hit_head,Hit_tail,Smallstep,Smallsplash,Ammo1,Ammo2,Ammo3;
	//*************************************************************************************************************************************************
	//Play sound
	void OnCollisionStay(Collision col)
	{
		int rndPainsnd=Random.Range(0, 3); AudioClip painSnd=null;
		switch (rndPainsnd) { case 0: painSnd=Ammo1; break; case 1: painSnd=Ammo2; break; case 2: painSnd=Ammo3; break; }
		ManageCollision(col, 0.0f, 0.0f, source, painSnd, Hit_jaw, Hit_head, Hit_tail);
	}
	void PlaySound(string name, int time)
	{
		if(time==currframe && lastframe!=currframe)
		{
			switch (name)
			{
			case "Swim": source[1].pitch=Random.Range(0.5f, 0.75f); 
				if(IsOnWater && IsOnGround) source[1].PlayOneShot(Smallsplash, 0.1f);
				else if(IsOnGround && !IsInWater) source[1].PlayOneShot(Smallstep, 0.1f);
				else if(IsOnWater) source[1].PlayOneShot(Waterflush, 0.1f);
				lastframe=currframe; break;
			case "Atk":int rnd = Random.Range(0, 2); source[0].pitch=Random.Range(0.9f, 1.1f);
				if(rnd==0) source[0].PlayOneShot(Ammo1, 0.1f);
				else source[0].PlayOneShot(Ammo2, 0.1f);
				lastframe=currframe; break;
			case "Die": source[0].pitch=Random.Range(0.8f, 1.0f); source[0].PlayOneShot(Ammo3, 0.1f);
				lastframe=currframe; IsDead=true; break;
			}
		}
	}

  //*************************************************************************************************************************************************
  // Add forces to the Rigidbody
  void FixedUpdate ()
	{
		StatusUpdate(); if(!IsActive | AnimSpeed==0.0f) { body.Sleep(); return; }
		Vector3 dir=-Root.up.normalized; OnJump=false; OnAttack=false; IsOnLevitation=false; IsConstrained=false; OnReset=false;

    if(UseAI&&Health!=0)// CPU
    {
      AICore(1, 2, 3, 0, 4, 0, 5); if(behavior.EndsWith("Hunt")|behavior.EndsWith("Food")|behavior.Equals("Battle")) OnInvert=true; else OnInvert=false;
    } else if(Health!=0) { GetUserInputs(1, 2, 3, 0, 4, 0, 5); }// Human
    else { anm.SetBool("Attack", false); anm.SetInteger("Move", 0); anm.SetInteger("Idle", -1); } //Dead

    //Set Y position
    if(IsInWater)
		{
      body.drag=1; body.angularDrag=1;
      if(Health!=0&&!OnAnm.IsName("Ammo|ToHide")&&!OnAnm.IsName("Ammo|ToHide-"))
			{
        transform.rotation=Quaternion.Lerp(transform.rotation, normAng, Ang_T);
			  pitch = Mathf.Lerp(pitch, anm.GetFloat("Pitch")*(OnInvert?-90f:90f), Ang_T);
			  if(anm.GetInteger("Move").Equals(-1)) Move(OnInvert?dir:-dir, 25);
        else if(anm.GetInteger("Move").Equals(1)) Move(OnInvert?-dir:dir, 25);
				else if(anm.GetInteger("Move").Equals(10)) Move(OnInvert?-Head.right.normalized:Head.right.normalized, 25);
				else if(anm.GetInteger("Move").Equals(-10)) Move(OnInvert?Head.right.normalized:-Head.right.normalized, 25);
				else if(!anm.GetInteger("Move").Equals(0)) Move(OnInvert?-dir:dir, 50);
        else Move(Vector3.zero);
        IsOnLevitation=true;
			}
      anm.SetBool("OnGround", false);
			if(IsOnWater) ApplyGravity();
		}
		else if(IsOnGround) { body.drag=4; body.angularDrag=4; anm.SetBool("OnGround", true); ApplyYPos(); }
    else
    {
      if(Health!=0) { Move(Vector3.zero); pitch = Mathf.Lerp(pitch, anm.GetFloat("Pitch")*90f, Ang_T); }
      anm.SetBool("OnGround", false); OnJump=true; body.drag=0.5f; body.angularDrag=0.5f; ApplyGravity();
    }


		//Stopped
		if(OnAnm.IsName("Ammo|Die") | OnAnm.IsName("Ammo|DieGround"))
		{
			OnReset=true;
      if(!IsDead) PlaySound("Die", 2);
		}

		//Forward
		else if(OnAnm.IsName("Ammo|Swim"))
		{
			PlaySound("Swim", 5);
		}

		//Running
		else if(OnAnm.IsName("Ammo|SwimFast"))
		{
			PlaySound("Swim", 5); PlaySound("Swim", 10);
		}
		
		//Backward/Strafe
		else if(OnAnm.IsName("Ammo|Swim-"))
		{
			PlaySound("Swim", 5);
		}

		//Attack
		else if(OnAnm.IsName("Ammo|Atk"))
		{
			OnAttack=true;
			PlaySound("Atk", 5); PlaySound("Swim", 10);
		}

		//Impulse
		else if(OnAnm.IsName("Ammo|IdleC"))
		{
      if(IsInWater&&OnAnm.normalizedTime<0.4) { PlaySound("Flush", 2); Move(OnInvert?-dir:dir, 60); }
			PlaySound("Atk", 5); PlaySound("Swim", 10);
		}

		//On Ground
		else if(OnAnm.IsName("Ammo|OnGround"))
		{
			OnReset=true; Move(transform.forward, 40);
			PlaySound("Swim", 5); PlaySound("Swim", 10);
		}
		else if(OnAnm.IsName("Ammo|Eat")) { PlaySound("Atk", 1); }
		else if(OnAnm.IsName("Ammo|ToHide") | OnAnm.IsName("Ammo|ToHide-") ) OnReset=true;
		else if(OnAnm.IsName("Ammo|Die-")) { PlaySound("Atk", 1);  IsDead=false; }

    RotateBone(IkType.None, 60f, 30f, false); if(OnInvert) spineY*=-1;
	}

  //*************************************************************************************************************************************************
	// Bone rotation
	void LateUpdate()
	{
    if(!IsActive) return; HeadPos=Head.GetChild(0).GetChild(0).position;
    Root.RotateAround(transform.position, Vector3.up, reverse = Mathf.Lerp(reverse, OnInvert?180f : 0.0f, Ang_T) );
		Root.rotation*= Quaternion.Euler(Mathf.Clamp(-pitch, -90, 90), roll*2f, 0);
		Body.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tentacles.rotation*= Quaternion.Euler(spineY*4.0f, 0, -spineX*2.0f);
		Right0.rotation*= Quaternion.Euler(0, -spineY, -spineX);
		Right1.rotation*= Quaternion.Euler(0, -spineY, -spineX);
		Right2.rotation*= Quaternion.Euler(0, -spineY, -spineX);
		Right3.rotation*= Quaternion.Euler(0, -spineY, -spineX);
		Right4.rotation*= Quaternion.Euler(0, -spineY, -spineX);
		Right5.rotation*= Quaternion.Euler(0, -spineY, -spineX);
		Right6.rotation*= Quaternion.Euler(0, -spineY, -spineX);
		Right7.rotation*= Quaternion.Euler(0, -spineY, -spineX);
		Right8.rotation*= Quaternion.Euler(0, -spineY, -spineX);
		Left0.rotation*= Quaternion.Euler(spineY, 0, -spineX);
		Left1.rotation*= Quaternion.Euler(spineY, 0, -spineX);
		Left2.rotation*= Quaternion.Euler(spineY, 0, -spineX);
		Left3.rotation*= Quaternion.Euler(spineY, 0, -spineX);
		Left4.rotation*= Quaternion.Euler(spineY, 0, -spineX);
		Left5.rotation*= Quaternion.Euler(spineY, 0, -spineX);
		Left6.rotation*= Quaternion.Euler(spineY, 0, -spineX);
		Left7.rotation*= Quaternion.Euler(spineY, 0, -spineX);
		Left8.rotation*= Quaternion.Euler(spineY, 0, -spineX);

		//Check for ground layer
		GetGroundPos(IkType.None);
	}
}










