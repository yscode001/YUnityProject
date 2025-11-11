using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YUnity
{
    public static class SMRUtil
    {
        /// <summary>
        /// 将fromSMR拷贝至toSMR
        /// </summary>
        /// <param name="allBones">全量骨骼(可能会对其进行添加操作)</param>
        /// <param name="allBoneNames">全量骨骼名称(可能会对其进行添加操作)</param>
        /// <param name="fromSMR"></param>
        /// <param name="toSMR"></param>
        public static void CopyClothesToAnother(List<Transform> allBones, List<string> allBoneNames, SkinnedMeshRenderer fromSMR, SkinnedMeshRenderer toSMR)
        {
            if (allBones == null || allBones.Count <= 0 ||
                allBoneNames == null || allBoneNames.Count <= 0 ||
                fromSMR == null || toSMR == null) { return; }

            toSMR.transform.localPosition = fromSMR.transform.localPosition;
            toSMR.transform.localRotation = fromSMR.transform.localRotation;
            toSMR.transform.localScale = fromSMR.transform.localScale;
            toSMR.sharedMesh = fromSMR.sharedMesh;
            toSMR.sharedMaterial = fromSMR.sharedMaterial;

            foreach (Transform bone in allBones)
            {
                if (bone.name == fromSMR.rootBone.name)
                {
                    toSMR.rootBone = bone;
                    break;
                }
            }

            // 把from独有的骨骼加入到basic里面
            if (fromSMR.bones != null && fromSMR.bones.Length > 0)
            {
                for (int i = 0; i < fromSMR.bones.Length; i++)
                {
                    Transform fromBone = fromSMR.bones[i];
                    if (fromBone == null || allBoneNames.Contains(fromBone.name)) { continue; }
                    Transform newBone = fromBone.CopySelfEmptyGoToInArrayWithSameParentName(allBones, false);
                    if (newBone == null) { continue; }
                    allBones.Add(newBone);
                    allBoneNames.Add(newBone.name);
                }
            }

            // 真正的换骨
            List<Transform> boneList = new List<Transform>();
            for (int i = 0; i < fromSMR.bones.Length; i++)
            {
                for (int j = 0; j < allBones.Count; j++)
                {
                    if (fromSMR.bones[i].name == allBones[j].name)
                    {
                        boneList.Add(allBones[j]);
                        break;
                    }
                }
            }
            toSMR.bones = boneList.ToArray();
        }

        /// <summary>
        /// 清理，包括(sharedMesh,sharedMaterial,rootBone,bones)
        /// </summary>
        /// <param name="smr"></param>
        public static void Clear(this SkinnedMeshRenderer smr)
        {
            if (smr == null) { return; }
            smr.sharedMesh = null;
            smr.sharedMaterial = null;
            smr.rootBone = null;
            smr.bones = null;
        }

        /// <summary>
        /// 销毁所有的骨骼(骨骼必须不在nameNotIn里面才会被销毁)
        /// </summary>
        /// <param name="smr"></param>
        /// <param name="allBones">全量骨骼(可能会对其进行移除操作)</param>
        /// <param name="allBoneNames">全量骨骼名称(可能会对其进行移除操作)</param>
        /// <param name="nameNotIn">骨骼必须不在nameNotIn里面才会被销毁</param>
        public static void DestroyAllBones(this SkinnedMeshRenderer smr, List<Transform> allBones, List<string> allBoneNames, string[] nameNotIn)
        {
            if (smr == null) { return; }
            Transform[] willDeleteArray = smr.bones;
            if (willDeleteArray == null || willDeleteArray.Length <= 0) { return; }

            bool constraint = nameNotIn != null && nameNotIn.Length > 0;

            List<string> deleteNameList = new List<string>();
            for (int i = willDeleteArray.Length - 1; i >= 0; i--)
            {
                if (!constraint || !nameNotIn.Contains(willDeleteArray[i].name))
                {
                    deleteNameList.Add(willDeleteArray[i].name);
                    GameObject.Destroy(willDeleteArray[i].gameObject);
                }
            }

            if (allBones != null && allBones.Count > 0)
            {
                for (int i = allBones.Count - 1; i >= 0; i--)
                {
                    if (deleteNameList.Contains(allBones[i].name))
                    {
                        allBones.RemoveAt(i);
                    }
                }
            }
            if (allBoneNames != null && allBoneNames.Count > 0)
            {
                for (int i = allBoneNames.Count - 1; i >= 0; i--)
                {
                    if (deleteNameList.Contains(allBoneNames[i]))
                    {
                        allBoneNames.RemoveAt(i);
                    }
                }
            }
        }
    }
}