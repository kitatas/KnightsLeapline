using System.Collections.Generic;
using System.Linq;
using BoardTower.Common.Application;
using UniEx;
using UnityEngine;

namespace BoardTower.Game.Application
{
    public sealed class BoardTransitionVO
    {
        public readonly TransitionVO transition;

        public BoardTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static BoardTransitionVO Create(Fade fade)
        {
            var transition = new TransitionVO(fade, BoardConfig.FADE_DURATION);
            return new BoardTransitionVO(transition);
        }
    }

    public sealed class ChessmenTransitionVO
    {
        public readonly TransitionVO transition;
        public readonly SquareVO square;

        public ChessmenTransitionVO(TransitionVO transition, SquareVO square)
        {
            this.transition = transition;
            this.square = square;
        }

        public static ChessmenTransitionVO Create(Fade fade, SquareVO square)
        {
            var transition = new TransitionVO(fade, ChessmenConfig.FADE_DURATION);
            return new ChessmenTransitionVO(transition, square);
        }
    }

    public sealed class ChessmenVO
    {
        public readonly ChessmenType type;
        public readonly SquareVO square;

        public ChessmenVO(ChessmenType type, SquareVO square)
        {
            this.type = type;
            this.square = square;
        }

        public ChessmenVO(ChessmenVO chessmen, SquareVO square)
        {
            this.type = chessmen.type;
            this.square = square;
        }
    }

    public sealed class ChessmenMovementRuleVO
    {
        public readonly ChessmenType type;
        public readonly ChessmenMovementType movement;
        public readonly ChessmenMovementOffsetVO[] offsets;

        public ChessmenMovementRuleVO(int type, int movement, (int dx, int dy)[] offsets)
        {
            var chessmenType = type.ToChessmenType();
            if (chessmenType is ChessmenType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_CHESSMEN);

            var movementType = movement.ToChessmenMovementType();
            if (movementType is ChessmenMovementType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_CHESSMEN_MOVEMENT);

            this.type = chessmenType;
            this.movement = movementType;
            this.offsets = offsets
                .Select(x => new ChessmenMovementOffsetVO(x.dx, x.dy))
                .ToArray();
        }
    }

    public sealed class ChessmenMovementOffsetVO
    {
        public readonly int dx;
        public readonly int dy;

        public ChessmenMovementOffsetVO(int dx, int dy)
        {
            this.dx = dx;
            this.dy = dy;
        }
    }

    public sealed class SquareVO
    {
        public readonly int file;
        public readonly int rank;

        public SquareVO(int file, int rank)
        {
            this.file = file;
            this.rank = rank;
        }

        public float localX => file - BoardConfig.MIN_FILE - 3.5f;
        public float localZ => rank - BoardConfig.MIN_RANK - 3.5f;

        public bool IsEqual(SquareVO square) => file == square.file && rank == square.rank;
    }

    public sealed class HighlightSquareVO
    {
        public readonly SquareVO square;
        public readonly HighlightSquareType highlight;

        public HighlightSquareVO(SquareVO square, HighlightSquareType highlight)
        {
            if (highlight is HighlightSquareType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_HIGHLIGHT);

            this.square = square;
            this.highlight = highlight;
        }
    }

    public sealed class HighlightIndexVO
    {
        public readonly int index;
        public readonly HighlightSquareType highlight;

        public HighlightIndexVO(int index, HighlightSquareType highlight)
        {
            if (highlight is HighlightSquareType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_HIGHLIGHT);

            this.index = index;
            this.highlight = highlight;
        }
    }

    public sealed class ClickSquareVO
    {
        public readonly SquareVO square;

        public ClickSquareVO(int file, int rank)
        {
            this.square = new SquareVO(file, rank);
        }
    }

    public sealed class EventSquareVO
    {
        public readonly SquareVO square;
        public readonly SquareEventVO squareEvent;

        public EventSquareVO(SquareVO square, SquareEventVO squareEvent)
        {
            this.square = square;
            this.squareEvent = squareEvent;
        }

        public static EventSquareVO Create(int file, int rank, SquareEventVO squareEvent)
        {
            var square = new SquareVO(file, rank);
            return new EventSquareVO(square, squareEvent);
        }
    }

    public sealed class RenderEventSquareVO
    {
        public readonly RenderType render;
        public readonly EventSquareVO[] eventSquares;

        public RenderEventSquareVO(RenderType render, EventSquareVO[] eventSquares)
        {
            if (render is RenderType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_RENDER);

            this.render = render;
            this.eventSquares = eventSquares;
        }
    }

    public sealed class ChessmenMovementVO
    {
        public readonly SquareVO square;

        public ChessmenMovementVO(SquareVO square)
        {
            this.square = square;
        }
    }

    public sealed class SquareEventVO
    {
        public readonly SquareEventType type;
        public readonly GameObject eventObject;

