using CometRun.Data;
using CometRun.UI;
using UnityEngine.UI;

namespace CometRun.Screens
{
    // 06 Gameplay. The lab is only about the menus so this isn't a real game,
    // it's just the HUD. I needed it so Pause has something to open on top of
    // and "Finish Run" has a way to get to the Results screen.
    public class GameplayScreen : UIScreen
    {
        public Button pauseButton;
        public Button finishButton;
        public Text levelText;

        public UIScreen pause;
        public ResultsScreen results;

        public string Summary => levelText.text + "  |  Score 120";

        protected override void Awake()
        {
            base.Awake();
            pauseButton.onClick.AddListener(() => Nav.Push(pause));
            finishButton.onClick.AddListener(Finish);
        }

        public override void Refresh()
        {
            levelText.text = GameData.Mode == GameMode.Endless ? "ENDLESS" : "LEVEL " + (GameData.SelectedLevel + 1);
        }

        public override void OnBack()
        {
            Nav.Push(pause);
        }

        // there's no real score so I just give a fixed one (enough for 2 gems).
        // That way Results has numbers and the next level unlocks, so I can test the whole flow.
        void Finish()
        {
            if (GameData.ExtraTimeCharges > 0)
            {
                GameData.ExtraTimeCharges--;
                GameData.Save();
            }
            int sampleScore = GameData.ScoreForGem(2, GameData.SelectedLevel);
            results.SetResult(GameData.RecordResult(sampleScore));
            Nav.Replace(results);
        }
    }
}
