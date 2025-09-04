﻿
namespace Domain.Entities.Common
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public Guid? LastModifiedBy { get; set; }
    }
}
