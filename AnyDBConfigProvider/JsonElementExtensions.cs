﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AnyDBConfigProvider;

static class JsonElementExtensions
{
    public static string GetValueForConfig(this JsonElement e)
    {
        if (e.ValueKind == JsonValueKind.String)
        {
            //remove the quotes, "ab"-->ab
            return e.GetString();
        }
        else if (e.ValueKind == JsonValueKind.Null
            || e.ValueKind == JsonValueKind.Undefined)
        {
            //remove the quotes, "null"-->null
            return null;
        }
        else
        {
            return e.GetRawText();
        }
    }
}
