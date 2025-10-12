using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week2_TaskManagement.Models
{
    internal record AppTask(
        int Id,
        string Title,
        string? Description,
        bool IsCompleted,
        DateTime CreatedAt
    );
}
    