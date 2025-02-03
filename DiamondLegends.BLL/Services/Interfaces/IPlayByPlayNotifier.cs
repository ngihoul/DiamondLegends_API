using DiamondLegends.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Services.Interfaces
{
    public interface IPlayByPlayNotifier
    {
        Task SendEvent(GameEvent gameEvent);
    }
}
