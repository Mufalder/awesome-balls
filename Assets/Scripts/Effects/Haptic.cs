using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
    public static class Haptic
    {

        public static class PredefinedEffect
        {
            public enum Effects { Click, DoubleClick, HeavyClick, Tick };

            public static int EFFECT_CLICK;
            public static int EFFECT_DOUBLE_CLICK;
            public static int EFFECT_HEAVY_CLICK;
            public static int EFFECT_TICK;

            public static int GetEffect(Effects effect)
            {
                switch (effect)
                {
                    case Effects.Click:
                        return EFFECT_CLICK;

                    case Effects.DoubleClick:
                        return EFFECT_DOUBLE_CLICK;

                    case Effects.HeavyClick:
                        return EFFECT_HEAVY_CLICK;

                    default:
                        return EFFECT_TICK;
                }
            }
        }

        private static bool log = false;

        private static int apiLevel = 1;

        private static bool SupportVibration => apiLevel >= 26;
        private static bool SupportPredefined => apiLevel >= 29;

        private static AndroidJavaObject vibrator = null;
        private static AndroidJavaClass vibrationEffectClass = null;

        private static bool initialized;

        private static int defaultAmplitude = 255;

        private static void Temp()
        {
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        private static void Initialize()
        {
            if (initialized)
                return;

#if UNITY_EDITOR
            //nothing here
#elif UNITY_ANDROID
            using(AndroidJavaClass androidVersionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                apiLevel = androidVersionClass.GetStatic<int>("SDK_INT");
            }

            using(AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using(AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    if (currentActivity != null)
                        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

                    if (SupportVibration)
                    {
                        vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                        defaultAmplitude = Mathf.Clamp(vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE"), 1, 255);
                    }

                    if (SupportPredefined)
                    {
                        PredefinedEffect.EFFECT_CLICK = vibrationEffectClass.GetStatic<int>("EFFECT_CLICK");
                        PredefinedEffect.EFFECT_DOUBLE_CLICK = vibrationEffectClass.GetStatic<int>("EFFECT_DOUBLE_CLICK");
                        PredefinedEffect.EFFECT_HEAVY_CLICK = vibrationEffectClass.GetStatic<int>("EFFECT_HEAVY_CLICK");
                        PredefinedEffect.EFFECT_TICK = vibrationEffectClass.GetStatic<int>("EFFECT_TICK");
                    }
                }
            }
#elif UNITY_IOS
            //nothing here either
#endif

            initialized = true;
        }

        private static bool HasVibrator()
        {
            return vibrator != null && vibrator.Call<bool>("hasVibrator");
        }

        private static bool HasAmplitudeEffect()
        {
            if (HasVibrator() && SupportVibration)
            {
                return vibrator.Call<bool>("hasAmplitudeControl");
            }
            else
            {
                return false;
            }
        }

        private static void AndroidVibrate(long ms, int amplitude)
        {
            using (AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", ms, amplitude))
            {
                vibrator.Call("vibrate", effect);
            }
        }

        private static void AndroidVibrateLegacy(long ms)
        {
            vibrator.Call("vibrate", ms);
        }

        private static void AndroidPredefinedVibrate(PredefinedEffect.Effects effectId)
        {
            using (AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createPredefined", (int)effectId))
            {
                vibrator.Call("vibrate", effect);
            }
        }

        public static void Vibrate(long ms, int amplitude, System.Action iosFeedback)
        {
            Initialize();

#if UNITY_EDITOR
            if (log)
                Debug.Log("Haha vibration goes BRBRBRBRBBRBRBRBRRB....");
#elif UNITY_ANDROID
            if (!HasVibrator())
                return;

            if (SupportVibration)
            {
                if (HasAmplitudeEffect())
                {
                    amplitude = Mathf.Clamp(amplitude, -1, 255);
                    if (amplitude == -1)
                        amplitude = 255;
                    if (amplitude == 0)
                        amplitude = defaultAmplitude;
                }
                else
                {
                    amplitude = 255;
                }

                AndroidVibrate(ms, amplitude);
            }
            else
            {
                AndroidVibrateLegacy(ms);
            }
#elif UNITY_IOS
            iosFeedback?.Invoke();
#endif
        }

        public static void Vibrate(long ms, int amplitude, TapticPlugin.ImpactFeedback iosFeedback)
        {
            Vibrate(ms, amplitude, () =>
            {
                TapticPlugin.TapticManager.Impact(iosFeedback);
            });
        }

        public static void VibratePredefined(PredefinedEffect.Effects effect, System.Action notSupported)
        {
            Initialize();

#if UNITY_EDITOR
            if (log)
                Debug.Log("PreDefined: " + effect.ToString());
#elif UNITY_ANDROID
            if (!HasVibrator())
                return;

            if (SupportPredefined)
            {
                AndroidPredefinedVibrate(effect);
            }
            else
            {
                notSupported?.Invoke();
            }
#elif UNITY_IOS
            switch (effect)
            {
                case PredefinedEffect.Effects.Click:
                    TapticPlugin.TapticManager.Impact(TapticPlugin.ImpactFeedback.Medium);
                    break;

                case PredefinedEffect.Effects.DoubleClick:
                    TapticPlugin.TapticManager.Impact(TapticPlugin.ImpactFeedback.Heavy);
                    break;

                case PredefinedEffect.Effects.HeavyClick:
                    TapticPlugin.TapticManager.Impact(TapticPlugin.ImpactFeedback.Heavy);
                    break;

                case PredefinedEffect.Effects.Tick:
                    TapticPlugin.TapticManager.Impact(TapticPlugin.ImpactFeedback.Light);
                    break;
            }
#endif
        }

    }
}