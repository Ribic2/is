namespace Jira.Controllers.dtos;

public class ProjectDto
{
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<TicketDto> Tickets { get; set; } = new List<TicketDto>();

}