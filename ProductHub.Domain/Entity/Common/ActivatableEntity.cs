using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Domain.Entity.Common;

public abstract class ActivatableEntity : BaseEntity
{
    public bool IsActive { get; protected set; }

    public void Activate() => IsActive = true;
    public void DeActivate() => IsActive = false;
}
