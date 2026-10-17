using BoardTower.Common.Application;
using FastEnumUtility;
using UniEx;

namespace BoardTower.Game.Application
{
    public static class EnumExtension
    {
        public static ChessmenType ToChessmenType(this int self)
        {
            return FastEnum.IsDefined<ChessmenType>(self)
                ? (ChessmenType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_CHESSMEN);
        }

        public static ChessmenMovementType ToChessmenMovementType(this int self)
        {
            return FastEnum.IsDefined<ChessmenMovementType>(self)
                ? (ChessmenMovementType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_CHESSMEN_MOVEMENT);
        }

        public static RelicType ToRelicType(this int self)
        {
            return FastEnum.IsDefined<RelicType>(self)
                ? (RelicType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_RELIC);
        }

        public static SquareEventType ToSquareEventType(this int self)
        {
            return FastEnum.IsDefined<SquareEventType>(self)
                ? (SquareEventType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT);
        }

        public static ScoreRateType ToScoreRateType(this int self)
        {
            return FastEnum.IsDefined<ScoreRateType>(self)
                ? (ScoreRateType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_SCORE_RATE);
        }

        public static bool IsBeltEvent(this SquareEventType self)
        {
            return self is SquareEventType.BeltUp or
                SquareEventType.BeltDown or
                SquareEventType.BeltLeft or
                SquareEventType.BeltRight;
        }

        public static bool IsOverrideEmptyEvent(this SquareEventType self)
        {
            return self is SquareEventType.Gem;
        }

        public static ChessmenMovementOffsetVO ToBeltOffset(this SquareEventType self)
        {
            return self switch
            {
                SquareEventType.BeltUp => MovementOffsetConfig.UP,
                SquareEventType.BeltDown => MovementOffsetConfig.DOWN,
                SquareEventType.BeltLeft => MovementOffsetConfig.LEFT,
                SquareEventType.BeltRight => MovementOffsetConfig.RIGHT,
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT),
            };
        }

        public static (int file, int rank) RotateFileRank(this RotateType self, int file, int rank)
        {
            return self switch
            {
                RotateType.Angle0 => (rank, file),
                RotateType.Angle90 => (3 - file, rank),
                RotateType.Angle180 => (3 - rank, 3 - file),
                RotateType.Angle270 => (file, 3 - rank),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_ROTATE),
            };
        }

        public static SquareEventType RotateBelt(this RotateType self, SquareEventType type)
        {
            if (!type.IsBeltEvent()) return type;

            var t = type.ToInt32();
            for (int i = 0; i < self.ToInt32() - 1; i++)
            {
                t.RepeatIncrement(SquareEventType.BeltRight.ToInt32(), SquareEventType.BeltUp.ToInt32());
            }

            return t.ToSquareEventType();
        }

        public static string ToURL(this GameModalType self)
        {
            return self switch
            {
                GameModalType.Policy => UrlConfig.URL_POLICY,
                GameModalType.License => UrlConfig.URL_LICENSE,
                GameModalType.Credit => UrlConfig.URL_CREDIT,
                _ => throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_WEBVIEW),
            };
        }

        public static AchievementType ToAchievementType(this int self)
        {
            return FastEnum.IsDefined<AchievementType>(self)
                ? (AchievementType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_ACHIEVEMENT);
        }

        public static AchievementRankType ToAchievementRankType(this int self)
        {
            return FastEnum.IsDefined<AchievementRankType>(self)
                ? (AchievementRankType)self
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_ACHIEVEMENT_RANK);
        }

        public static AchievementEffectVO ToAchievementEffect(this AchievementRankType self)
        {
            return self switch
            {
                AchievementRankType.Normal => AchievementConfig.NORMAL,
                AchievementRankType.Gold => AchievementConfig.GOLD,
                AchievementRankType.Silver => AchievementConfig.SILVER,
                AchievementRankType.Bronze => AchievementConfig.BRONZE,
                AchievementRankType.Platinum => AchievementConfig.PLATINUM,
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_ACHIEVEMENT_RANK),
            };
        }

        public static SeType ToSeType(this RenderType self)
        {
            return self switch
            {
                RenderType.RetainUp => SeType.GetItem,
                RenderType.RetainDown => SeType.Needle,
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_RENDER),
            };
        }
    }
}