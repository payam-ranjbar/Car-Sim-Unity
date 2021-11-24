using UnityEngine;

namespace CarSystem
{
    [CreateAssetMenu(fileName = "CarSkinProperties", menuName = "Car/Car Skin", order = 0)]
    public class CarSkinProperties : ScriptableObject
    {
        public Material bodyMat;
        public Material glassMat;
        public Material innerMat;
        public Material tierMat;
        public Material ringMat;
        public Material lightFrontMat;
        public Material lightBackMat;
        public Material shieldMat;


        private static void SetMat(MeshRenderer[] meshRenderers, Material mat)
        {
            if(meshRenderers == null) return;

            foreach (var renderer in meshRenderers)
            {
                renderer.sharedMaterial = mat;
            }
        }

        public void SetMatBody(MeshRenderer[] meshRenderers)
        {
            SetMat(meshRenderers, bodyMat);
        }

        public void SetMatGlass(MeshRenderer[] meshRenderers)
        {
            SetMat(meshRenderers,glassMat);
        }
        public void SetMatLightFront(MeshRenderer[] meshRenderers)
        {
            SetMat(meshRenderers,lightFrontMat);
        }
        public void SetMatLightBack(MeshRenderer[] meshRenderers)
        {
            SetMat(meshRenderers,lightBackMat);
        }
        public void SetMatShield(MeshRenderer[] meshRenderers)
        {
            SetMat(meshRenderers,shieldMat);
        }
        public void SetMatTier(MeshRenderer[] meshRenderers)
        {
            SetMat(meshRenderers,tierMat);
        }
        public void SetMatInner(MeshRenderer[] meshRenderers)
        {
            SetMat(meshRenderers, innerMat);
        }     
        public void SetMatRing(MeshRenderer[] meshRenderers)
        {
            SetMat(meshRenderers,ringMat);
        }     

    }
}