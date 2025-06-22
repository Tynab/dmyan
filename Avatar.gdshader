shader_type canvas_item;

void fragment() {
    vec2 uv = UV - vec2(0.5);
    float radius = 0.5;
    if (length(uv) > radius) {
        COLOR = vec4(0.0);
    } else {
        COLOR = texture(TEXTURE, UV);
    }
}
