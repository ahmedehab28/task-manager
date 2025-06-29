using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Boards.DTOs
{
    public record BoardDto (
        Guid Id,
        string Title,
        string Description,
        DateTime CreatedAt);
}
