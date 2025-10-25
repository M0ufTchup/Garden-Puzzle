extends Sprite2D


@export var DissolveDirection = 180;

func _input(event):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		burnCard(DissolveDirection)

func burnCard(direction):
	if material and material is ShaderMaterial:
		var tween = create_tween()
		# set burning direction in degrees
		material.set_shader_parameter("direction", direction)
		# use tweens to animate the progress value
		tween.tween_method(update_progress, -1.5, 1.5, 1.0)
	 
func update_progress(value: float):
	if material:
		material.set_shader_parameter("progress", value)
