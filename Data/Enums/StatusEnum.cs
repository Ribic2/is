using System.ComponentModel;

namespace GigaJira.Data.Enums;

public enum StatusEnum
{
    [Description("In Progress")]
    IN_PROGRESS,
    [Description("Backlog")]
    BACKLOG,
    [Description("Dome")]
    DONE,
    [Description("QA")]
    QA,
    [Description("Working on")]
    WORKING_ON
}