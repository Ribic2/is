using Jira.Enums;

namespace Jira.Controllers.dtos;

public class TicketDto
{
    public int TicketId { get; set; }
    public string TicketName { get; set; }
    public string TitleDescription { get; set; }
    public StatusEnum Status { get; set; }
}