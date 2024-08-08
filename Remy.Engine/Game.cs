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

namespace Remy.Engine
{
    /// <summary>
    /// Classe principal do Remy
    /// </summary>
    public class Game(NativeWindowSettings nws) : GameWindow(GameWindowSettings.Default, nws)
    {
        // Propriedades da engine
        internal static Vector2i Janela;
        internal static string Titulo;
        internal static IReadOnlyList<JoystickState> Joysticks;

        private LogFile LogFile;
        private GerenciarFontes Fontes;
        private InputControl InputControl;

        private Retangulo Retangulo1;





        private readonly float[] _vertices =
       {
            // Position   Texture coordinates
             0.5f,  0.5f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private BufferObject<uint> _elementBufferObject;

        private BufferObject<float> _vertexBufferObject;

        private ArrayObject _vertexArrayObject;

        private Shader _shader;

        private Texture _texture;



        ConstrutorTexto sampleText;



        public void AddCena(Cena _cena)
        {
            GCena.AddCena(_cena);
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

            LogFile.WriteLine(Environment.CurrentDirectory);

            Fontes = new GerenciarFontes();
            InputControl = new InputControl(this);

            sampleText = new ConstrutorTexto("This is sample text", 0.0f, 0.0f, 1.0f, Color4.Crimson);

            Retangulo1 = new Retangulo(300, 250, 50, 80, "Retangulo1");




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





            BaseComportamento.Start();
        }

        // Em seguida vem o update
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Title = $"{Titulo}: Resolução: {Janela.X}x{Janela.Y} {API}: {APIVersion}/{Profile} (Vsync: {VSync}) FPS: {1f / e.Time:0}";

            Joysticks = JoystickStates;
            InputControl.Update();

            BaseComportamento.Update();
        }

        // logo após, vem o render
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            sampleText.Desenhar();
            // Fonte.RenderText("(C) LearnOpenGL.com", 540.0f, 900.0f, 0.5f, Color4.HotPink);

            // Fonte.RenderText($"X: {Mouse.MousePosition.X} / {Mouse.X}", 490.0f, 440.0f, 0.5f, Color4.HotPink);
            // Fonte.RenderText($"Y: {Mouse.MousePosition.Y} / {Mouse.Y}", 490.0f, 470.0f, 0.5f, Color4.HotPink);

            // if (Joystick.JoystickIsConnected)
            // {
            //     Fonte.RenderText($"RT {Joystick.GetJoystick(0).GetAxis(JoystickAxis.RT)}", 490.0f, 120.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"RB {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.RB)}", 490.0f, 150.0f, 0.5f, Color4.Black);

            //     Fonte.RenderText($"RButton {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.RButton)}", 490.0f, 180.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"R Vertical {Joystick.GetJoystick(0).GetAxis(JoystickAxis.RVertical)}", 490.0f, 210.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"R Horizontal {Joystick.GetJoystick(0).GetAxis(JoystickAxis.RHorizontal)}", 490.0f, 240.0f, 0.5f, Color4.Black);

            //     Fonte.RenderText($"LT {Joystick.GetJoystick(0).GetAxis(JoystickAxis.LT)}", 70.0f, 120.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"LB {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.LB)}", 70.0f, 150.0f, 0.5f, Color4.Black);

            //     Fonte.RenderText($"LButton {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.LButton)}", 70.0f, 180.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"L Vertical {Joystick.GetJoystick(0).GetAxis(JoystickAxis.LVertical)}", 70.0f, 210.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"L Horizontal {Joystick.GetJoystick(0).GetAxis(JoystickAxis.LHorizontal)}", 70.0f, 240.0f, 0.5f, Color4.Black);

            //     Fonte.RenderText($"A {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.A)}", 490.0f, 300.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"B {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.B)}", 490.0f, 330.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"X {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.X)}", 490.0f, 360.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"Y {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.Y)}", 490.0f, 390.0f, 0.5f, Color4.Black);

            //     Fonte.RenderText($"Back {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.Back)}", 100.0f, 520.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"Mode {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.Mode)}", 230.0f, 520.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"Start {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.Start)}", 370.0f, 520.0f, 0.5f, Color4.Black);

            //     Fonte.RenderText($"Cima {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.Up)}", 140.0f, 350.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"Baixo {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.Down)}", 140.0f, 410.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"Esqueda {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.Left)}", 50.0f, 380.0f, 0.5f, Color4.Black);
            //     Fonte.RenderText($"Direita {Joystick.GetJoystick(0).IsButtonDown(JoystickButtons.Right)}", 220.0f, 380.0f, 0.5f, Color4.Black);
            // }

            Retangulo1.desenhar();



            _vertexArrayObject.Bind();

            _texture.Bind(TextureUnit.Texture0);
            _shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);




            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            Janela = new(Size.X, Size.Y - (Bounds.Size.Y - ClientRectangle.Size.Y));

            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        // Evento é carregado quando o OpenTK encerra o aplicativo
        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            base.OnUnload();
        }
    }
}