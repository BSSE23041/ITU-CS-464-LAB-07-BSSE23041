using CometRun.UI;
using UnityEngine.UI;

namespace CometRun.Screens
{
    // 07 Pause. Opens on top of gameplay instead of replacing it, so you still see the game behind.
    public class PauseScreen : UIScreen
    {
        public Button resumeButton;
        public Button restartButton;
        public Button settingsButton;
        public Button quitButton;
        public Text infoText;

        public GameplayScreen gameplay;
        public UIScreen settings;
        public UIScreen mainMenu;

        protected override void Awake()
        {
            base.Awake();
            resumeButton.onClick.AddListener(() => Nav.Back());
            restartButton.onClick.AddListener(() =>
            {
                Nav.Back();
                Nav.ShowToast("Run restarted");
            });
            settingsButton.onClick.AddListener(() => Nav.Push(settings));
            quitButton.onClick.AddListener(() => Nav.BackTo(mainMenu));
        }

        public override void Refresh()
        {
            infoText.text = gameplay.Summary;
        }
    }
}
