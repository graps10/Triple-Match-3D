using System.Collections.Generic;
using NUnit.Framework;

namespace TripleMatch.Domain.Tests
{
    public class MatchResolverTests
    {
        private readonly MatchResolver _resolver = new();

        [Test]
        public void Resolve_ReturnsNoMatch_WhenAllSlotsAreDifferent()
        {
            List<ItemType> slots = new() { ItemType.Apple, ItemType.Bread, ItemType.Carrot };

            MatchResult result = _resolver.Resolve(slots);

            Assert.IsFalse(result.Matched);
        }

        [Test]
        public void Resolve_ReturnsNoMatch_WhenFewerThanThreeSlots()
        {
            List<ItemType> slots = new() { ItemType.Apple, ItemType.Apple };

            MatchResult result = _resolver.Resolve(slots);

            Assert.IsFalse(result.Matched);
        }

        [Test]
        public void Resolve_FindsMatch_WhenThreeIdenticalAreAdjacent()
        {
            List<ItemType> slots = new() { ItemType.Apple, ItemType.Apple, ItemType.Apple };

            MatchResult result = _resolver.Resolve(slots);

            Assert.IsTrue(result.Matched);
            Assert.AreEqual(ItemType.Apple, result.Type);
            Assert.AreEqual(0, result.SlotIndex);
            Assert.AreEqual(3, result.Length);
        }

        [Test]
        public void Resolve_FindsMatch_NotAtTheStart()
        {
            List<ItemType> slots = new()
            {
                ItemType.Apple, ItemType.Bread, ItemType.Bread, ItemType.Bread, ItemType.Carrot
            };

            MatchResult result = _resolver.Resolve(slots);

            Assert.IsTrue(result.Matched);
            Assert.AreEqual(ItemType.Bread, result.Type);
            Assert.AreEqual(1, result.SlotIndex);
        }

        [Test]
        public void Resolve_ReturnsFirstMatch_WhenMultipleMatchesExist()
        {
            List<ItemType> slots = new()
            {
                ItemType.Apple, ItemType.Apple, ItemType.Apple,
                ItemType.Bread, ItemType.Bread, ItemType.Bread
            };

            MatchResult result = _resolver.Resolve(slots);

            Assert.IsTrue(result.Matched);
            Assert.AreEqual(ItemType.Apple, result.Type);
            Assert.AreEqual(0, result.SlotIndex);
        }
    }
}
