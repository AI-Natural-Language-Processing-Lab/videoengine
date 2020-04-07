using System.Linq;
using System.Linq.Dynamic.Core;

public static class DynamicSort
{
    public static IQueryable Sort(this IQueryable collection, string sortBy, bool reverse = false)
    {
        return collection.OrderBy(sortBy + (reverse ? " descending" : ""));
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
