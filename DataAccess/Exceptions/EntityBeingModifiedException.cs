﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Exceptions
{
    public class EntityBeingModifiedException : Exception
    {
        public EntityBeingModifiedException(string msg) : base(msg)
        {

        }
    }
 }
