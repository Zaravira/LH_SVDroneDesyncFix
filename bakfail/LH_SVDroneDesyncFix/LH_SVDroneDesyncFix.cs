using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using System.Security.Authentication;
using static UnityEngine.GraphicsBuffer;

namespace LH_SVDroneDesyncFix
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class LH_SVDroneDesyncFix : BaseUnityPlugin
    {
        public const string pluginGuid = "LH_SVDroneDesyncFix";
        public const string pluginName = "LH_SVDroneDesyncFix";
        public const string pluginVersion = "0.0.4";

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(LH_SVDroneDesyncFix));
        }

        [HarmonyPatch(typeof(Drone), "FindNewTarget")]
        [HarmonyPostfix]
        private static void DroneAttackRepair(Drone __instance, int mode, ref Entity ___targetEntity, float ___maxDistance)
        {
            if (!__instance.owner.gameObject.CompareTag("Player"))
                return;
            if (___targetEntity == null || Vector3.Distance(__instance.owner.position, ___targetEntity.transform.position) > ___maxDistance)
            {
                if (__instance.ownerSS == null)
                    __instance.ownerSS = __instance.owner.GetComponent<SpaceShip>();
                Collider[] array = Physics.OverlapSphere(__instance.owner.position, ___maxDistance, 512);
                Transform x = null;
                if (array.Length == 0)
                    return;
                array = array.OrderBy(c => (__instance.owner.position - c.transform.position).sqrMagnitude).ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == null)
                        continue;
                    Transform transform = array[i].transform;
                    SpaceShip spaceShip;
                    if (transform.CompareTag("Collider"))
                    {
                        spaceShip = (transform.GetComponent<ColliderControl>().ownerEntity as SpaceShip);
                        transform = spaceShip.transform;
                    }
                    else
                    {
                        spaceShip = transform.GetComponent<SpaceShip>();
                    }
                    if (spaceShip == null)
                        continue;
                    if ((mode == 1 && spaceShip != __instance.ownerSS && __instance.ownerSS.ffSys.TargetIsEnemy(spaceShip.ffSys) && !spaceShip.IsCloaked) || (mode == 2 && __instance.ownerSS.ffSys == spaceShip.ffSys && spaceShip.currHP < spaceShip.baseHP))
                    {
                        x = transform;
                        break;
                    }
                }
                if (x != null)
                {
                    __instance.target = x;
                    ___targetEntity = __instance.target.GetComponent<Entity>();
                    AccessTools.Method(typeof(Drone), "GetDesiredDistance").Invoke(__instance, null);
                }
                else
                {
                    Debug.Log("attack/repair shootin me fair");
                    __instance.target = __instance.ownerSS.transform;
                    ___targetEntity = __instance.ownerSS.GetComponent<Entity>();
                    AccessTools.Method(typeof(Drone), "GetDesiredDistance").Invoke(__instance, null);
                }
            }
        }

        [HarmonyPatch(typeof(Drone), "SearchForAsteroids")]
        [HarmonyPostfix]

        public static void DroneMiner(Drone __instance, ref Entity ___targetEntity, float ___maxDistance)
        {
            if (!__instance.owner.gameObject.CompareTag("Player"))
                return;
            if (___targetEntity == null || Vector3.Distance(__instance.owner.position, ___targetEntity.transform.position) > ___maxDistance)
            {
                int layerMask = 1024;
                Collider[] array = Physics.OverlapSphere(__instance.owner.position, ___maxDistance, layerMask);
                if (array.Length == 0)
                    return;
                array = array.OrderBy(c => (__instance.owner.position - c.transform.position).sqrMagnitude).ToArray();
                __instance.target = array[0].transform;
                ___targetEntity = __instance.target.GetComponent<Entity>();
                AccessTools.Method(typeof(Drone), "GetDesiredDistance").Invoke(__instance, null);

                if (___targetEntity == null)
                {
                    Debug.Log("miney whiney shootin me blimey");
                    __instance.target = __instance.ownerSS.transform;
                    ___targetEntity = __instance.ownerSS.GetComponent<Entity>();
                    AccessTools.Method(typeof(Drone), "GetDesiredDistance").Invoke(__instance, null);
                }
            }
        }

        [HarmonyPatch(typeof(Drone), "Recall")]
        [HarmonyPrefix]

        public static bool StopRecall(Drone __instance, float ___maxDistance, ref Entity ___targetEntity)
        {
            if (Vector3.Distance(__instance.owner.position, __instance.transform.position) > ___maxDistance && __instance != null && __instance.owner.gameObject.CompareTag("Player"))
            {
                if (___targetEntity != null && Vector3.Distance(__instance.owner.position, ___targetEntity.transform.position) < ___maxDistance)
                    return false;
                else
                {
                    __instance.target = null;
                    ___targetEntity = null;
                    if (__instance.isMiningDrone)
                        __instance.SearchForAsteroids();
                    else if (__instance.autoTargeting)
                    {
                        if (__instance.isAttackDrone)
                            AccessTools.Method(typeof(Drone), "FindNewTarget").Invoke(__instance, new object[] { 1 });
                        if (__instance.isRepairDrone)
                            AccessTools.Method(typeof(Drone), "FindNewTarget").Invoke(__instance, new object[] { 2 });
                    }
                    if (__instance.target != null)
                    {
                        ___targetEntity = __instance.target.GetComponent<Entity>();
                        if (Vector3.Distance(__instance.owner.position, ___targetEntity.transform.position) > ___maxDistance)
                            return true;
                        else
                            return false;
                    }
                }
            }
            return true;
        }

        [HarmonyPatch(typeof(Weapon), "Fire")]
        [HarmonyPrefix]

        public static bool HoldFire(Drone ___drone, Transform target)
        {
            if (___drone && ___drone.owner.gameObject.CompareTag("Player") && ___drone.droneType == 2 && target == ___drone.owner)
            {
                Debug.Log("fire! fire at me mom! coz she mean!");
                return false;
            }
            else
                return true;
        }

        [HarmonyPatch(typeof(Drone), "SetActions")]
        [HarmonyPostfix]
        public static void Drone_SetActions_Reaquire(Drone __instance, ref Transform ___target, ref Entity ___targetEntity)
        {
            //Already has a target, don't reacquire
            if (___targetEntity != null)
                return;

            //Only for player drones
            if (!__instance.owner.gameObject.CompareTag("Player"))
                return;

            ___target = __instance.owner;
            ___targetEntity = __instance.owner.GetComponent<Entity>();
        }

    }
}



