using System.Linq;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using FastEnumUtility;
using NUnit.Framework;
using UnityEngine;

namespace BoardTower.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class ValueObjectTests
    {

        // ---- BoardTransitionVO ----

        [Test]
        public void BoardTransitionVO_Constructor_AssignsTransition()
        {
            var transition = new TransitionVO(Fade.Out, BoardConfig.FADE_DURATION);

            var sut = new BoardTransitionVO(transition);

            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public void BoardTransitionVO_Create_SetsCorrectFade(Fade fade)
        {
            var sut = BoardTransitionVO.Create(fade);

            Assert.That(sut.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public void BoardTransitionVO_Create_SetsFadeDurationFromConfig()
        {
            var sut = BoardTransitionVO.Create(Fade.Out);

            Assert.That(sut.transition.duration, Is.EqualTo(BoardConfig.FADE_DURATION));
        }

        // ---- ChessmenTransitionVO ----

        [Test]
        public void ChessmenTransitionVO_Constructor_AssignsTransitionAndSquare()
        {
            var transition = new TransitionVO(Fade.Out, ChessmenConfig.FADE_DURATION);
            var square = new SquareVO(1, 1);

            var sut = new ChessmenTransitionVO(transition, square);

            Assert.That(sut.transition, Is.EqualTo(transition));
            Assert.That(sut.square, Is.EqualTo(square));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public void ChessmenTransitionVO_Create_SetsCorrectFade(Fade fade)
        {
            var square = new SquareVO(3, 5);

            var sut = ChessmenTransitionVO.Create(fade, square);

            Assert.That(sut.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public void ChessmenTransitionVO_Create_SetsFadeDurationFromConfig()
        {
            var square = new SquareVO(1, 1);

            var sut = ChessmenTransitionVO.Create(Fade.Out, square);

            Assert.That(sut.transition.duration, Is.EqualTo(ChessmenConfig.FADE_DURATION));
        }

        [Test]
        public void ChessmenTransitionVO_Create_AssignsSquare()
        {
            var square = new SquareVO(4, 7);

            var sut = ChessmenTransitionVO.Create(Fade.In, square);

            Assert.That(sut.square, Is.EqualTo(square));
        }

        // ---- ChessmenVO ----

        [Test]
        public void ChessmenVO_Constructor_WithTypeAndSquare_AssignsValues()
        {
            var square = new SquareVO(2, 3);

            var sut = new ChessmenVO(ChessmenType.Knight, square);

            Assert.That(sut.type, Is.EqualTo(ChessmenType.Knight));
            Assert.That(sut.square, Is.EqualTo(square));
        }

        [Test]
        public void ChessmenVO_Constructor_WithChessmenAndSquare_InheritsType()
        {
            var original = new ChessmenVO(ChessmenType.Queen, new SquareVO(1, 1));
            var newSquare = new SquareVO(5, 5);

            var sut = new ChessmenVO(original, newSquare);

            Assert.That(sut.type, Is.EqualTo(ChessmenType.Queen));
            Assert.That(sut.square, Is.EqualTo(newSquare));
        }

        [TestCase(ChessmenType.King)]
        [TestCase(ChessmenType.Queen)]
        [TestCase(ChessmenType.Rook)]
        [TestCase(ChessmenType.Bishop)]
        [TestCase(ChessmenType.Knight)]
        public void ChessmenVO_Constructor_WithAllTypes_SetsType(ChessmenType type)
        {
            var square = new SquareVO(1, 1);

            var sut = new ChessmenVO(type, square);

            Assert.That(sut.type, Is.EqualTo(type));
        }

        // ---- ChessmenMovementRuleVO ----

        [Test]
        public void ChessmenMovementRuleVO_Constructor_WithNoneChessmenType_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new ChessmenMovementRuleVO(ChessmenType.None.ToInt32(), ChessmenMovementType.Leaper.ToInt32(), new[] { (0, 1) }),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void ChessmenMovementRuleVO_Constructor_WithNoneMovementType_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new ChessmenMovementRuleVO(ChessmenType.Knight.ToInt32(), ChessmenMovementType.None.ToInt32(), new[] { (0, 1) }),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void ChessmenMovementRuleVO_Constructor_WithValidParams_AssignsType()
        {
            var sut = new ChessmenMovementRuleVO(
                ChessmenType.Knight.ToInt32(),
                ChessmenMovementType.Leaper.ToInt32(),
                new[] { (1, 2), (-1, 2) });

            Assert.That(sut.type, Is.EqualTo(ChessmenType.Knight));
        }

        [Test]
        public void ChessmenMovementRuleVO_Constructor_WithValidParams_AssignsMovement()
        {
            var sut = new ChessmenMovementRuleVO(
                ChessmenType.Knight.ToInt32(),
                ChessmenMovementType.Leaper.ToInt32(),
                new[] { (1, 2) });

            Assert.That(sut.movement, Is.EqualTo(ChessmenMovementType.Leaper));
        }

        [Test]
        public void ChessmenMovementRuleVO_Constructor_WithValidParams_AssignsOffsets()
        {
            var sut = new ChessmenMovementRuleVO(
                ChessmenType.King.ToInt32(),
                ChessmenMovementType.Slider.ToInt32(),
                new[] { (1, 0), (0, 1) });

            Assert.That(sut.offsets.Length, Is.EqualTo(2));
            Assert.That(sut.offsets[0].dx, Is.EqualTo(1));
            Assert.That(sut.offsets[0].dy, Is.EqualTo(0));
        }

        // ---- ChessmenMovementOffsetVO ----

        [TestCase(0, 0)]
        [TestCase(1, 2)]
        [TestCase(-1, -2)]
        public void ChessmenMovementOffsetVO_Constructor_AssignsDxDy(int dx, int dy)
        {
            var sut = new ChessmenMovementOffsetVO(dx, dy);

            Assert.That(sut.dx, Is.EqualTo(dx));
            Assert.That(sut.dy, Is.EqualTo(dy));
        }

        // ---- SquareVO ----

        [TestCase(1, 1)]
        [TestCase(4, 4)]
        [TestCase(8, 8)]
        public void SquareVO_Constructor_AssignsFileAndRank(int file, int rank)
        {
            var sut = new SquareVO(file, rank);

            Assert.That(sut.file, Is.EqualTo(file));
            Assert.That(sut.rank, Is.EqualTo(rank));
        }

        [Test]
        public void SquareVO_LocalX_CalculatesCorrectly()
        {
            // file=1: 1 - BoardConfig.MIN_FILE(1) - 3.5f = -3.5f
            var sut = new SquareVO(1, 1);

            Assert.That(sut.localX, Is.EqualTo(1 - BoardConfig.MIN_FILE - 3.5f));
        }

        [Test]
        public void SquareVO_LocalZ_CalculatesCorrectly()
        {
            // rank=1: 1 - BoardConfig.MIN_RANK(1) - 3.5f = -3.5f
            var sut = new SquareVO(1, 1);

            Assert.That(sut.localZ, Is.EqualTo(1 - BoardConfig.MIN_RANK - 3.5f));
        }

        [Test]
        public void SquareVO_IsEqual_WithSameFileAndRank_ReturnsTrue()
        {
            var a = new SquareVO(3, 5);
            var b = new SquareVO(3, 5);

            Assert.That(a.IsEqual(b), Is.True);
        }

        [Test]
        public void SquareVO_IsEqual_WithDifferentFile_ReturnsFalse()
        {
            var a = new SquareVO(3, 5);
            var b = new SquareVO(4, 5);

            Assert.That(a.IsEqual(b), Is.False);
        }

        [Test]
        public void SquareVO_IsEqual_WithDifferentRank_ReturnsFalse()
        {
            var a = new SquareVO(3, 5);
            var b = new SquareVO(3, 6);

            Assert.That(a.IsEqual(b), Is.False);
        }

        // ---- HighlightSquareVO ----

        [Test]
        public void HighlightSquareVO_Constructor_WithNoneHighlight_ThrowsQuitExceptionVO()
        {
            var square = new SquareVO(1, 1);

            Assert.That(
                () => new HighlightSquareVO(square, HighlightSquareType.None),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(HighlightSquareType.Movable)]
        [TestCase(HighlightSquareType.Default)]
        public void HighlightSquareVO_Constructor_WithValidHighlight_AssignsValues(HighlightSquareType highlight)
        {
            var square = new SquareVO(2, 3);

            var sut = new HighlightSquareVO(square, highlight);

            Assert.That(sut.square, Is.EqualTo(square));
            Assert.That(sut.highlight, Is.EqualTo(highlight));
        }

        // ---- HighlightIndexVO ----

        [Test]
        public void HighlightIndexVO_Constructor_WithNoneHighlight_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new HighlightIndexVO(0, HighlightSquareType.None),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(0, HighlightSquareType.Movable)]
        [TestCase(5, HighlightSquareType.Default)]
        [TestCase(63, HighlightSquareType.Movable)]
        public void HighlightIndexVO_Constructor_WithValidParams_AssignsValues(int index, HighlightSquareType highlight)
        {
            var sut = new HighlightIndexVO(index, highlight);

            Assert.That(sut.index, Is.EqualTo(index));
            Assert.That(sut.highlight, Is.EqualTo(highlight));
        }

        // ---- ClickSquareVO ----

        [TestCase(1, 1)]
        [TestCase(4, 7)]
        [TestCase(8, 8)]
        public void ClickSquareVO_Constructor_CreatesInternalSquare(int file, int rank)
        {
            var sut = new ClickSquareVO(file, rank);

            Assert.That(sut.square.file, Is.EqualTo(file));
            Assert.That(sut.square.rank, Is.EqualTo(rank));
        }

        // ---- EventSquareVO ----

        [Test]
        public void EventSquareVO_Constructor_AssignsSquareAndSquareEvent()
        {
            var square = new SquareVO(3, 4);
            var squareEvent = new SquareEventVO(SquareEventType.Gem, null);

            var sut = new EventSquareVO(square, squareEvent);

            Assert.That(sut.square, Is.EqualTo(square));
            Assert.That(sut.squareEvent, Is.EqualTo(squareEvent));
        }

        [TestCase(1, 1)]
        [TestCase(4, 4)]
        [TestCase(8, 8)]
        public void EventSquareVO_Create_AssignsCorrectFileAndRank(int file, int rank)
        {
            var squareEvent = new SquareEventVO(SquareEventType.Empty, null);

            var sut = EventSquareVO.Create(file, rank, squareEvent);

            Assert.That(sut.square.file, Is.EqualTo(file));
            Assert.That(sut.square.rank, Is.EqualTo(rank));
        }

        [Test]
        public void EventSquareVO_Create_AssignsSquareEvent()
        {
            var squareEvent = new SquareEventVO(SquareEventType.Ply, null);

            var sut = EventSquareVO.Create(1, 1, squareEvent);

            Assert.That(sut.squareEvent, Is.EqualTo(squareEvent));
        }

        // ---- RenderEventSquareVO ----

        [Test]
        public void RenderEventSquareVO_Constructor_WithNoneRender_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new RenderEventSquareVO(RenderType.None, System.Array.Empty<EventSquareVO>()),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(RenderType.Refresh)]
        [TestCase(RenderType.RetainUp)]
        [TestCase(RenderType.RetainDown)]
        public void RenderEventSquareVO_Constructor_WithValidRender_AssignsValues(RenderType render)
        {
            var eventSquares = new[] { EventSquareVO.Create(1, 1, new SquareEventVO(SquareEventType.Empty, null)) };

            var sut = new RenderEventSquareVO(render, eventSquares);

            Assert.That(sut.render, Is.EqualTo(render));
            Assert.That(sut.eventSquares, Is.EqualTo(eventSquares));
        }

        // ---- ChessmenMovementVO ----

        [Test]
        public void ChessmenMovementVO_Constructor_AssignsSquare()
        {
            var square = new SquareVO(5, 3);

            var sut = new ChessmenMovementVO(square);

            Assert.That(sut.square, Is.EqualTo(square));
        }

        // ---- SquareEventVO ----

        [Test]
        public void SquareEventVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new SquareEventVO(SquareEventType.None, null),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(SquareEventType.Empty)]
        [TestCase(SquareEventType.Gem)]
        [TestCase(SquareEventType.Ply)]
        [TestCase(SquareEventType.Block)]
        [TestCase(SquareEventType.Collapse)]
        [TestCase(SquareEventType.BeltRight)]
        [TestCase(SquareEventType.BeltDown)]
        [TestCase(SquareEventType.BeltLeft)]
        [TestCase(SquareEventType.BeltUp)]
        public void SquareEventVO_Constructor_WithValidType_AssignsType(SquareEventType type)
        {
            var sut = new SquareEventVO(type, null);

            Assert.That(sut.type, Is.EqualTo(type));
        }

        // ---- BoardPatternVO ----

        [Test]
        public void BoardPatternVO_Constructor_WithNon16ElementArray_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new BoardPatternVO(new int[15]),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void BoardPatternVO_Constructor_WithEmptyArray_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new BoardPatternVO(System.Array.Empty<int>()),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void BoardPatternVO_Constructor_With17ElementArray_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new BoardPatternVO(new int[17]),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void BoardPatternVO_Constructor_With16ElementArray_SetsTypes()
        {
            // all Empty(1)
            var types = Enumerable.Repeat(SquareEventType.Empty.ToInt32(), 16).ToArray();

            var sut = new BoardPatternVO(types);

            Assert.That(sut.types.Length, Is.EqualTo(BoardConfig.HALF_FILE * BoardConfig.HALF_RANK));
        }

        [Test]
        public void BoardPatternVO_Constructor_With16ElementArray_SetsRotate()
        {
            var types = Enumerable.Repeat(SquareEventType.Empty.ToInt32(), 16).ToArray();

            var sut = new BoardPatternVO(types);

            Assert.That(sut.rotate, Is.Not.EqualTo(RotateType.None));
        }

        // ---- EventResultVO ----

        [Test]
        public void EventResultVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new EventResultVO(SquareEventType.None, false, 0, 0),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void EventResultVO_Constructor_WithValidParams_AssignsValues()
        {
            var sut = new EventResultVO(SquareEventType.Gem, false, 1, 0);

            Assert.That(sut.type, Is.EqualTo(SquareEventType.Gem));
            Assert.That(sut.isBelt, Is.False);
            Assert.That(sut.gemNum, Is.EqualTo(1));
            Assert.That(sut.plyNum, Is.EqualTo(0));
        }

        [TestCase(SquareEventType.Empty, false, 0, 0)]
        [TestCase(SquareEventType.Gem, false, 1, 0)]
        [TestCase(SquareEventType.Ply, false, 0, 1)]
        [TestCase(SquareEventType.Block, true, 0, 0)]
        public void EventResultVO_Constructor_WithVariousParams_AssignsCorrectly(
            SquareEventType type, bool isBelt, int gemNum, int plyNum)
        {
            var sut = new EventResultVO(type, isBelt, gemNum, plyNum);

            Assert.That(sut.type, Is.EqualTo(type));
            Assert.That(sut.isBelt, Is.EqualTo(isBelt));
            Assert.That(sut.gemNum, Is.EqualTo(gemNum));
            Assert.That(sut.plyNum, Is.EqualTo(plyNum));
        }

        // ---- RoundVO ----

        [TestCase(1, 10, 3)]
        [TestCase(0, 0, 0)]
        [TestCase(7, 5, 2)]
        public void RoundVO_Constructor_AssignsAllFields(int round, int plyCount, int gemCount)
        {
            var sut = new RoundVO(round, plyCount, gemCount);

            Assert.That(sut.round, Is.EqualTo(round));
            Assert.That(sut.plyCount, Is.EqualTo(plyCount));
            Assert.That(sut.gemCount, Is.EqualTo(gemCount));
        }

        // ---- FinishTransitionVO ----

        [Test]
        public void FinishTransitionVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            var transition = new TransitionVO(Fade.Out, 0.0f);

            Assert.That(
                () => new FinishTransitionVO(FinishType.None, transition),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(FinishType.Clear)]
        [TestCase(FinishType.Fail)]
        public void FinishTransitionVO_Constructor_WithValidType_AssignsValues(FinishType type)
        {
            var transition = new TransitionVO(Fade.Out, 0.25f);

            var sut = new FinishTransitionVO(type, transition);

            Assert.That(sut.type, Is.EqualTo(type));
            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [TestCase(FinishType.Clear, Fade.In, 0.25f)]
        [TestCase(FinishType.Fail, Fade.Out, 0.0f)]
        public void FinishTransitionVO_Create_SetsCorrectValues(FinishType type, Fade fade, float duration)
        {
            var sut = FinishTransitionVO.Create(type, fade, duration);

            Assert.That(sut.type, Is.EqualTo(type));
            Assert.That(sut.transition.fade, Is.EqualTo(fade));
            Assert.That(sut.transition.duration, Is.EqualTo(duration));
        }

        // ---- TapScreenTransitionVO ----

        [Test]
        public void TapScreenTransitionVO_Constructor_AssignsTransition()
        {
            var transition = new TransitionVO(Fade.In, TapScreenConfig.FADE_DURATION);

            var sut = new TapScreenTransitionVO(transition);

            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [TestCase(Fade.In)]
        [TestCase(Fade.Out)]
        [TestCase(Fade.InOut)]
        public void TapScreenTransitionVO_Create_SetsCorrectFade(Fade fade)
        {
            var sut = TapScreenTransitionVO.Create(fade);

            Assert.That(sut.transition.fade, Is.EqualTo(fade));
        }

        [Test]
        public void TapScreenTransitionVO_Create_SetsFadeDurationFromConfig()
        {
            var sut = TapScreenTransitionVO.Create(Fade.Out);

            Assert.That(sut.transition.duration, Is.EqualTo(TapScreenConfig.FADE_DURATION));
        }

        // ---- HudRootTransitionVO ----

        [Test]
        public void HudRootTransitionVO_Constructor_AssignsTransition()
        {
            var transition = new TransitionVO(Fade.Out, HudRootConfig.FADE_DURATION);

            var sut = new HudRootTransitionVO(transition);

            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [TestCase(Fade.In, 0.25f)]
        [TestCase(Fade.Out, 0.0f)]
        [TestCase(Fade.InOut, 0.5f)]
        public void HudRootTransitionVO_Create_SetsCorrectValues(Fade fade, float duration)
        {
            var sut = HudRootTransitionVO.Create(fade, duration);

            Assert.That(sut.transition.fade, Is.EqualTo(fade));
            Assert.That(sut.transition.duration, Is.EqualTo(duration));
        }

        // ---- GameModalVO ----

        [Test]
        public void GameModalVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new GameModalVO(GameModalType.None, Fade.In),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(GameModalType.Menu, Fade.In)]
        [TestCase(GameModalType.Sound, Fade.Out)]
        [TestCase(GameModalType.Achievement, Fade.InOut)]
        public void GameModalVO_Constructor_WithValidParams_AssignsValues(GameModalType type, Fade fade)
        {
            var sut = new GameModalVO(type, fade);

            Assert.That(sut.type, Is.EqualTo(type));
            Assert.That(sut.fade, Is.EqualTo(fade));
        }

        // ---- GameModalTransitionVO ----

        [Test]
        public void GameModalTransitionVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            var transition = new TransitionVO(Fade.Out, GameModalConfig.FADE_DURATION);

            Assert.That(
                () => new GameModalTransitionVO(GameModalType.None, transition),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(GameModalType.Menu, Fade.In, 0.25f)]
        [TestCase(GameModalType.Ranking, Fade.Out, 0.0f)]
        public void GameModalTransitionVO_Constructor_WithValidParams_AssignsValues(
            GameModalType type, Fade fade, float duration)
        {
            var transition = new TransitionVO(fade, duration);

            var sut = new GameModalTransitionVO(type, transition);

            Assert.That(sut.type, Is.EqualTo(type));
            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [TestCase(GameModalType.Menu, Fade.In, 0.25f)]
        [TestCase(GameModalType.Sound, Fade.Out, 0.0f)]
        public void GameModalTransitionVO_Create_WithTypeAndFadeAndDuration_SetsCorrectValues(
            GameModalType type, Fade fade, float duration)
        {
            var sut = GameModalTransitionVO.Create(type, fade, duration);

            Assert.That(sut.type, Is.EqualTo(type));
            Assert.That(sut.transition.fade, Is.EqualTo(fade));
            Assert.That(sut.transition.duration, Is.EqualTo(duration));
        }

        [Test]
        public void GameModalTransitionVO_Create_WithGameModalVO_SetsCorrectValues()
        {
            var gameModal = new GameModalVO(GameModalType.Achievement, Fade.In);

            var sut = GameModalTransitionVO.Create(gameModal, GameModalConfig.FADE_DURATION);

            Assert.That(sut.type, Is.EqualTo(GameModalType.Achievement));
            Assert.That(sut.transition.fade, Is.EqualTo(Fade.In));
            Assert.That(sut.transition.duration, Is.EqualTo(GameModalConfig.FADE_DURATION));
        }

        // ---- RelicVO ----

        [Test]
        public void RelicVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new RelicVO(RelicType.None.ToInt32(), "name", "content", 0, 0, false),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(RelicType.Boots)]
        [TestCase(RelicType.Anchor)]
        [TestCase(RelicType.Charm)]
        [TestCase(RelicType.Medal)]
        public void RelicVO_Constructor_WithValidType_AssignsValues(RelicType type)
        {
            var sut = new RelicVO(type.ToInt32(), "relic", "desc", 30, 2, true);

            Assert.That(sut.type, Is.EqualTo(type));
            Assert.That(sut.relicName, Is.EqualTo("relic"));
            Assert.That(sut.content, Is.EqualTo("desc"));
            Assert.That(sut.probability, Is.EqualTo(30));
            Assert.That(sut.value, Is.EqualTo(2));
            Assert.That(sut.isUniq, Is.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RelicVO_Constructor_WithIsUniqVariants_SetsCorrectly(bool isUniq)
        {
            var sut = new RelicVO(RelicType.Boots.ToInt32(), "name", "content", 0, 0, isUniq);

            Assert.That(sut.isUniq, Is.EqualTo(isUniq));
        }

        // ---- LotRelicVO ----

        [Test]
        public void LotRelicVO_Constructor_AssignsRelics()
        {
            var relics = new[]
            {
                new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true),
                new RelicVO(RelicType.Charm.ToInt32(), "Charm", "desc", 0, 0, false),
            };

            var sut = new LotRelicVO(relics);

            Assert.That(sut.relics, Is.EqualTo(relics));
        }

        [Test]
        public void LotRelicVO_Constructor_WithEmptyRelics_AssignsEmpty()
        {
            var sut = new LotRelicVO(Enumerable.Empty<RelicVO>());

            Assert.That(sut.relics, Is.Empty);
        }

        // ---- LotRelicTransitionVO ----

        [Test]
        public void LotRelicTransitionVO_Constructor_AssignsTransition()
        {
            var transition = new TransitionVO(Fade.Out, RelicConfig.LOT_FADE_DURATION);

            var sut = new LotRelicTransitionVO(transition);

            Assert.That(sut.transition, Is.EqualTo(transition));
        }

        [TestCase(Fade.In, 0.25f)]
        [TestCase(Fade.Out, 0.0f)]
        public void LotRelicTransitionVO_Create_SetsCorrectValues(Fade fade, float duration)
        {
            var sut = LotRelicTransitionVO.Create(fade, duration);

            Assert.That(sut.transition.fade, Is.EqualTo(fade));
            Assert.That(sut.transition.duration, Is.EqualTo(duration));
        }

        // ---- PickRelicVO ----

        [Test]
        public void PickRelicVO_Constructor_AssignsRelics()
        {
            var relics = new[] { new RelicVO(RelicType.Boots.ToInt32(), "Boots", "desc", 0, 0, true) };

            var sut = new PickRelicVO(relics);

            Assert.That(sut.relics, Is.EqualTo(relics));
        }

        [Test]
        public void PickRelicVO_Empty_ReturnsInstanceWithNoRelics()
        {
            var sut = PickRelicVO.Empty();

            Assert.That(sut.relics, Is.Empty);
        }

        // ---- SelectRelicVO ----

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void SelectRelicVO_Constructor_AssignsIndex(int index)
        {
            var sut = new SelectRelicVO(index, Vector3.zero);

            Assert.That(sut.index, Is.EqualTo(index));
        }

        [Test]
        public void SelectRelicVO_Constructor_AssignsPosition()
        {
            var pos = new Vector3(1f, 2f, 3f);

            var sut = new SelectRelicVO(0, pos);

            Assert.That(sut.position, Is.EqualTo(pos));
        }

        // ---- ScoreRateVO ----

        [Test]
        public void ScoreRateVO_Constructor_WithNoneType_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => new ScoreRateVO(ScoreRateType.None.ToInt32(), 0, 1.0f),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [TestCase(ScoreRateType.RoundGem, 1, 1.5f)]
        [TestCase(ScoreRateType.RoundClear, 3, 2.0f)]
        [TestCase(ScoreRateType.GemCombo, 5, 1.2f)]
        public void ScoreRateVO_Constructor_WithValidParams_AssignsValues(
            ScoreRateType type, int threshold, float value)
        {
            var sut = new ScoreRateVO(type.ToInt32(), threshold, value);

            Assert.That(sut.type, Is.EqualTo(type));
            Assert.That(sut.threshold, Is.EqualTo(threshold));
            Assert.That(sut.value, Is.EqualTo(value));
        }

        // ---- ScoreRankingEntryVO ----

        [Test]
        public void ScoreRankingEntryVO_Constructor_AssignsAllFields()
        {
            var sut = new ScoreRankingEntryVO(1, "PlayerA", "9999", true);

            Assert.That(sut.rank, Is.EqualTo(1));
            Assert.That(sut.displayName, Is.EqualTo("PlayerA"));
            Assert.That(sut.score, Is.EqualTo("9999"));
            Assert.That(sut.isSelf, Is.True);
        }

        [TestCase(1, "Alpha", "100", true)]
        [TestCase(10, "Beta", "0", false)]
        [TestCase(100, "Gamma", "-1", false)]
        public void ScoreRankingEntryVO_Constructor_WithVariousValues_AssignsCorrectly(
            int rank, string displayName, string score, bool isSelf)
        {
            var sut = new ScoreRankingEntryVO(rank, displayName, score, isSelf);

            Assert.That(sut.rank, Is.EqualTo(rank));
            Assert.That(sut.displayName, Is.EqualTo(displayName));
            Assert.That(sut.score, Is.EqualTo(score));
            Assert.That(sut.isSelf, Is.EqualTo(isSelf));
        }

        // ---- ScoreRankingVO ----

        [Test]
        public void ScoreRankingVO_Constructor_AssignsEntries()
        {
            var entries = new[]
            {
                new ScoreRankingEntryVO(1, "Player1", "1000", false),
                new ScoreRankingEntryVO(2, "Player2", "500", true),
            };

            var sut = new ScoreRankingVO(entries);

            Assert.That(sut.entries, Is.EqualTo(entries));
        }

        [Test]
        public void ScoreRankingVO_Constructor_WithEmptyEntries_AssignsEmpty()
        {
            var sut = new ScoreRankingVO(Enumerable.Empty<ScoreRankingEntryVO>());

            Assert.That(sut.entries, Is.Empty);
        }

        // ---- AchievementVO ----

        [Test]
        public void AchievementVO_Constructor_AssignsTypeRankAndValue()
        {
            var sut = new AchievementVO(AchievementType.Play.ToInt32(), AchievementRankType.Normal.ToInt32(), 5);

            Assert.That(sut.type, Is.EqualTo(AchievementType.Play));
            Assert.That(sut.rank, Is.EqualTo(AchievementRankType.Normal));
            Assert.That(sut.value, Is.EqualTo(5));
        }

        // ---- ProgressVO ----

        [Test]
        public void ProgressVO_Constructor_AssignsTypeAndValue()
        {
            var sut = new ProgressVO(AchievementType.Score.ToInt32(), 10);

            Assert.That(sut.type, Is.EqualTo(AchievementType.Score.ToInt32()));
            Assert.That(sut.value, Is.EqualTo(10));
        }

        [TestCase(AchievementType.None, 0)]
        [TestCase(AchievementType.Play, 1)]
        [TestCase(AchievementType.Clear, 999)]
        public void ProgressVO_Constructor_WithVariousValues_AssignsCorrectly(AchievementType type, int value)
        {
            var sut = new ProgressVO(type.ToInt32(), value);

            Assert.That(sut.type, Is.EqualTo(type.ToInt32()));
            Assert.That(sut.value, Is.EqualTo(value));
        }

        // ---- AchievementContentVO ----

        [Test]
        public void AchievementContentVO_Constructor_AssignsAchievementIsAchieveAndContent()
        {
            var achievement = new AchievementVO(AchievementType.Play.ToInt32(), AchievementRankType.Normal.ToInt32(), 5);

            var sut = new AchievementContentVO(achievement, true, "5回プレイする");

            Assert.That(sut.achievement, Is.EqualTo(achievement));
            Assert.That(sut.isAchieve, Is.True);
            Assert.That(sut.content, Is.EqualTo("5回プレイする"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AchievementContentVO_Constructor_WithIsAchieveVariants_SetsCorrectly(bool isAchieve)
        {
            var achievement = new AchievementVO(AchievementType.Score.ToInt32(), AchievementRankType.Gold.ToInt32(), 100);

            var sut = new AchievementContentVO(achievement, isAchieve, "content");

            Assert.That(sut.isAchieve, Is.EqualTo(isAchieve));
        }

        // AchievementVO は参照型フィールドのため、同一インスタンスが保持されることを検証する
        [Test]
        public void AchievementContentVO_Constructor_KeepsSameAchievementReference()
        {
            var achievement = new AchievementVO(AchievementType.Clear.ToInt32(), AchievementRankType.Platinum.ToInt32(),
                999);

            var sut = new AchievementContentVO(achievement, false, "content");

            Assert.That(sut.achievement, Is.SameAs(achievement));
        }

        // バリデーションを持たない VO のため、空文字・null もそのまま保持される
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void AchievementContentVO_Constructor_WithEmptyOrNullContent_AssignsAsIs(string content)
        {
            var achievement = new AchievementVO(AchievementType.None.ToInt32(), AchievementRankType.None.ToInt32(), 0);

            var sut = new AchievementContentVO(achievement, false, content);

            Assert.That(sut.content, Is.EqualTo(content));
        }

        [Test]
        public void AchievementContentVO_Constructor_WithNullAchievement_AssignsNull()
        {
            var sut = new AchievementContentVO(null, false, "content");

            Assert.That(sut.achievement, Is.Null);
        }

        [TestCase(AchievementType.None, AchievementRankType.None, 0)]
        [TestCase(AchievementType.Play, AchievementRankType.Bronze, 1)]
        [TestCase(AchievementType.Score, AchievementRankType.Silver, 100)]
        [TestCase(AchievementType.Clear, AchievementRankType.Platinum, 999)]
        public void AchievementContentVO_Constructor_WithVariousAchievements_AssignsCorrectly(AchievementType type,
            AchievementRankType rank, int value)
        {
            var achievement = new AchievementVO(type.ToInt32(), rank.ToInt32(), value);

            var sut = new AchievementContentVO(achievement, true, "content");

            Assert.That(sut.achievement.type, Is.EqualTo(type));
            Assert.That(sut.achievement.rank, Is.EqualTo(rank));
            Assert.That(sut.achievement.value, Is.EqualTo(value));
        }

        // ---- EventLogVO ----

        [TestCase(EventLogType.Ride, 1)]
        [TestCase(EventLogType.Combo, 0)]
        [TestCase(EventLogType.Gem, 5)]
        [TestCase(EventLogType.Score, 100)]
        [TestCase(EventLogType.Heart, 3)]
        public void EventLogVO_Constructor_AssignsTypeAndValue(EventLogType type, int value)
        {
            var sut = new EventLogVO(type, value);

            Assert.That(sut.type, Is.EqualTo(type));
            Assert.That(sut.value, Is.EqualTo(value));
        }

        // ---- EventLogMessageVO ----

        [Test]
        public void EventLogMessageVO_Constructor_AssignsMessage()
        {
            var sut = new EventLogMessageVO("test message", 1.5f);

            Assert.That(sut.message, Is.EqualTo("test message"));
        }

        [Test]
        public void EventLogMessageVO_Constructor_AssignsDuration()
        {
            var sut = new EventLogMessageVO("msg", 2.0f);

            Assert.That(sut.duration, Is.EqualTo(2.0f));
        }

        [TestCase("hello", 0.5f)]
        [TestCase("", 0.0f)]
        [TestCase("long message text", 3.0f)]
        public void EventLogMessageVO_Constructor_WithVariousValues_AssignsCorrectly(string message, float duration)
        {
            var sut = new EventLogMessageVO(message, duration);

            Assert.That(sut.message, Is.EqualTo(message));
            Assert.That(sut.duration, Is.EqualTo(duration));
        }
    }
}