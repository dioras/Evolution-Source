﻿using UnityEngine;

public class LeedLP : Creature
{
	public Transform Root,Spine0,Spine1,Spine2,Spine3,Spine4,Spine5,Spine6,Spine7,Tail0,Tail1,Tail2,Tail3,Tail4,Tail5,Tail6,Tail7,Tail8;
  public AudioClip Waterflush,Hit_jaw,Hit_head,Hit_tail,Slip,Bite,Swallow,Largesplash;

	//*************************************************************************************************************************************************
	//Play sound
	void OnCollisionStay(Collision col)
	{
		ManageCollision(col, 0.0f, 0.0f, source, Bite, Hit_jaw, Hit_head, Hit_tail);
	}
	void PlaySound(string name, int time)
	{
		if(time==currframe && lastframe!=currframe)
		{
			switch (name)
			{
			case "Swim": source[1].pitch=Random.Range(0.75f, 1.0f);
				if(IsOnWater && IsOnGround) source[1].PlayOneShot(Largesplash,  0.1f);
				else if(IsOnGround && !IsInWater) source[1].PlayOneShot(Slip, 0.1f);
				else if(IsOnWater) source[1].PlayOneShot(Waterflush,  0.1f);
				lastframe=currframe; break;
			case "Bite": source[0].pitch=Random.Range(0.25f, 0.5f); source[0].PlayOneShot(Bite, 0.5f);
				lastframe=currframe; break;
			case "Food": source[0].pitch=Random.Range(0.25f, 0.5f); source[0].PlayOneShot(Swallow,  0.1f);
				lastframe=currframe; break;
			case "Die":source[0].pitch=Random.Range(0.5f, 0.75f); source[0].PlayOneShot(Swallow, 0.5f);
				lastframe=currframe; IsDead=true; break;
			}
		}
	}

	//*************************************************************************************************************************************************
	// Add forces to the Rigidbody
	void FixedUpdate ()
	{
		StatusUpdate(); if(!IsActive | AnimSpeed==0.0f) { body.Sleep(); return; }
		OnJump=false; OnAttack=false; IsOnLevitation=false; IsConstrained=false; OnReset=false;
		Vector3 dir=-Root.up;

    if(UseAI && Health!=0) { AICore(1, 2, 0, 0, 3, 0, 0); } // CPU
		else if(Health!=0) { GetUserInputs(1, 2, 0, 0, 3, 0, 0); }// Human
		else { anm.SetBool("Attack", false); anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", -1); }//Dead

    //Set Y position
		if(IsInWater)
		{
      body.drag=1; body.angularDrag=1; 
      if(Health!=0)
			{
        anm.SetBool("OnGround", false);
			  pitch = Mathf.Lerp(pitch, anm.GetFloat("Pitch")*90f, Ang_T);
			  if(anm.GetInteger("Move").Equals(-1)) Move(-dir,20);
        else if(anm.GetInteger("Move").Equals(1)) Move(dir,40);
				else if(anm.GetInteger("Move").Equals(10)) Move(Head.right.normalized,20);
				else if(anm.GetInteger("Move").Equals(-10)) Move(-Head.right.normalized,20);
				else if(!anm.GetInteger("Move").Equals(0)) Move(dir, 100);
        else Move(Vector3.zero);
        IsOnLevitation=true;
			}
      if(IsOnWater) ApplyGravity();
		}
		else if(IsOnGround) { body.drag=4; body.angularDrag=4; anm.SetBool("OnGround", true); ApplyYPos(); }
    else
    {
      if(Health!=0) { Move(Vector3.zero); pitch = Mathf.Lerp(pitch, anm.GetFloat("Pitch")*90f, Ang_T); }
      OnJump=true; body.drag=1; body.angularDrag=1; ApplyGravity();
    }

		//Stopped
		if(OnAnm.IsName("Leed|Die") | OnAnm.IsName("Leed|DieOnGround"))
		{
			OnReset=true; if(!IsDead) PlaySound("Die", 2);
		}

		//Forward
		else if(OnAnm.IsName("Leed|Swim"))
		{
			PlaySound("Swim", (int) currframe);
		}

		//Backward/Strafe
		else if(OnAnm.IsName("Leed|Swim-"))
		{
			PlaySound("Swim", (int) currframe);
		}

		//Running
		else if(OnAnm.IsName("Leed|SwimFast") )
		{
			PlaySound("Swim",  (int) currframe);
		}

		//Attack
		else if(OnAnm.IsName("Leed|SwimFastAtk") | OnAnm.IsName("Leed|SwimAtk"))
		{
			if(OnAnm.IsName("Leed|SwimFastAtk"))  { PlaySound("Bite", 8); OnAttack=true; }
			else { PlaySound("Bite", 3);  PlaySound("Bite", 8);  OnAttack=true; }
			PlaySound("Swim",  (int) currframe);
		}

		//On Ground
		else if(OnAnm.IsName("Leed|OnGround"))
		{
			Move(transform.forward, 60);
			PlaySound("Swim", 5); PlaySound("Swim", 10);
		}

		//Various
		else if(OnAnm.IsName("Leed|Eat")) { OnReset=true; IsConstrained=true; PlaySound("Food", 2); }
		else if(OnAnm.IsName("Leed|Die-")) IsDead=false;

    RotateBone(IkType.None, 30f, 20f, false);
	}

  //*************************************************************************************************************************************************
	// Bone rotation
	void LateUpdate()
	{
		if(!IsActive) return; HeadPos=Head.GetChild(0).GetChild(0).position;
		Root.rotation*= Quaternion.Euler(Mathf.Clamp(-pitch, -90, 90), 0, 0);
		Spine0.rotation*= Quaternion.Euler(spineY, 0, spineX);
		Spine1.rotation*= Quaternion.Euler(spineY, 0, spineX);
		Spine2.rotation*= Quaternion.Euler(spineY, 0, spineX);
		Spine3.rotation*= Quaternion.Euler(spineY, 0, spineX);
    Spine4.rotation*= Quaternion.Euler(spineY, 0, spineX);
		Spine5.rotation*= Quaternion.Euler(spineY, 0, spineX);
		Spine6.rotation*= Quaternion.Euler(spineY, 0, spineX);
    Spine7.rotation*= Quaternion.Euler(spineY, 0, spineX);
		Head.rotation*= Quaternion.Euler(spineY, 0, spineX);
		Tail0.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail1.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail2.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail3.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail4.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail5.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail6.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail7.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		Tail8.rotation*= Quaternion.Euler(-spineY, 0, -spineX);
		if(!IsDead) Head.GetChild(0).transform.rotation*=Quaternion.Euler(-lastHit, 0, 0);
		//Check for ground layer
		GetGroundPos(IkType.None);
	}
}



