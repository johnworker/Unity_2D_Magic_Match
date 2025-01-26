using System;
using UnityEngine;

namespace Match3
{
    /// <summary>
    /// 包含此等級的所有資料：目標和最大移動次數。這也將通知
    /// GameManager，我們載入了一個級別
    /// </summary>
    [DefaultExecutionOrder(12000)]
    public class LevelData : MonoBehaviour
    {
        public static LevelData Instance { get; private set; }
    
        [Serializable]
        public class GemGoal
        {
            public Gem Gem;
            public int Count;
        }

        public string LevelName = "Level";
        public int MaxMove;
        public int LowMoveTrigger = 10;
        public GemGoal[] Goals;
        
        [Header("Visuals")]
        public float BorderMargin = 0.3f;
        public SpriteRenderer Background;
        
        [Header("Audio")] 
        public AudioClip Music;

        public delegate void GoalChangeDelegate(int gemType,int newAmount);
        public delegate void MoveNotificationDelegate(int moveRemaining);

        public Action OnAllGoalFinished;
        public Action OnNoMoveLeft;
    
        public GoalChangeDelegate OnGoalChanged;
        public MoveNotificationDelegate OnMoveHappened;

        public int RemainingMove { get; private set; }
        public int GoalLeft { get; private set; }

        private int m_StartingWidth;
        private int m_StartingHeight;

        private void Awake()
        {
            Instance = this;
            RemainingMove = MaxMove;
            GoalLeft = Goals.Length;
            GameManager.Instance.StartLevel();
        }

        void Start()
        {
            m_StartingWidth = Screen.width;
            m_StartingHeight = Screen.height;

            if (Background != null)
                Background.gameObject.SetActive(false);
        }

        void Update()
        {
            //為了偵測裝置方向變化或解析度變化，我們檢查自初始化以來螢幕是否發生變化
            //並重新計算相機變焦
            if (Screen.width != m_StartingWidth || Screen.height != m_StartingHeight)
            {
                GameManager.Instance.ComputeCamera();
            }
        }

        public bool Matched(Gem gem)
        {
            foreach (var goal in Goals)
            {
                if (goal.Gem.GemType == gem.GemType)
                {
                    if (goal.Count == 0)
                        return false;
                
                    UIHandler.Instance.AddMatchEffect(gem);
                
                    goal.Count -= 1;
                    OnGoalChanged?.Invoke(gem.GemType, goal.Count);

                    if (goal.Count == 0)
                    {
                        GoalLeft -= 1;
                        if (GoalLeft == 0)
                        {
                            GameManager.Instance.WinStar();
                            GameManager.Instance.Board.ToggleInput(false);
                            OnAllGoalFinished?.Invoke();
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public void DarkenBackground(bool darken)
        {
            if (Background == null)
                return;

            Background.gameObject.SetActive(darken);
        }

        public void Moved()
        {
            var prev = RemainingMove;
        
        
            RemainingMove = Mathf.Max(0, RemainingMove - 1);
            OnMoveHappened?.Invoke(RemainingMove);

            if (prev > LowMoveTrigger && RemainingMove <= LowMoveTrigger)
            {
                UIHandler.Instance.TriggerCharacterAnimation(UIHandler.CharacterAnimation.LowMove);
            }

            if (RemainingMove <= 0)
            {
                OnNoMoveLeft();
            }
        }
    }
}