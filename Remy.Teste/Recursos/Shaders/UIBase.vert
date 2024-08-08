#version 450

layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec3 aColor;

// layout (location = 2) uniform mat4 projection;

out vec3 ourColor;

void main()
{
    ourColor = aColor;
    gl_Position = vec4(aPosition, 0., 1.);
}