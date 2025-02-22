using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class TaskModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public ETaskStatus Status { get; set; } = ETaskStatus.Pending;
    }
}
