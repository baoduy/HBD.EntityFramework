﻿#region using

using System.Collections.Generic;

#endregion

namespace HBD.EntityFramework.Core
{
    public interface IPagable
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPage { get; }
    }

    public interface IPagable<TEntity> : IPagable
    {
        List<TEntity> Items { get; }
    }
}