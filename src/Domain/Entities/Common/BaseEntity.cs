﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public required Guid Id { get; init;  } = Guid.NewGuid();
    }
}
