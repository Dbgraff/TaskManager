using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using category;

namespace task
{
    public class Task
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int CategoryId { get; set; }

        public Task(string description)
        {
            Description = description;
            IsCompleted = false;
            CategoryId = -1;
        }
    }
}
