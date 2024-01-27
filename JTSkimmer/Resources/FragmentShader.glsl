#version 330 core

in vec2 pass_TexCoord;

uniform sampler2D indexedTexture;
uniform sampler2D paletteTexture;

uniform int in_ScreenWidth;
uniform float in_ScrollPos;
uniform float in_ScrollHeight;
uniform float in_Brightness;
uniform float in_Contrast;

out vec4 out_Color;

void main(void) 
{
  // y coordinate in the texture after scrolling
  float texelT = fract(2 - pass_TexCoord.t * in_ScrollHeight + in_ScrollPos);
  int texelY = int(textureSize(indexedTexture, 0).y * texelT);	
	
	// stretch or shrink factor
	float scale = textureSize(indexedTexture, 0).x / float(in_ScreenWidth);	
	float luminance = 0;

	if (scale > 1)
	{
    // shrink: take the max of all texels in the range
	  int pixelX = int(pass_TexCoord.s * in_ScreenWidth);
  	int texelXstart = int(pixelX * scale);
	  int texelXstop = int((pixelX + 1) * scale);
	  for (int texelX = texelXstart; texelX < texelXstop; texelX++)
	    luminance = max(luminance, texelFetch(indexedTexture, ivec2(texelX, texelY), 0).r);	
	}
	else
	{
		// stretch: interpolate between 2 texels
		float texelX = pass_TexCoord.s * textureSize(indexedTexture, 0).x;
		int texelXint = int(texelX);
		float a = texelX - texelXint;
		float v1 = texelFetch(indexedTexture, ivec2(texelXint, texelY), 0).r;
	  float v2 = texelFetch(indexedTexture, ivec2(texelXint+1, texelY), 0).r;
		luminance = v1 * (1-a) + v2 * a;
	}

	// apply brightness and contrast	
	luminance = in_Brightness + luminance * in_Contrast;

	// get color from palette
	out_Color = texture(paletteTexture, vec2(luminance, 0));
}