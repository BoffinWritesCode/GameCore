using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables
{
    public interface IDropCondition
    {
        bool CanDrop();
    }
}
