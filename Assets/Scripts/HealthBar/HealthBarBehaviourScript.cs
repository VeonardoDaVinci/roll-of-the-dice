using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HealthBar
{
    internal interface IHealthBar
    {
        public void DecreaseHealth();
    }
    
    public class HealthBarBehaviourScript : MonoBehaviour, IHealthBar
    {
        private const string HealthBarPartialTag = "HealthBarPartial";
        private const int MaxHealthValue = 7;
        private const int HealthDecreaseStep = 1;
    
        private SpriteRenderer[] _partials;

        private HealthValue _health;

        void Start()
        {
            LoadPartials();

            _health = new HealthValue(MaxHealthValue, HealthDecreaseStep);
        }

        public void DecreaseHealth()
        {
            if (_health.ReachedZero())
            {
                return;
            }
            
            try
            {
                HideNextBarPartial();
                
                _health.Decrease();
            }
            catch (HealthValueReachedZeroException e)
            {
                // przegraliśmy, co teraz? :p
            }
        }

        private void HideNextBarPartial()
        {
            var partialNumber = _health.GetTakenHitsCount() + 1;
            var partialIndex = partialNumber - 1;
            var partialToHide = _partials[partialIndex] ?? null;

            MakePartialSemiTransparent(partialToHide);
        }

        private static void MakePartialSemiTransparent(SpriteRenderer partial)
        {
            Color color = new Color();
            color.a = (float)0.5;
            partial.color = color;
        }

        private void LoadPartials()
        {
            var partials = GameObject.FindGameObjectsWithTag(HealthBarPartialTag);
            _partials = partials.Select(partial => partial.GetComponent<SpriteRenderer>()).ToArray();
        }

        void Update()
        {
        
        }
    }
}
