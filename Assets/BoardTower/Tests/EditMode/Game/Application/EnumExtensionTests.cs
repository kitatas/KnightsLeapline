using System.Collections.Generic;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class EnumExtensionTests
    {
        // ---- ToChessmenType ----

        [TestCase(0, ChessmenType.None)]
        [TestCase(1, ChessmenType.King)]
        [TestCase(5, ChessmenType.Knight)]
        public void ToChessmenType_WithDefinedValue_ReturnsMatchingType(int value, ChessmenType expected)
        {
            Assert.That(value.ToChessmenType(), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(6)]
        [TestCase(99)]
        public void ToChessmenType_WithUndefinedValue_ThrowsQuitExceptionVO(int value)
        {
            Assert.That(
                () => value.ToChessmenType(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.INVALID_CHESSMEN));
        }

        // ---- ToChessmenMovementType ----

        [TestCase(0, ChessmenMovementType.None)]
        [TestCase(1, ChessmenMovementType.Leaper)]
        [TestCase(2, ChessmenMovementType.Slider)]
        public void ToChessmenMovementType_WithDefinedValue_ReturnsMatchingType(int value,
            ChessmenMovementType expected)
        {
            Assert.That(value.ToChessmenMovementType(), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(3)]
        [TestCase(99)]
        public void ToChessmenMovementType_WithUndefinedValue_ThrowsQuitExceptionVO(int value)
        {
            Assert.That(
                () => value.ToChessmenMovementType(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message")
                    .EqualTo(ExceptionConfig.INVALID_CHESSMEN_MOVEMENT));
        }

        // ---- ToRelicType ----

        [TestCase(0, RelicType.None)]
        [TestCase(1, RelicType.Boots)]
        [TestCase(11, RelicType.Medal)]
        public void ToRelicType_WithDefinedValue_ReturnsMatchingType(int value, RelicType expected)
        {
            Assert.That(value.ToRelicType(), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(12)]
        [TestCase(99)]
        public void ToRelicType_WithUndefinedValue_ThrowsQuitExceptionVO(int value)
        {
            Assert.That(
                () => value.ToRelicType(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.INVALID_RELIC));
        }

        // ---- ToSquareEventType ----

        [TestCase(0, SquareEventType.None)]
        [TestCase(2, SquareEventType.BeltRight)]
        [TestCase(9, SquareEventType.Collapse)]
        public void ToSquareEventType_WithDefinedValue_ReturnsMatchingType(int value, SquareEventType expected)
        {
            Assert.That(value.ToSquareEventType(), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(10)]
        [TestCase(99)]
        public void ToSquareEventType_WithUndefinedValue_ThrowsQuitExceptionVO(int value)
        {
            Assert.That(
                () => value.ToSquareEventType(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message")
                    .EqualTo(ExceptionConfig.INVALID_SQUARE_EVENT));
        }

        // ---- ToScoreRateType ----

        [TestCase(0, ScoreRateType.None)]
        [TestCase(1, ScoreRateType.RoundGem)]
        [TestCase(3, ScoreRateType.GemCombo)]
        public void ToScoreRateType_WithDefinedValue_ReturnsMatchingType(int value, ScoreRateType expected)
        {
            Assert.That(value.ToScoreRateType(), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(6)]
        [TestCase(99)]
        public void ToScoreRateType_WithUndefinedValue_ThrowsQuitExceptionVO(int value)
        {
            Assert.That(
                () => value.ToScoreRateType(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.INVALID_SCORE_RATE));
        }

        // ---- ToAchievementType ----

        [TestCase(0, AchievementType.None)]
        [TestCase(1, AchievementType.Play)]
        [TestCase(3, AchievementType.Clear)]
        public void ToAchievementType_WithDefinedValue_ReturnsMatchingType(int value, AchievementType expected)
        {
            Assert.That(value.ToAchievementType(), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(4)]
        [TestCase(99)]
        public void ToAchievementType_WithUndefinedValue_ThrowsQuitExceptionVO(int value)
        {
            Assert.That(
                () => value.ToAchievementType(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.INVALID_ACHIEVEMENT));
        }

        // ---- ToAchievementRankType ----

        [TestCase(0, AchievementRankType.None)]
        [TestCase(1, AchievementRankType.Normal)]
        [TestCase(5, AchievementRankType.Platinum)]
        public void ToAchievementRankType_WithDefinedValue_ReturnsMatchingType(int value,
            AchievementRankType expected)
        {
            Assert.That(value.ToAchievementRankType(), Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(6)]
        [TestCase(99)]
        public void ToAchievementRankType_WithUndefinedValue_ThrowsQuitExceptionVO(int value)
        {
            Assert.That(
                () => value.ToAchievementRankType(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message")
                    .EqualTo(ExceptionConfig.INVALID_ACHIEVEMENT_RANK));
        }

        // ---- IsBeltEvent ----

        [TestCase(SquareEventType.BeltRight, true)]
        [TestCase(SquareEventType.BeltDown, true)]
        [TestCase(SquareEventType.BeltLeft, true)]
        [TestCase(SquareEventType.BeltUp, true)]
        [TestCase(SquareEventType.None, false)]
        [TestCase(SquareEventType.Empty, false)]
        [TestCase(SquareEventType.Block, false)]
        [TestCase(SquareEventType.Gem, false)]
        [TestCase(SquareEventType.Ply, false)]
        [TestCase(SquareEventType.Collapse, false)]
        public void IsBeltEvent_WithType_ReturnsTrueOnlyForBelts(SquareEventType type, bool expected)
        {
            Assert.That(type.IsBeltEvent(), Is.EqualTo(expected));
        }

        // ---- IsOverrideEmptyEvent ----

        [TestCase(SquareEventType.Gem, true)]
        [TestCase(SquareEventType.Empty, false)]
        [TestCase(SquareEventType.Ply, false)]
        [TestCase(SquareEventType.Block, false)]
        [TestCase(SquareEventType.BeltUp, false)]
        [TestCase(SquareEventType.Collapse, false)]
        public void IsOverrideEmptyEvent_WithType_ReturnsTrueOnlyForGem(SquareEventType type, bool expected)
        {
            Assert.That(type.IsOverrideEmptyEvent(), Is.EqualTo(expected));
        }

        // ---- ToBeltOffset ----

        [TestCase(SquareEventType.BeltUp, 0, 1)]
        [TestCase(SquareEventType.BeltDown, 0, -1)]
        [TestCase(SquareEventType.BeltLeft, -1, 0)]
        [TestCase(SquareEventType.BeltRight, 1, 0)]
        public void ToBeltOffset_WithBelt_ReturnsOffsetInBeltDirection(SquareEventType type, int dx, int dy)
        {
            var offset = type.ToBeltOffset();

            Assert.That((offset.dx, offset.dy), Is.EqualTo((dx, dy)));
        }

        [TestCase(SquareEventType.None)]
        [TestCase(SquareEventType.Empty)]
        [TestCase(SquareEventType.Block)]
        [TestCase(SquareEventType.Gem)]
        public void ToBeltOffset_WithNonBelt_ThrowsQuitExceptionVO(SquareEventType type)
        {
            Assert.That(
                () => type.ToBeltOffset(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message")
                    .EqualTo(ExceptionConfig.INVALID_SQUARE_EVENT));
        }

        // ---- RotateFileRank ----

        [TestCase(RotateType.Angle0, 0, 1, 1, 0)]
        [TestCase(RotateType.Angle0, 2, 3, 3, 2)]
        [TestCase(RotateType.Angle90, 0, 1, 3, 1)]
        [TestCase(RotateType.Angle90, 2, 3, 1, 3)]
        [TestCase(RotateType.Angle180, 0, 1, 2, 3)]
        [TestCase(RotateType.Angle180, 2, 3, 0, 1)]
        [TestCase(RotateType.Angle270, 0, 1, 0, 2)]
        [TestCase(RotateType.Angle270, 2, 3, 2, 0)]
        public void RotateFileRank_WithRotate_ReturnsRotatedFileRank(RotateType rotate, int file, int rank,
            int expectedFile, int expectedRank)
        {
            var result = rotate.RotateFileRank(file, rank);

            Assert.That(result, Is.EqualTo((expectedFile, expectedRank)));
        }

        [TestCase(RotateType.Angle0)]
        [TestCase(RotateType.Angle90)]
        [TestCase(RotateType.Angle180)]
        [TestCase(RotateType.Angle270)]
        public void RotateFileRank_WithEveryCellOf4x4_MapsToEveryCellExactlyOnce(RotateType rotate)
        {
            var expected = new List<(int file, int rank)>();
            var actual = new List<(int file, int rank)>();
            for (int file = 0; file < 4; file++)
            {
                for (int rank = 0; rank < 4; rank++)
                {
                    expected.Add((file, rank));
                    actual.Add(rotate.RotateFileRank(file, rank));
                }
            }

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void RotateFileRank_WithNone_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => RotateType.None.RotateFileRank(0, 0),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.INVALID_ROTATE));
        }

        // ---- RotateBelt ----

        [TestCase(RotateType.Angle0, SquareEventType.BeltRight, SquareEventType.BeltRight)]
        [TestCase(RotateType.Angle0, SquareEventType.BeltUp, SquareEventType.BeltUp)]
        [TestCase(RotateType.Angle90, SquareEventType.BeltRight, SquareEventType.BeltDown)]
        [TestCase(RotateType.Angle90, SquareEventType.BeltDown, SquareEventType.BeltLeft)]
        [TestCase(RotateType.Angle90, SquareEventType.BeltLeft, SquareEventType.BeltUp)]
        [TestCase(RotateType.Angle90, SquareEventType.BeltUp, SquareEventType.BeltRight)]
        [TestCase(RotateType.Angle180, SquareEventType.BeltRight, SquareEventType.BeltLeft)]
        [TestCase(RotateType.Angle180, SquareEventType.BeltDown, SquareEventType.BeltUp)]
        [TestCase(RotateType.Angle180, SquareEventType.BeltLeft, SquareEventType.BeltRight)]
        [TestCase(RotateType.Angle180, SquareEventType.BeltUp, SquareEventType.BeltDown)]
        [TestCase(RotateType.Angle270, SquareEventType.BeltRight, SquareEventType.BeltUp)]
        [TestCase(RotateType.Angle270, SquareEventType.BeltDown, SquareEventType.BeltRight)]
        [TestCase(RotateType.Angle270, SquareEventType.BeltLeft, SquareEventType.BeltDown)]
        [TestCase(RotateType.Angle270, SquareEventType.BeltUp, SquareEventType.BeltLeft)]
        public void RotateBelt_WithBelt_ReturnsBeltRotatedClockwise(RotateType rotate, SquareEventType type,
            SquareEventType expected)
        {
            Assert.That(rotate.RotateBelt(type), Is.EqualTo(expected));
        }

        [TestCase(RotateType.Angle90, SquareEventType.Empty)]
        [TestCase(RotateType.Angle180, SquareEventType.Block)]
        [TestCase(RotateType.Angle270, SquareEventType.Gem)]
        [TestCase(RotateType.Angle90, SquareEventType.Ply)]
        [TestCase(RotateType.Angle180, SquareEventType.Collapse)]
        public void RotateBelt_WithNonBelt_ReturnsSameType(RotateType rotate, SquareEventType type)
        {
            Assert.That(rotate.RotateBelt(type), Is.EqualTo(type));
        }

        // ---- ToURL ----

        [TestCase(GameModalType.Policy, UrlConfig.URL_POLICY)]
        [TestCase(GameModalType.License, UrlConfig.URL_LICENSE)]
        [TestCase(GameModalType.Credit, UrlConfig.URL_CREDIT)]
        public void ToURL_WithWebviewModal_ReturnsConfiguredUrl(GameModalType type, string expected)
        {
            Assert.That(type.ToURL(), Is.EqualTo(expected));
        }

        [TestCase(GameModalType.None)]
        [TestCase(GameModalType.Menu)]
        [TestCase(GameModalType.Sound)]
        [TestCase(GameModalType.HowTo)]
        public void ToURL_WithNonWebviewModal_ThrowsQuitExceptionVO(GameModalType type)
        {
            Assert.That(
                () => type.ToURL(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message").EqualTo(ExceptionConfig.NOT_FOUND_WEBVIEW));
        }

        // ---- ToAchievementEffect ----

        private static IEnumerable<TestCaseData> AchievementEffectCases()
        {
            yield return new TestCaseData(AchievementRankType.Normal, AchievementConfig.NORMAL);
            yield return new TestCaseData(AchievementRankType.Bronze, AchievementConfig.BRONZE);
            yield return new TestCaseData(AchievementRankType.Silver, AchievementConfig.SILVER);
            yield return new TestCaseData(AchievementRankType.Gold, AchievementConfig.GOLD);
            yield return new TestCaseData(AchievementRankType.Platinum, AchievementConfig.PLATINUM);
        }

        [TestCaseSource(nameof(AchievementEffectCases))]
        public void ToAchievementEffect_WithRank_ReturnsConfiguredEffectOfThatRank(AchievementRankType rank,
            AchievementEffectVO expected)
        {
            Assert.That(rank.ToAchievementEffect(), Is.SameAs(expected));
        }

        [Test]
        public void ToAchievementEffect_WithNone_ThrowsQuitExceptionVO()
        {
            Assert.That(
                () => AchievementRankType.None.ToAchievementEffect(),
                Throws.TypeOf<QuitExceptionVO>().With.Property("Message")
                    .EqualTo(ExceptionConfig.INVALID_ACHIEVEMENT_RANK));
        }
    }
}
