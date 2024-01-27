namespace JTSkimmer
{
  partial class WaterfallControl
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      OpenglControl = new SharpGL.OpenGLControl();
      ((System.ComponentModel.ISupportInitialize)OpenglControl).BeginInit();
      SuspendLayout();
      // 
      // OpenglControl
      // 
      OpenglControl.Dock = DockStyle.Fill;
      OpenglControl.DrawFPS = false;
      OpenglControl.Location = new Point(0, 0);
      OpenglControl.Margin = new Padding(4, 3, 4, 3);
      OpenglControl.Name = "OpenglControl";
      OpenglControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL3_3;
      OpenglControl.RenderContextType = SharpGL.RenderContextType.NativeWindow;
      OpenglControl.RenderTrigger = SharpGL.RenderTrigger.Manual;
      OpenglControl.Size = new Size(388, 150);
      OpenglControl.TabIndex = 0;
      OpenglControl.OpenGLInitialized += OpenglControl_OpenGLInitialized;
      OpenglControl.OpenGLDraw += OpenglControl_OpenGLDraw;
      // 
      // WaterfallControl
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(OpenglControl);
      Name = "WaterfallControl";
      Size = new Size(388, 150);
      ((System.ComponentModel.ISupportInitialize)OpenglControl).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private SharpGL.OpenGLControl OpenglControl;
  }
}
