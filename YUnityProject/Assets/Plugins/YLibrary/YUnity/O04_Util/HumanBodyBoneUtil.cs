// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-12-26
// ------------------------------

using UnityEngine;

namespace YUnity
{
    public struct HumanBodyBone
    {
        public Transform Bone;
        public Vector3 InitLocalPosition;
        public Vector3 InitLocalEuler;
        public Vector3 InitScale;

        public HumanBodyBone(Transform bone)
        {
            Bone = bone;
            InitLocalPosition = bone.localPosition;
            InitLocalEuler = bone.localEulerAngles;
            InitScale = bone.localScale;
        }
    }

    public partial class HumanBodyBoneUtil
    {
        public Animator animator { get; private set; } = null;
        public bool IsInited { get; private set; } = false;

        public HumanBodyBoneUtil() { }
        public HumanBodyBoneUtil(Animator animator)
        {
            this.animator = animator;
            Init();
        }
        public void SetupAnimator(Animator animator)
        {
            this.animator = animator;
            Init();
        }
    }
    public partial class HumanBodyBoneUtil
    {
        public HumanBodyBone Hips;
        public HumanBodyBone LeftUpperLeg;
        public HumanBodyBone RightUpperLeg;
        public HumanBodyBone LeftLowerLeg;
        public HumanBodyBone RightLowerLeg;
        public HumanBodyBone LeftFoot;
        public HumanBodyBone RightFoot;
        public HumanBodyBone Spine;
        public HumanBodyBone Chest;
        public HumanBodyBone UpperChest;
        public HumanBodyBone Neck;
        public HumanBodyBone Head;
        public HumanBodyBone LeftShoulder;
        public HumanBodyBone RightShoulder;
        public HumanBodyBone LeftUpperArm;
        public HumanBodyBone RightUpperArm;
        public HumanBodyBone LeftLowerArm;
        public HumanBodyBone RightLowerArm;
        public HumanBodyBone LeftHand;
        public HumanBodyBone RightHand;
        public HumanBodyBone LeftToes;
        public HumanBodyBone RightToes;
        public HumanBodyBone LeftEye;
        public HumanBodyBone RightEye;
        public HumanBodyBone Jaw;
        public HumanBodyBone LeftThumbProximal;
        public HumanBodyBone LeftThumbIntermediate;
        public HumanBodyBone LeftThumbDistal;
        public HumanBodyBone LeftIndexProximal;
        public HumanBodyBone LeftIndexIntermediate;
        public HumanBodyBone LeftIndexDistal;
        public HumanBodyBone LeftMiddleProximal;
        public HumanBodyBone LeftMiddleIntermediate;
        public HumanBodyBone LeftMiddleDistal;
        public HumanBodyBone LeftRingProximal;
        public HumanBodyBone LeftRingIntermediate;
        public HumanBodyBone LeftRingDistal;
        public HumanBodyBone LeftLittleProximal;
        public HumanBodyBone LeftLittleIntermediate;
        public HumanBodyBone LeftLittleDistal;
        public HumanBodyBone RightThumbProximal;
        public HumanBodyBone RightThumbIntermediate;
        public HumanBodyBone RightThumbDistal;
        public HumanBodyBone RightIndexProximal;
        public HumanBodyBone RightIndexIntermediate;
        public HumanBodyBone RightIndexDistal;
        public HumanBodyBone RightMiddleProximal;
        public HumanBodyBone RightMiddleIntermediate;
        public HumanBodyBone RightMiddleDistal;
        public HumanBodyBone RightRingProximal;
        public HumanBodyBone RightRingIntermediate;
        public HumanBodyBone RightRingDistal;
        public HumanBodyBone RightLittleProximal;
        public HumanBodyBone RightLittleIntermediate;
        public HumanBodyBone RightLittleDistal;
        public HumanBodyBone LastBone;
    }
    public partial class HumanBodyBoneUtil
    {
        private void Init()
        {
            if (animator == null) { return; }
            IsInited = true;
            Hips = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.Hips));

            LeftUpperLeg = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg));
            RightUpperLeg = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightUpperLeg));
            LeftLowerLeg = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg));
            RightLowerLeg = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightLowerLeg));
            LeftFoot = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftFoot));
            RightFoot = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightFoot));
            Spine = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.Spine));
            Chest = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.Chest));
            UpperChest = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.UpperChest));
            Neck = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.Neck));
            Head = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.Head));

            LeftShoulder = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftShoulder));
            RightShoulder = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightShoulder));
            LeftUpperArm = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftUpperArm));
            RightUpperArm = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightUpperArm));
            LeftLowerArm = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftLowerArm));
            RightLowerArm = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightLowerArm));
            LeftHand = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftHand));
            RightHand = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightHand));
            LeftToes = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftToes));
            RightToes = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightToes));
            LeftEye = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftEye));

            RightEye = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightEye));
            Jaw = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.Jaw));
            LeftThumbProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal));
            LeftThumbIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate));
            LeftThumbDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftThumbDistal));
            LeftIndexProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal));
            LeftIndexIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate));
            LeftIndexDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftIndexDistal));
            LeftMiddleProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal));
            LeftMiddleIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate));
            LeftMiddleDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal));

            LeftRingProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftRingProximal));
            LeftRingIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftRingIntermediate));
            LeftRingDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftRingDistal));
            LeftLittleProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal));
            LeftLittleIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate));
            LeftLittleDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LeftLittleDistal));
            RightThumbProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightThumbProximal));
            RightThumbIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate));
            RightThumbDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightThumbDistal));
            RightIndexProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightIndexProximal));
            RightIndexIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate));

            RightIndexDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightIndexDistal));
            RightMiddleProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal));
            RightMiddleIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate));
            RightMiddleDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal));
            RightRingProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightRingProximal));
            RightRingIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate));
            RightRingDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightRingDistal));
            RightLittleProximal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightLittleProximal));
            RightLittleIntermediate = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate));
            RightLittleDistal = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.RightLittleDistal));
            LastBone = new HumanBodyBone(animator.GetBoneTransform(HumanBodyBones.LastBone));
        }
    }
}