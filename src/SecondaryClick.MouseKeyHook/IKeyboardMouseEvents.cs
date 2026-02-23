using System;

namespace System.MouseKeyHook;

/// <summary>
/// Provides keyboard and mouse events.
/// </summary>
public interface IKeyboardMouseEvents : IKeyboardEvents, IMouseEvents, IDisposable;
