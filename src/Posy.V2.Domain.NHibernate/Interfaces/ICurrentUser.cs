using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Domain.Interfaces
{
    public interface ICurrentUser
    {
        GlobalUser GetCurrentUser();
        int GetCurrentUserId();
    }
}
