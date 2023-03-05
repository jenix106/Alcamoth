using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace Alcamoth
{
    public class AlcamothModule : LevelModule
    {
        public override IEnumerator OnLoadCoroutine()
        {
            EventManager.onUnpossess += EventManager_onUnpossess;
            EventManager.OnPlayerSpawned += EventManager_OnPlayerSpawned;
            return base.OnLoadCoroutine();
        }

        private void EventManager_OnPlayerSpawned()
        {
            Player.local.head.cam.farClipPlane *= 4;
        }

        private void EventManager_onUnpossess(Creature creature, EventTime eventTime)
        {
            if (eventTime == EventTime.OnStart)
                Player.local.head.cam.farClipPlane /= 4;
        }
        public override void OnUnload()
        {
            base.OnUnload();
            EventManager.onUnpossess -= EventManager_onUnpossess;
            EventManager.OnPlayerSpawned -= EventManager_OnPlayerSpawned;
        }
    }
    public class SetTexture : StateMachineBehaviour
    {
        public List<Renderer> renderers;
        public Texture2D texture;
        public void Start()
        {
            if(Level.current?.customReferences[0]?.transforms[0] != null)
            foreach(Transform reference in Level.current.customReferences[0].transforms)
            {
                if (!renderers.Contains(reference.GetComponent<Renderer>())) renderers.Add(reference.GetComponent<Renderer>());
            }
        }
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (renderers.IsNullOrEmpty()) 
                foreach (Transform reference in Level.current.customReferences[0].transforms)
                {
                    if (!renderers.Contains(reference.GetComponent<Renderer>())) renderers.Add(reference.GetComponent<Renderer>());
                }
            if (!renderers.IsNullOrEmpty())
                foreach (Renderer renderer in renderers)
                {
                    if (renderer != null)
                        renderer.material.SetTexture("_BaseMap", texture);
                }
        }
    }
    public class TeleportCreature : MonoBehaviour
    {
        public Transform exit;
        Zone zone;
        public void Start()
        {
            zone = GetComponent<Zone>();
        }
        public void Update()
        {
            if (zone.creaturesInZone.IsNullOrEmpty()) return;
            foreach(Creature creature in zone.creaturesInZone.Keys)
            {
                creature.Teleport(exit.position, exit.rotation);
                creature.locomotion.rb.velocity = Vector3.zero;
                foreach (RagdollPart part in creature.ragdoll.parts) part.rb.velocity = Vector3.zero;
            }
        }
    }
}