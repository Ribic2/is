using System.ComponentModel;

namespace Jira.Enums;

public enum StatusEnum
{
    [Description("No test performed")]
    BACKLOG,
    WORKING_ON,
    QA,
    DONE
}