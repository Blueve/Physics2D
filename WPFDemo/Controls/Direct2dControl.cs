using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Device = SharpDX.Direct3D10.Device1;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;

namespace WPFDemo.Controls
{
    public abstract class Direct2DControl : System.Windows.Controls.Image
    {
        private Device _device;
        private Texture2D _renderTarget;
        private Dx10ImageSource _d3DSurface;
        private readonly Stopwatch _renderTimer;
        private RenderTarget _d2DRenderTarget;
        private SharpDX.Direct2D1.Factory _mD2DFactory;

        public Direct2DControl()
        {
            _renderTimer = new Stopwatch();
            Loaded += Window_Loaded;
            Unloaded += Window_Closing;
            Stretch = System.Windows.Media.Stretch.Fill;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {
                return;
            }

            StartD3D();
            StartRendering();
        }

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {
                return;
            }

            StopRendering();
            EndD3D();
        }

        private void StartD3D()
        {
            _device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_0);

            _d3DSurface = new Dx10ImageSource();
            _d3DSurface.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

            CreateAndBindTargets();

            Source = _d3DSurface;
        }

        private void EndD3D()
        {
            _d3DSurface.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;
            Source = null;

            Disposer.SafeDispose(ref _d2DRenderTarget);
            Disposer.SafeDispose(ref _mD2DFactory);
            Disposer.SafeDispose(ref _d3DSurface);
            Disposer.SafeDispose(ref _renderTarget);
            Disposer.SafeDispose(ref _device);
        }

        private void CreateAndBindTargets()
        {
            _d3DSurface.SetRenderTargetDx10(null);

            Disposer.SafeDispose(ref _d2DRenderTarget);
            Disposer.SafeDispose(ref _mD2DFactory);
            Disposer.SafeDispose(ref _renderTarget);

            var width = Math.Max((int)ActualWidth, 100);
            var height = Math.Max((int)ActualHeight, 100);

            var colordesc = new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.Shared,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1
            };

            _renderTarget = new Texture2D(_device, colordesc);

            var surface = _renderTarget.QueryInterface<Surface>();

            _mD2DFactory = new SharpDX.Direct2D1.Factory();
            var rtp = new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied));
            _d2DRenderTarget = new RenderTarget(_mD2DFactory, surface, rtp);

            _d3DSurface.SetRenderTargetDx10(_renderTarget);
        }

        private void StartRendering()
        {
            if (_renderTimer.IsRunning)
            {
                return;
            }

            System.Windows.Media.CompositionTarget.Rendering += OnRendering;
            _renderTimer.Start();
        }

        private void StopRendering()
        {
            if (!_renderTimer.IsRunning)
            {
                return;
            }

            System.Windows.Media.CompositionTarget.Rendering -= OnRendering;
            _renderTimer.Stop();
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (!_renderTimer.IsRunning)
            {
                return;
            }

            PrepareAndCallRender();
            _d3DSurface.InvalidateD3DImage();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            CreateAndBindTargets();
            base.OnRenderSizeChanged(sizeInfo);
        }

        private void PrepareAndCallRender()
        {
            var device = _device;
            if (device == null)
            {
                return;
            }

            var renderTarget = _renderTarget;
            if (renderTarget == null)
            {
                return;
            }

            var targetWidth = renderTarget.Description.Width;
            var targetHeight = renderTarget.Description.Height;

            device.Rasterizer.SetViewports(new Viewport(0, 0, targetWidth, targetHeight, 0.0f, 1.0f));

            // Does the acttual rendering
            _d2DRenderTarget.BeginDraw();
            OnRender(_d2DRenderTarget);
            _d2DRenderTarget.EndDraw();

            ShowFramesPerSecondsOnDebug();

            device.Flush();
        }

        /// <summary>
        /// Does the actual rendering. 
        /// BeginDraw and EndDraw are already called by the caller. 
        /// </summary>
        public abstract void OnRender(RenderTarget target);

        /// <summary>
        /// Shows the number of frames per seconds on the debug line
        /// </summary>
        private void ShowFramesPerSecondsOnDebug()
        {
            // Spits out the Frames per second
            _f++;
            if ((DateTime.Now - _last).TotalSeconds > 1)
            {
                Debug.WriteLine(_f);
                _f = 0;
                _last = DateTime.Now;
            }
        }

        private DateTime _last;
        private int _f;

        private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // this fires when the screensaver kicks in, the machine goes into sleep or hibernate
            // and any other catastrophic losses of the d3d device from WPF's point of view
            if (_d3DSurface.IsFrontBufferAvailable)
            {
                StartRendering();
            }
            else
            {
                StopRendering();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                var isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                return isDesignMode;
            }
        }
    }
}