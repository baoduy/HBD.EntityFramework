﻿namespace HBD.EntityFramework.Entities
{
    public interface IEntity
    {
    }

    public interface IEntity<out TKey> : IEntity
    {
        TKey Id { get; }
    }
}