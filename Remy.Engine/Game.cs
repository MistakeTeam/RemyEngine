using System.Reflection;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Remy.Engine.Core;
using Remy.Engine.Graficos.Interface;
using Remy.Engine.Graficos.Texto;
using Remy.Engine.Graficos.OpenGL;
using Remy.Engine.Input;
using Remy.Engine.IO;
using Remy.Engine.Logs;
using Remy.Engine.Graficos;
using System.Text;

namespace Remy.Engine
{
    /// <summary>
    /// Classe principal do Remy
    /// </summary>
    public partial class Game(NativeWindowSettings nws) : GameWindow(GameWindowSettings.Default, nws)
    {
        // Propriedades da engine
        internal static Vector2i Janela;
        internal static string Titulo;
        internal static IReadOnlyList<JoystickState> Joysticks;

        private Render Render;
        private LogFile LogFile;
        private GerenciarFontes Fontes;
        private InputControl InputControl;

        private Retangulo Retangulo1;

        // ===============================================================================================================

        private readonly float[] _vertices =
       [
            // Position   Texture coordinates
             0.5f,  0.5f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 1.0f  // top left
        ];

        private readonly uint[] _indices =
        [
            0, 1, 3,
            1, 2, 3
        ];

        private BufferObject<uint> _elementBufferObject;
        private BufferObject<float> _vertexBufferObject;
        private ArrayObject _vertexArrayObject;
        private Shader _shader;
        private TexturaImage _texture;

        // ===============================================================================================================

        ConstrutorTexto sampleText;
        ConstrutorTexto MousePositionText;


        protected virtual string GetExtensions()
        {
            GL.GetInteger((GetPName)All.NumExtensions, out int numExtensions);

            var extensionsBuilder = new StringBuilder();

            for (int i = 0; i < numExtensions; i++)
                extensionsBuilder.Append($"{GL.GetString(StringNameIndexed.Extensions, i)} ");

            return extensionsBuilder.ToString().TrimEnd();
        }

        // Evento é garregado quando o OpenTK inicia o aplicativo
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.3f, 0.4f, 0.5f, 1.0f);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            LogFile = new LogFile();

            if (GCena.Cenas.Count <= 0)
            {
                LogFile.WriteLine("Nenuma cena disponivel para ser carregada!");
                Close();
                return;
            }

            LogFile.WriteLine(Environment.CurrentDirectory);

            void tre(object? sender, EventArgs e)
            {
                LogFile.WriteLine("[ProcessExit] Remy foi forçado a fechar.");
                LogFile.Close();
            }

            AppDomain.CurrentDomain.ProcessExit += tre;

            string extensions = GetExtensions();

            LogFile.WriteLine($@"GL Initialized
                        GL Version:                 {GL.GetString(StringName.Version)}
                        GL Renderer:                {GL.GetString(StringName.Renderer)}
                        GL Shader Language version: {GL.GetString(StringName.ShadingLanguageVersion)}
                        GL Vendor:                  {GL.GetString(StringName.Vendor)}
                        GL Extensions:              {extensions.Replace(" ", "\n                                                    ")}");

            foreach (
                Type component in Assembly.GetEntryAssembly()!.GetTypes().Where(t =>
                {
                    return t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Comportamento));
                })
            )
            {
                Comportamento cc = (Comportamento)Activator.CreateInstance(component)!;
            }

            Janela = new(Size.X, Size.Y - (Bounds.Size.Y - ClientRectangle.Size.Y));
            Titulo = Title;

            Render = new Render();
            Fontes = new GerenciarFontes();
            InputControl = new InputControl(this);

            Retangulo1 = new Retangulo(300, 250, 10, 0, "Retangulo1");

            // ===============================================================================================================

            _vertexArrayObject = new(4 * sizeof(float));

            _vertexBufferObject = new(_vertices.Length, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            _vertexBufferObject.SetData(_vertices);

            _elementBufferObject = new(_indices.Length, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            _elementBufferObject.SetData(_indices);

            _shader = new("Recursos/Shaders/Texture.vert", "Recursos/Shaders/Texture.frag");
            _shader.Use();

            _vertexArrayObject.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0);
            _vertexArrayObject.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float));

            _texture = new("Recursos/Textura/container.jpg");
            _texture.Bind();

            // ===============================================================================================================

            LogFile.WriteLine($"Remy terminou de carregar");

            BaseComportamento.Start();
        }

        // Em seguida vem o update
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Render.NovoQuadro();

            Title = $"{Titulo} - Resolução: {Janela.X}x{Janela.Y} {API}: {APIVersion}/{Profile} (Vsync: {VSync}) FPS: {1f / e.Time:0}";

            Joysticks = JoystickStates;
            InputControl.Update();

            sampleText = new ConstrutorTexto("This is sample text", 0.0f, 0.0f, 1.0f, Color4.Crimson);
            MousePositionText = new ConstrutorTexto($"P: {Mouse.Posição}", 20.0f, 700.0f, 1.0f, Color4.Crimson);

            BaseComportamento.Update();
        }

        // logo após, vem o render
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            Retangulo1.desenhar();

            // ===============================================================================================================

            _vertexArrayObject.Bind();

            _texture.Bind(TextureUnit.Texture0);
            _shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            // ===============================================================================================================

            Render.Update(); // Renderizar o frame armazenado no cache do render

            SwapBuffers(); // Aqui o quadro será pintado na tela

            Render.Clear(); // Limpar o cache do render
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            Janela = new(ClientSize.X, ClientSize.Y);

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        }

        // Evento é carregado antes do aplicativo ser encerrado
        protected override void OnUnload()
        {
            base.OnUnload();

            LogFile.WriteLine("Remy está desligando");

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}