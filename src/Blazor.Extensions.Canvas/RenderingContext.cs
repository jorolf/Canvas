using System;
using Microsoft.AspNetCore.Blazor;
using Microsoft.JSInterop;

namespace Blazor.Extensions
{
    public abstract class RenderingContext : IDisposable
    {
        private const string NAMESPACE_PREFIX = "BlazorExtensions";
        private const string SET_PROPERTY_ACTION = "setProperty";
        private const string CALL_METHOD_ACTION = "call";
        private const string ADD_ACTION = "add";
        private const string REMOVE_ACTION = "remove";
        private readonly string _contextName;

        public ElementRef Canvas { get; }

        internal RenderingContext(BECanvasComponent reference, string contextName)
        {
            this.Canvas = reference.CanvasReference;
            this._contextName = contextName;
            ((IJSInProcessRuntime)JSRuntime.Current).Invoke<object>($"{NAMESPACE_PREFIX}.{this._contextName}.{ADD_ACTION}", this.Canvas);
        }

        #region Private Methods
        protected void SetProperty(string property, object value)
        {
            ((IJSInProcessRuntime)JSRuntime.Current).Invoke<object>($"{NAMESPACE_PREFIX}.{this._contextName}.{SET_PROPERTY_ACTION}", this.Canvas, property, value);
        }

        protected T CallMethod<T>(string method)
        {
            return ((IJSInProcessRuntime)JSRuntime.Current).Invoke<T>($"{NAMESPACE_PREFIX}.{this._contextName}.{CALL_METHOD_ACTION}", this.Canvas, method);
        }

        protected T CallMethod<T>(string method, object value)
        {
            return ((IJSInProcessRuntime)JSRuntime.Current).Invoke<T>($"{NAMESPACE_PREFIX}.{this._contextName}.{CALL_METHOD_ACTION}", this.Canvas, method, value);
        }

        public void Dispose()
        {
            ((IJSInProcessRuntime)JSRuntime.Current).Invoke<object>($"{NAMESPACE_PREFIX}.{this._contextName}.{REMOVE_ACTION}", this.Canvas);
        }
        #endregion
    }
}