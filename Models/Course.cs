using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Education.Models
{
    public class Course
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid TeacherId { get; set; }
        public User Teacher { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
