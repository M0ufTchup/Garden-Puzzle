extends Control

var using_mouse: bool = false
var ui_mode: String = "Main"
var languages: Array[String] = ["en_US", "fr", "es", "de", "it", "pt", "ru", "el", "tr", "da", "no", "sv", "nl", "pl", "fi", "ja", "zh_cn", "zh_tw", "ko", "cs_CZ", "hu_HU", "ro_RO", "th_TH", "bg_BG", "he_IL", "ar", "bs_BA"]
var lang_index:int = 0

#Control node to hide menus
@export_category("Menu Control Nodes")
@export var save_menu_node: Control
@export var option_menu_node: Control
@export var title_menu_node: Control

#Control node to get focus back when returning on a menu / going from mouse to keyboard or Controller
@export_category("Focus Control Nodes")
@export var save_node_focus: Control
@export var main_node_focus: Control
@export var option_node_focus: Control
var main_option_node_focus: Control
@export var titles_node_focus: Control

#Control node to change values on start
#SAVES
@export_category("SAVE1 Nodes")
@export var background_node1: Sprite2D
@export var time_text_node1: Label
@export var title_text_node1: Label
@export_category("SAVE2 Nodes")
@export var background_node2: Sprite2D
@export var time_text_node2: Label
@export var title_text_node2: Label
@export_category("SAVE3 Nodes")
@export var background_node3: Sprite2D
@export var time_text_node3: Label
@export var title_text_node3: Label

#OPTIONS
@export_category("Options Nodes")
@export var main_volume_slider: HSlider
@export var music_volume_slider: HSlider
@export var sfx_volume_slider: HSlider

@export var window_mode_button: OptionButton
@export var resolution_button: OptionButton
@export var vsync_checkbox: CheckButton


#TESTS :
@export_category("TESTS")
@export var texture1: Texture
@export var texture2: Texture
var title1: String = "TITLES_GARDEN_TITLE_01"
var title2: String = "TITLES_GARDEN_TITLE_02"

func _ready():
	#GET ALL VARIABLES FROM GLOBAL / UPDATE
	load_option_values()
	update_save_UI()
	
	main_node_focus.grab_focus()
	save_menu_node.visible = false
	option_menu_node.visible = false
	title_menu_node.visible = false
	main_option_node_focus = option_node_focus

func _unhandled_input(event):
	print(event.as_text())
	if event.as_text().contains("Mouse") :
		var current_focus_control = get_viewport().gui_get_focus_owner()
		if current_focus_control :
			if ui_mode == "Option" :
				option_node_focus = current_focus_control
			current_focus_control.release_focus()
		using_mouse = true
	elif using_mouse :
		using_mouse = false
		set_focus(ui_mode)

func set_focus(mode : String) :
	ui_mode = mode
	match mode :
		"Save" :
			save_node_focus.grab_focus()
		"Main" :
			main_node_focus.grab_focus()
		"Option" :
			option_node_focus.grab_focus()
		"Title" :
			titles_node_focus.grab_focus()

func load_option_values():
	_on_main_vol_slider_value_changed(SaveLoad.SaveFileData.OptionData.main_volume)
	_on_music_vol_slider_value_changed(SaveLoad.SaveFileData.OptionData.music_volume)
	_on_sfx_vol_slider_value_changed(SaveLoad.SaveFileData.OptionData.sfx_volume)
	_on_window_mode_select_item_selected(SaveLoad.SaveFileData.OptionData.window_mode_index)
	_on_resolution_option_item_selected(SaveLoad.SaveFileData.OptionData.resolution_index)
	_on_vsync_check_toggled(SaveLoad.SaveFileData.OptionData.vsync_enabled)
	
	main_volume_slider.value = SaveLoad.SaveFileData.OptionData.main_volume
	music_volume_slider.value = SaveLoad.SaveFileData.OptionData.music_volume
	sfx_volume_slider.value = SaveLoad.SaveFileData.OptionData.sfx_volume
	window_mode_button.selected = SaveLoad.SaveFileData.OptionData.window_mode_index
	resolution_button.selected = SaveLoad.SaveFileData.OptionData.resolution_index
	vsync_checkbox.button_pressed = SaveLoad.SaveFileData.OptionData.vsync_enabled
	
	lang_index = SaveLoad.SaveFileData.OptionData.language
	TranslationServer.set_locale(languages[lang_index])
	
	SaveLoad._save()

#MAIN BUTTONS

func _on_play_button_button_down():
	save_menu_node.visible = true
	set_focus("Save")

func _on_options_button_button_down():
	option_menu_node.visible = true
	set_focus("Option")

func _on_titles_button_button_down():
	title_menu_node.visible = true
	set_focus("Title")

