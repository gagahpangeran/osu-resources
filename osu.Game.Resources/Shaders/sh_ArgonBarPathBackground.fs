#ifndef ARGON_BAR_PATH_BACKGROUND_FS
#define ARGON_BAR_PATH_BACKGROUND_FS

#undef HIGH_PRECISION_VERTEX
#define HIGH_PRECISION_VERTEX

#include "sh_Utils.h"
#include "sh_Masking.h"
#include "sh_TextureWrapping.h"
#include "sh_ArgonBarPathUtils.h"

layout(location = 2) in highp vec2 v_TexCoord;

layout(std140, set = 0, binding = 0) uniform m_ArgonBarPathBackgroundParameters
{
    mediump vec2 size;
    mediump float pathRadius;
    mediump float padding;
};

layout(location = 0) out vec4 o_Colour;

lowp vec4 bgColAt(highp float absoluteTexturePos)
{
    highp float relativeTexturePos = clamp(absoluteTexturePos / pathRadius, 0.0, 1.5);
    return mix(vec4(vec3(0.0), 0.2), vec4(vec3(1.0), 0.8), relativeTexturePos / 1.5);
}

lowp vec4 getColour(highp float absoluteTexturePos)
{
    if (absoluteTexturePos > pathRadius - 1.0)
        return mix(vec4(1.0), vec4(vec3(1.0), 0.0), absoluteTexturePos - (pathRadius - 1.0));

    if (absoluteTexturePos > pathRadius - 2.0)
        return mix(bgColAt(absoluteTexturePos), vec4(1.0), absoluteTexturePos - (pathRadius - 2.0));

    return bgColAt(absoluteTexturePos);
}    

void main(void)
{
    highp vec2 resolution = v_TexRect.zw - v_TexRect.xy;
    highp vec2 pixelPos = (v_TexCoord - v_TexRect.xy) / resolution;

    mediump vec2 absolutePos = size * pixelPos;
    highp float absoluteTexturePos = getBarTexturePosition(size, 0.0, 1.0, padding, absolutePos);
    o_Colour = getRoundedColor(getColour(absoluteTexturePos), v_TexCoord);
}

#endif