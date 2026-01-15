using System;
using MyApp.Domain.Entities.Common;

namespace MyApp.Domain.Entities
{
    public class Tenant : MutableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}