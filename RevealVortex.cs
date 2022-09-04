using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace RevealVortex
{
    [BepInPlugin("Zephreo.PotionCraft.RevealVortex", "RevealVortex", "1.0.0")]
    [BepInProcess("Potion Craft.exe")]
    public class RevealVortex : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("Zephreo.PotionCraft.RevealVortex");

        void Awake()
        {
            Debug.Log($"[Reveal Vortex] Patching...");
            harmony.PatchAll();
            Debug.Log($"[Reveal Vortex] Patching complete.");
        }


        // Grinding status can be set to the end, which will then update the map
        [HarmonyPatch(typeof(PotionCraft.ObjectBased.RecipeMap.RecipeMapItem.VortexMapItem.VortexMapItem))]
        [HarmonyPatch("SetLockedState")]
        class ForceLockedState
        {
            // Skip the grinding status update if fully ground, to prevent it from going backward
            static void Prefix(ref bool locked)
            {
                locked = false;
            }
        }

        // Helper functions to access private fields, properties, and methods
        public static object InvokePrivateMethod(object instance, string methodName, params object[] parameters)
        {
            var method = instance.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (method is not null)
            {
                return method.Invoke(instance, parameters);
            }
            return null;
        }

        public static void SetPrivateField<T>(object instance, string fieldName, T value)
        {
            var prop = instance.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop.SetValue(instance, value);
        }

        public static T GetPrivateProperty<T>(object instance, string propertyName)
        {
            var prop = instance.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)prop.GetValue(instance);
        }
    }
}
