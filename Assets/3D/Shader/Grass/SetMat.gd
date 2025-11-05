extends MultiMeshInstance3D
@export var GrassMaterial : Material;

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	set_material_override(GrassMaterial)
