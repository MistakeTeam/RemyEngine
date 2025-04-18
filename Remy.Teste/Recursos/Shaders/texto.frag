#version 450

in vec2 vUV;

layout(binding=0)uniform sampler2D textura;

layout(location=2)uniform vec3 textColor;

out vec4 cor;

void main()
{
  // vec4 text=vec4(1.,1.,1.,texture(textura,vUV).r);
  // cor=vec4(textColor,1.)*text;

  float text=texture(textura,vUV.xy).r;
  cor=vec4(textColor.rgb*text, text);
}