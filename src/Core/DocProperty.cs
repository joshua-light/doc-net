﻿namespace Summary;

/// <summary>
///     A <see cref="DocMember" /> that represents a documented property in the parsed source code.
/// </summary>
public record DocProperty : DocMember
{
    /// <summary>
    ///     The type of the property.
    /// </summary>
    public required DocType Type { get; init; }

    /// <summary>
    ///     The accessors of the property (e.g., <c>get</c>, <c>set</c>, <c>init</c>).
    /// </summary>
    public required DocPropertyAccessor[] Accessors { get; init; }

    /// <summary>
    ///     Whether this property was generated by compiler (e.g., it's a property of a record).
    /// </summary>
    public required bool Generated { get; init; }

    /// <summary>
    ///     Whether this property represents an event.
    /// </summary>
    public required bool Event { get; init; }
}