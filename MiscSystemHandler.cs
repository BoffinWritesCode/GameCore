using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public class MiscSystemHandler
    {
        public ObjectRegister<IMiscSystem> MiscSystems { get; private set; } = new ObjectRegister<IMiscSystem>();

        public void Update()
        {
            foreach (IMiscSystem system in MiscSystems)
            {
                system.Update();
            }
        }

        public void Draw()
        {
            foreach (IMiscSystem system in MiscSystems)
            {
                system.Draw();
            }
        }
    }
}
