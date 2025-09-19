using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
    public class HapticAnchor : MonoBehaviour
    {

        [System.Serializable]
        public class Effect
        {
            [SerializeField]
            private string name = string.Empty;
            public string Name => name;
            [SerializeField]
            private Haptic.PredefinedEffect.Effects predefinedEffect = Haptic.PredefinedEffect.Effects.Tick;
            public Haptic.PredefinedEffect.Effects PredefinedEffect => predefinedEffect;
            [SerializeField, Header("Callback")]
            private float ms = 10;
            public float MS => ms;
            [SerializeField]
            private int amplitude = 30;
            public int Amplitude => amplitude;
            [SerializeField]
            private TapticPlugin.ImpactFeedback iosFeedback = TapticPlugin.ImpactFeedback.Light;
            public TapticPlugin.ImpactFeedback IosFeedback => iosFeedback;
        }

        [SerializeField]
        private List<Effect> effects = new List<Effect>();

        public void ActivateEffect(int index)
        {
            if (index < 0 || index >= effects.Count)
                return;

            Haptic.VibratePredefined(effects[index].PredefinedEffect, () =>
            {
                Haptic.Vibrate((long)effects[index].MS, effects[index].Amplitude, effects[index].IosFeedback);
            });
        }

        public void ActivateEffect(string name)
        {
            Effect eff = effects.FirstOrDefault(x => x.Name == name);
            if (eff != null)
            {
                ActivateEffect(effects.IndexOf(eff));
            }
        }

    }
}