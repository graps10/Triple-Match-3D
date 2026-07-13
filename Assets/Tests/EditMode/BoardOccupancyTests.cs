using NUnit.Framework;

namespace TripleMatch.Domain.Tests
{
    public class BoardOccupancyTests
    {
        private BoardOccupancy _occupancy;

        // BoardOccupancy is stateful (Register/Remove mutate it), unlike MatchResolver -
        // [SetUp] guarantees a fresh instance before every [Test], regardless of whether
        // NUnit reuses the same fixture instance across tests in this class.
        [SetUp]
        public void SetUp() => _occupancy = new BoardOccupancy();

        [Test]
        public void IsTopmost_ReturnsFalse_WhenPositionWasNeverRegistered()
        {
            Assert.IsFalse(_occupancy.IsTopmost(0f, 0f, 0));
        }

        [Test]
        public void IsTopmost_ReturnsTrue_ForSingleRegisteredLayer()
        {
            _occupancy.Register(0f, 0f, 0);

            Assert.IsTrue(_occupancy.IsTopmost(0f, 0f, 0));
        }

        [Test]
        public void IsTopmost_OnlyTheHighestLayer_IsTopmost()
        {
            _occupancy.Register(0f, 0f, 0);
            _occupancy.Register(0f, 0f, 1);

            Assert.IsFalse(_occupancy.IsTopmost(0f, 0f, 0));
            Assert.IsTrue(_occupancy.IsTopmost(0f, 0f, 1));
        }

        [Test]
        public void Remove_RevealsTheLayerBelow()
        {
            _occupancy.Register(0f, 0f, 0);
            _occupancy.Register(0f, 0f, 1);

            _occupancy.Remove(0f, 0f, 1);

            Assert.IsTrue(_occupancy.IsTopmost(0f, 0f, 0));
        }

        [Test]
        public void Remove_OfALayerThatIsNotTopmost_DoesNothing()
        {
            _occupancy.Register(0f, 0f, 0);
            _occupancy.Register(0f, 0f, 1);

            // Layer 0 is covered - removing it "out of turn" must not disturb the real top.
            _occupancy.Remove(0f, 0f, 0);

            Assert.IsTrue(_occupancy.IsTopmost(0f, 0f, 1));
            Assert.IsFalse(_occupancy.IsTopmost(0f, 0f, 0));
        }

        [Test]
        public void Positions_AreIndependentOfEachOther()
        {
            _occupancy.Register(0f, 0f, 0);
            _occupancy.Register(1f, 0f, 0);
            _occupancy.Register(1f, 0f, 1);

            Assert.IsTrue(_occupancy.IsTopmost(0f, 0f, 0));
            Assert.IsFalse(_occupancy.IsTopmost(1f, 0f, 0));
            Assert.IsTrue(_occupancy.IsTopmost(1f, 0f, 1));
        }
    }
}
