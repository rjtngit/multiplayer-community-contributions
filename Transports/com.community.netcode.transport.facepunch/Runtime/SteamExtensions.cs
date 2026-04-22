using System.Reflection;
using Steamworks.Ugc;

namespace Steamworks
{
    public static class SteamExtensions
    {
        public enum OverlayToStoreFlag
        {
            None,
            AddToCart,
            AddToCartAndShow
        }

        private static object Invoke<T>(string methodName, params object[] parameters)
        {
            var classType = typeof(T);
            var internalProperty =
                classType.GetProperty("Internal", BindingFlags.NonPublic | BindingFlags.Static);
            var internalValue = internalProperty.GetValue(null);
            var internalType = internalValue.GetType();

            var method = internalType.GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            return method.Invoke(internalValue, parameters);
        }

        public static void OpenStoreOverlay(AppId id, OverlayToStoreFlag overlayToStoreFlag)
        {
            Invoke<SteamFriends>("ActivateGameOverlayToStore", (AppId) id.Value, (int) overlayToStoreFlag);
        }

        public static void AddAppDependency(this Item item, AppId appId)
        {
            Invoke<SteamUGC>("AddAppDependency", item.Id, appId);
        }

        public static void RemoveAppDependency(this Item item, AppId appId)
        {
            Invoke<SteamUGC>("RemoveAppDependency", item.Id, appId);
        }

        public static Query ForAppId(this Query q, AppId app)
        {
            var field = typeof(Query).GetField("consumerApp", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValueDirect(__makeref(q), app);
            return q;
        }

        public static Query WithCreatorAppId(this Query q, AppId app)
        {
            var field = typeof(Query).GetField("creatorApp", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValueDirect(__makeref(q), app);
            return q;
        }
    }
}