func _on_quit_button_button_down():
	get_tree().quit()

#SAVE MENU




#OPTION MENU

func _on_main_vol_slider_value_changed(value):
	_change_volume("Master", value)
	SaveLoad.SaveFileData.OptionData.main_volume = value

func _on_music_vol_slider_value_changed(value):
	_change_volume("Music", value)
	SaveLoad.SaveFileData.OptionData.music_volume = value

func _on_sfx_vol_slider_value_changed(value):
	_change_volume("SFX", value)
	SaveLoad.SaveFileData.OptionData.sfx_volume = value

func _change_volume(bus_name: String, volume: float):
	var db = linear_to_db(volume)
	var audio_bus_id = AudioServer.get_bus_index(bus_name)
	AudioServer.set_bus_volume_db(audio_bus_id, db)


func _on_window_mode_select_item_selected(index):
	var options = [DisplayServer.WINDOW_MODE_FULLSCREEN, DisplayServer.WINDOW_MODE_WINDOWED]
	var value = options[index]
	DisplayServer.window_set_mode(value)
	SaveLoad.SaveFileData.OptionData.window_mode_index = index

func _on_resolution_option_item_selected(index):
	var options = [1.0, 0.75, 0.5, 0.25]
	var value = options[index]
	get_tree().root.scaling_3d_scale = value
	SaveLoad.SaveFileData.OptionData.resolution_index = index

func _on_vsync_check_toggled(toggled_on):
	if toggled_on :
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_ENABLED)
	else :
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_DISABLED)
	SaveLoad.SaveFileData.OptionData.vsync_enabled = toggled_on

func _on_reset_button_button_down():
	SaveLoad.SaveFileData.OptionData = OptionDataResource.new()
	load_option_values()

func _on_options_back_button_button_down():
	option_menu_node.visible = false
	option_node_focus = main_option_node_focus
	set_focus("Main")
	SaveLoad._save()
	
func _on_language_button_button_down():
	lang_index = (lang_index + 1) % languages.size()
	TranslationServer.set_locale(languages[lang_index])
	SaveLoad.SaveFileData.OptionData.language = lang_index

#TITLES MENU

func _on_back_button_button_down():
	title_menu_node.visible = false
	save_menu_node.visible = false
	set_focus("Main")


func _on_save_1_button_button_down():
	#LAUNCH SAVE 1
	pass # Replace with function body.

func _on_delete_1_button_down():
	SaveLoad.SaveFileData.SaveData1 = SaveDataResource.new()
	update_save_UI()

func _on_save_2_button_button_down():
	#LAUNCH SAVE 2
	pass # Replace with function body.

func _on_delete_2_button_down():
	SaveLoad.SaveFileData.SaveData2 = SaveDataResource.new()
	update_save_UI()

func _on_save_3_button_button_down():
	#LAUNCH SAVE 3
	pass # Replace with function body.

func _on_delete_3_button_down():
	SaveLoad.SaveFileData.SaveData3 = SaveDataResource.new()
	update_save_UI()

func update_save_UI():
	background_node1.texture = SaveLoad.SaveFileData.SaveData1.BackgroundImage
	time_text_node1.text = SaveLoad.SaveFileData.SaveData1.HoursPlayed
	title_text_node1.text = SaveLoad.SaveFileData.SaveData1.GardenTitle
	background_node2.texture = SaveLoad.SaveFileData.SaveData2.BackgroundImage
	time_text_node2.text = SaveLoad.SaveFileData.SaveData2.HoursPlayed
	title_text_node2.text = SaveLoad.SaveFileData.SaveData2.GardenTitle
	background_node3.texture = SaveLoad.SaveFileData.SaveData3.BackgroundImage
	time_text_node3.text = SaveLoad.SaveFileData.SaveData3.HoursPlayed
	title_text_node3.text = SaveLoad.SaveFileData.SaveData3.GardenTitle
	SaveLoad._save()


func _on_button_button_down():
	SaveLoad.SaveFileData.SaveData1.BackgroundImage = texture1
	SaveLoad.SaveFileData.SaveData1.HoursPlayed = "10%s" % tr("MENU_SAVE_HOURS_H") + " 03%s" % tr("MENU_SAVE_MINUTES_M")
	SaveLoad.SaveFileData.SaveData1.GardenTitle = title1
	SaveLoad.SaveFileData.SaveData2.BackgroundImage = texture2
	SaveLoad.SaveFileData.SaveData2.HoursPlayed = "200%s" % tr("MENU_SAVE_HOURS_H") + " 32%s" % tr("MENU_SAVE_MINUTES_M")
	SaveLoad.SaveFileData.SaveData2.GardenTitle = title2
	update_save_UI()
