using System;
using UnityEngine;

namespace CarSystem
{
    [Serializable]
    public class CarSkin
    {
        [SerializeField] private CarSkinProperties carSkin;
        
        [SerializeField] private MeshRenderer[] glass;
        [SerializeField] private MeshRenderer[] body;
        [SerializeField] private MeshRenderer[] shield;
        [SerializeField] private MeshRenderer[] lightFront;
        [SerializeField] private MeshRenderer[] lightBack;
        [SerializeField] private MeshRenderer[] inner;
        [SerializeField] private MeshRenderer[] tier;
        [SerializeField] private MeshRenderer[] ring;

        public CarSkinProperties Skin
        {
            get => carSkin;
            set
            {
                carSkin = value;
                ColorCar();
            } 
        }

        public void OnValidate()
        {
            if(carSkin != null) ColorCar();
        }

        public void ColorCar()
        {
            if(carSkin == null) return;
             
            carSkin.SetMatBody(body);
            carSkin.SetMatGlass(glass);
            carSkin.SetMatInner(inner);
            carSkin.SetMatLightFront(lightFront);
            carSkin.SetMatLightBack(lightBack);
            carSkin.SetMatShield(shield);
            carSkin.SetMatRing(ring);
            carSkin.SetMatTier(tier);
        }
    }
}