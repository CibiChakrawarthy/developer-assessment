using csharp.Services;
using Xunit;


namespace csharp.Tests
{
    public class ItemServiceTests
    {
        private readonly ItemService _service;
        public ItemServiceTests()
        {
            _service = new ItemService();
        }

        [Fact]
        public void AddItem_ShouldAddAndAudit()
        {
            var result = _service.AddItem("  Apple  ", "John");

            var items = _service.GetItems().ToList();
            Assert.Single(items);
            Assert.Equal("Apple", items[0]);

            var audit = _service.GetAuditTrail("Admin").ToList();
            Assert.Single(audit);
            Assert.Equal("Add", audit[0].Action);
            Assert.Equal("Apple", audit[0].Item);
            Assert.Equal("John", audit[0].performedBy);
        }

        [Fact]
        public void DeleteItem_ShouldRemoveItem()
        {
            _service.AddItem("Banana", "Sarah");
            var result = _service.DeleteItem("Banana", "Sarah");

            var items = _service.GetItems().ToList();
            Assert.Empty(items);

            var audit = _service.GetAuditTrail("Admin").ToList();
            Assert.Equal(2, audit.Count); // Add + Delete
            Assert.Equal("Delete", audit.Last().Action);
            Assert.Equal("Sarah", audit.Last().performedBy);
        }

        [Fact]
        public void GetAuditTrail_ShouldBlockIfNotAdmin()
        {
            _service.AddItem("Orange", "Mike");

            var audit = _service.GetAuditTrail("User");

            Assert.Empty(audit);
        }

    }
}