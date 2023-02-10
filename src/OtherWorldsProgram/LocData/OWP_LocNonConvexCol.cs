using System;
using System.Collections.Generic;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWP_LocNonConvexCol : OWP_LocData {
			
			private string[] childsToModify;
			private bool modifyEveryChild;

			public OWP_LocNonConvexCol(string bodyName, string pqsName, string[] childsToModify) : base(bodyName, pqsName, 10.0f) {
				this.childsToModify = childsToModify;
				this.modifyEveryChild = false;
				this.gameObject = GetGameObject();
				this.OnLoad();
			}

			public OWP_LocNonConvexCol(string bodyName, string pqsName) : base(bodyName, pqsName, 10.0f) {
				this.childsToModify = null;
				this.modifyEveryChild = true;
				this.gameObject = GetGameObject();
				this.OnLoad();
			}

			public override void OnLoad() {
				if(gameObject != null) {
					if(modifyEveryChild) {
						ModifyAllChildren(gameObject.transform);
					} else {
						foreach(string childName in childsToModify) {
							Transform child = gameObject.transform.Find(childName);
							ModifyMeshColliders(child);
						}
					}
				}
			}

			private void ModifyMeshColliders(Transform obj) {
				MeshCollider[] mcs = obj.GetComponents<MeshCollider>();
				foreach(MeshCollider mc in mcs) {
					mc.convex = false;
				}
			}

			private void ModifyAllChildren(Transform parent) {
				ModifyMeshColliders(parent);
				foreach(Transform child in parent.transform) {
					ModifyMeshColliders(child);
					ModifyAllChildren(child);
				}
			}
		}
	}
}