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
            // Act: Add a trimmed item by "John"
            var result = _service.AddItem("  Apple  ", "John");

            // Assert: Item is added correctly
            var items = _service.GetItems().ToList();
            Assert.Single(items);
            Assert.Equal("Apple", items[0]);

            // Assert: Audit trail entry is created
            var audit = _service.GetAuditTrail("Admin").ToList();
            Assert.Single(audit);
            Assert.Equal("Add", audit[0].Action);
            Assert.Equal("Apple", audit[0].Item);
            Assert.Equal("John", audit[0].performedBy);
        }

        [Fact]
        public void DeleteItem_ShouldRemoveItem()
        {
            // Arrange: Add item first
            _service.AddItem("Banana", "Sarah");

            // Act: Delete the item
            var result = _service.DeleteItem("Banana", "Sarah");

            // Assert: Item list should be empty
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
            // Arrange: Add item
            _service.AddItem("Orange", "Mike");

            // Act: Attempt to fetch audit trail as a non-admin
            var audit = _service.GetAuditTrail("User");

            // Assert: Should be denied (empty list)
            Assert.Empty(audit);
        }

        [Fact]
        public void AddItem_ShouldIgnoreEmpty()
        {
            // Act: Try adding an empty item (spaces only)
            var result = _service.AddItem("   ", "Test");

            // Assert: Should still be considered as one item (depends on trim logic)
            var items = _service.GetItems().ToList();
            Assert.Single(items); 
        }

        [Fact]
        public void Cibi_AsAdmin_CanViewAuditTrail()
        {
            // Arrange: Cibi adds an item
            _service.AddItem("Laptop", "Cibi");

            // Act: Fetch audit trail as Admin
            var audit = _service.GetAuditTrail("Admin").ToList();

            // Assert: Entry should be visible and match Cibi
            Assert.Single(audit);
            Assert.Equal("Cibi", audit[0].performedBy);
        }

    }
}