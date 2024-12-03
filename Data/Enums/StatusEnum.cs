using System.ComponentModel;

namespace GigaJira.Data.Enums;

public enum StatusEnum
{
    [Description("Backlog")]
    BACKLOG,
    [Description("In Progress")]
    IN_PROGRESS,
    [Description("Working on")]
    WORKING_ON,
    [Description("QA")]
    QA,
    [Description("Done")]
    DONE,
}