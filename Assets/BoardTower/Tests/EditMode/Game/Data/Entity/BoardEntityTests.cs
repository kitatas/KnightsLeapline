using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class BoardEntityTests
    {
        private BoardEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new BoardEntity();
        }

        [Test]
        public void Add_EventSquare_CanBeFoundAfterAdding()
        {
            var square = new SquareVO(3, 4);
            var squareEvent = new SquareEventVO(SquareEventType.Gem, null);
            var eventSquare = new EventSquareVO(square, squareEvent);

            _entity.Add(eventSquare);

            var (found, _) = _entity.FindEvent(square);
            Assert.That(found, Is.Not.Null);
        }

        [Test]
        public void Add_StoresEventType()
        {
            var square = new SquareVO(3, 4);
            var squareEvent = new SquareEventVO(SquareEventType.Gem, null);
            var eventSquare = new EventSquareVO(square, squareEvent);

            _entity.Add(eventSquare);

            var (found, _) = _entity.FindEvent(square);
            Assert.That(found.type, Is.EqualTo(SquareEventType.Gem));
        }

        [Test]
        public void Clear_RemovesAllSquares()
        {
            _entity.Add(EventSquareVO.Create(1, 1, new SquareEventVO(SquareEventType.Gem, null)));
            _entity.Add(EventSquareVO.Create(2, 2, new SquareEventVO(SquareEventType.Ply, null)));

            _entity.Clear();

            var (found, index) = _entity.FindEvent(new SquareVO(1, 1));
            Assert.That(found, Is.Null);
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void FindEvent_ForNonExistentSquare_ReturnsNullAndMinusOne()
        {
            var (found, index) = _entity.FindEvent(new SquareVO(5, 5));

            Assert.That(found, Is.Null);
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void FindEvent_ForExistingSquare_ReturnsCorrectIndexAndEvent()
        {
            var square = new SquareVO(2, 3);
            var squareEvent = new SquareEventVO(SquareEventType.Ply, null);
            _entity.Add(EventSquareVO.Create(1, 1, new SquareEventVO(SquareEventType.Empty, null)));
            _entity.Add(new EventSquareVO(square, squareEvent));

            var (found, index) = _entity.FindEvent(square);

            Assert.That(found.type, Is.EqualTo(SquareEventType.Ply));
            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public void Set_UpdatesExistingSquareEvent()
        {
            var square = new SquareVO(1, 1);
            _entity.Add(EventSquareVO.Create(1, 1, new SquareEventVO(SquareEventType.Empty, null)));
            var newEvent = new SquareEventVO(SquareEventType.Gem, null);

            _entity.Set(0, newEvent);

            var (found, _) = _entity.FindEvent(square);
            Assert.That(found.type, Is.EqualTo(SquareEventType.Gem));
        }

        [Test]
        public void Set_WithInvalidIndex_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _entity.Set(0, new SquareEventVO(SquareEventType.Gem, null)),
                Throws.TypeOf<QuitExceptionVO>());
        }

        [Test]
        public void GetRenderEventSquare_WithRefreshType_ReturnsCorrectRenderType()
        {
            _entity.Add(EventSquareVO.Create(1, 1, new SquareEventVO(SquareEventType.Empty, null)));

            var result = _entity.GetRenderEventSquare(RenderType.Refresh);

            Assert.That(result.render, Is.EqualTo(RenderType.Refresh));
        }

        [Test]
        public void GetRenderEventSquare_ReturnsAllAddedSquares()
        {
            _entity.Add(EventSquareVO.Create(1, 1, new SquareEventVO(SquareEventType.Gem, null)));
            _entity.Add(EventSquareVO.Create(2, 2, new SquareEventVO(SquareEventType.Ply, null)));

            var result = _entity.GetRenderEventSquare(RenderType.RetainUp);

            Assert.That(result.eventSquares.Length, Is.EqualTo(2));
        }

        [Test]
        public void IsMovable_WhenSquareNotInBoard_ReturnsTrue()
        {
            var result = _entity.IsMovable(new SquareVO(5, 5), false);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsMovable_WhenSquareIsEmpty_ReturnsTrue()
        {
            _entity.Add(EventSquareVO.Create(3, 3, new SquareEventVO(SquareEventType.Empty, null)));

            var result = _entity.IsMovable(new SquareVO(3, 3), false);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsMovable_WhenSquareIsBlock_AndCannotMoveToBlock_ReturnsFalse()
        {
            _entity.Add(EventSquareVO.Create(4, 4, new SquareEventVO(SquareEventType.Block, null)));

            var result = _entity.IsMovable(new SquareVO(4, 4), false);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsMovable_WhenSquareIsBlock_AndCanMoveToBlock_ReturnsTrue()
        {
            _entity.Add(EventSquareVO.Create(4, 4, new SquareEventVO(SquareEventType.Block, null)));

            var result = _entity.IsMovable(new SquareVO(4, 4), true);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsMovable_WhenSquareIsGem_ReturnsTrue()
        {
            _entity.Add(EventSquareVO.Create(2, 2, new SquareEventVO(SquareEventType.Gem, null)));

            var result = _entity.IsMovable(new SquareVO(2, 2), false);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetRenderEventSquare_WithNoneType_ThrowsQuitExceptionVO()
        {
            Assert.That(() => _entity.GetRenderEventSquare(RenderType.None),
                Throws.TypeOf<QuitExceptionVO>());
        }
    }
}
