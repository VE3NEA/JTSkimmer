#version 330 core

layout(location = 0) in vec2 in_Position;
layout(location = 1) in vec2 in_TexCoord;

out vec2 pass_TexCoord;

void main(void) 
{
	gl_Position = vec4(in_Position, 0.0, 1.0);
	pass_TexCoord = in_TexCoord;
}