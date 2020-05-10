﻿using System;

using ClockBlockers.AI.States;
using ClockBlockers.ReplaySystem.ReplayRunner;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.AI.AiControllers
{
	[BurstCompile]
	public abstract class AiController : MonoBehaviour
	{
		
		// TODO: Look into the 'Command Pattern'
		public AiState currentState;

		[NonSerialized]
		public ReplayRunner replayRunner;

		[NonSerialized]
		public AiPathfinder aiPathfinder;

		private void Awake()
		{
			replayRunner = GetComponent<ReplayRunner>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref replayRunner, this, "Replay Runner");

			aiPathfinder = GetComponent<AiPathfinder>();
			Logging.CheckIfCorrectMonoBehaviourInstantiation(ref aiPathfinder, this, "Ai Pathfinder");
		}

		private void Start()
		{
			Begin();
		}

		private void Update()
		{
			currentState.Update();
		}

		public void SetState(AiState newState)
		{
			Logging.Log(name + " is changing state to: " + newState.GetType().Name);

			currentState?.End();
			
			aiPathfinder.StopAllCoroutines();

			currentState = newState;


			StartCoroutine(currentState.Begin());
		}

		public abstract void Begin();

		public abstract void End();
	}
}