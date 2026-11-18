using CometRun.Data;
using CometRun.Tweening;
using CometRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CometRun.Screens
{
    // 08 Results. Shows how many gems, the score and coins you got,
    // then next level / retry / menu.
    public class ResultsScreen : UIScreen
    {
        public Text titleText;
        public Text noteText;
        public Text scoreText;
        public Text coinsText;
        public Image[] gems;
        public Button nextButton;
        public Text nextLabel;
        public Button retryButton;
        public Button menuButton;

        public UIScreen gameplay;
        public UIScreen loadout;
        public UIScreen levelSelect;
        public UIScreen mainMenu;

        RunResult result;

        protected override void Awake()
        {
            base.Awake();
            nextButton.onClick.AddListener(() =>
            {
                GameData.SelectedLevel++;
                Nav.BackTo(loadout);
            });
            retryButton.onClick.AddListener(() => Nav.Replace(gameplay));
            menuButton.onClick.AddListener(() => Nav.BackTo(mainMenu));
        }

        public void SetResult(RunResult r)
        {
            result = r;
        }

        public override void Refresh()
        {
            bool endless = result.mode == GameMode.Endless;
            if (endless) titleText.text = "RUN OVER";
            else if (result.cleared) titleText.text = "LEVEL " + (result.level + 1) + " CLEAR";
            else titleText.text = "TIME UP";
            titleText.color = result.cleared || endless ? UIPalette.Primary : UIPalette.Danger;

            if (result.unlockedNext) noteText.text = "Level " + (result.level + 2) + " unlocked!";
            else if (result.gems < 3) noteText.text = "Score " + result.nextGemScore + " for the next gem";
            else noteText.text = "Perfect run!";

            scoreText.text = result.score.ToString();
            coinsText.text = "+" + result.coins;

            for (int i = 0; i < gems.Length; i++)
            {
                gems[i].color = i < result.gems ? UIPalette.Gold : UIPalette.GemEmpty;
                gems[i].transform.localScale = Vector3.one;
            }

            // can't go to the next level if you didn't clear this one
            nextButton.interactable = result.hasNext;
            nextLabel.text = result.hasNext ? "NEXT LEVEL" : endless ? "NO LEVELS IN ENDLESS" : "CLEAR TO CONTINUE";
            nextLabel.color = result.hasNext ? UIPalette.OnPrimary : UIPalette.TextMuted;
            ((Image)nextButton.targetGraphic).color = result.hasNext ? UIPalette.Primary : UIPalette.Disabled;
        }

        public override void OnEnter()
        {
            // gems pop in one by one and then the numbers count up, feels more rewarding
            for (int i = 0; i < result.gems && i < gems.Length; i++)
            {
                gems[i].transform.localScale = Vector3.zero;
                gems[i].transform.ScaleTo(Vector3.one, 0.35f, Ease.OutBack).SetDelay(0.35f + i * 0.2f);
            }

            int finalScore = result.score;
            int finalCoins = result.coins;
            scoreText.text = "0";
            coinsText.text = "+0";
            Tweens.Value(scoreText, "count", 0, finalScore, 0.8f, Ease.OutQuad, v => scoreText.text = Mathf.RoundToInt(v).ToString()).SetDelay(0.2f);
            Tweens.Value(coinsText, "count", 0, finalCoins, 0.6f, Ease.OutQuad, v => coinsText.text = "+" + Mathf.RoundToInt(v)).SetDelay(0.6f);
        }

        public override void OnBack()
        {
            if (!Nav.BackTo(levelSelect)) Nav.BackTo(mainMenu);
        }
    }
}
