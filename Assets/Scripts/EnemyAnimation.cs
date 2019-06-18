using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the animation for the enemies
/// </summary>
public class EnemyAnimation : MonoBehaviour
{
	/// <summary>
	/// The Animator component of the player
	/// </summary>
	Animator animator;

	/// <summary>
	/// The Unity Start method
	/// </summary>
	private void Start()
	{
		animator = gameObject.GetComponent<Animator>();
	}

	/// <summary>
	///	Changes animation parameters to play the goal reached animation
	/// </summary>
	public void ReachedGoal()
	{
		animator.SetBool("reachedGoal", true);
	}
}
