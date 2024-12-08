using System.Collections.Generic;
using System.Threading.Tasks;
using GreonAssets.UI.ComponentAnimations;
using UnityEngine;

namespace GreonAssets.UI.Extensions
{
    public static class GameObjectExtensions
    {
        public static void CloseWithAnimation(this GameObject obj)
        {
            if(obj == null) return;
            if(obj.TryGetComponent(out UIOpenCloseAnimations animationComponent))
            {
                animationComponent.Close();
                return;
            }
            obj.gameObject.SetActive(false);
        }
        public static async Task CloseWithAnimationAsync(this GameObject obj)
        {
            if(obj == null) return;
            if(obj.TryGetComponent(out UIOpenCloseAnimations animationComponent))
            {
                await animationComponent.CloseAsync();
                return;
            }
            obj.gameObject.SetActive(false);
        }
        
        public static void CloseWithChildrensAnimation(this GameObject obj)
        {
            _ = CloseWithChildrensAnimationAsync(obj);
        }
        public static async Task CloseWithChildrensAnimationAsync(this GameObject obj)
        {
            if (obj == null) return;

            var animations = obj.GetComponentsInChildren<UIOpenCloseAnimations>();

            bool rootObjectAnimated = false;
            List<Task> closeTasks = new List<Task>();

            foreach (var animation in animations)
            {
                if (animation.gameObject == obj)
                    rootObjectAnimated = true;
                closeTasks.Add(animation.CloseAsync(animation.gameObject == obj));
            }

            await Task.WhenAll(closeTasks);

            if (!rootObjectAnimated)
                obj.SetActive(false);
        }
    }
}
