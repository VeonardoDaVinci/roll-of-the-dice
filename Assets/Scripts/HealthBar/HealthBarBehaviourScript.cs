using System;
using System.Collections.Generic;
using System.Linq;
using HealthBar.events;
using ScoreScreen;
using UnityEngine;

namespace HealthBar
{
    internal interface IHealthBar
    {
        public void DecreaseHealth();
        public GameOverEvent GameOverEvent { get; }
    }
    
    public class HealthBarBehaviourScript : MonoBehaviour, IHealthBar
    {
        private const string HealthBarPartialTag = "HealthBarPartial";
        private const int MaxHealthValue = 7;
        private const int HealthDecreaseStep = 1;
    
        private SpriteRenderer[] _partials;

        private HealthValue _health;

        public GameOverEvent GameOverEvent { get; private set; }

        public HealthBarBehaviourScript()
        {
            InstantiateEvents();
            BindGameOverEvent(); // @todo: to podpinanie powinno pewnie być poza tą klasą, może w GameManagerze
        }

        void Start()
        {
            LoadPartials();

            _health = new HealthValue(MaxHealthValue, HealthDecreaseStep);
        }

        private void InstantiateEvents()
        {
            GameOverEvent = new GameOverEvent();
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
                GameOverEvent.Invoke();
            }
        }

        private void BindGameOverEvent()
        {
            GameOverEvent.AddListener(ScoreScreenEventsHandler.handleGameOverEvent);
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
