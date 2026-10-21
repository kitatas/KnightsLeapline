using UnityEngine;

namespace BoardTower.Game.Application
{
    public sealed class BoardConfig
    {
        public const int MIN_FILE = 1;
        public const int MAX_FILE = 8;
        public const int HALF_FILE = MAX_FILE / 2;
        public const int MIN_RANK = 1;
        public const int MAX_RANK = 8;
        public const int HALF_RANK = MAX_RANK / 2;
        public const float DELAY_RATE = 0.1f;
        public const float FADE_DURATION = 0.25f;
        public const float HIGHLIGHT_DURATION = 0.5f;
        public const float EVENT_DURATION = 0.0f;
        public const float EVENT_OBJECT_DURATION = 3.0f;

        public static readonly RotateType[] ROTATES = {
            RotateType.Angle0,
            RotateType.Angle90,
            RotateType.Angle180,
            RotateType.Angle270,
        };

        public static readonly SquareEventType[] BELTS =
        {
            SquareEventType.BeltRight,
            SquareEventType.BeltDown,
            SquareEventType.BeltLeft,
            SquareEventType.BeltUp,
        };
    }

    public sealed class ChessmenConfig
    {
        public static readonly ChessmenType DEFAULT_TYPE = ChessmenType.Knight;
        public const float FADE_DURATION = 0.25f;
        public const float MOVE_DURATION = 0.25f;
    }

    public sealed class MovementOffsetConfig
    {
        public static readonly ChessmenMovementOffsetVO UP = new(0, 1);
        public static readonly ChessmenMovementOffsetVO DOWN = new(0, -1);
        public static readonly ChessmenMovementOffsetVO LEFT = new(-1, 0);
        public static readonly ChessmenMovementOffsetVO RIGHT = new(1, 0);
    }

    public sealed class RoundConfig
    {
        public const int MAX_NUM = 7;
    }

    public sealed class TapScreenConfig
    {
        public const float FADE_DURATION = 0.25f;
        public const float FLASH_DURATION = 1.0f;
    }

    public sealed class HudRootConfig
    {
        public const float FADE_DURATION = 0.25f;
        public const float FADE_DELAY_RATE = 0.2f;
    }

    public sealed class GameModalConfig
    {
        public const float FADE_DURATION = 0.25f;
    }

    public sealed class RelicConfig
    {
        public const int LOT_NUM = 3;
        public const float LOT_FADE_DURATION = 0.25f;
        public const float LOT_DELAY_RATE = 0.1f;
    }

    public sealed class ScoreConfig
    {
        public const int BASE_GEM_VALUE = 100;
        public const int BASE_ROUND_CLEAR_VALUE = 500;

        public const float OVERFLOW_ROUND_GEM_RATE = 2.0f;
        public const float HALVED_RATE = 1.5f;
    }

    public sealed class AchievementConfig
    {
        public static readonly AchievementType[] ACHIEVEMENTS =
        {
            AchievementType.Play,
            AchievementType.Score,
            AchievementType.Clear,
        };

        public static readonly AchievementEffectVO LOCKED = new(
            new Color32(0x00, 0x00, 0x00, 0xFF),
            new Color32(0x00, 0x00, 0x00, 0xFF),
            new Color32(0x00, 0x00, 0x00, 0xFF),
            new Color32(0x00, 0x00, 0x00, 0xFF),
            new Color32(0x00, 0x00, 0x00, 0xFF),
            new Color32(0x00, 0x00, 0x00, 0xFF),
            0.65f,
            0.015f,
            0f,
            false,
            false
        );

        public static readonly AchievementEffectVO NORMAL = new(
            new Color32(0x12, 0x2E, 0x48, 0xFF),
            new Color32(0x55, 0x91, 0xBB, 0xFF),
            new Color32(0xFF, 0xFF, 0xFF, 0xFF),
            new Color32(0x2A, 0x52, 0x70, 0xFF),
            new Color32(0xF0, 0xFC, 0xFF, 0xFF),
            new Color32(0x06, 0x14, 0x20, 0xFF),
            1.05f,
            0.07f,
            0.10f,
            false,
            false
        );

        public static readonly AchievementEffectVO BRONZE = new(
            new Color32(0x50, 0x22, 0x00, 0xFF),
            new Color32(0xD8, 0x78, 0x00, 0xFF),
            new Color32(0xFF, 0xF8, 0x70, 0xFF),
            new Color32(0xA5, 0x4A, 0x00, 0xFF),
            new Color32(0xFF, 0xFF, 0xC0, 0xFF),
            new Color32(0x30, 0x10, 0x00, 0xFF),
            1.10f,
            0.08f,
            0.10f,
            true,
            false
        );

        public static readonly AchievementEffectVO SILVER = new(
            new Color32(0x55, 0x5B, 0x62, 0xFF),
            new Color32(0xA8, 0xAF, 0xB6, 0xFF),
            new Color32(0xFF, 0xFF, 0xFF, 0xFF),
            new Color32(0x78, 0x80, 0x88, 0xFF),
            new Color32(0xFF, 0xFF, 0xFF, 0xFF),
            new Color32(0x28, 0x2C, 0x31, 0xFF),
            1.15f,
            0.08f,
            0.11f,
            true,
            false
        );

        public static readonly AchievementEffectVO GOLD = new(
            new Color32(0x5C, 0x45, 0x00, 0xFF),
            new Color32(0xE6, 0xB8, 0x00, 0xFF),
            new Color32(0xFF, 0xF5, 0x65, 0xFF),
            new Color32(0xB8, 0x82, 0x00, 0xFF),
            new Color32(0xFF, 0xFF, 0xB0, 0xFF),
            new Color32(0x3D, 0x2A, 0x00, 0xFF),
            1.20f,
            0.09f,
            0.13f,
            true,
            false
        );

        public static readonly AchievementEffectVO PLATINUM = new(
            new Color32(0x48, 0x8E, 0xB0, 0xFF),
            new Color32(0xB8, 0xEC, 0xFF, 0xFF),
            new Color32(0xFF, 0xFF, 0xFF, 0xFF),
            new Color32(0x78, 0xD8, 0xFF, 0xFF),
            new Color32(0xFF, 0xFF, 0xFF, 0xFF),
            new Color32(0x18, 0x4C, 0x68, 0xFF),
            1.25f,
            0.09f,
            0.13f,
            true,
            true
        );

        public static readonly Color UNACHIEVED_COLOR = new(0.3f, 0.3f, 0.3f, 1.0f);
        public const string SECRET_CONTENT = "???";
    }

    public sealed class OfflineConfig
    {
        public static readonly GameModalType[] DEACTIVE_GAME_MODALS =
        {
            GameModalType.DeleteConfirm,
            GameModalType.DeleteComplete,
            GameModalType.Account,
            GameModalType.Achievement,
            GameModalType.Ranking,
        };
    }

    public sealed class EventLogConfig
    {
        public const string LOCALE_KEY = "EventLog_{0}";
        public const float DURATION = 0.25f;
    }
}