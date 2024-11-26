using GigaJira.Data.Enums;

namespace GigaJira.Models.Entities;


public class Status
{
    public Guid Id { get; set; }
    public StatusEnum status { get; set; }
}