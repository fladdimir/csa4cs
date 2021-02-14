using System;
using System.Collections.Generic;
using System.Numerics;
using Gtk;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.Gtk;

namespace Csa4cs.visualization.skiasharp
{
    // experimental skiasharp canvas
    public class SkiaSharpCanvas : ICanvas
    {
        private Window window;

        private readonly Dictionary<object, Uri> shownImages = new(); // key -> image-url
        private readonly Dictionary<Uri, SKImage> loadedImages = new(); // image-url -> image-data
        private readonly Dictionary<object, SKPoint> locations = new(); // key -> location

        private readonly Uri EntityIconUri;

        public SkiaSharpCanvas(Uri bgImageUri, Uri entityIconUri)
        {
            InitCanvas(bgImageUri);
            EntityIconUri = entityIconUri;
        }

        private void InitCanvas(Uri bgImageUri)
        {
            InitAppWindow(bgImageUri);
            var skiaView = new SKDrawingArea();
            skiaView.PaintSurface += OnPaintSurface;
            skiaView.Show();
            window.Child = skiaView;
            RunIterationX(10);
        }
        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas; // canvas!
            canvas.Clear(SKColors.White); // make sure the canvas is blank

            foreach (var entry in shownImages)
            {
                var image = loadedImages[entry.Value];
                var point = locations[entry.Key];
                canvas.DrawImage(image, point);
            }
        }

        private void InitAppWindow(Uri bgImageUri)
        {
            var bgImage = LoadBackgroundImage(bgImageUri);
            Application.Init();
            var app = new Application("org.gtktest.gtktest", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);
            window = new Window("Csa4cs Canvas Animation");
            window.SetDefaultSize(bgImage.Width, bgImage.Height);
            app.AddWindow(window);
            window.DeleteEvent += delegate { Application.Quit(); };
            window.Show();

            Update();
        }

        public void Update()
        {
            window.QueueDraw();
            RunIterationX(5);
        }

        public static void RunIterationX(int n)
        {
            for (int i = 0; i < n; i++)
            {
                RunIteration();
            }
        }


        public static void RunIteration()
        {
            Application.RunIteration(false);
        }

        public void DrawEntity(Entity entity, Vector2 currentPosition)
        {
            if (!shownImages.ContainsKey(entity))
                shownImages[entity] = EntityIconUri;

            var uri = shownImages[entity];
            LoadImage(uri);
            locations[entity] = new SKPoint(currentPosition.X, currentPosition.Y);
            Update();
        }

        private SKImage LoadImage(Uri uri)
        {
            if (!loadedImages.ContainsKey(uri))
            {
                SKBitmap bitmap = SKBitmap.Decode(uri.LocalPath);
                SKImage image = SKImage.FromBitmap(bitmap);
                loadedImages[uri] = image;
            }
            return loadedImages[uri];
        }

        private SKImage LoadBackgroundImage(Uri uri)
        {
            var id = "background-image";
            shownImages[id] = uri;
            locations[id] = new SKPoint(0, 0);
            return LoadImage(uri);
        }

        public void RemoveEntity(Entity entity)
        {
            locations.Remove(entity);
            shownImages.Remove(entity);
            Update();
        }
    }
}