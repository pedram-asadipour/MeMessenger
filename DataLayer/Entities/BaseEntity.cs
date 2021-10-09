using System;

namespace DataLayer.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }

        public BaseEntity()
        {
            CreationDate = DateTime.Now;
        }
    }
}