using csharp.Models;

namespace csharp.Interface
{
    /// <summary>
    /// Interface for managing items and capturing audit trail.
    /// </summary>
    public interface IItemService
        {
        /// <summary>
        /// Retrieves all items from the list.
        /// </summary>
        /// <returns>A collection of item strings.</returns>
        IEnumerable<string> GetItems();

        /// <summary>
        /// Adds a new item to the list and records an audit entry.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="performedBy">The username who added the item.</param>
        /// <returns>Success message.</returns>
        string AddItem(string item, string performedBy);

        /// <summary>
        /// Deletes an item from the list and records an audit entry.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        /// <param name="performedBy">The username who deleted the item.</param>
        /// <returns>Success or failure message.</returns>
        string DeleteItem(string item, string performedBy);

        /// <summary>
        /// Retrieves the audit trail, restricted to admin role.
        /// </summary>
        /// <param name="role">User's role (must be "Admin").</param>
        /// <returns>List of audit entries or empty if unauthorized.</returns>
        IEnumerable<AuditEntry> GetAuditTrail(string role);
        }
    
}
