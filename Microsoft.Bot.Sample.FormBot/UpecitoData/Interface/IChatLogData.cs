﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upecito.Model;

namespace Upecito.Data.Interface
{
    public interface IChatLogData
    {
        ChatLog Crear(ChatLog model);
    }
}
