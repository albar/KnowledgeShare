using System;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Manager
{
    public struct UpdatableCourse
    {
        public string Title { get; set; }

        public void PutInto(Course course)
        {
            course.Title = Title;
        }
    }
}