using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GameCore.Utility.DropTables.Conditions
{
    public class MeetOneCondition : IDropCondition
    {
        private List<IDropCondition> _conditions;

        public MeetOneCondition(params IDropCondition[] conditions)
        {
            _conditions = conditions.ToList();
        }

        public MeetOneCondition AddCondition(IDropCondition condition)
        {
            _conditions.Add(condition);
            return this;
        }

        public bool CanDrop()
        {
            foreach(IDropCondition condition in _conditions)
            {
                if (condition.CanDrop()) return true;
            }
            return false;
        }
    }
}
