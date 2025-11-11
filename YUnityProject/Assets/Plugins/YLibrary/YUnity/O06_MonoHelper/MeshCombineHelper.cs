using System.Collections.Generic;
using UnityEngine;

namespace YUnity
{
    public class MeshCombineHelper : MonoBehaviour
    {
        private MeshFilter mf;
        private MeshRenderer mr;

        private void Start()
        {
            mf = this.GetOrAddComponent<MeshFilter>();
            mr = this.GetOrAddComponent<MeshRenderer>();
            MeshCombine();
        }

        private void MeshCombine()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            List<Material> materials = new List<Material>();

            for (int i = 0; i < meshFilters.Length; i++)
            {
                foreach (Material mat in meshRenderers[i].sharedMaterials)
                {
                    if (mat != null)
                    {
                        materials.Add(mat);
                    }
                }

                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].SetAct(false);
            }

            mf.sharedMesh = new Mesh();
            // 为mesh.CombineMeshes添加一个false参数，表示并不是合并为一个网格，而是一个子网格列表
            mf.sharedMesh.CombineMeshes(combine, false);
            mr.sharedMaterials = materials.ToArray();
            transform.SetAct(true);
        }
    }
}