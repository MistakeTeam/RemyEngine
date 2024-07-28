using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using Remy.Engine.Graficos;
using Remy.Engine.Input;
using Remy.Engine.Input.Joystick;
using Remy.Engine.UI;

namespace Remy.Engine
{
    /// <summary>
    /// Classe principal do Remy/OpenTK
    /// </summary>
    /// <param name="nws"></param>
    public class Game(NativeWindowSettings nws) : GameWindow(GameWindowSettings.Default, nws)
    {
        // Propriedades basicas da janela
        public static Vector2i Janela { get; private set; }
        public static string Titulo { get; private set; }



        private Fonte Fonte;

        private InputControl InputControl;


        private Retangulo Retangulo1;
        private Retangulo Retangulo2;
        private Retangulo Retangulo3;
        private Retangulo Retangulo4;


        // Evento é garregado quando o OpenTK inicia o aplicativo
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.3f, 0.4f, 0.5f, 1.0f);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Fonte = new Fonte();
            InputControl = new InputControl(this);

            Janela = new(Size.X, Size.Y - (Bounds.Size.Y - ClientRectangle.Size.Y));
            Titulo = Title;

            Console.WriteLine(Environment.CurrentDirectory);

            Matrix4 projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, this.ClientSize.X, this.ClientSize.Y, 0.0f, -1.0f, 1.0f);
            GL.UniformMatrix4(1, false, ref projectionM);

            Retangulo1 = new Retangulo(120, 50, 800, 600, "Retangulo1");
            Retangulo2 = new Retangulo(120, 50, 0, 600, "Retangulo2");
            Retangulo3 = new Retangulo(120, 50, 800, 0, "Retangulo3");
            Retangulo4 = new Retangulo(120, 50, 0, 0, "Retangulo4");
        }

        // Em seguida vem o update
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Title = $"{Titulo}: Resolução: {Janela.X}x{Janela.Y} {API}: {APIVersion}/{Profile} (Vsync: {VSync}) FPS: {1f / e.Time:0}";

            InputControl.Update();
        }

        // logo após, vem o render
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            Fonte.RenderText("This is sample text", 0.0f, 0.0f, 1.0f, Color4.Crimson);

            Fonte.RenderText("(C) LearnOpenGL.com", 540.0f, 900.0f, 0.5f, Color4.HotPink);


            Fonte.RenderText($"{InputControl.MousePosition.X}", 490.0f, 440.0f, 0.5f, Color4.HotPink);
            Fonte.RenderText($"{InputControl.MousePosition.Y}", 490.0f, 470.0f, 0.5f, Color4.HotPink);

            if (InputControl.JoystickIsConnected)
            {
                Fonte.RenderText($"RT {InputControl.Joysticks[0].GetAxis((int)JoystickAxis.RT)}", 490.0f, 120.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"RB {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.RB)}", 490.0f, 150.0f, 0.5f, Color4.Black);

                Fonte.RenderText($"RButton {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.RButton)}", 490.0f, 180.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"R Vertical {InputControl.Joysticks[0].GetAxis((int)JoystickAxis.RVertical)}", 490.0f, 210.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"R Horizontal {InputControl.Joysticks[0].GetAxis((int)JoystickAxis.RHorizontal)}", 490.0f, 240.0f, 0.5f, Color4.Black);

                Fonte.RenderText($"LT {InputControl.Joysticks[0].GetAxis((int)JoystickAxis.LT)}", 70.0f, 120.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"LB {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.LB)}", 70.0f, 150.0f, 0.5f, Color4.Black);

                Fonte.RenderText($"LButton {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.LButton)}", 70.0f, 180.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"L Vertical {InputControl.Joysticks[0].GetAxis((int)JoystickAxis.LVertical)}", 70.0f, 210.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"L Horizontal {InputControl.Joysticks[0].GetAxis((int)JoystickAxis.LHorizontal)}", 70.0f, 240.0f, 0.5f, Color4.Black);

                Fonte.RenderText($"A {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.A)}", 490.0f, 300.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"B {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.B)}", 490.0f, 330.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"X {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.X)}", 490.0f, 360.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"Y {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.Y)}", 490.0f, 390.0f, 0.5f, Color4.Black);

                Fonte.RenderText($"Back {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.Back)}", 100.0f, 520.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"Mode {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.Mode)}", 230.0f, 520.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"Start {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.Start)}", 370.0f, 520.0f, 0.5f, Color4.Black);

                Fonte.RenderText($"Cima {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.Up)}", 140.0f, 350.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"Baixo {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.Down)}", 140.0f, 410.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"Esqueda {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.Left)}", 50.0f, 380.0f, 0.5f, Color4.Black);
                Fonte.RenderText($"Direita {InputControl.Joysticks[0].IsButtonDown((int)JoystickButtons.Right)}", 220.0f, 380.0f, 0.5f, Color4.Black);
            }

            Retangulo1.desenhar();
            Retangulo2.desenhar();
            Retangulo3.desenhar();
            Retangulo4.desenhar();

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);

            Janela = new(Size.X, Size.Y - (Bounds.Size.Y - ClientRectangle.Size.Y));
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