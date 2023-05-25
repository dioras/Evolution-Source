using UnityEngine;

public class MegaLP : Creature
{
	public Transform Root,Spine0,Spine1, Tail0,Tail1,Tail2,Tail3,Tail4,Tail5, Right_Front_Hip, Left_Front_Hip, Right_Mid_Hip, Left_Mid_Hip, Right_Back_Hip, Left_Back_Hip;
  public AudioClip Waterflush,Hit_jaw,Hit_head,Hit_tail,Smallstep,Smallsplash,Bite, Ammo2, Ammo3;

	//*************************************************************************************************************************************************
	//Play sound
	void OnCollisionStay(Collision col)
	{
		int rndPainsnd=Random.Range(0, 2); AudioClip painSnd=null;
		switch (rndPainsnd) { case 0: painSnd=Ammo2; break; case 1: painSnd=Ammo3; break; }
		ManageCollision(col, Pitch_Max, Crouch_Max, source, painSnd, Hit_jaw, Hit_head, Hit_tail);
	}
	void PlaySound(string name, int time)
	{
		if(time==currframe && lastframe!=currframe)
		{
			switch (name)
			{
			case "Step": source[1].pitch=Random.Range(1.25f, 1.5f); 
				if(IsInWater) source[1].PlayOneShot(Waterflush, Random.Range(0.1f, 0.25f));
				else if(IsOnWater) source[1].PlayOneShot(Smallsplash, Random.Range(0.1f, 0.25f));
				else if(IsOnGround) source[1].PlayOneShot(Smallstep, Random.Range(0.1f, 0.25f));
				lastframe=currframe; break;
			case "Bite": source[1].pitch=Random.Range(1.25f, 1.5f); source[1].PlayOneShot(Bite, 1.0f);
				lastframe=currframe; break;
			case "Fly": source[1].pitch=Random.Range(0.4f, 0.6f);
				if(IsInWater) source[1].PlayOneShot(Smallsplash, Random.Range(0.1f, 0.25f));
				else source[1].PlayOneShot(Bite, Random.Range(0.1f, 0.25f));
				lastframe=currframe; break;
			case "Die": source[1].pitch=Random.Range(0.5f, 0.6f); source[1].PlayOneShot(IsOnWater|IsInWater?Smallsplash:Smallstep, 1.0f);
				lastframe=currframe; IsDead=true; break;
			case "Atk":source[0].pitch=Random.Range(1.0f, 1.75f);
				source[0].PlayOneShot(Ammo2, 0.25f);
				lastframe=currframe; break;
			case "Growl": source[0].pitch=Random.Range(1.0f, 1.75f);
				source[0].PlayOneShot(Ammo3, 0.25f);
				lastframe=currframe; break;
			}
		}
	}

