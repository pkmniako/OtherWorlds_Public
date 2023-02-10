using System;
using System.Collections.Generic;

using UnityEngine;

namespace NiakoKerbalMods 
{
	namespace OtherWorldsProgram
	{
		public class OWPArtifactModule : PartModule {

			[KSPField]
			public int type;

			[KSPEvent(guiActive = true, active = true, guiName = "Pick Up Alien Artifact", guiActiveEditor = false, externalToEVAOnly = true, guiActiveUnfocused = true, unfocusedRange = 1.5f)]
			public void PickUp() {
				Vessel active = FlightGlobals.ActiveVessel;
				bool isKerbal = active.isEVA;

				if(!isKerbal) return;

				ModuleInventoryPart inventory = active.evaController.ModuleInventoryPartReference;

				//Pick up part
				if(!inventory.InventoryIsFull) {
					AddArtifactToInventory(inventory);
					OWP.ui.CheckIfArtifactOnCraft();
					this.vessel.Die();
				}
				//Can't pick up part, inventory is full
				else {
					ScreenMessages.PostScreenMessage(OWP_LOC.Get("Event_Artifact_Inventory_Full"), 3.0f);
				}
			}

			public void AddArtifactToInventory(ModuleInventoryPart inv) {
				int freeSlot = inv.FirstEmptySlot();
				if(freeSlot < 0) return;

				if(type == 0) {
					Debug.Log("[Other Worlds] The moment you touch the artifact, it quickly splits open, revealing what appears to be a screen, and worringly, text you can read");
					OWP.SetProgressBit(28, true);
					ScreenMessages.PostScreenMessage(OWP_LOC.Get("Event_Artifact_Added_1"), 7.5f);
				}
				else if(type == 1) {
					Debug.Log("[Other Worlds] Forgotten by the sands themselves, another small mystery joins your hands... Time gives it another chance");
					OWP.SetProgressBit(29, true);
					ScreenMessages.PostScreenMessage(OWP_LOC.Get("Event_Artifact_Added_2"), 7.5f);
				}
				else {
					Debug.Log("[Other Worlds] Adding artifact (type " + type + ") to inventory... wait... that's not a thing!");
				}

				ProtoPartSnapshot proto = new ProtoPartSnapshot(part, null);

				//Part inventoryPart = UIPartActionControllerInventory.Instance.CreatePartFromInventory(proto);
				inv.StoreCargoPartAtSlot(proto, freeSlot);
				inv.UpdateStackAmountAtSlot(freeSlot, 1);
			}
		}
	}
}