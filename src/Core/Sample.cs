﻿namespace Summary;

/// <summary>
///     A sample delegate.
/// </summary>
public delegate void GlobalDelegate(int x, int y);

/// <summary>
///     A sample class that is documented.
///
///     This a second paragraph.
///     It is indented.
///
///     This is the third paragraph.
///
///     Here is a <i>italic</i>, <em>italic2</em> fragment.
///     Here is a <b>bold</b>, <strong>bold2</strong> fragment.
///     Here is a <c>code</c> fragment.
///     Here is a <strike>strikethrough</strike> fragment.
/// </summary>
/// <remarks>
///     Remarks section.
///     Second line.
///
///     Another paragraph.
/// </remarks>
/// <typeparam name="T0">A first type parameter.</typeparam>
/// <typeparam name="T1">A second type parameter.</typeparam>
public class Sample<T0, T1>
{
    /// <summary>
    ///     A sample delegate.
    /// </summary>
    public delegate void Delegate1(int x, int y);

    /// <summary>
    ///     A sample field.
    /// </summary>
    public int Field1;

    /// <summary>
    ///     A sample property.
    /// </summary>
    public int Property1 { get; set; }

    /// <summary>
    ///     A sample property with custom visibility.
    /// </summary>
    public int Property2 { private get; set; }

    /// <summary>
    ///     A sample property with custom visibility (2).
    /// </summary>
    public int Property3 { get; protected set; }

    /// <summary>
    ///     A sample property with custom accessors.
    /// </summary>
    public int Property4
    {
        get => 0;
        set { }
    }

    /// <summary>
    ///     A sample field event.
    /// </summary>
    public event Action Event1 = () => { };

    /// <summary>
    ///     A sample property event.
    /// </summary>
    public event Action Event2
    {
        add { }
        remove { }
    }

    /// <summary>
    ///     A sample indexer.
    /// </summary>
    /// <param name="i">The parameter for indexer.</param>
    /// <returns>What indexer returns.</returns>
    public int this[int i] => 0;

    /// <summary>
    ///     A simple method.
    ///
    ///     It contains two parameters:
    ///     - <paramref name="x"/> means `x`
    ///     - <paramref name="y"/> means `y`
    ///
    ///     It contains three type parameters:
    ///     - <typeparamref name="M0"/> is the first one
    ///     - <typeparamref name="M1"/> is the second one
    ///     - <typeparamref name="M2"/> is the third one
    /// </summary>
    /// <param name="x">The `x` of the method.</param>
    /// <param name="y">The `y` of the method.</param>
    /// <typeparam name="M0">The first type parameter of the method.</typeparam>
    /// <typeparam name="M1">The second type parameter of the method.</typeparam>
    /// <typeparam name="M2">The third type parameter of the method.</typeparam>
    /// <returns>The `TimeSpan` instance.</returns>
    public TimeSpan Method<M0, M1, M2>(int x, string y) =>
        TimeSpan.Zero;

    /// <summary>
    ///     The overloaded <see cref="Method{M0,M1,M2}(int,string)"/>.
    /// </summary>
    /// <inheritdoc cref="Method{M0,M1,M2}(int,string)"/>
    public TimeSpan Method<M0, M1, M2>(short x, string y) =>
        TimeSpan.Zero;
}