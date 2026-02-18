using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashyBoard.Domain.Entities.ExternalEntities;
public class WaitTimeApiModel
{
    public string? Airport { get; set; }
    public string? FlightId { get; set; }
    public DateTime? Date { get; set; }
    public int WaitTimeMinutes { get; set; }
}