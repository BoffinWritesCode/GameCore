using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GameCore.Utility.DropTables.Conditions
{
    public class MeetAllConditions : IDropCondition
    {
        private List<IDropCondition> _conditions;

        public MeetAllConditions(params IDropCondition[] conditions)
        {
            _conditions = conditions.ToList();
        }

        public MeetAllConditions AddCondition(IDropCondition condition)
        {
            _conditions.Add(condition);
            return this;
        }

        public bool CanDrop()
        {
            foreach(IDropCondition condition in _conditions)
            {
                if (!condition.CanDrop()) return false;
            }
            return true;
        }
    }
}
