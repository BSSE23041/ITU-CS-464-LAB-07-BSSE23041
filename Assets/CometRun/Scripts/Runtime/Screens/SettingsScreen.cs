using CometRun.Data;
using CometRun.Tweening;
using CometRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CometRun.Screens
{
    // 10 Settings. You can open it from the main menu or from pause,
    // and back takes you to whichever one you came from.
    public class SettingsScreen : UIScreen
    {
        public ToggleSwitch music;
        public ToggleSwitch sfx;
        public ToggleSwitch vibration;
        public Slider volume;
        public Text volumeValue;
        public Button resetButton;
        public Text resetLabel;

        bool confirmReset;

        protected override void Awake()
        {
            base.Awake();
            music.Changed += v => { GameData.Music = v; GameData.Save(); };
            sfx.Changed += v => { GameData.Sfx = v; GameData.Save(); };
            vibration.Changed += v =>
            {
                GameData.Vibration = v;
                GameData.Save();
#if UNITY_ANDROID || UNITY_IOS
                if (v) Handheld.Vibrate();
#endif
            };
            volume.onValueChanged.AddListener(v =>
            {
                GameData.Volume = v;
                AudioListener.volume = v;
                volumeValue.text = Mathf.RoundToInt(v * 100f) + "%";
                GameData.Save();
            });
            resetButton.onClick.AddListener(OnReset);
        }

        public override void Refresh()
        {
            music.SetValue(GameData.Music, false);
            sfx.SetValue(GameData.Sfx, false);
            vibration.SetValue(GameData.Vibration, false);
            volume.SetValueWithoutNotify(GameData.Volume);
            volumeValue.text = Mathf.RoundToInt(GameData.Volume * 100f) + "%";
            CancelReset();
        }

        // reset deletes everything so you have to tap twice (within 3 sec) to be safe
        void OnReset()
        {
            if (!confirmReset)
            {
                confirmReset = true;
                resetLabel.text = "TAP AGAIN TO CONFIRM";
                ((RectTransform)resetButton.transform).Shake(12f, 0.3f);
                Tweens.Wait(this, "resetTimeout", 3f).OnComplete(CancelReset);
                return;
            }

            GameData.ResetProgress();
            CancelReset();
            Nav.ShowToast("Progress reset", true);
        }

        void CancelReset()
        {
            confirmReset = false;
            resetLabel.text = "RESET PROGRESS";
        }
    }
}