	//*************************************************************************************************************************************************
	// Add forces to the Rigidbody
	void FixedUpdate ()
	{
		StatusUpdate(); if(!IsActive | AnimSpeed==0.0f) { body.Sleep(); return; }
    Vector3 dir=-Root.right; anm.SetBool("OnGround", IsOnGround);
		OnReset=false; OnAttack=false; IsOnLevitation=false; IsConstrained= false; OnJump=false;

		if(UseAI && Health!=0) { AICore(1, 2, 3, 0, 4, 5, 6); }// CPU
		else if(Health!=0) { GetUserInputs(1, 2, 3, 0, 4, 5, 6); }// Human
		else { anm.SetBool("Attack", false); anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", -1); }//Dead

    //Set Y position
    if(IsInWater) { body.drag=4; body.angularDrag=4; ApplyYPos(); anm.SetInteger ("Move", 1); }
    else if(IsOnGround)
    {
      roll = Mathf.Lerp(roll, 0.0f, 0.1f); pitch = Mathf.Lerp(pitch, 0.0f, 0.1f);
      body.drag=4; body.angularDrag=4; ApplyYPos();
    }
    else if(Health==0) { body.drag=4; body.angularDrag=4; ApplyGravity(0.4f); }
    else { body.drag=1; body.angularDrag=1; } //in air drag

		//Stopped
		if(OnAnm.IsName("Mega|IdleA") | OnAnm.IsName("Mega|Die1") | OnAnm.IsName("Mega|Die2") | OnAnm.IsName("Mega|Fall"))
		{
      Move(Vector3.zero);
			if(OnAnm.IsName("Mega|Die1")) { OnReset=true; if(!IsDead) { PlaySound("Growl", 1); PlaySound("Die", 11); } }
			else if(OnAnm.IsName("Mega|Die2"))
			{ OnReset=true; if(!IsDead) PlaySound("Die", 0); }
			else if(OnAnm.IsName("Mega|Fall"))
			{
				OnReset=true; IsOnLevitation=true;
				if(IsInWater) anm.SetBool("OnGround", true);
				if(OnAnm.normalizedTime<0.05f) source[0].PlayOneShot(Ammo3, 0.5f);
			} 
		}
		
		//Forward
		else if(OnAnm.IsName("Mega|Walk"))
		{
			Move(transform.forward, 5);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

		//Running
		else if(OnAnm.IsName("Mega|Run"))
		{
			IsOnLevitation=true; Move(transform.forward, 10);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}
		
		//Backward
		else if(OnAnm.IsName("Mega|Walk-"))
		{
			Move(-transform.forward, 5);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}
		
		//Strafe/Turn right
		else if(OnAnm.IsName("Mega|Strafe+"))
		{
			Move(transform.right, 5);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}
		
		//Strafe/Turn left
		else if(OnAnm.IsName("Mega|Strafe-"))
		{
			Move(-transform.right, 5);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

		//Takeoff
		else if(OnAnm.IsName("Mega|Takeoff"))
		{
			IsOnLevitation=true; OnJump=true;
			if(OnAnm.normalizedTime > 0.6) Move(Vector3.up, 50);
			PlaySound("Fly", 3); PlaySound("Fly", 6); PlaySound("Fly", 9);PlaySound("Fly", 12); PlaySound("Fly", 15);
		}

		//Fly
		else if(OnAnm.IsName("Mega|Flight") | OnAnm.IsName("Mega|FlightForw") | OnAnm.IsName("Mega|FlightBack") |
		        OnAnm.IsName("Mega|FlyAtk") | OnAnm.IsName("Mega|FlyIdleA") | OnAnm.IsName("Mega|FlyIdleB"))
		{
			IsOnLevitation=true;

			if(IsOnGround&&OnAnm.IsName("Mega|FlyAtk")) Move(Vector3.up, 50);//takeoff

      Move(Vector3.up, 50*-anm.GetFloat("Pitch")); //fly up/down

      if(anm.GetInteger("Move")==0) //Stationnary
      { roll = Mathf.Lerp(roll, 0.0f, Ang_T); pitch = Mathf.Lerp(pitch, 0.0f, Ang_T); }
      else if(anm.GetInteger("Move")>0 && anm.GetInteger("Move")<4) //fly forward
      {
        if(anm.GetInteger("Move")==1) Move(-Root.up, 50); else Move(-Root.up, 100);
        pitch = Mathf.Lerp(pitch, Mathf.Clamp(anm.GetFloat("Pitch"),-0.75f, 1.0f)*90f, Ang_T); roll = Mathf.Lerp(roll, -spineX*16.0f, Ang_T);
      }
			else if(anm.GetInteger("Move")== -1) //fly backward
      { 
        Move(-transform.forward, 75);
        pitch = Mathf.Lerp(pitch, 0.0f, Ang_T); roll = Mathf.Lerp(roll, 0.0f, Ang_T);
      }
			else if(anm.GetInteger("Move")== -10) //fly right
      {
        Move(transform.right, 75);
        pitch = Mathf.Lerp(pitch, 0.0f, Ang_T); roll = Mathf.Lerp(roll, 45, Ang_T); 
      }
			else if(anm.GetInteger("Move")== 10)//fly left
      {
        Move(-transform.right, 75);
        pitch = Mathf.Lerp(pitch, 0.0f, Ang_T); roll = Mathf.Lerp(roll, -45, Ang_T);
      }

			if(OnAnm.IsName("Mega|FlyAtk")) { OnAttack=true; PlaySound("Atk", 2); PlaySound("Bite", 5);  PlaySound("Bite", 10); }

			if(OnAnm.IsName("Mega|FlyIdleA") | OnAnm.IsName("Mega|FlyIdleB")) 
      { PlaySound("Fly", 2); PlaySound("Fly", 4);PlaySound("Fly", 6);PlaySound("Fly", 8); PlaySound("Fly", 10);}
			else { PlaySound("Fly", 4); PlaySound("Fly", 7);PlaySound("Fly", 10);PlaySound("Fly", 13); PlaySound("Fly", 1);}
		}

		//Various
		else if(OnAnm.IsName("Mega|Landing")) { IsOnLevitation=true; PlaySound("Step", 9); PlaySound("Step", 10); }
		else if(OnAnm.IsName("Mega|IdleB")) { OnReset=true; IsConstrained=true; }
		else if(OnAnm.IsName("Mega|IdleC")) { OnReset=true; IsConstrained=true; }
		else if(OnAnm.IsName("Mega|Eat")) { OnReset=true; IsConstrained=true; }
		else if(OnAnm.IsName("Mega|Drink")) { OnReset=true; IsConstrained=true; }
		else if(OnAnm.IsName("Mega|Sleep")) { OnReset=true; IsConstrained=true; }
		else if(OnAnm.IsName("Mega|Die-")) { IsConstrained=true; PlaySound("Atk", 10);  IsDead=false; }

    RotateBone(IkType.None, 30f, 20f, false);
	}

  //*************************************************************************************************************************************************
	// Bone rotation
	void LateUpdate()
	{
		if(!IsActive) return; HeadPos=Head.GetChild(0).position;

		Root.rotation*= Quaternion.Euler(-pitch, roll, 0);

		Right_Front_Hip.rotation*= Quaternion.Euler(roll, 0, 0);
		Left_Front_Hip.rotation*= Quaternion.Euler(0, roll, 0);
		Right_Mid_Hip.rotation*= Quaternion.Euler(roll, 0, 0);
		Left_Mid_Hip.rotation*= Quaternion.Euler(0, roll, 0);
		Right_Back_Hip.rotation*= Quaternion.Euler(0, roll, 0);
		Left_Back_Hip.rotation*=Quaternion.Euler(roll, 0, 0);

		Spine0.rotation*= Quaternion.Euler(spineY, 0, spineX);
		Spine1.rotation*= Quaternion.Euler(spineY, 0, spineX);
		Head.rotation*= Quaternion.Euler(spineY, 0, spineX);

		Tail0.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail1.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail2.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail3.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail4.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail5.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
    if(!IsDead) Head.GetChild(0).transform.rotation*=Quaternion.Euler(0, lastHit, 0);
		//Check for ground layer
		GetGroundPos(IkType.None);
    anm.SetBool("OnGround", IsOnGround);
	}
}