        public SquareEventVO(SquareEventType type, GameObject eventObject)
        {
            if (type is SquareEventType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT);

            this.type = type;
            this.eventObject = eventObject;
        }
    }

    public sealed class BoardPatternVO
    {
        public readonly RotateType rotate;
        public readonly SquareEventType[] types;

        public BoardPatternVO(int[] types)
        {
            if (types.Length != 16)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_PATTERN_LENGTH);

            this.rotate = BoardConfig.ROTATES.GetRandom();

            this.types = new SquareEventType[BoardConfig.HALF_FILE * BoardConfig.HALF_RANK];
            for (int file = 0; file < BoardConfig.HALF_FILE; file++)
            {
                for (int rank = 0; rank < BoardConfig.HALF_RANK; rank++)
                {
                    var (nx, ny) = rotate.RotateFileRank(file, rank);
                    var srcIndex = file * BoardConfig.HALF_FILE + rank;
                    var dstIndex = ny * BoardConfig.HALF_RANK + nx;
                    this.types[dstIndex] = rotate.RotateBelt(types[srcIndex].ToSquareEventType());
                }
            }
        }
    }

    public sealed class EventResultVO
    {
        public readonly SquareEventType type;
        public readonly bool isBelt;
        public readonly int gemNum;
        public readonly int plyNum;

        public EventResultVO(SquareEventType type, bool isBelt, int gemNum, int plyNum)
        {
            if (type is SquareEventType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SQUARE_EVENT);

            this.type = type;
            this.isBelt = isBelt;
            this.gemNum = gemNum;
            this.plyNum = plyNum;
        }

        public static EventResultVO Create(SquareEventType type, PickRelicVO pickRelic)
        {
            var types = pickRelic.relics.ToArray();
            var isBelt = !types.Any(x => x.type is RelicType.Anchor) && type.IsBeltEvent();
            var isBlockBelt = !types.Any(x => x.type is RelicType.Boots) && type is SquareEventType.Block;

            var additionGemRelic = types.FirstOrDefault(x => x.type is RelicType.Charm);
            var additionGem = Random.Range(0, 100) < additionGemRelic?.probability ? additionGemRelic.value : 0;

            var additionHeartRelic = types.FirstOrDefault(x => x.type is RelicType.Chalice);
            var additionHeart = Random.Range(0, 100) < additionHeartRelic?.probability ? additionHeartRelic.value : 0;

            return new EventResultVO(
                type,
                isBelt || isBlockBelt,
                type is SquareEventType.Gem ? 1 + additionGem : 0,
                type is SquareEventType.Ply ? 1 + additionHeart : 0
            );
        }
    }

    public sealed class RoundVO
    {
        public readonly int round;
        public readonly int plyCount;
        public readonly int gemCount;

        public RoundVO(int round, int plyCount, int gemCount)
        {
            this.round = round;
            this.plyCount = plyCount;
            this.gemCount = gemCount;
        }
    }

    public sealed class FinishTransitionVO
    {
        public readonly FinishType type;
        public readonly TransitionVO transition;

        public FinishTransitionVO(FinishType type, TransitionVO transition)
        {
            if (type is FinishType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_FINISH);

            this.type = type;
            this.transition = transition;
        }

        public static FinishTransitionVO Create(FinishType type, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new FinishTransitionVO(type, transition);
        }
    }

    public sealed class TapScreenTransitionVO
    {
        public readonly TransitionVO transition;

        public TapScreenTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static TapScreenTransitionVO Create(Fade fade)
        {
            var transition = new TransitionVO(fade, TapScreenConfig.FADE_DURATION);
            return new TapScreenTransitionVO(transition);
        }
    }

    public sealed class HudRootTransitionVO
    {
        public readonly TransitionVO transition;

        public HudRootTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static HudRootTransitionVO Create(Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new HudRootTransitionVO(transition);
        }
    }

    public sealed class GameModalVO : BaseModalVO<GameModalType>
    {
        public GameModalVO(GameModalType type, Fade fade) : base(type, fade)
        {
            if (type is GameModalType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_GAME_MODAL);
        }
    }

    public sealed class GameModalTransitionVO : BaseModalTransitionVO<GameModalType>
    {
        public GameModalTransitionVO(GameModalType type, TransitionVO transition) : base(type, transition)
        {
            if (type is GameModalType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_GAME_MODAL);
        }

        public static GameModalTransitionVO Create(GameModalType type, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new GameModalTransitionVO(type, transition);
        }

        public static GameModalTransitionVO Create(GameModalVO gameModal, float duration)
        {
            var transition = new TransitionVO(gameModal.fade, duration);
            return new GameModalTransitionVO(gameModal.type, transition);
        }
    }

    public sealed class RelicVO
    {
        public readonly RelicType type;
        public readonly string relicName;
        public readonly string content;
        public readonly int probability;
        public readonly int value;
        public readonly bool isUniq;

        public RelicVO(int type, string relicName, string content, int probability, int value, bool isUniq)
        {
            var t = type.ToRelicType();
            if (t is RelicType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_RELIC);

            this.type = t;
            this.relicName = relicName;
            this.content = content;
            this.probability = probability;
            this.value = value;
            this.isUniq = isUniq;
        }
    }

    public sealed class LotRelicVO
    {
        public readonly IEnumerable<RelicVO> relics;

        public LotRelicVO(IEnumerable<RelicVO> relics)
        {
            this.relics = relics;
        }
    }

    public sealed class LotRelicTransitionVO
    {
        public readonly TransitionVO transition;

        public LotRelicTransitionVO(TransitionVO transition)
        {
            this.transition = transition;
        }

        public static LotRelicTransitionVO Create(Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);
            return new LotRelicTransitionVO(transition);
        }
    }

    public sealed class PickRelicVO
    {
        public readonly IEnumerable<RelicVO> relics;

        public PickRelicVO(IEnumerable<RelicVO> relics)
        {
            this.relics = relics;
        }

        public static PickRelicVO Empty()
        {
            return new PickRelicVO(Enumerable.Empty<RelicVO>());
        }
    }

    public sealed class SelectRelicVO
    {
        public readonly int index;
        public readonly Vector3 position;

        public SelectRelicVO(int index, Vector3 position)
        {
            this.index = index;
            this.position = position;
        }
    }

    public sealed class ScoreRateVO
    {
        public readonly ScoreRateType type;
        public readonly int threshold;
        public readonly float value;

        public ScoreRateVO(int type, int threshold, float value)
        {
            var t = type.ToScoreRateType();
            if (t is ScoreRateType.None)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_SCORE_RATE);

            this.type = t;
            this.threshold = threshold;
            this.value = value;
        }
    }

    public sealed class AchievementVO
    {
        public readonly AchievementType type;
        public readonly AchievementRankType rank;
        public readonly int value;

        public AchievementVO(int type, int rank, int value)
        {
            this.type = type.ToAchievementType();
            this.rank = rank.ToAchievementRankType();
            this.value = value;
        }
    }

    public sealed class AchievementEffectVO
    {
        public readonly Color gradationColor1;
        public readonly Color gradationColor2;
        public readonly Color gradationColor3;
        public readonly Color gradationColor4;
        public readonly Color edgeColor;
        public readonly Color shadowColor;
        public readonly float gradationIntensity;
        public readonly float edgeWidth;
        public readonly float edgeShinyWidth;
        public readonly bool edgeShiny;
        public readonly bool transitionShiny;

        public AchievementEffectVO(Color gradationColor1, Color gradationColor2, Color gradationColor3,
            Color gradationColor4, Color edgeColor, Color shadowColor, float gradationIntensity, float edgeWidth,
            float edgeShinyWidth, bool edgeShiny, bool transitionShiny)
        {
            this.gradationColor1 = gradationColor1;
            this.gradationColor2 = gradationColor2;
            this.gradationColor3 = gradationColor3;
            this.gradationColor4 = gradationColor4;
            this.edgeColor = edgeColor;
            this.shadowColor = shadowColor;
            this.gradationIntensity = gradationIntensity;
            this.edgeWidth = edgeWidth;
            this.edgeShinyWidth = edgeShinyWidth;
            this.edgeShiny = edgeShiny;
            this.transitionShiny = transitionShiny;
        }
    }

    public sealed class AchievementContentVO
    {
        public readonly AchievementVO achievement;
        public readonly bool isAchieve;
        public readonly string content;

        public AchievementContentVO(AchievementVO achievement, bool isAchieve, string content)
        {
            this.achievement = achievement;
            this.isAchieve = isAchieve;
            this.content = content;
        }
    }

    public sealed class ScoreRankingVO
    {
        public readonly IEnumerable<ScoreRankingEntryVO> entries;

        public ScoreRankingVO(IEnumerable<ScoreRankingEntryVO> entries)
        {
            this.entries = entries;
        }
    }

    public sealed class ScoreRankingEntryVO
    {
        public readonly int rank;
        public readonly string displayName;
        public readonly string score;
        public readonly bool isSelf;

        public ScoreRankingEntryVO(int rank, string displayName, string score, bool isSelf)
        {
            this.rank = rank;
            this.displayName = displayName;
            this.score = score;
            this.isSelf = isSelf;
        }
    }

    public sealed class EventLogVO
    {
        public readonly EventLogType type;
        public readonly int value;

        public EventLogVO(EventLogType type, int value)
        {
            this.type = type;
            this.value = value;
        }
    }

    public sealed class EventLogMessageVO
    {
        public readonly string message;
        public readonly float duration;

        public EventLogMessageVO(string message, float duration)
        {
            this.message = message;
            this.duration = duration;
        }
    }
}