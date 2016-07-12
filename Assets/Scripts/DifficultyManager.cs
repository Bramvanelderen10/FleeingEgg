using System;
using System.Collections;
namespace Difficulty
{
    public enum DifficultyLevel
    {
        VERYEASY,
        EASY,
        NORMAL,
        HARD,
        VERYHARD,
        INSANE
    }

    public class DifficultyManager
    {       
        public static DifficultyManager Instance;
        private bool _isActive;
        private float _time;
        private float _timer = 5;
        private DifficultyLevel _difficulty;

        public DifficultyManager()
        {
            Instance = this;
        }

        public void Start(float time)
        {
            _time = time;
            _timer = 5;
            _difficulty = DifficultyLevel.VERYEASY;
        }

        public void Update(float time)
        {
            if (time - _time > _timer)
            {
                _time = time;
                _difficulty++;

                if ((int)_difficulty > Enum.GetNames(typeof(DifficultyLevel)).Length - 1)
                {
                    _difficulty = (DifficultyLevel)Enum.GetNames(typeof(DifficultyLevel)).Length - 1;
                }

                switch (_difficulty)
                {
                    case DifficultyLevel.VERYEASY:
                        _timer = 5;
                        break;
                    case DifficultyLevel.EASY:
                        _timer = 10;
                        break;
                    case DifficultyLevel.NORMAL:
                        _timer = 15;
                        break;
                    case DifficultyLevel.HARD:
                        _timer = 1;
                        break;
                    case DifficultyLevel.VERYHARD:
                        _timer = 20;
                        break;
                    case DifficultyLevel.INSANE:
                        _timer = 100;
                        break;
                }
            }
        }

        public DifficultyLevel GetDifficulty()
        {
            return _difficulty;
        }

        public bool IsActive()
        {
            return _isActive;
        }

    }
}

