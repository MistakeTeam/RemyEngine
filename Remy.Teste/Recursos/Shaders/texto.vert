#version 450

layout(location=0)in vec4 in_pos;

out vec2 vUV;

uniform mat4 projection;

void main()
{
    gl_Position=projection*vec4(in_pos.xy,0.,1.);
    vUV=in_pos.zw;
}