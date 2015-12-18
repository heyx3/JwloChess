using System;
using System.Collections.Generic;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;


namespace Menu
{
	public class PathFollower : MonoBehaviour
	{
		public Transform PathParent;
		public float Speed;
		public bool Loop;


		public Transform MyTr { get; private set; }
		public List<Transform> Targets { get; private set; }
		public int CurrentTarget { get; private set; }


		void Awake()
		{
			Assert.IsNotNull(PathParent, "PathParent field");

			Targets = new List<Transform>();
			int i = 0;
			while (true)
			{
				i += 1;

				Transform childO = PathParent.Find("Point " + i.ToString());
				if (childO == null)
				{
					break;
				}
				else
				{
					Targets.Add(childO);
				}
			}

			Assert.IsFalse(Targets.Count == 0, "Number of targets is 0");
		}
		void Start()
		{
			MyTr = transform;

			MyTr.position = Targets[0].position;
			CurrentTarget = 1;
		}
		void Update()
		{
			//Don't move if there are no more target positions.
			if (CurrentTarget < Targets.Count)
			{
				Vector3 myPos = MyTr.position,
						targetPos = Targets[CurrentTarget].position;

				float distToNext = Vector3.Distance(myPos, targetPos);
				float deltaP = Speed * Time.deltaTime;

				//If we're about to reach the next waypoint, snap to it.
				if (deltaP > distToNext)
				{
					myPos = targetPos;

					CurrentTarget += 1;
					if (Loop)
					{
						CurrentTarget %= Targets.Count;
					}

					//If there is still some distance left to move this frame,
					//    start moving towards the next waypoint.
					if (CurrentTarget < Targets.Count)
					{
						deltaP -= distToNext;
						targetPos =

						myPos += deltaP * (Targets[CurrentTarget].position - myPos).normalized;
					}

					MyTr.position = myPos;
				}
				else
				{
					MyTr.position += (targetPos - myPos).normalized * deltaP;
				}
			}
		}
	}
}