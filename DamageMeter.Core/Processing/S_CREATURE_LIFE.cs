﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageMeter.Processing
{
    internal class S_CREATURE_LIFE
    {

        internal S_CREATURE_LIFE(Tera.Game.Messages.SCreatureLife message)
        {
            NetworkController.Instance.EntityTracker.Update(message);
            NetworkController.Instance.AbnormalityTracker.Update(message);
        }
    }
}
