using UnityEngine;

namespace YUnity
{
    public static class ComponentExt
    {
        /// <summary>
        /// 获取组件(如果没有，先添加后再获取)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.GetOrAddComponent<T>();
        }

        /// <summary>
        /// 启用或禁用组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <param name="enable"></param>
        public static void EnableOrDisableComponent<T>(this Component component, bool enable) where T : Behaviour
        {
            component.gameObject.EnableOrDisableComponent<T>(enable);
        }

        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="component"></param>
        /// <param name="active"></param>
        public static void SetAct(this Component component, bool active)
        {
            component.gameObject.SetAct(active);
        }

        /// <summary>
        /// 销毁所有子物体
        /// </summary>
        /// <param name="component"></param>
        /// <param name="immediate"></param>
        public static void DestroyAllChild(this Component component, bool immediate = false)
        {
            component.gameObject.DestroyAllChild(immediate);
        }

        /// <summary>
        /// 重置自己和所有孩子身上的所有Render的shader
        /// </summary>
        /// <param name="component"></param>
        public static void ResetAllRendersShader(this Component component)
        {
            Renderer[] renderers = component.GetComponentsInChildren<Renderer>(true);
            if (renderers != null)
            {
                foreach (var renderer in renderers)
                {
                    // 处理单个材质的情况
                    if (renderer.material != null)
                    {
                        renderer.material.ResetShader();
                    }
                    // 处理多材质的情况
                    if (renderer.materials != null && renderer.materials.Length > 0)
                    {
                        Material[] materials = renderer.materials;
                        for (int i = 0; i < materials.Length; i++)
                        {
                            if (materials[i] != null)
                            {
                                materials[i].ResetShader();
                            }
                        }
                        // 重新赋值材质数组（确保修改生效）
                        renderer.materials = materials;
                    }
                }
            }
        }
    }
}