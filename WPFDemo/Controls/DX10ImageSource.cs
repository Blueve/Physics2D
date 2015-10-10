using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDemo.Controls
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using SharpDX.Direct3D9;

    class Dx10ImageSource : D3DImage, IDisposable
    {
        private static int _activeClients;
        private static Direct3DEx _d3DContext;
        private static DeviceEx _d3DDevice;
        private Texture _renderTarget;

        public Dx10ImageSource()
        {
            StartD3D();
            _activeClients++;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "RenderTarget")]
        public void Dispose()
        {
            SetRenderTargetDx10(null);

            // Here, we have our Dispose call
            Disposer.SafeDispose(ref _renderTarget);

            _activeClients--;
            EndD3D();
        }

        public void InvalidateD3DImage()
        {
            if (_renderTarget != null)
            {
                Lock();
                AddDirtyRect(new Int32Rect(0, 0, PixelWidth, PixelHeight));
                Unlock();
            }
        }

        public void SetRenderTargetDx10(SharpDX.Direct3D10.Texture2D renderTarget)
        {
            if (_renderTarget != null)
            {
                _renderTarget = null;

                Lock();
                SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
                Unlock();
            }

            if (renderTarget == null)
                return;

            if (!IsShareable(renderTarget))
            {
                throw new ArgumentException("Texture must be created with ResourceOptionFlags.Shared");
            }

            Format format = TranslateFormat(renderTarget);
            if (format == Format.Unknown)
            {
                throw new ArgumentException("Texture format is not compatible with OpenSharedResource");
            }

            IntPtr handle = GetSharedHandle(renderTarget);
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException("Handle");
            }

            _renderTarget = new Texture(_d3DDevice, renderTarget.Description.Width, renderTarget.Description.Height, 1, Usage.RenderTarget, format, Pool.Default, ref handle);
            using (Surface surface = _renderTarget.GetSurfaceLevel(0))
            {
                Lock();
                SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
                Unlock();
            }
        }

        private void StartD3D()
        {
            if (_activeClients != 0)
            {
                return;
            }

            _d3DContext = new Direct3DEx();

            var presentparams = new PresentParameters();
            presentparams.Windowed = true;
            presentparams.SwapEffect = SwapEffect.Discard;
            presentparams.DeviceWindowHandle = NativeMethods.GetDesktopWindow();
            presentparams.PresentationInterval = PresentInterval.Default;

            _d3DDevice = new DeviceEx(_d3DContext, 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve, presentparams);
        }

        private void EndD3D()
        {
            if (_activeClients != 0)
            {
                return;
            }

            Disposer.SafeDispose(ref _renderTarget);
            Disposer.SafeDispose(ref _d3DDevice);
            Disposer.SafeDispose(ref _d3DContext);
        }

        private IntPtr GetSharedHandle(SharpDX.Direct3D10.Texture2D texture)
        {
            var resource = texture.QueryInterface<SharpDX.DXGI.Resource>();
            var result = resource.SharedHandle;
            resource.Dispose();
            return result;
        }

        private static Format TranslateFormat(SharpDX.Direct3D10.Texture2D texture)
        {
            switch (texture.Description.Format)
            {
                case SharpDX.DXGI.Format.R10G10B10A2_UNorm:
                    return Format.A2B10G10R10;

                case SharpDX.DXGI.Format.R16G16B16A16_Float:
                    return Format.A16B16G16R16F;

                case SharpDX.DXGI.Format.B8G8R8A8_UNorm:
                    return Format.A8R8G8B8;

                default:
                    return Format.Unknown;
            }
        }

        private static bool IsShareable(SharpDX.Direct3D10.Texture2D texture)
        {
            return (texture.Description.OptionFlags & SharpDX.Direct3D10.ResourceOptionFlags.Shared) != 0;
        }
    }
}
