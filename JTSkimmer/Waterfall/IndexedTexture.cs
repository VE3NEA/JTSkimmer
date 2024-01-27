using System.Diagnostics;
using System.Drawing.Imaging;
using SharpGL;

namespace JTSkimmer
{
  public class IndexedTexture
  {
    public const int PALETTE_SIZE = 256;

    private uint[] textureIds = new uint[2];
    private readonly OpenGL gl;
    private readonly int Width;
    private readonly int Height;
    private int[] IntBuffer;


    public IndexedTexture(OpenGL gl, int width, int height)
    {
      CheckError(gl, false);

      this.gl = gl;
      Width = width;
      Height = height;

      IntBuffer = new int[Width];

      gl.GenTextures(2, textureIds);
      CheckError(gl);

      ClearBitmap();
      //SetPalette();
    }

    public void SetPalette(int[]? palette = null)
    {
      CheckError(gl, false);

      // default to gray scale
      if (palette == null)
      {
        palette = new int[PALETTE_SIZE];
        for (int i = 0; i < PALETTE_SIZE; i++) palette[i] = i * 0x10101;
      }

      // copy to texture
      gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureIds[1]);
      CheckError(gl, false); // produces false positives

      gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGB, PALETTE_SIZE, 1, 0, OpenGL.GL_BGRA,
        OpenGL.GL_UNSIGNED_BYTE, palette);
      CheckError(gl);
    }

    public void SetBitmap(Bitmap bmp)
    {
      CheckError(gl, false);

      Debug.Assert(bmp.Width == Width && bmp.Height == Height);

      var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

      gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureIds[0]);
      CheckError(gl);
      gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGB, bmp.Width, bmp.Height, 0, 
        OpenGL.GL_R32F, OpenGL.GL_UNSIGNED_BYTE, data.Scan0);
      CheckError(gl);

      bmp.UnlockBits(data);
    }

    public void ClearBitmap()
    {
      CheckError(gl, false);

      gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureIds[0]);
      CheckError(gl);
      gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_R32F, Width, Height, 0, OpenGL.GL_BGRA,
        OpenGL.GL_UNSIGNED_BYTE, (byte[]?)null);
      CheckError(gl);
    }

    public void SetRow(int row, float[] data)
    {
      CheckError(gl, false);

      // TexSubImage2D in SharpGL accepts only int[], so give it floats in an int[] buffer
      Buffer.BlockCopy(data, 0, IntBuffer, 0, Width * sizeof(float));

      gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureIds[0]);
      CheckError(gl);
      gl.TexSubImage2D(OpenGL.GL_TEXTURE_2D, 0, 0, row, Width, 1, OpenGL.GL_LUMINANCE, OpenGL.GL_FLOAT, IntBuffer);
      CheckError(gl);
    }

    public void Bind(OpenGL gl)
    {
      CheckError(gl, false);

      gl.ActiveTexture(OpenGL.GL_TEXTURE0);
      CheckError(gl);
      gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureIds[0]);
      CheckError(gl);

      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST); // shrink
      CheckError(gl);
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);  // stretch
      CheckError(gl);
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_CLAMP_TO_EDGE);  // x
      CheckError(gl);
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT); // y
      CheckError(gl);

      gl.ActiveTexture(OpenGL.GL_TEXTURE1);
      CheckError(gl);
      gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureIds[1]);
      CheckError(gl);

      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);
      CheckError(gl);
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
      CheckError(gl);
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_CLAMP_TO_EDGE);
      CheckError(gl);
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_CLAMP_TO_EDGE);
      CheckError(gl);
    }

    private void CheckError(OpenGL gl, bool log = true)
    {
      WaterfallControl.CheckError(gl, log);
    }
  }
}
