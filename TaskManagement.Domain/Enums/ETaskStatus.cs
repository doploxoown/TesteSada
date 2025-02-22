using System.ComponentModel;

namespace TaskManagement.Domain.Enums
{
    public enum ETaskStatus
    {
        /// <summary>
        /// Task is pending
        /// </summary>
        [Description("Pendente")]
        Pending = 0,

        /// <summary>
        /// Task is in progress
        /// </summary>
        [Description("Em Progresso")]
        InProgress = 1,

        /// <summary>
        /// Task is completed
        /// </summary>
        [Description("Concluído")]
        Completed = 2
    }
}
